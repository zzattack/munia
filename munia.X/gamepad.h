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

const uint8_t hat_lookup_n64_snes[16] = {
    // l and r are swapped compared to ngc
    HAT_SWITCH_NULL,       // 0b0000, centered
    HAT_SWITCH_EAST,       // 0b0001, right
    HAT_SWITCH_WEST,       // 0b0010, left
    HAT_SWITCH_NULL,       // 0b0011, left+right, not possible
    HAT_SWITCH_SOUTH,      // 0b0100, down
    HAT_SWITCH_SOUTH_EAST, // 0b0101, down+right
    HAT_SWITCH_SOUTH_WEST, // 0b0110, down+left
    HAT_SWITCH_NULL,       // 0b0111, // 3 at once
    HAT_SWITCH_NORTH, // 0b1000, up
    HAT_SWITCH_NORTH_EAST, // 0b1001, up+right
    HAT_SWITCH_NORTH_WEST, // 0b1010, up+left
    HAT_SWITCH_NULL, // 0b1011, // 3 at once
    HAT_SWITCH_NULL, // 0b1100, up+down, not possible
    HAT_SWITCH_NULL, // 0b1101, // 3 at once
    HAT_SWITCH_NULL, // 0b1110, // 3 at once
    HAT_SWITCH_NULL, // 0b1111, // 3 at once
};

typedef struct {
    uint8_t ngc_test : 1;
    uint8_t n64_test : 1;
    uint8_t snes_test : 1;
    uint8_t ngc_avail : 1;
    uint8_t n64_avail : 1;
    uint8_t snes_avail : 1;
    uint8_t ngc_fake_avail : 1;
} packet_state_t;

volatile uint8_t bit_count @ 0x002;
volatile packet_state_t packets @ 0x006;
volatile uint8_t sample_buff[256] @ 0x700 = {0}; // reserved memory where samples may be written
volatile uint8_t portc_mask @ 0x007;
volatile uint8_t* fsr_backup @ 0x008;
volatile uint8_t* sample_w @ 0x010;

#define CLR() TRISC &= ~portc_mask; // NEVER CALL THIS WHEN CORRESPONDING LATC IS HIGH! WE SHOULD NOT DRIVE THIS LINE
#define SET() TRISC |= portc_mask;
void HIGH();
void LOW();
#endif	/* GAMEPAD_H */

