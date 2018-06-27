#pragma once

#include <stdint.h>
#include <stdbool.h>

struct ps2_packet {
	bool isNew;
	uint8_t pktLength;
	uint8_t cmd[32];
	uint8_t data[32];
};
