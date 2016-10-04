#include "snes.h"
#include "globals.h"
#include "gamepad.h"
#include "menu.h"
#include "fakeout.h"
#include <usb/usb_device_hid.h>

snes_packet_t joydata_snes_last;
void snes_create_ngc_fake();

void snes_tasks() {
    if (pollNeeded && (in_menu || config.input_snes && (config.output_mode == output_pc || config.output_mode != output_snes))) {
        USBDeviceTasks();        
        di();        
        snes_poll();
    }
    
    if (packets.snes_test) {
        packets.snes_test = false;
        snes_handle_packet();
    }
    INTCONbits.IOCIF = 0; // don't bother with stuff that happened in the meantime
    ei();
    
    if (packets.snes_avail) {
        // see if this packet is equal to the last transmitted one, and if so, discard it
        if (memcmp(&joydata_snes, &joydata_snes_last, sizeof(snes_packet_t)) == 0)
            packets.snes_avail = false;
        else if (!in_menu) { // if in menu, menu_tasks will clear bit
            // new, changed packet available; unpack if faking and send over usb
            if (config.input_snes && config.output_mode == output_ngc) {
                snes_create_ngc_fake();
                fake_unpack((uint8_t*)&joydata_ngc_raw, sizeof(ngc_packet_t));
            }        
            if (USB_READY && !HIDTxHandleBusy(USBInHandleSNES)) {
                USBInHandleSNES = HIDTxPacket(HID_EP_SNES, (uint8_t*)&joydata_snes, sizeof(snes_packet_t));
                // save last packet
                memcpy(&joydata_snes_last, &joydata_snes, sizeof(snes_packet_t));            
            }
            packets.snes_avail = false;
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
    
    // 6 �s after the fall of the data latch pulse, the CPU sends out 16 data clock pulses on
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
	if (idx == 16 && !HIDTxHandleBusy(USBInHandleSNES)) {
		uint8_t* w = (uint8_t*)&joydata_snes;
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
        joydata_snes_raw = joydata_snes;
        joydata_snes.dpad = hat_lookup_n64_snes[joydata_snes.dpad];

        packets.snes_avail = true;
	}
}
