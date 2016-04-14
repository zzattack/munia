#include <stdint.h>
#include <usb/usb.h>
#include <usb/usb_device_hid.h>

void USBCBSuspend() { }
void USBCBWakeFromSuspend() { }
void USBCB_SOF_Handler() { }
void USBCBErrorHandler() { }
void USBCBStdSetDscHandler() { }
void USBCBSendResume() {
    static uint16_t delay_count;
    if (USBGetRemoteWakeupStatus() == true) {
        //Verify that the USB bus is in fact suspended, before we send
        //remote wakeup signalling.
        if (USBIsBusSuspended() == true) {
            USBMaskInterrupts();

            //Clock switch to settings consistent with normal USB operation.
            USBCBWakeFromSuspend();
            USBSuspendControl = 0;
            USBBusIsSuspended = false; //So we don't execute this code again,
            //until a new suspend condition is detected.
            delay_count = 3600U;
            do {
                delay_count--;
            } while (delay_count);

            //Now drive the resume K-state signalling onto the USB bus.
            USBResumeControl = 1; // Start RESUME signaling
            delay_count = 1800U; // Set RESUME line for 1-13 ms
            do {
                delay_count--;
            } while (delay_count);
            USBResumeControl = 0; //Finished driving resume signalling

            USBUnmaskInterrupts();
        }
    }
}
bool USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, uint16_t size) {
    switch (event) {
        case EVENT_TRANSFER:
            //Add application specific callback task or callback function here if desired.
            break;
        case EVENT_SOF:
            USBCB_SOF_Handler();
            break;
        case EVENT_SUSPEND:
            USBCBSuspend();
            break;
        case EVENT_RESUME:
            USBCBWakeFromSuspend();
            break;
        case EVENT_CONFIGURED:
            // enable the HID endpoint
            USBEnableEndpoint(HID_EP_SNES, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP_N64, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP_NGC, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);        
			break;
        case EVENT_SET_DESCRIPTOR:
            USBCBStdSetDscHandler();
            break;
        case EVENT_EP0_REQUEST:
            USBCheckHIDRequest();
			break;
        case EVENT_BUS_ERROR:
            USBCBErrorHandler();
            break;
        case EVENT_TRANSFER_TERMINATED:
            break;
        default:
            break;
    }
    return true;
}

