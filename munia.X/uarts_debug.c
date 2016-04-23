#include "uarts.h"
#include <stdio.h>

// debug stuff
void U1writeVal(uint16_t val) {
    char buff[10];
    sprintf(buff, "%u", val);
    U1putsSync(buff);
}
void U1writeVal32(uint32_t val) {
    char buff[10];
    sprintf(buff, "%lu", val);
    U1putsSync(buff);
}
void U1writeValHex(uint16_t val) {
    char buff[10];
    sprintf(buff, "%x", val);
    U1putsSync(buff);
}
void U1writeValFloat(float val) {
    uint8_t buff[10];
    sprintf(buff, "%f", val);
    U1putsSync(buff);
}
void U1writeValDouble(double val) {
    uint8_t buff[10];
    sprintf(buff, "%.10f", val);
    U1putsSync(buff);
}
