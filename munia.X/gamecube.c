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
        di();
        USBDeviceTasks();
        ngc_poll();
        sample_w = sample_buff + 25;
        // waste some more instructions before sampling
        TMR0 = 255-25; TMR0IF=0; while (!TMR0IF);
        ngc_sample();
        USBDeviceTasks();
        ei();
    }

    if (ngc_test_packet) {
        ngc_test_packet = false;
        ngc_handle_packet();
    }
    
    if (ngc_packet_available) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        if (memcmp(&joydata_ngc, &joydata_ngc_last, sizeof(ngc_packet_t)) == 0) 
            ngc_packet_available = false;
    }
    
    if (ngc_packet_available && !in_menu) {
        if (USB_READY && !HIDTxHandleBusy(USBInHandleNGC)) {
            // hid tx
            USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(ngc_packet_t));
            // save last packet
            memcpy(&joydata_ngc_last, &joydata_ngc, sizeof(ngc_packet_t));
        }
        ngc_packet_available = false;
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
    CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void ngc_sample() {
    // latency from interrupt to calling this should be about 12 cycles
    *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry
    //LATA &= 0b11111110;
    sample_w++;
    
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;
    while (!NGC_DAT && !INTCONbits.TMR0IF);
    TMR0 = 255 - 60; // 1.5 bit wait

loop:
    if (INTCONbits.TMR0IF) { // timeout - no bit received
        uint8_t idx = sample_w - sample_buff;
        if (config.snes_mode == SNES_MODE_NGC) {            
            if (idx == 9 && (sample_buff[1] & 1) == 0 && (sample_buff[7] & 1) == 0) {
                // tell wii we're here
                ngc_outbuf_idx = 24;
                ngc_outbuf = ngc_id_packet;
                ngc_fakeout();
            }
            else if (idx == 9) {
                ngc_outbuf_idx = 80;
                ngc_outbuf = ngc_init_packet;
                ngc_fakeout();
            }
            else if (idx == 25) {
                ngc_outbuf_idx = 64;
                ngc_outbuf = ngc_fake_buffer;
                ngc_fakeout();
                WRITETIMER3(40000); // request next poll cycle asap
            }
        }
        else
            ngc_test_packet = true;
        return;
    }
    if (NGC_DAT) goto loop;

    // waste some time (aligned with scope)
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();

    *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry
    sample_w++;
    
    while (!NGC_DAT);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;
    goto loop;
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
            Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
            Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
            SET(); // use stopwatch to verify this happens 3µs after CLR())
        }
        else {
            Nop(); Nop(); Nop();
            SET(); // use stopwatch to verify this happens 3µs after CLR()
            r++; ngc_outbuf_idx--; Nop();
            Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
            Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
        }
        Nop(); Nop(); Nop(); Nop();
    }
        
    // stop bit, 2 us
    CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up    
}

void ngc_handle_packet() {
    // translate samples in sample_buff to joydata_ngc
	uint8_t idx = sample_w - sample_buff;
    
	if (idx == 90 && !HIDTxHandleBusy(USBInHandleNGC)) {
		uint8_t* w = (uint8_t*)&joydata_ngc;
        
		// bit 0 is not sampled, then 0-23 are request, 24 is stop bit, 25-88 is data
		uint8_t* r = sample_buff + 25;
		for (uint8_t i = 0; i < sizeof(ngc_packet_t); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000001)
                   x |= m;
            }            
            *w++ = x;
		}
        
        // these are inverted in HID reports
        joydata_ngc.joy_y = -joydata_ngc.joy_y;
        joydata_ngc.c_y = -joydata_ngc.c_y;
        joydata_ngc.hat = hat_lookup_ngc[joydata_ngc.hat];
        
        ngc_packet_available = true;
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