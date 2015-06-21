#include "n64.h"
#include "hardware.h"
#include "globals.h"
#include <usb/usb_device_hid.h>
#include "gamepad.h"

n64_packet_t joydata_n64_last;

void n64_tasks() {
    if (n64_test_packet) n64_handle_packet();
    
    if (n64_mode == pc && pollNeeded && USB_READY) {
        di();
        // todo: count time a sample takes
        n64_poll();
        sample_w = sample_buff + 25;
        // waste some more instructions before sampling
        TMR0 = 255-30; TMR0IF=0; while (!TMR0IF);
        n64_sample();
        ei();
    }

    if (n64_packet_available) {
        // see if this packet is equal to the last transmitted 
        // one, and if so, discard it
        //if (memcmp(&joydata_n64, &joydata_n64_last, sizeof(joydata_n64)) == 0) 
        //    n64_packet_available = FALSE;
    }
    
    if (n64_packet_available && (n64_mode == pc || n64_mode == real) && USB_READY && !HIDTxHandleBusy(USBInHandleN64)) {
        // hid tx
        USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64, sizeof(joydata_n64));
        n64_packet_available = FALSE;
        // save last packet
        //memcpy(&joydata_n64_last, &joydata_n64, sizeof(joydata_n64));
    }
}

void n64_handle_packet() {
	UINT8 idx = sample_w - sample_buff;
	if (idx == 90) {
		BYTE* w = (BYTE*)&joydata_n64;
        
		// bits 0-23 are request, 24 is stop bit, 25-88 is data
		BYTE* r = sample_buff + 25;
		for (UINT8 i = 0; i < sizeof(joydata_n64); i++) {
            uint8_t x = 0;
            for (uint8_t m = 0x80; m; m >>= 1) {
                if (*r++ & 0b00000001)
                   x |= m;
            }            
            *w++ = x;
		}
		n64_packet_available = TRUE;
	}
}