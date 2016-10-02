#include "gamepad.h"
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
