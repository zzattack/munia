#pragma once

#include <stdint.h>
#include <stdbool.h>

#define EEPROM_SIZE sizeof(EELayout)

typedef enum { MODE_SNIFFER = 0, MODE_POLLER, MODE_INVALID = 0xFF } musia_mode;

typedef struct __attribute__((__packed__)) {
	musia_mode mode;
	bool allowVibrate;
	uint8_t pollFreq;
} EELayout;

const EELayout FactoryEEPROM = {
	.mode = MODE_SNIFFER,
	.allowVibrate = true,
	.pollFreq = 60,
};
