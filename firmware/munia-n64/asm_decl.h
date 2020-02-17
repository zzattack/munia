#ifndef ASM_DECL_H
#define	ASM_DECL_H

#include "gamepad.h"

volatile uint8_t WREGCPY        @ 0x002;
volatile uint8_t STATUSCPY      @ 0x003;
volatile uint8_t BSRCPY         @ 0x004;

volatile packet_state_t packets @ 0x005;
volatile uint8_t bit_count      @ 0x006;

volatile uint8_t* fsr_backup @ 0x007;
volatile uint8_t* sample_w @ 0x009;
near uint8_t portc_mask;

volatile uint8_t sample_buff[256] @ 0x700 = {0}; // reserved memory where samples may be written


#endif	/* ASM_DECL_H */

