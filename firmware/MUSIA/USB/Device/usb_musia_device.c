#include "usb_musia_device.h"
#include <usbd.h>
#include <usbd_types.h>
#include <usbd_hid.h>

extern USBD_HandleType *const UsbDevice;
extern USBD_HID_IfHandleType *const joy_if;
extern USBD_HID_IfHandleType *const cfg_if;

#define DEVICE_ID_REG        ((const uint32_t *)UID_BASE)

/** @brief USB device configuration */
const USBD_DescriptionType hdev_cfg = {
	.Vendor = {
		.Name = "munia.io",
		.ID = 0x1209,
	},
	.Product = {
		.Name = "MUSIA",
		.ID = 0x8844,
		.Version.bcd = 0x010B,
	},
	.SerialNumber = (USBD_SerialNumberType*)DEVICE_ID_REG,
	.Config = {
		.Name = "MUSIA",
		.MaxCurrent_mA = 500,
		.RemoteWakeup = 0,
		.SelfPowered = 1,
	},
}, *const dev_cfg = &hdev_cfg;

USBD_HandleType hUsbDevice, *const UsbDevice = &hUsbDevice;

/**
 * @brief This function handles the setup of the USB device:
 *         - Assigns endpoints to USB interfaces
 *         - Mounts the interfaces on the device
 *         - Sets up the USB device
 *         - Determines the USB port type
 *         - Establishes logical connection with the host
 */
void UsbDevice_Init() {
	/* Initialize the device */
	USBD_Init(UsbDevice, dev_cfg);

	/* All fields of Config have to be properly set up */
	joy_if->Config.InEp.Num = 0x81;
	cfg_if->Config.InEp.Num = 0x82;
	cfg_if->Config.OutEp.Num = 0x02;


	// Mount the interfaces to the device
	USBD_HID_MountInterface(joy_if, UsbDevice);
	USBD_HID_MountInterface(cfg_if, UsbDevice);

	// After charger detection the device connection can be made
	USBD_Connect(UsbDevice);
}

void UsbDevice_Deinit() {
	USBD_Deinit(UsbDevice);
}
