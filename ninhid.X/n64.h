#ifndef N64_H
#define	N64_H

#include <stdint.h>
#include <stdbool.h>

typedef struct {
    uint8_t a : 8;
    uint8_t b : 8;
    int8_t joy_x;    
    int8_t joy_y;
} n64_packet_t;

n64_packet_t joydata_n64 @ 0x510;
bool n64_test_packet = false;

void n64_tasks();
void n64_fake();
void n64_real(); 
void n64_poll();
void n64_handle_packet();



#endif	/* N64_H */

