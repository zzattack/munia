#ifndef GLOBALS_H
#define	GLOBALS_H

#include <stdint.h>
#include <usb/usb_device.h>
#include "./config.h"

#define FW_MAJOR 1
#define FW_MINOR 6

#define USB_READY (USBDeviceState >= CONFIGURED_STATE && !USBSuspendControl)

bool pollNeeded = false;
config_t config;
extern void save_config();
extern void apply_config();

USB_HANDLE USBInHandleNGC = 0;
USB_HANDLE USBOutHandleCfg = 0;
USB_HANDLE USBInHandleCfg = 0;

char usbOutBuffer[32] @ 0x512; // placed behind joydata_snes
char usbInBuffer[32] @ 0x532;

#endif	/* GLOBALS_H */