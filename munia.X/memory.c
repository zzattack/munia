#include <xc.h>
#include "memory.h"

void DATAEE_WriteByte(uint16_t bAdd, uint8_t bData)
{
    di();
    EEADR = bAdd;
    EEDATA = bData;
    EECON1bits.EEPGD = 0;
    EECON1bits.CFGS = 0;
    EECON1bits.WREN = 1;
    INTCONbits.GIE = 0;     // Disable interrupts
    EECON2 = 0x55;
    EECON2 = 0xAA;
    EECON1bits.WR = 1;
    // Wait for write to complete
    while (EECON1bits.WR);
    EECON1bits.WREN = 0;
    ei();
}

uint8_t DATAEE_ReadByte(uint8_t bAdd)
{
    EECON1 = 0;
    EEADR = (bAdd & 0xFF);
    EECON1bits.RD = 1;
    NOP();  // NOPs may be required for latency at high frequencies
    NOP();
    return EEDATA;
}