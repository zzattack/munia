#include "crc.h"

EXTERNC uint8_t crc_xor(const uint8_t* data, uint16_t start, uint16_t length) {
    uint8_t x = 0;
    for (uint16_t i = start; i < start + length; i++)
        x ^= data[i];
    
    return x;
}

EXTERNC uint8_t crc_sum(const uint8_t* data, uint16_t start, uint16_t length) {
    uint8_t x = 0;
    for (uint16_t i = start; i < start + length; i++)
        x -= data[i];
    
    return x;
}

EXTERNC uint8_t crc8(const uint8_t* data, uint16_t start, uint16_t length) {
	const uint8_t Poly = 0xe0;
	const uint8_t StartCRC = 0xff;
	const uint8_t FinalCRC =  0xff;
    
    uint8_t crc = StartCRC;
    for (uint16_t i = start; i < start + length; i++) {
        uint8_t inbyte = data[i];
        for (uint8_t j = 0; j < 8; j++) {
            uint8_t bt = inbyte & 0x01;
            uint8_t lsb = crc & 0x01;
            crc >>= 1;
            if ((bt ^ lsb) == 0x01)
                crc ^= Poly;
            inbyte >>= 1;
        }
    }
    crc ^= FinalCRC;
    return crc;
}

EXTERNC uint16_t crc16(const uint8_t* data, uint16_t start, uint16_t length) {
	const uint16_t poly  =  0x8408;
	const uint16_t initCRC = 0xFFFF;
	const uint16_t finalCRC = 0xFFFF;

	uint16_t crc = initCRC;
	for (uint16_t i = start; i < start + length; i++) {
		uint8_t b = data[i];
		for (uint8_t j = 0; j < 8; j++) {
			uint8_t bt = b & 0x01;
			uint8_t lsb = crc & 0x01;
			crc >>= 1;
			if ((bt ^ lsb) == 0x01) {
				crc ^= poly;
			}
			b >>= 1;
		}
	}
	crc ^= finalCRC;
	return crc;
}

EXTERNC uint32_t crc32(const uint8_t* data, uint16_t start, uint16_t length) {
	const uint32_t poly = 0xEDB88320;
	const uint32_t initCRC = 0xFFFFFFFF;
	const uint32_t finalCRC = 0xFFFFFFFF;

	uint32_t crc = initCRC;
	for (uint16_t i = start; i < start + length; i++) {
		crc ^= data[i];
		for (int j = 7; j >= 0; j--) {    // Do eight times.
			uint32_t mask = -(crc & 1);
			crc = (crc >> 1) ^ (poly & mask);
		}
	}
	crc ^= finalCRC;
	return crc;
}
