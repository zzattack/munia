#ifndef FAKEOUT_H
#define FAKEOUT_H

#include <stdint.h>
#include "gamepad.h"
#include "snes.h"
#include "n64.h"
#include "gamecube.h"

uint8_t fake_buffer[64];
uint8_t* pfake_out = 0;
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

// Packets below should not be stored in program memory but kept in RAM,
// otherwise timing requirements cannot be met in ngc_fakeout()

// controller sends these 24 bits when wii sends a 8-bit all low packet
uint8_t ngc_id_packet[] = { 
    0, 0, 0, 0, 1, 0, 0, 1, 
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 1, 1
};
// controller sends these 80 bits when wii sends a 8-bit packet with 2 bits high
uint8_t ngc_init_packet[] = {
    0, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 1, 0,
    1, 0, 0, 0, 0, 0, 0, 0,
    1, 0, 0, 0, 0, 0, 1, 1,
    1, 0, 0, 0, 1, 0, 0, 0,
    0, 0, 1, 0, 0, 0, 1, 1,
    0, 0, 1, 0, 1, 0, 1, 1,
    0, 0, 0, 0, 0, 0, 0, 1,
    0, 0, 0, 0, 0, 0, 0, 1, 1
};

// controller sends these 24 bits when wii sends a 8-bit all low packet
uint8_t n64_id_packet[] = { 
    0, 0, 0, 0, 1, 0, 0, 1, 
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 1, 0
};


#endif