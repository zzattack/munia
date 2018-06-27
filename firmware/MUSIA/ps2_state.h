#pragma once

#include <stdint.h>

enum class device_mode : uint8_t {
	digital = 0x04,
	analog = 0x07,
	config = 0x0f,
};

struct pressures_t {
	uint8_t	dpad_right,
			dpad_left,
			dpad_up,
			dpad_down,
			triangle,
			circle,
			cross,
			square,
			rx,
			ry,
			lx,
			ly,
			l1,
			r1,
			l2,
			r2;
	bool available = false;
};

class ps2_state {	
public:
	bool dpad_up, dpad_down, dpad_left, dpad_right;
	bool start, select;
	bool l1, l2, r1, r2;
	bool triangle, square, cross, circle;
	int8_t analog1_x, analog1_y;
	int8_t analog2_x, analog2_y;
	bool l_stick, r_stick;
	pressures_t pressures;
	
	bool update(uint8_t* cmd, uint8_t* data, uint8_t pkt_len);
	bool updatePoll(uint8_t* data, int len);
	bool updateConfig(uint8_t* cmd, uint8_t* data, int len);
	static void print_packet(uint8_t* cmd, uint8_t* data, uint8_t pkt_len);
};