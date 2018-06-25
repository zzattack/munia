#pragma once

#include <stdint.h>

typedef struct {
	uint8_t data[8];	
} cfg_read_report_t;

typedef struct {
	uint8_t data[8];	
} cfg_write_report_t;
