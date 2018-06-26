#pragma once

#include <stdint.h>

EXTERNC uint8_t  crc_xor(const uint8_t* data, uint16_t start, uint16_t length);
EXTERNC uint8_t  crc_sum(const uint8_t* data, uint16_t start, uint16_t length);
EXTERNC uint8_t  crc8(const uint8_t* data, uint16_t start, uint16_t length);
EXTERNC uint16_t crc16(const uint8_t* data, uint16_t start, uint16_t length);
EXTERNC uint32_t crc32(const uint8_t* data, uint16_t start, uint16_t length);