#ifndef SNES_H
#define	SNES_H

#include <stdint.h>

typedef struct {
    uint8_t a : 1;
    uint8_t b : 1;
    uint8_t x : 1;
    uint8_t y : 1;
    uint8_t start : 1;
    uint8_t select : 1;
    uint8_t l : 1;
    uint8_t r : 1;
    uint8_t up : 1;
    uint8_t down : 1;
    uint8_t left : 1;
    uint8_t right : 1;    
    uint8_t pad : 4;
} snes_packet_t;

snes_packet_t joydata_snes @ 0x500;

void snes_tasks();
void snes_fake();
void snes_real();
void snes_poll();
void snes_sample();
void snes_handle_packet();

#endif	/* SNES_H */

