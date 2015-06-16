#ifndef GAMEPADS_H
#define	GAMEPADS_H

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
} snes_packet;

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
} n64_packet;

typedef struct {
    uint8_t zero1 : 1;
    uint8_t zero2 : 1;
    uint8_t zero3 : 1;
    uint8_t start : 1;
    uint8_t y : 1;
    uint8_t x : 1;
    uint8_t b : 1;
    uint8_t a : 1;
    uint8_t one : 1;
    uint8_t l : 1;
    uint8_t r : 1;
    uint8_t z : 1;
    uint8_t dup : 1;
    uint8_t ddown : 1;
    uint8_t dright : 1;
    uint8_t dleft : 1;
    int8_t joy_x;
    int8_t joy_y;
    int8_t c_x;
    int8_t c_y;
    uint8_t left_trig;
    uint8_t right_trig;
} ngc_packet;

typedef struct {
	uint16_t buttons;
	int8_t left_x;
	int8_t left_y;
	int8_t right_x;
	int8_t right_y;
} wii_packet;


#endif	/* GAMEPADS_H */

