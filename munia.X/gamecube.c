#include "gamecube.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"
#include "menu.h"

uint8_t ngc_outbuf_idx = 0;


// Packets below should not be stored in program memory but kept in RAM,
// otherwise timing requirements cannot be met in ngc_fakeout()

// controller sends these 24 bits when wii sends a 9-bit all low packet
uint8_t ngc_id_packet[] = { 
    0, 0, 0, 0, 1, 0, 0, 1, 
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 1, 1
};
// controller sends these 80 bits when wii sends a 9-bit packet with 2 bits high
uint8_t ngc_init_packet[] = {
    0, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 1, 0,
    1, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 1, 1,
    1, 0, 0, 0, 1, 0, 0, 0,
    0, 0, 1, 0, 0, 0, 1, 1,
    0, 0, 1, 0, 1, 0, 1, 1,
    0, 0, 0, 0, 0, 0, 0, 1,
    0, 0, 0, 0, 0, 0, 0, 1, 1
};
    

void ngc_fakeout();
void ngc_send_id();

ngc_packet_t joydata_ngc_last;

// ngc report: <left><right><down>up>
const uint8_t hat_lookup_ngc[16] = {
    HAT_SWITCH_NULL,       // 0b0000, centered
    HAT_SWITCH_WEST,       // 0b0001, left
    HAT_SWITCH_EAST,       // 0b0010, right
    HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
    HAT_SWITCH_SOUTH,      // 0b0100, down
    HAT_SWITCH_SOUTH_WEST, // 0b0101, down+left
    HAT_SWITCH_SOUTH_EAST, // 0b0110, down+right
    HAT_SWITCH_NULL,       // 0b0111, // 3 at once
    HAT_SWITCH_NORTH, // 0b1000, up
    HAT_SWITCH_NORTH_WEST, // 0b1001, up+left
    HAT_SWITCH_NORTH_EAST, // 0b1010, up+right
    HAT_SWITCH_NULL, // 0b1011, // 3 at once
    HAT_SWITCH_NULL, // 0b1100, up+down, not possible
    HAT_SWITCH_NULL, // 0b1101, // 3 at once
    HAT_SWITCH_NULL, // 0b1110, // 3 at once
    HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

void ngc_tasks() {
    if (config.ngc_mode == NGC_MODE_PC && pollNeeded && (in_menu || USB_READY)) {
        USBDeviceTasks();
        di();
        ngc_poll();
        // waste some more instructions before sampling
        _delay(40);
        asm("lfsr 0, _sample_buff+25"); // setup FSR0
        ngc_sample();
        asm("movff FSR0L, _sample_w+0"); // update sample_w
        asm("movff FSR0H, _sample_w+1");
        ei();
        USBDeviceTasks();
    }

    if (packets.ngc_test) {
        packets.ngc_test = false;
        ngc_handle_packet();
    }
    
    if (packets.ngc_avail) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        if (memcmp(&joydata_ngc, &joydata_ngc_last, sizeof(ngc_packet_t)) == 0) 
            packets.ngc_avail = false;
    }
    
    if (packets.ngc_avail && !in_menu) {
        if (USB_READY && !HIDTxHandleBusy(USBInHandleNGC)) {
            // hid tx
            USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(ngc_packet_t));
            // save last packet
            memcpy(&joydata_ngc_last, &joydata_ngc, sizeof(ngc_packet_t));
        }
        packets.ngc_avail = false;
    }
}

void ngc_poll() {
    portc_mask = 0b00000001;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low
    // send 01000000
    //      00000011    
    //      00000010
    LOW(); HIGH(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH(); HIGH(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void ngc_fakeout() {
    portc_mask = 0b00000001;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low
    uint8_t* r = ngc_outbuf;
        
    while (ngc_outbuf_idx) {
        CLR(); // use stopwatch to verify this happens either 1 or 3 µs after SET() call below)
        if (!*r) {
            // low
            r++; ngc_outbuf_idx--; Nop();
            _delay(24);
            SET(); // use stopwatch to verify this happens 3µs after CLR())
        }
        else {
            _delay(3);
            SET(); // use stopwatch to verify this happens 3µs after CLR()
            r++; ngc_outbuf_idx--;
            _delay(22);
        }
        _delay(4);
    }
        
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up    
}

void ngc_handle_packet() {
    // translate samples in sample_buff to joydata_ngc
	uint8_t idx = sample_w - sample_buff;
    // dbgs("ngc packet len: "); dbgsval(idx); dbgs("\n");
    
	if (idx == 90 && !HIDTxHandleBusy(USBInHandleNGC)) {        
		uint8_t* w = (uint8_t*)&joydata_ngc;
        
		// bit 0 is not sampled, then 0-23 are request, 24 is stop bit, 25-88 is data
		uint8_t* r = sample_buff + 25;
		for (uint8_t i = 0; i < sizeof(ngc_packet_t); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                // dbgs("*r = "); dbgsval(*r); dbgs("\n");
                if (*r++ >= 0x07) // threshold is 7 samples high
                   x |= m;
            }            
            *w++ = x;
		}
        
        // these are inverted in HID reports
        joydata_ngc.joy_y = -joydata_ngc.joy_y;
        joydata_ngc.c_y = -joydata_ngc.c_y;
        joydata_ngc.hat = hat_lookup_ngc[joydata_ngc.hat];
        
        packets.ngc_avail = true;
	}
}

void ngc_fake_unpack() {
    uint8_t* r = (uint8_t*)&ngc_fake_packet;
    uint8_t* w = ngc_fake_buffer;
    for (uint8_t i = 0; i < sizeof(ngc_packet_t); i++) {
        uint8_t b = *r++;
        for (uint8_t m = 0x80; m; m >>= 1)
            *w++ = (b & m) ? 1 : 0;
    }
}