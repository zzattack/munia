#ifndef _EASYTIMER_H
#define _EASYTIMER_H

#include <stdint.h>
#include "hardware.h" // for possible config

volatile uint8_t EasyTimerTick;
#ifdef WITH_EASYTIMER_2KHZ
void tick2000HzInternal();
extern void tickTimer2000Hz();
#endif
void tick1000HzInternal();
extern void tickTimer1000Hz();
extern void tickTimer100Hz();
extern void tickTimer10Hz();
extern void tickTimer1Hz();
#ifdef WITH_EASYTIMER_2HZ
extern void tickTimer2Hz();
#endif


#endif