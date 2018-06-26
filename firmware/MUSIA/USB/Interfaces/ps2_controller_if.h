#pragma once

#include <stdint.h>
// extern USBD_HID_IfHandleType *const ps2controller_if;

typedef struct {
	uint8_t report_id;
	uint8_t cross : 1;
	uint8_t square : 1;
	uint8_t circle : 1;
	uint8_t triangle : 1;
	
	uint8_t select : 1;
	uint8_t start : 1;
	uint8_t l_stick : 1;
	uint8_t r_stick : 1;
	
	union {
		struct {
			uint8_t l1 : 1;
			uint8_t r1 : 1;
			uint8_t l2 : 1;
			uint8_t r2 : 1;
			uint8_t dleft : 1;
			uint8_t dright : 1;
			uint8_t ddown : 1;
			uint8_t dup : 1;
		};
		struct {
			uint8_t face : 4;
			uint8_t hat : 4;
		};
	};
    
	uint8_t left_x;
	uint8_t left_y;
	uint8_t right_x;
	uint8_t right_y;
} ps2_hid_packet;
