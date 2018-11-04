#include <xc.h>
#include "memory.h"

void ee_write(uint16_t bAdd, uint8_t bData) {
    EEADR = bAdd;
    EEDATA = bData;
    EECON1bits.EEPGD = 0;
    EECON1bits.CFGS = 0;
    EECON1bits.WREN = 1;
    di();
    EECON2 = 0x55;
    EECON2 = 0xAA;
    EECON1bits.WR = 1;
    // Wait for write to complete
    while (EECON1bits.WR);
    EECON1bits.WREN = 0;
    ei();
}

uint8_t ee_read(uint8_t bAdd) {
    EECON1 = 0;
    EEADR = bAdd;
    EECON1bits.RD = 1;
    NOP();  // NOPs may be required for latency at high frequencies
    NOP();
    return EEDATA;
}