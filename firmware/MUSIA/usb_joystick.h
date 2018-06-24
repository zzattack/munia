#pragma once

#include "ps2_state.h"

class usb_joystick {
private:
	ps2_state* state;
public:	
	usb_joystick(ps2_state* state);
	void sendReport();
};