#ifndef N64_H
#define	N64_H

#include <stdint.h>
#include <stdbool.h>
#include "gamecube.h"

typedef struct {
    union {
        struct {
            uint8_t dright : 1;
            uint8_t dleft : 1;
            uint8_t ddown : 1;
            uint8_t dup : 1;
            uint8_t start : 1;
            uint8_t z : 1;
            uint8_t b : 1;
            uint8_t a : 1;
        };
        struct{
            uint8_t dpad : 4;
            uint8_t __buttons1 : 4;
        };
    };
    
    uint8_t cright : 1;
    uint8_t cleft : 1;
    uint8_t cdown : 1;
    uint8_t cup : 1;
    uint8_t r : 1;
    uint8_t l : 1;
    uint8_t __unused2 : 1;
    uint8_t __unused1 : 1; // seems to be an alternative for start
    int8_t joy_x;    
    int8_t joy_y;
} n64_packet_t;

n64_packet_t joydata_n64 @ 0x508; // (0x500 + sizeof(ngc_packet_t));
n64_packet_t joydata_n64_raw;
void n64_tasks();
void n64_fake();
void n64_real(); 
void n64_poll();
extern void n64_sample() @ 0x3f90;
void n64_handle_packet();

#endif	/* N64_H */
