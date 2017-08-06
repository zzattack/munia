#include "gamepad.h"
#include "asm_decl.h"
#include <xc.h>

void HIGH() { 
    CLR();
    _delay(8);
    SET(); 
    _delay(32);
}
void LOW() {
    CLR(); 
    _delay(32);
    SET(); 
    _delay(8);
}

uint8_t pack_byte(int8_t* r) {
    uint8_t x = 0;
    for (uint8_t m = 0x80; m; m >>= 1) {
        if (*r++ >= 0)
           x |= m;
    }            
    return x;
}