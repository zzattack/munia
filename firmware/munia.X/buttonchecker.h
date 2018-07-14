/*
 * Usage:
 * 
 *  1) include buttonchecker.h and buttonchecker.c in project
 *  2) call bcInit() during program initialization
 *  3) in hardware.h, define BUTTON_COUNT
 *       #define BUTTON_COUNT 2
 *  4) in hardware.h, define buttons
 *        #define BUTTON_0 PORTAbits.RA4
 *        #define BUTTON_1 PORTCbits.RC2
 *  5) in main.c, call bcCheck() at 10Hz from tickTimer10Hz()
 *  6) if bcCheck() returned true, check individual button changes with bcTick(i),
 *     and check whether pressed/unpressed with bcPressed(i)
 *  
*/

#ifndef BUTTONCHECKER_H
#define BUTTONCHECKER_H

#include "hardware.h"       /* For the port mappings */
#include <stdbool.h>
#include <stdint.h>

#ifndef BC_PRESS_MSEC
#define BC_PRESS_MSEC 40 // register press after 40 msec
#endif
#ifndef BC_RELEASE_MSEC
#define BC_RELEASE_MSEC 60 // register release after 60 msec
#endif
#ifndef BC_REPEAT_MSEC
#define BC_REPEAT_MSEC 1200 // register repeat after 1200 msec
#endif

typedef struct {
    uint8_t count;
    uint8_t repeatCount;
    uint8_t debouncedState : 1;
    uint8_t tick : 1;
    uint8_t repeat : 1;
} ButtonState;

ButtonState bcState;

/******************************************************************************/
/* Function Prototypes                                                        */
/******************************************************************************/
void bcInit();
bool bcCheck(); // Check if a button is pressed/unpressed call @ 1000Hz
bool bcTick();
bool bcRepeat();
bool bcPressed();

#endif	/* BUTTONCHECKER_H */