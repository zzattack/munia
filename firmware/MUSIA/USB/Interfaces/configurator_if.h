#pragma once

#define FW_MAJOR 0
#define FW_MINOR 9
#define HW_REVISION 1

#include <stdint.h>
#include "eeprom_layout.h"

// extern USBD_HID_IfHandleType *const configurator_if;

typedef struct __attribute__((__packed__)) {
	union {
		uint8_t data[8];	
		EELayout eeprom;
	};
} cfg_read_report_t;

typedef struct __attribute__((__packed__)) {
	union {
		uint8_t data[8];
		EELayout eeprom;
	};
} cfg_write_report_t;


typedef struct __attribute__((__packed__)) {
	union {
		uint8_t data[8];	
		struct {
			uint32_t device_code;
			uint8_t fw_ver_major : 4;
			uint8_t hw_rev : 4;
			uint8_t fw_ver_minor;
		};
	};
} cfg_info_report_t;
