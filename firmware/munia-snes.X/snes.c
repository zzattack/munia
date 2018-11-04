#include "snes.h"
#include "hardware.h"
#include "globals.h"
#include "gamepad.h"
#include "asm_decl.h"
#include <usb/usb_device_hid.h>
#include "config.h"

extern config_t config;
snes_packet_t joydata_snes_last_raw;

void snes_tasks() {
    if (pollNeeded && (config.input_snes && config.output_mode != output_snes)) {
        USBDeviceTasks();        
        di();        
        snes_poll();
    }
        
    if (packets.snes_test) {
        snes_handle_packet();
        packets.snes_test = false;
    }
    INTCONbits.IOCIF = 0; // don't bother with stuff that happened in the meantime
    ei();
    
    if (packets.snes_avail) {
        // see if this packet is equal to the last transmitted one, and if so, discard it
        // also when in menu, menu_tasks will clear bit
        if (memcmp(&joydata_snes_raw, &joydata_snes_last_raw, sizeof(snes_packet_t))) {
            // new, changed packet available; unpack if faking and send over usb
            if (USB_READY && !HIDTxHandleBusy(USBInHandleSNES)) {
                // dbgs("snes_joydata_createhid()\n");
                snes_joydata_createhid();
                USBInHandleSNES = HIDTxPacket(HID_EP_SNES, (uint8_t*)&joydata_snes_usb, sizeof(snes_packet_t));
            }
            
            // save last packet
            memcpy(&joydata_snes_last_raw, &joydata_snes_raw, sizeof(snes_packet_t));            
        } 
        packets.snes_avail = false;
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
    
    // 6 us after the fall of the data latch pulse, the CPU sends out 16 data clock pulses on
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
    packets.snes_test = true;
}

void snes_handle_packet() {
    uint8_t idx = sample_w - sample_buff;
	if (idx == 16) {
		uint8_t* w = (uint8_t*)&joydata_snes_raw;
		uint8_t* r = (uint8_t*)sample_buff;
		for (uint8_t i = 0; i < sizeof(snes_packet_t); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000100) // RC2
                   x |= m;
            }            
            *w++ = ~x; // high's are not pressed, lows are pressed --> invert
		}
        // these are inverted in HID reports

        packets.snes_avail = true;
	}
}

void snes_joydata_createhid() {
    joydata_snes_usb = joydata_snes_raw;
    joydata_snes_usb.dpad = hat_lookup_n64_snes[joydata_snes_usb.dpad];
}