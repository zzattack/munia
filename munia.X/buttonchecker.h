#ifndef BUTTONCHECKER_H
#define BUTTONCHECKER_H

#include <stdint.h>
#include <stdbool.h>
#include "hardware.h"       /* For the port mappings */

typedef struct {
    uint8_t downTime;
    uint8_t upTime;
    uint8_t state  : 1;
    uint8_t repeat : 1;
} ButtonState;

bool bcPressed[BUTTON_COUNT];
ButtonState bcState[BUTTON_COUNT];

/******************************************************************************/
/* Function Prototypes                                                        */
/******************************************************************************/
void bcInit();
bool bcCheck();        /* Check if a button is pressen (should be 100Hz) */

#endif	/* BUTTONCHECKER_H */

