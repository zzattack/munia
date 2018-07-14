/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "buttonchecker.h"
#include <xc.h>

void bcInit() {
    bcState.debouncedState = BUTTON_0;
    bcState.count = BC_PRESS_MSEC;
    bcState.repeatCount = BC_REPEAT_MSEC;
    bcState.tick = 0;
    bcState.repeat = 0;
}

bool bcCheck() {
    bool result = false;
    
    // no change
    if (BUTTON_0 == bcState.debouncedState) {
        // (re)set the timer which allows a change from the current state

        if (BUTTON_0) {
            bcState.count = BC_RELEASE_MSEC;            

            // update repeat counter while pressed
            if (--bcState.repeatCount == 0 && BC_REPEAT_MSEC != 0) {
                // dbgs("repeat @ "); dbgsval(i); dbgs("\n");
                bcState.repeatCount = BC_REPEAT_MSEC;
                bcState.repeat = 1;
                result = true;
            }
        }

        else 
            bcState.count = BC_PRESS_MSEC;
    }
    else {
        // raw press state has changed, wait for new state to stabilize
        if (--bcState.count == 0) {            
            bcState.debouncedState = BUTTON_0;
            bcState.tick = 1;
            bcState.repeatCount = BC_REPEAT_MSEC;
            bcState.repeat = 0;
            result = true;

            // and reset the timer
            if (bcState.debouncedState) bcState.count = BC_RELEASE_MSEC;
            else bcState.count = BC_PRESS_MSEC;
        }
    }
    
    return result;
}

bool bcTick() {
    bool ret = bcState.tick;
    bcState.tick = 0;
    return ret;
}
bool bcRepeat() {
    bool ret = bcState.repeat;
    bcState.repeat = 0;
    return ret;
}

bool bcPressed() {
    return bcState.debouncedState;
}