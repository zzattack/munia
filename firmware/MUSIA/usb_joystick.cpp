#include "usb_joystick.h"
#include <usbd_hid.h>
#include <cstring>

extern USBD_HID_IfHandleType *const joy_if;

usb_joystick::usb_joystick(ps2_state* state) {
	this->state = state;	
}

//http://www.microsoft.com/whdc/archive/hidgame.mspx
#define HAT_SWITCH_NORTH            0x0
#define HAT_SWITCH_NORTH_EAST       0x1
#define HAT_SWITCH_EAST             0x2
#define HAT_SWITCH_SOUTH_EAST       0x3
#define HAT_SWITCH_SOUTH            0x4
#define HAT_SWITCH_SOUTH_WEST       0x5
#define HAT_SWITCH_WEST             0x6
#define HAT_SWITCH_NORTH_WEST       0x7
#define HAT_SWITCH_NULL             0x8

const uint8_t hat_lookup_n64_snes[16] = {
    // l and r are swapped compared to ngc
	HAT_SWITCH_NULL,       // 0b0000, centered
	HAT_SWITCH_EAST,       // 0b0001, right
	HAT_SWITCH_WEST,       // 0b0010, left
	HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
	HAT_SWITCH_SOUTH,      // 0b0100, down
	HAT_SWITCH_SOUTH_EAST, // 0b0101, down+right
	HAT_SWITCH_SOUTH_WEST, // 0b0110, down+left
	HAT_SWITCH_NULL,       // 0b0111, // 3 at once
	HAT_SWITCH_NORTH, // 0b1000, up
	HAT_SWITCH_NORTH_EAST, // 0b1001, up+right
	HAT_SWITCH_NORTH_WEST, // 0b1010, up+left
	HAT_SWITCH_NULL, // 0b1011, // 3 at once
	HAT_SWITCH_NULL, // 0b1100, up+down, not possible
	HAT_SWITCH_NULL, // 0b1101, // 3 at once
	HAT_SWITCH_NULL, // 0b1110, // 3 at once
	HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

const uint8_t hat_lookup_ps2[16] = {
	HAT_SWITCH_NULL,       // 0b0000, centered
	HAT_SWITCH_WEST,       // 0b0001, left
	HAT_SWITCH_EAST,       // 0b0010, right
	HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
	HAT_SWITCH_SOUTH,      // 0b0100, down
	HAT_SWITCH_SOUTH_WEST, // 0b0101, down+left
	HAT_SWITCH_SOUTH_EAST, // 0b0110, down+right
	HAT_SWITCH_NULL,       // 0b0111, // 3 at once
	HAT_SWITCH_NORTH, // 0b1000, up
	HAT_SWITCH_NORTH_WEST, // 0b1001, up+left
	HAT_SWITCH_NORTH_EAST, // 0b1010, up+right
	HAT_SWITCH_NULL, // 0b1011, // 3 at once
	HAT_SWITCH_NULL, // 0b1100, up+down, not possible
	HAT_SWITCH_NULL, // 0b1101, // 3 at once
	HAT_SWITCH_NULL, // 0b1110, // 3 at once
	HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

void usb_joystick::updateState() {
	ps2_hid_packet pkt;
	pkt.ddown = state->dpad_down;
	pkt.dup = state->dpad_up;
	pkt.dleft = state->dpad_left;
	pkt.dright = state->dpad_right;
	pkt.hat = hat_lookup_ps2[pkt.hat];
	
	pkt.cross = state->cross;
	pkt.circle = state->circle;
	pkt.triangle = state->triangle;
	pkt.square = state->square;
	
	pkt.l1  = state->l1;
	pkt.l2  = state->l2;
	pkt.r1  = state->r1;
	pkt.r2  = state->r2;
	pkt.start  = state->start;
	pkt.select  = state->select;
	pkt.l_stick  = state->l_stick;
	pkt.r_stick  = state->r_stick;
	
	pkt.left_x = state->analog1_x + 128;
	pkt.left_y = state->analog1_y + 128;
	pkt.right_x = state->analog2_x + 128;
	pkt.right_y = state->analog2_y + 128;
	
	if (memcmp(&pkt, &last_pkt, sizeof(pkt))) {
		sendReport(&pkt);
		last_pkt = pkt;
	}
}


void usb_joystick::sendReport(ps2_hid_packet* pkt) {
	USBD_HID_ReportIn(joy_if, (uint8_t*)pkt, sizeof(*pkt));
}