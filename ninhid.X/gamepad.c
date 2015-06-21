#include "gamepad.h"
#include <xc.h>

void portc_sample() {
    // *sample_w = PORTC; // total 4 instr, samples at 4th
    sample_w++; //
    while (!PORTCbits.RC0);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;
    LATA &= 0b11111110;
loop:
    if (INTCONbits.TMR0IF) 
        return;

    if (PORTCbits.RC0) goto loop;

    // getting from here to the point where PORTC is read takes 12 instructions,
    // so waste the other 12:
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 

    *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry
    sample_w++;

    while (!PORTCbits.RC0);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;
    goto loop;
}
    
void portc_poll() {
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    CLR(); // set data pin to output, making the pin low
    // send 01000000
    //      00000011    
    //      00000010
    LOW(); HIGH(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); HIGH(); HIGH(); 
    LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); LOW(); 
    
    // stop bit, 2 us
    CLR(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
}