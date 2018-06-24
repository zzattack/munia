#include "usb_joystick.h"
#include "usbd_customhid.h"

usb_joystick::usb_joystick(ps2_state* state) {
	this->state = state;	
}

void usb_joystick::sendReport() {
		
}