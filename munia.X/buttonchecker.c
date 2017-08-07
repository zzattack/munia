/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "buttonchecker.h"
#include <xc.h>

void bcInit() {
    
#if BUTTON_COUNT >= 1
    bcState[0].debouncedState = BUTTON_0;
#endif
#if BUTTON_COUNT >= 2
    bcState[1].debouncedState = BUTTON_1;
#endif
#if BUTTON_COUNT >= 3
    bcState[2].debouncedState = BUTTON_2;
#endif
#if BUTTON_COUNT >= 4
    bcState[3].debouncedState = BUTTON_3;
#endif
#if BUTTON_COUNT >= 5
    bcState[4].debouncedState = BUTTON_4;
#endif
#if BUTTON_COUNT >= 6
    bcState[5].debouncedState = BUTTON_5;
#endif
#if BUTTON_COUNT >= 7
    bcState[6].debouncedState = BUTTON_6;
#endif
#if BUTTON_COUNT >= 8
    bcState[7].debouncedState = BUTTON_7;
#endif
#if BUTTON_COUNT >= 9
    bcState[8].debouncedState = BUTTON_8;
#endif
#if BUTTON_COUNT >= 10
    bcState[9].debouncedState = BUTTON_9;
#endif
#if BUTTON_COUNT >= 11
    bcState[10].debouncedState = BUTTON_10;
#endif
#if BUTTON_COUNT >= 12
    bcState[11].debouncedState = BUTTON_11;
#endif
#if BUTTON_COUNT >= 13
    bcState[12].debouncedState = BUTTON_12;
#endif
#if BUTTON_COUNT >= 14
    bcState[13].debouncedState = BUTTON_13;
#endif
#if BUTTON_COUNT >= 15
    bcState[14].debouncedState = BUTTON_14;
#endif
#if BUTTON_COUNT >= 16
    bcState[15].debouncedState = BUTTON_15;
#endif
#if BUTTON_COUNT >= 17
    bcState[16].debouncedState = BUTTON_16;
#endif
#if BUTTON_COUNT >= 18
    bcState[17].debouncedState = BUTTON_17;
#endif
#if BUTTON_COUNT >= 19
    bcState[18].debouncedState = BUTTON_18;
#endif
#if BUTTON_COUNT >= 20
    bcState[19].debouncedState = BUTTON_19;
#endif
    
    for (uint8_t i = 0; i < BUTTON_COUNT; i++) {
        bcState[i].count = BC_PRESS_MSEC;
        bcState[i].repeatCount = BC_REPEAT_MSEC;
        bcState[i].tick = 0;
        bcState[i].repeat = 0;
    }
}

bool bcCheck() {
    bool result = false;
    bool raw_button[BUTTON_COUNT];
    
#if BUTTON_COUNT >= 1
    raw_button[0] = BUTTON_0;
#endif
#if BUTTON_COUNT >= 2
    raw_button[1] = BUTTON_1;
#endif
#if BUTTON_COUNT >= 3
    raw_button[2] = BUTTON_2;
#endif
#if BUTTON_COUNT >= 4
    raw_button[3] = BUTTON_3;
#endif
#if BUTTON_COUNT >= 5
    raw_button[4] = BUTTON_4;
#endif
#if BUTTON_COUNT >= 6
    raw_button[5] = BUTTON_5;
#endif
#if BUTTON_COUNT >= 7
    raw_button[6] = BUTTON_6;
#endif
#if BUTTON_COUNT >= 8
    raw_button[7] = BUTTON_7;
#endif
#if BUTTON_COUNT >= 9
    raw_button[8] = BUTTON_8;
#endif
#if BUTTON_COUNT >= 10
    raw_button[9] = BUTTON_9;
#endif
#if BUTTON_COUNT >= 11
    raw_button[10] = BUTTON_10;
#endif
#if BUTTON_COUNT >= 12
    raw_button[11] = BUTTON_11;
#endif
#if BUTTON_COUNT >= 13
    raw_button[12] = BUTTON_12;
#endif
#if BUTTON_COUNT >= 14
    raw_button[13] = BUTTON_13;
#endif
#if BUTTON_COUNT >= 15
    raw_button[14] = BUTTON_14;
#endif
#if BUTTON_COUNT >= 16
    raw_button[15] = BUTTON_15;
#endif
#if BUTTON_COUNT >= 17
    raw_button[16] = BUTTON_16;
#endif
#if BUTTON_COUNT >= 18
    raw_button[17] = BUTTON_17;
#endif
#if BUTTON_COUNT >= 19
    raw_button[18] = BUTTON_18;
#endif
#if BUTTON_COUNT >= 20
    raw_button[19] = BUTTON_19;
#endif
    
    for (int i = 0; i < BUTTON_COUNT; i++) {
        // no change
        if (raw_button[i] == bcState[i].debouncedState) {
            // (re)set the timer which allows a change from the current state

            if (raw_button[i]) {
                bcState[i].count = BC_RELEASE_MSEC;            
            
                // update repeat counter while pressed
                if (--bcState[i].repeatCount == 0 && BC_REPEAT_MSEC != 0) {
                    // dbgs("repeat @ "); dbgsval(i); dbgs("\n");
                    bcState[i].repeatCount = BC_REPEAT_MSEC;
                    bcState[i].repeat = 1;
                    result = true;
                }
            }
            
            else 
                bcState[i].count = BC_PRESS_MSEC;
        }
        else {
            // raw press state has changed, wait for new state to stabilize
            if (--bcState[i].count == 0) {            
                bcState[i].debouncedState = raw_button[i];
                bcState[i].tick = 1;
                bcState[i].repeatCount = BC_REPEAT_MSEC;
                bcState[i].repeat = 0;
                result = true;
                
                // and reset the timer
                if (bcState[i].debouncedState) bcState[i].count = BC_RELEASE_MSEC;
                else bcState[i].count = BC_PRESS_MSEC;
            }
        }
    }
    
    return result;
}

bool bcTick(uint8_t i) {
    bool ret = bcState[i].tick;
    bcState[i].tick = 0;
    return ret;
}
bool bcRepeat(uint8_t i) {
    bool ret = bcState[i].repeat;
    bcState[i].repeat = 0;
    return ret;
}

bool bcPressed(uint8_t i) {
    return bcState[i].debouncedState;
}