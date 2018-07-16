#include "n64.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"
#include "asm_decl.h"
#include "config.h"

extern config_t config;
n64_packet_t joydata_n64_last_raw; // last non-fake packet sent over usb

void n64_tasks() {
    if (pollNeeded && (config.input_n64 && config.output_mode != output_n64)) {
        USBDeviceTasks();
        di();
        n64_poll();
        // waste some more instructions before sampling
        _delay(4);
        asm("lfsr 0, _sample_buff+8"); // setup FSR0
        n64_sample();
        asm("movff FSR0L, _sample_w+0"); // update sample_w
    }
    
    if (packets.n64_test) {
        n64_handle_packet();
        packets.n64_test = false;
    }
    
    INTCONbits.IOCIF = 0; // don't bother with stuff that happened in the meantime
    ei();
    
    if (packets.n64_avail) {
        // see if this packet is equal to the last transmitted one, and if so, discard it
        // also when in menu, menu_tasks will clear bit
        if (memcmp(&joydata_n64_raw, &joydata_n64_last_raw, sizeof(n64_packet_t))) {
            // new, changed packet available; unpack if faking and send over usb
            if (USB_READY && !HIDTxHandleBusy(USBInHandleN64)) {
                // dbgs("n64_joydata_createhid()\n");
                n64_joydata_createhid();
                USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64_usb, sizeof(n64_packet_t));
            }
            
            // save last packet
            memcpy(&joydata_n64_last_raw, &joydata_n64_raw, sizeof(n64_packet_t));
        }
        packets.n64_avail = false; 
    }
}

void n64_poll() {
    portc_mask = 0b00000010;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low despite the pull up
    // send 000000101
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH();
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void n64_handle_packet() {
	uint8_t idx = sample_w - sample_buff;
	if (idx == 41) {
		int8_t* r = (int8_t*)sample_buff;
        
        // see if this is a 'command 1' request from the console
        uint8_t cmd = pack_byte(r);
        // if not controller data, it's possibly identification 
        // or memory pack data -- not of interest to us
        if (config.output_mode == output_n64 && cmd != 0x01) { 
            return;
        }
        r += 9;

		// bits 0-9 are request, 10 is stop bit, 11-42 is data
		uint8_t* w = (uint8_t*)&joydata_n64_raw;
		for (uint8_t i = 0; i < sizeof(n64_packet_t); i++) {
            *w++ = pack_byte(r);
            r += 8;
		}
        
        // when l and r are pressed, the start button bit seems to shift to unused1
        joydata_n64_raw.start |= joydata_n64_raw.__unused1 & joydata_n64_raw.l & joydata_n64_raw.r;
        packets.n64_avail = true;
	}
}

void n64_joydata_createhid() {
    joydata_n64_usb = joydata_n64_raw;
    joydata_n64_usb.joy_x -= 128;
    joydata_n64_usb.joy_y = 128 - joydata_n64_usb.joy_y;
    joydata_n64_usb.dpad = hat_lookup_n64_snes[joydata_n64_usb.dpad];
}