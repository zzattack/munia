#ifndef GAMECUBE_H
#define	GAMECUBE_H

#include <stdint.h>
#include <stdbool.h>

typedef struct {
    uint8_t a : 1;
    uint8_t b : 1;
    uint8_t x : 1;
    uint8_t y : 1;
    uint8_t start : 1;
    uint8_t zero1 : 1;
    uint8_t zero2 : 1;
    uint8_t zero3 : 1;
    
    union {
        struct {
            uint8_t dleft : 1;
            uint8_t dright : 1;
            uint8_t ddown : 1;
            uint8_t dup : 1;
            uint8_t z : 1;
            uint8_t r : 1;
            uint8_t l : 1;
            uint8_t one : 1;
        };
        struct {
            uint8_t hat : 4;
            uint8_t pad : 4;
        };
    };
    
    int8_t joy_x;
    int8_t joy_y;
    int8_t c_x;
    int8_t c_y;
    uint8_t left_trig;
    uint8_t right_trig;
} ngc_packet_t;

ngc_packet_t joydata_ngc @ 0x500;
bool ngc_test_packet = false;
bool ngc_fake_packet_available = false;
ngc_packet_t ngc_fake_packet;
uint8_t ngc_fake_buffer[64];
uint8_t* ngc_outbuf = 0;

void ngc_tasks();
void ngc_fake(); 
void ngc_real();
void ngc_poll();
void ngc_sample();
void ngc_handle_packet();
void ngc_fake_unpack();
#endif	/* GAMECUBE_H */

