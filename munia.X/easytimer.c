#include "easytimer.h"

#ifdef WITH_EASYTIMER_2KHZ
void tick2000HzInternal() {
    static uint8_t timer100hz = 0;
    static uint8_t timer10hz = 0;
    static uint16_t timer1hz = 0;

    tickTimer2000Hz();
    
    timer100hz++;
    if (timer100hz & 1) 
        tickTimer1000Hz();
    
    if (timer100hz >= 20) {
        tickTimer100Hz();
        timer100hz = 0;
        timer1hz++;
        timer10hz++;

        if (timer10hz >= 10) {
            tickTimer10Hz();
            timer10hz = 0;
        }
        
        
#ifdef WITH_EASYTIMER_2HZ
        if (timer1hz == 50) {
            tickTimer2Hz();
        }
#endif

        if (timer1hz >= 100) {
#ifdef WITH_EASYTIMER_2HZ
            tickTimer2Hz();
#endif
            tickTimer1Hz();
            timer1hz = 0;
        }
    }    
}
#endif


void tick1000HzInternal() {
    static uint8_t timer100hz = 0;
    static uint8_t timer10hz = 0;
    static uint16_t timer1hz = 0;

    tickTimer1000Hz();
    timer100hz++;
    if (timer100hz >= 10) {
        tickTimer100Hz();
        timer100hz = 0;
        timer1hz++;
        timer10hz++;

        if (timer10hz >= 10) {
            tickTimer10Hz();
            timer10hz = 0;
        }
        
        
#ifdef WITH_EASYTIMER_2HZ
        if (timer1hz == 50) {
            tickTimer2Hz();
        }
#endif

        if (timer1hz >= 100) {
#ifdef WITH_EASYTIMER_2HZ
            tickTimer2Hz();
#endif
            tickTimer1Hz();
            timer1hz = 0;
        }
    }
}