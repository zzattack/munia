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

// ngc report: <left><right><down>up>
const uint8_t hat_lookup[] = {
    HAT_SWITCH_NULL,       // 0b0000, centered
    HAT_SWITCH_WEST,       // 0b0001, left
    HAT_SWITCH_EAST,       // 0b0010, right
    HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
    HAT_SWITCH_SOUTH,      // 0b0100, down
    HAT_SWITCH_SOUTH_WEST, // 0b0101, down+left
    HAT_SWITCH_SOUTH_EAST, // 0b0110, down+right
    HAT_SWITCH_NULL,       // 0b0111, // 3 at once
    HAT_SWITCH_NORTH, // 0b1000, up
    HAT_SWITCH_NORTH_WEST, // 0b1001, up+left
    HAT_SWITCH_NORTH_EAST, // 0b1010, up+right
    HAT_SWITCH_NULL, // 0b1011, // 3 at once
    HAT_SWITCH_NULL, // 0b1100, up+down, not possible
    HAT_SWITCH_NULL, // 0b1101, // 3 at once
    HAT_SWITCH_NULL, // 0b1110, // 3 at once
    HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

void portc_sample();
void portc_poll();

uint8_t sample_buff[90] = {0}; // reserved memory where samples may be written
uint8_t* sample_w = sample_buff;
uint8_t portc_mask;

#define CLR() TRISC &= ~portc_mask; // NEVER CALL THIS WHEN CORRESPONDING LATC IS HIGH! WE SHOULD NOT DRIVE THIS LINE
#define SET() TRISC |= portc_mask;
#define HIGH() CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
#define LOW() CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 

#define n64_poll() do { portc_mask = 0b00000010; portc_poll(); } while (0);
#define ngc_poll() do { portc_mask = 0b00000001; portc_poll(); } while (0);
#define n64_sample() do { portc_mask = 0b00000010; portc_sample(); n64_test_packet = TRUE; } while (0);
#define ngc_sample() do { portc_mask = 0b00000001; portc_sample(); ngc_test_packet = TRUE; } while (0);

#endif	/* GAMEPAD_H */

