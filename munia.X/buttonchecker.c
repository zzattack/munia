/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "buttonchecker.h"

#define STATE_PRESSED 1
#define STATE_RELEASED 0

void bcInit() {
    for (uint8_t i = 0; i < BUTTON_COUNT; i++) {
        bcPressed[i] = false;
        bcState[i].downTime = bcState[i].upTime = STATE_RELEASED;
    }
}

bool bcCheck() {
    bool result = false;
    bool button[BUTTON_COUNT];
    
#if BUTTON_COUNT >= 1
    button[0] = BUTTON_0;
#endif
#if BUTTON_COUNT >= 2
    button[1] = BUTTON_1;
#endif
#if BUTTON_COUNT >= 3
    button[2] = BUTTON_2;
#endif
#if BUTTON_COUNT >= 4
    button[3] = BUTTON_3;
#endif
#if BUTTON_COUNT >= 5
    button[4] = BUTTON_4;
#endif
#if BUTTON_COUNT >= 6
    button[5] = BUTTON_5;
#endif
#if BUTTON_COUNT >= 7
    button[6] = BUTTON_6;
#endif
#if BUTTON_COUNT >= 8
    button[7] = BUTTON_7;
#endif
#if BUTTON_COUNT >= 9
    button[8] = BUTTON_8;
#endif
#if BUTTON_COUNT >= 10
    button[9] = BUTTON_9;
#endif

    for (int i = 0; i < BUTTON_COUNT; i++) {
        // Button release
        if (button[i] == STATE_RELEASED && bcState[i].state == STATE_PRESSED && bcState[i].downTime > 5) {
            bcState[i].upTime = 0;
        }

        // Button press
        if (button[i] == STATE_PRESSED && bcState[i].state == STATE_RELEASED && bcState[i].upTime > 5) {
            bcState[i].downTime = 0;
            bcState[i].repeat = 0;
        }

        // Button pressed
        if (button[i] == STATE_PRESSED && bcState[i].downTime == 5 && bcState[i].upTime > 5) {
            bcPressed[i] = true;
            result = true;
        }

        bcState[i].state = button[i];

        bcState[i].downTime++;
        if (bcState[i].downTime == 75) {
            bcState[i].downTime = 0;
            bcState[i].repeat = 1;
        }

        if (bcState[i].upTime != 250)
            bcState[i].upTime++;
    }

    return result;
}