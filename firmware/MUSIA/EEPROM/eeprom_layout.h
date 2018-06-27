#pragma once

#include <stdint.h>

#define EEPROM_SIZE sizeof(EELayout)


enum class musia_mode { sniffer = 0, poller };

typedef struct __attribute__((__packed__)) {
	musia_mode mode;
	bool allowVibrate;
	uint8_t pollFreq;
} EELayout;

const EELayout FactoryEEPROM = {
	.mode =  musia_mode::sniffer,
	.allowVibrate = true,
	.pollFreq = 60,
};
