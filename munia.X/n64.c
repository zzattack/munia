#include "n64.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"
#include "menu.h"
#include "uarts.h"
#include "fakeout.h"

n64_packet_t joydata_n64_last; // last non-fake packet sent over usb

void n64_tasks() {
    if (pollNeeded && (in_menu || config.input_n64 && (config.output_mode == output_pc || config.output_mode != output_n64))) {
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
        if (config.output_mode == output_n64 && !(config.input_sources & input_n64)) n64_fakeout_test();
        else n64_handle_packet();
        packets.n64_test = false;
    }
    INTCONbits.IOCIF = 0; // don't bother with stuff that happened in the meantime
    ei();
    
    if (packets.n64_avail) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        if (memcmp(&joydata_n64, &joydata_n64_last, sizeof(n64_packet_t)) == 0) 
            packets.n64_avail = false; 
        else if (!in_menu && USB_READY && !HIDTxHandleBusy(USBInHandleN64)) {
            // hid tx
            USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64, sizeof(n64_packet_t));
            // save last packet
            memcpy(&joydata_n64_last, &joydata_n64, sizeof(n64_packet_t));
        }
    }
}

void n64_poll() {
    portc_mask = 0b00000010;
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low despite the pull up
    // send 01000000
    //      00000011    
    //      00000010
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH();
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}

void n64_handle_packet() {
	uint8_t idx = sample_w - sample_buff;
    // dbgs("n64 packet len: "); dbgsval(idx); dbgs("\n");
	if (idx == 41) {
		int8_t* r = (int8_t*)sample_buff;
        
        // see if this is a 'command 1' request from the console
        uint8_t cmd = pack_byte(r);
        // if not controller data, it's possibly identification 
        // or memory pack data -- not of interest to us
        if (cmd != 0x01) { 
            dbgs("invalid n64 command: "); dbgsval(cmd); dbgs("\n");
            return;
        }
        r += 9;

		// bits 0-9 are request, 10 is stop bit, 11-42 is data
		uint8_t* w = (uint8_t*)&joydata_n64;
		for (uint8_t i = 0; i < sizeof(n64_packet_t); i++) {
            *w++ = pack_byte(r);
            r += 8;
		}
        
        joydata_n64.joy_x -= 128;
        joydata_n64.joy_y = 128 - joydata_n64.joy_y;
        joydata_n64.dpad = hat_lookup_n64_snes[joydata_n64.dpad];
        // when l and r are pressed, the start button bit seems to shift to unused1
        joydata_n64.start |= joydata_n64.__unused1 & joydata_n64.l & joydata_n64.r;
        
		packets.n64_avail = true;
	}
}