#ifndef GLOBALS_H
#define	GLOBALS_H

#include <stdint.h>
#include <GenericTypeDefs.h>
#include <usb/usb_device.h>

#define USB_READY (USBDeviceState >= CONFIGURED_STATE && !USBSuspendControl)

enum __mode { real, pc, fake_snes, fake_n64, fake_gc };
enum __sampleSource { snes, n64, ngc };
uint8_t sampleSource;
uint8_t snes_mode, n64_mode, ngc_mode;
bool in_menu = FALSE;
uint8_t lcd_backLightValue;
bool pollNeeded = FALSE;
bool snes_packet_available, n64_packet_available,  ngc_packet_available;

USB_HANDLE USBInHandleSNES = 0;
USB_HANDLE USBInHandleN64 = 0;
USB_HANDLE USBInHandleNGC = 0;

#endif	/* GLOBALS_H */