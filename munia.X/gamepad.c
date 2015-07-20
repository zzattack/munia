#include "gamepad.h"
#include <xc.h>
#include <stdint.h>

void HIGH() { 
    CLR(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    SET(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    Nop(); 
}
void LOW() {
    CLR(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    SET(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    Nop();  
}
