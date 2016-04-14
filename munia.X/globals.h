#ifndef GLOBALS_H
#define	GLOBALS_H

#include <stdint.h>
#include <usb/usb_device.h>

#define USB_READY (USBDeviceState >= CONFIGURED_STATE && !USBSuspendControl)

bool pollNeeded = false;
bool snes_packet_available, n64_packet_available,  ngc_packet_available;

extern void load_config();
extern void save_config();
extern void apply_config();
        
USB_HANDLE USBInHandleSNES = 0;
USB_HANDLE USBInHandleN64 = 0;
USB_HANDLE USBInHandleNGC = 0;

#endif	/* GLOBALS_H */