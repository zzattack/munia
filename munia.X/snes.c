#include "snes.h"
#include "globals.h"
#include "gamepad.h"
#include "menu.h"
#include <usb/usb_device_hid.h>

snes_packet_t joydata_snes_last;
bool snes_test_packet = false;

void snes_tasks() {
    static uint8_t snes_block = 0;
    
    if (pollNeeded & snes_block > 0) snes_block--;
    else if (config.snes_mode == pc && pollNeeded && (in_menu || USB_READY)) {
        di();        
        USBDeviceTasks();
        
        // if latch is low without us driving it low, then there's a console attached!
        // in this case we should never drive the lines, this is dangerous!
        if (SNES_LATCH == 0) {
            // block snes lines for 2 seconds
            snes_block = 120; // 120 frames
        }
        else {
            snes_poll();
        }
        
        USBDeviceTasks();
        ei();
    }
    
    if (snes_test_packet) {
        snes_test_packet = false;
        snes_handle_packet();
    }
    
    if (snes_packet_available && !in_menu && (config.snes_mode == pc || config.snes_mode == console) && USB_READY && !HIDTxHandleBusy(USBInHandleSNES)) {
        if (memcmp(&joydata_snes, &joydata_snes_last, sizeof(snes_packet_t)) == 0) {
            // see if this packet is equal to the last transmitted 
            // one, and if so, discard it
            snes_packet_available = false;
        }
        else {
            // hid tx
            USBInHandleSNES = HIDTxPacket(HID_EP_SNES, (uint8_t*)&joydata_snes, sizeof(snes_packet_t));
            snes_packet_available = false;
            // save last packet
            memcpy(&joydata_snes_last, &joydata_snes, sizeof(snes_packet_t));
        }
    }
}

void snes_poll() {
    sample_w = sample_buff;

    // Every 16.67ms (or about 60Hz), the SNES CPU sends out a 12us wide, positive
    // going data latch pulse on pin 3
    SNES_CLK_TRIS = 0;
    SNES_LATCH = 1;
    SNES_LATCH_TRIS = 0;
    __delay_us(12);
    SNES_LATCH = 0;    
    
    // 6 ms after the fall of the data latch pulse, the CPU sends out 16 data clock pulses on
    // pin 2. These are 50% duty cycle with 12us per full cycle.
    __delay_us(6);
    for (uint8_t i = 0; i < 16; i++) {
        SNES_CLK = 0;
        __delay_us(6);
        *sample_w++ = PORTC;
        SNES_CLK = 1;
        __delay_us(6);
    }
    
    SNES_CLK_TRIS = 1;
    SNES_LATCH_TRIS = 1;
    snes_test_packet = true;
}

void snes_handle_packet() {
    uint8_t idx = sample_w - sample_buff;
	if (idx == 16 && !HIDTxHandleBusy(USBInHandleSNES)) {
		uint8_t* w = (uint8_t*)&joydata_snes;
        
		// bit 0 is not sampled
		uint8_t* r = sample_buff + 0;
		for (uint8_t i = 0; i < sizeof(snes_packet_t); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000100) // RC2
                   x |= m;
            }            
            *w++ = ~x; // high's are not pressed, lows are pressed --> invert
		}
        
        // these are inverted in HID reports
        joydata_snes.dpad = hat_lookup_n64_snes[joydata_snes.dpad];
        
        snes_packet_available = true;
	}
	sample_w = sample_buff;
}

void snes_sample() { 
    // called in interrupt: watches the SNES_CLK for 16 pulses
    // The controllers serially shift the latched button states out SNES_DAT on every rising edge
    // and the CPU samples the data on every falling edge.
    uint8_t i = 16;
    TMR0 = 150;
    TMR0IF = 0;
    
    while (SNES_LATCH) {
        if (TMR0IF) return;
    }
    TMR0 = 150;
    TMR0IF = 0;
    while (i && !TMR0IF && !SNES_LATCH) {
        while (SNES_CLK && !TMR0IF); 
        *sample_w = PORTC;
        sample_w++;
        i--;
        TMR0 = 255-100; TMR0IF = 0;
        while (!SNES_CLK && !TMR0IF);
        TMR0 = 255-100; TMR0IF = 0;
    }
    snes_test_packet = !i;
}