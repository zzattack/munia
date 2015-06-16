#include "config_bits.h"
#include "hardware.h"
#include "system_config.h"
#include <xc.h>
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

USB_HANDLE USBOutHandle1 = 0;
USB_HANDLE USBInHandle1 = 0;
USB_HANDLE USBOutHandle2 = 0;
USB_HANDLE USBInHandle2 = 0;
unsigned char usbOutBuffer1[16] @ 0x560;
unsigned char usbOutBuffer2[16] @ 0x580;

typedef struct _INTPUT_CONTROLS_TYPEDEF
{
    uint8_t val[7];
} INPUT_CONTROLS;
INPUT_CONTROLS joystick_input1 @ 0x500;
INPUT_CONTROLS joystick_input2 @ 0x540;

void init();

void main() {
    init();
	    
    usb_descriptors_check();
	USBDeviceInit();
#ifdef USB_INTERRUPT
    USBDeviceAttach();
#endif

    LED_SNES_GREEN = 0;
    LED_SNES_ORANGE = 0;
    LED_GC_ORANGE = 0;
    LED_GC_GREEN = 0;

    LED_SNES_GREEN = 1;
    LED_SNES_ORANGE = 1;
    LED_GC_ORANGE = 1;
    LED_GC_GREEN = 1;
    
	while (1) {
#ifdef USB_POLLING
        USBDeviceTasks();
#endif
        
		if (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)
			continue;
            
        if (!HIDTxHandleBusy(USBInHandle1)) {
            memset(&joystick_input1, 0x11, sizeof(joystick_input1));
            //Send the packet over USB to the host.
            USBInHandle1 = HIDTxPacket(HID_EP1, (uint8_t*)&joystick_input1, sizeof(joystick_input1));
        }  
        if (!HIDTxHandleBusy(USBInHandle2)) {
            memset(&joystick_input2, 0x22, sizeof(joystick_input2));
            //Send the packet over USB to the host.
            USBInHandle2 = HIDTxPacket(HID_EP2, (uint8_t*)&joystick_input2, sizeof(joystick_input2));
        }
    }
}

void init() {
	OSCTUNE = 0x80; // 3X PLL ratio mode selected
	OSCCON = 0x70;  // Switch to 16MHz HFINTOSC
	OSCCON2 = 0x10; // Enable PLL, SOSC, PRI OSC drivers turned off
	while (!OSCCON2bits.PLLRDY); // Wait for PLL lock
	ACTCON = 0x90;  // Enable active clock tuning for USB operation

    // analog ports
	ANSELA = 0b00000000; // all digital
	ANSELB = 0b00000000;
	ANSELC = 0b00000000;
    
    // io directions
    TRISA = 0b11000000; // RA0-4 for LCD, RA5 for led, RA6/7 snes LATCH/CLOCK
    TRISB = 0b00000000; // outputs for leds, LCD, fake signal, switches
    TRISC = 0b10000111; // usb sense, uart tx, snes/n64/gc input signals
    
    
	// cfg timer
	INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 0; // Timer0 is configured as an 16-bit timer/counter
	T0CONbits.T0CS = 0; // Internal instruction cycle clock (FOSC/4)
	T0CONbits.PSA = 0; // Timer0 prescaler is NOT assigned. Timer0 clock input bypasses prescaler
    T0CONbits.T0PS = 0b100;

	// setup interrupt on falling change of data on gamepad ports
	INTCONbits.IOCIE = 1; // IOC interrupt enabled
	INTCON2bits.IOCIP = 1; // High priority group
	IOCCbits.IOCC0 = 1; // enable IOC on RC0
	IOCCbits.IOCC1 = 1; // enable IOC on RC1
	IOCCbits.IOCC2 = 1; // enable IOC on RC2

	RCONbits.IPEN = 1; // Enable priority levels on interrupts
	INTCONbits.GIEH = 1; // Enable high priority interrupts
	INTCONbits.GIEL = 1; // Enable low priority interrupts
}


// ****************************************************************************
// ************** USB Callback Functions **************************************
// ****************************************************************************

void USBCBSuspend() { }
void USBCBWakeFromSuspend() { }
void USBCB_SOF_Handler() { }
void USBCBErrorHandler() { }
void USBCBStdSetDscHandler() { }

void USBCBCheckOtherReq() {
    USBCheckHIDRequest();
}

void USBCBSendResume() {
    static WORD delay_count;
    if (USBGetRemoteWakeupStatus() == TRUE) {
        //Verify that the USB bus is in fact suspended, before we send
        //remote wakeup signalling.
        if (USBIsBusSuspended() == TRUE) {
            USBMaskInterrupts();

            //Clock switch to settings consistent with normal USB operation.
            USBCBWakeFromSuspend();
            USBSuspendControl = 0;
            USBBusIsSuspended = FALSE; //So we don't execute this code again,
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

BOOL USER_USB_CALLBACK_EVENT_HANDLER(USB_EVENT event, void *pdata, WORD size) {
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
            USBEnableEndpoint(HID_EP1, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP2, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            // Re-arm the OUT endpoint for the next packet
            USBOutHandle1 = HIDRxPacket(HID_EP1, (BYTE*)&usbOutBuffer1, sizeof(usbOutBuffer1));
            USBOutHandle2 = HIDRxPacket(HID_EP2, (BYTE*)&usbOutBuffer2, sizeof(usbOutBuffer2));
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
    return TRUE;
}

