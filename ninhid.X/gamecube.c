#include "gamecube.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"

ngc_packet_t joydata_ngc_last;

void ngc_tasks() {
    if (ngc_mode == pc && pollNeeded && USB_READY) {
        di();
        // todo: count time a sample takes
        ngc_poll();
        sample_w = sample_buff + 25;
        // waste some more instructions before sampling
        TMR0 = 255-25; TMR0IF=0; while (!TMR0IF);
        ngc_sample();
        ei();
    }

    if (ngc_test_packet) {
        ngc_test_packet = FALSE;
        ngc_handle_packet();
    }
    
    if (ngc_packet_available) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        if (memcmp(&joydata_ngc, &joydata_ngc_last, sizeof(joydata_ngc)) == 0) 
            ngc_packet_available = FALSE;
    }
    
    if (ngc_packet_available && (ngc_mode == pc || ngc_mode == real) && USB_READY && !HIDTxHandleBusy(USBInHandleNGC)) {
        // hid tx
        USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(joydata_ngc));
        ngc_packet_available = FALSE;
        // save last packet
        memcpy(&joydata_ngc_last, &joydata_ngc, sizeof(joydata_ngc));
    }
}

void ngc_handle_packet() {
	UINT8 idx = sample_w - sample_buff;
	if (idx == 90) {
		BYTE* w = (BYTE*)&joydata_ngc;
        
		// bits 0-23 are request, 24 is stop bit, 25-88 is data
		BYTE* r = sample_buff + 25;
		for (UINT8 i = 0; i < sizeof(joydata_ngc); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000001)
                   x |= m;
            }            
            *w++ = x;
		}
        
        // these are inverted in HID reports
        joydata_ngc.joy_y = -joydata_ngc.joy_y;
        joydata_ngc.hat = hat_lookup[joydata_ngc.hat & 0xF];
        
		ngc_packet_available = TRUE;
	}
}
