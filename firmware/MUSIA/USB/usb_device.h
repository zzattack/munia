#pragma once

EXTERNC void UsbDevice_Init();
EXTERNC void UsbDevice_Deinit();

#define MUSIA_EPIN_SIZE						0x20
#define MUSIA_EPOUT_SIZE					0x20
	 
#define HID_INTF_PS2						0
#define HID_INTF_CFG						1

#define HID_EP_PS2							1
#define HID_EP_CFG							2

#define PS2_HID_EPIN_ADDR HID_EP_PS2 | 0x80
#define CFG_HID_EPIN_ADDR HID_EP_CFG | 0x80
#define CFG_HID_EPOUT_ADDR HID_EP_CFG | 0x00

#define HID_INT_IN_PS2_SIZE					32 // Valid Options: 8, 16, 32, or 64 bytes.
#define HID_INT_INOUT_CFG_SIZE				9 // one extra for report id