#pragma once

#include <stdint.h>
// extern USBD_HID_IfHandleType *const ps2controller_if;

typedef struct {
	uint8_t data[8];	
} cfg_read_report_t;

typedef struct {
	uint8_t data[8];	
} cfg_write_report_t;
