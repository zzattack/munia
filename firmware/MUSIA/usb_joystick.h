#pragma once

#include "ps2_controller_if.h"
#include "ps2_state.h"

class usb_joystick {
private:
	ps2_state* state;
	ps2_hid_packet last_pkt;
	
public:	
	usb_joystick(ps2_state* state);
	void updateState();
	void sendReport(ps2_hid_packet* pkt);
};