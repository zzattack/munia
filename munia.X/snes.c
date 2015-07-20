#include "snes.h"
#include "globals.h"
#include "gamepad.h"
#include "menu.h"
#include <usb/usb_device_hid.h>

snes_packet_t joydata_snes_last;
bool snes_test_packet = false;

void snes_tasks() { 
    if (config.snes_mode == pc && !snes_console_attached && pollNeeded && (in_menu || USB_READY)) {
        di();        
        USBDeviceTasks();
        snes_poll();
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

}

void snes_handle_packet() {
    uint8_t idx = sample_w - sample_buff;
	if (idx == 16 && !HIDTxHandleBusy(USBInHandleSNES)) {
		uint8_t* w = (uint8_t*)&joydata_snes;
        
		// bit 0 is not sampled, then 0-23 are request, 24 is stop bit, 25-88 is data
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
    sample_w = sample_buff;
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
        LCD_D7 = 0;
        sample_w++;
        i--;
        TMR0 = 255-100; TMR0IF = 0;
        while (!SNES_CLK && !TMR0IF);
        TMR0 = 255-100; TMR0IF = 0;
        LCD_D7 = 1;
    }
    snes_test_packet = !i;
}