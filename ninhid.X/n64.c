#include "n64.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"
#include "menu.h"

n64_packet_t joydata_n64_last;

const uint8_t hat_lookup_n64[16] = {
    // l and r are swapped compared to ngc
    HAT_SWITCH_NULL,       // 0b0000, centered
    HAT_SWITCH_EAST,       // 0b0001, right
    HAT_SWITCH_WEST,       // 0b0010, left
    HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
    HAT_SWITCH_SOUTH,      // 0b0100, down
    HAT_SWITCH_SOUTH_EAST, // 0b0101, down+right
    HAT_SWITCH_SOUTH_WEST, // 0b0110, down+left
    HAT_SWITCH_NULL,       // 0b0111, // 3 at once
    HAT_SWITCH_NORTH, // 0b1000, up
    HAT_SWITCH_NORTH_EAST, // 0b1001, up+right
    HAT_SWITCH_NORTH_WEST, // 0b1010, up+left
    HAT_SWITCH_NULL, // 0b1011, // 3 at once
    HAT_SWITCH_NULL, // 0b1100, up+down, not possible
    HAT_SWITCH_NULL, // 0b1101, // 3 at once
    HAT_SWITCH_NULL, // 0b1110, // 3 at once
    HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

void n64_tasks() {
    if (config.n64_mode == pc && pollNeeded && (in_menu || USB_READY)) {
        di();
        USBDeviceTasks();
        n64_poll();
        sample_w = sample_buff + 8;
        // waste some more instructions before sampling
        // TMR0 = 255-30; TMR0IF=0; while (!TMR0IF);
        n64_sample();
        USBDeviceTasks();
        ei();
    }
    else if (n64_test_packet) {
        n64_test_packet = false;
        n64_handle_packet();
    }
    
    if (n64_packet_available) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        if (memcmp(&joydata_n64, &joydata_n64_last, sizeof(n64_packet_t)) == 0) 
            n64_packet_available = false;
    }
    
    if (n64_packet_available && !in_menu && (config.n64_mode == pc || config.n64_mode == console) && USB_READY && !HIDTxHandleBusy(USBInHandleN64)) {
        // hid tx
        USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64, sizeof(n64_packet_t));
        n64_packet_available = false;
        // save last packet
        memcpy(&joydata_n64_last, &joydata_n64, sizeof(n64_packet_t));
    }
}

void n64_poll() {
    portc_mask = 0b00000010;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low
    // send 01000000
    //      00000011    
    //      00000010
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH();
    // stop bit, 2 us
    CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void n64_sample() {
    // latency from interrupt to calling this should be about 12 cycles
    *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry
    LATA &= 0b11111110;
    sample_w++;
    while (!N64_DAT);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;

loop:
    if (INTCONbits.TMR0IF) { // timeout - no bit received
        n64_test_packet = true;
        return;
    }
    if (N64_DAT) goto loop;

    // waste some time (aligned with scope)
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();

    *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry
    LATA ^= 0b00000001;
    sample_w++;
    
    while (!N64_DAT);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;
    goto loop;
}

void n64_handle_packet() {
	uint8_t idx = sample_w - sample_buff;
	if (idx == 42) {
		uint8_t* r = sample_buff;
        
        if (config.n64_mode == console) {
            // see if this is a 'command 1' request from the console
            uint8_t cmd = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000010)
                    cmd |= m;
            }
            // if not controller data, it's possibly identification 
            // or memory pack data -- not of interest to us
            if (cmd != 0x01) return;
            r++; // skip stopbit
        }
        else
            r += 9; // skip header+stopbit
        
		// bits 0-23 are request, 24 is stop bit, 25-88 is data
		uint8_t* w = (uint8_t*)&joydata_n64;
		for (uint8_t i = 0; i < sizeof(n64_packet_t); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000010)
                   x |= m;
            }            
            *w++ = x;
		}
        
        joydata_n64.joy_x -= 128;
        joydata_n64.joy_y = 128 - joydata_n64.joy_y;
        joydata_n64.dpad = hat_lookup_n64[joydata_n64.dpad];
        // when l and r are pressed, the start button bit seems to shift to unused1
        joydata_n64.start |= joydata_n64.__unused1 & joydata_n64.l & joydata_n64.r;
        
		n64_packet_available = true;
	}
    sample_w = sample_buff;
}