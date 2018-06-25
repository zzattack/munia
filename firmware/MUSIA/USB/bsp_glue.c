#include <usbd_types.h>
#include <xpd_usb_wrapper.h>

extern USB_HandleType *const UsbDevice;

extern void USB_vIRQHandler(void* h);
void USB_vClearRemoteWakeup(USB_HandleType * pxUSB);

void USB_IRQHandler() {
	// clear flag
	USB_vClearRemoteWakeup(UsbDevice);
	// handle interrupts
	USB_vIRQHandler(UsbDevice);
}
