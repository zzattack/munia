#ifndef UARTS_H
#define	UARTS_H

#include "hardware.h"
#include <stdint.h>
#include <stdbool.h>

void U1TxSync(char ch);
void U1putsSync(const char* msg);
void U1Init(uint32_t baudrate, bool read, bool write);
void U1SetBaudrate(uint32_t baudrate);
void U1Disable();

#if DEBUG
#define dbgb(x) U1TxSync(x)
#define dbgs(x) U1putsSync(x)
#define dbgsval(x) U1writeVal(x)
#define dbgsval32(x) U1writeVal32(x)
#define dbgsvalx(x) U1writeValHex(x)
#define dbgsvalf(x) U1writeValFloat(x)
#define dbgsvald(x) U1writeValDouble(x)
#else
#define dbgb(x) 
#define dbgs(x) 
#define dbgsval(x)
#define dbgsval32(x)
#define dbgsvalx(x)
#define dbgsvalf(x)
#define dbgsvald(x)
#endif

void U1writeVal(uint16_t val);
void U1writeVal32(uint32_t val);
void U1writeValHex(uint16_t val);
void U1writeValFloat(float val);
void U1writeValDouble(double val);

#endif	/* UARTS_H */

