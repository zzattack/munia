#ifndef SNES_H
#define	SNES_H

#include <stdint.h>
#include <stdbool.h>

typedef struct {
    union {
        struct {
            uint8_t right : 1;
            uint8_t left : 1;
            uint8_t down : 1;
            uint8_t up : 1;
            uint8_t start : 1;
            uint8_t select : 1;
            uint8_t y : 1;
            uint8_t b : 1;
        };
        struct {
            uint8_t dpad : 4;     
            uint8_t start2 : 1;
            uint8_t select2 : 1;
            uint8_t y2 : 1;
            uint8_t b2 : 1;
        };
    };
    uint8_t unused : 4;
    uint8_t r : 1;
    uint8_t l : 1;
    uint8_t x : 1;
    uint8_t a : 1;

} snes_packet_t;

snes_packet_t joydata_snes_usb @ 0x50D /*+ sizeof(n64_packet_t)*/;
snes_packet_t joydata_snes_raw;

void snes_tasks();
void snes_fake();
void snes_real();
void snes_poll();
extern void _snes_sample();
void snes_handle_packet();
void snes_joydata_createhid();

#endif	/* SNES_H */

