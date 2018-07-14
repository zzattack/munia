#ifndef FAKEOUT_H
#define FAKEOUT_H

#include <stdint.h>
#include "gamepad.h"
#include "snes.h"
#include "n64.h"
#include "gamecube.h"

uint8_t fake_buffer[96];
uint8_t fake_count = 0;
void fake_unpack(uint8_t* r, uint8_t n);

void snes_to_ngc();
void snes_to_n64();
void n64_to_ngc();
void n64_to_snes();
void ngc_to_n64();
void ngc_to_snes();

void ngc_fakeout_test();
void n64_fakeout_test();
void snes_fakeout_test();
void fakeout_ngc64();

#define ngc_fakeout() do { portc_mask = 0b00000001; fakeout_ngc64(); } while (0);
#define n64_fakeout() do { portc_mask = 0b00000010; fakeout_ngc64(); } while (0);

// Packets below should not be stored in program memory but kept in RAM,
// otherwise timing requirements cannot be met in ngc_fakeout()

// controller sends these 24 bits when wii sends a 8-bit all low packet
uint8_t ngc_id_packet[] = { 
    0b00001001, 
    0b00000000,
    0b00000011
};

// controller sends these 24 bits when wii sends a 8-bit all low packet
uint8_t n64_id_packet[] = { 
    0b00001001,
    0b00000000,
    0b00000010
};

// controller sends these 80 bits when wii sends a 8-bit packet with 2 bits high
uint8_t ngc_init_packet[] = {
    0b00000000,
    0b10000000,
    0b10000010,
    0b10000000,
    0b10000011,
    0b10001000,
    0b00100011,
    0b00101011,
    0b00000001,
    0b00000001
};



#endif