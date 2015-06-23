#ifndef GAMEPAD_H
#define	GAMEPAD_H

#include <stdint.h>

//http://www.microsoft.com/whdc/archive/hidgame.mspx
#define HAT_SWITCH_NORTH            0x0
#define HAT_SWITCH_NORTH_EAST       0x1
#define HAT_SWITCH_EAST             0x2
#define HAT_SWITCH_SOUTH_EAST       0x3
#define HAT_SWITCH_SOUTH            0x4
#define HAT_SWITCH_SOUTH_WEST       0x5
#define HAT_SWITCH_WEST             0x6
#define HAT_SWITCH_NORTH_WEST       0x7
#define HAT_SWITCH_NULL             0x8


void portc_poll();

uint8_t sample_buff[128] @ 0x700 = {0}; // reserved memory where samples may be written
uint8_t* sample_w = sample_buff;
uint8_t portc_mask;

#define CLR() TRISC &= ~portc_mask; // NEVER CALL THIS WHEN CORRESPONDING LATC IS HIGH! WE SHOULD NOT DRIVE THIS LINE
#define SET() TRISC |= portc_mask;
#define HIGH() CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
#define LOW() CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 

#endif	/* GAMEPAD_H */

