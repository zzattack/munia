#include "config_bits.h"
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include <stdint.h>
#include <GenericTypeDefs.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <xc.h>
#include "xlcd.h"

void init();
void sample();
void handle_packet_n64();
void handle_packet_gc();
void handle_packet_snes();
void (*packet_handler)();

void USBCheck_GetDescriptor_SerialNumber();

#define N64DAT PORTCbits.RC0
#define GCDAT PORTCbits.RC1
#define SNESDAT PORTCbits.RC2
#define SCOPE_TEAL LATAbits.LATA5

BYTE usbOutBuffer[HID_INT_OUT_EP_SIZE];
BYTE usbInBuffer[HID_INT_IN_EP_SIZE];
USB_HANDLE USBInHandle = 0;

BOOL packet_available = FALSE;
BYTE packet[8];

BYTE buff[90] = {0};
BYTE* w = buff;

typedef union _INTPUT_CONTROLS_TYPEDEF {
    struct {
        struct         {
            uint8_t square:1;
            uint8_t x:1;
            uint8_t o:1;
            uint8_t triangle:1;
            uint8_t L1:1;
            uint8_t R1:1;
            uint8_t L2:1;
            uint8_t R2:1;//
            uint8_t select:1;
            uint8_t start:1;
            uint8_t left_stick:1;
            uint8_t right_stick:1;
            uint8_t home:1;
            uint8_t :3;    //filler
        } buttons;
        struct        {
            uint8_t hat_switch:4;
            uint8_t :4;//filler
        } hat_switch;
        struct        {
            uint8_t X;
            uint8_t Y;
            uint8_t Z;
            uint8_t Rz;
        } analog_stick;
    } members;
    uint8_t val[7];
} INPUT_CONTROLS;

INPUT_CONTROLS joystick_input;

void sample_packet();

void init() {
	TRISA = 0xFF;
	TRISC = 0xFF;
    TRISCbits.TRISC0 = 1; // N64
    TRISCbits.TRISC1 = 1; // GC
    TRISCbits.TRISC2 = 1; // SNES
    TRISAbits.TRISA5 = 0; // scope teal

	ANSELA = 0b00000000; // all digital
	ANSELB = 0b00000000;
	ANSELC = 0b00000000;
	
	OSCTUNE = 0x80; // 3X PLL ratio mode selected
	OSCCON = 0x70;  // Switch to 16MHz HFINTOSC
	OSCCON2 = 0x10; // Enable PLL, SOSC, PRI OSC drivers turned off
	while (!OSCCON2bits.PLLRDY); // Wait for PLL lock
	ACTCON = 0x90;  // Enable active clock tuning for USB operation
}

void switch_operation_mode() {    
	// cfg timer
	INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 1; // Timer0 is configured as an 8-bit timer/counter
	T0CONbits.T0CS = 0; // Internal instruction cycle clock (FOSC/4)
	T0CONbits.PSA = 1; // TImer0 prescaler is NOT assigned. Timer0 clock input bypasses prescaler

	// setup interrupt on falling change of data on gamepad ports
	INTCONbits.IOCIE = 1; // IOC interrupt enabled
	INTCON2bits.IOCIP = 1; // High priority group
	// IOCCbits.IOCC0 = 1; // enable IOC on RC0
	IOCCbits.IOCC1 = 1; // enable IOC on RC1
	// IOCCbits.IOCC2 = 1; // enable IOC on RC2

	RCONbits.IPEN = 1; // Enable priority levels on interrupts
	INTCONbits.GIEH = 1; // Enable high priority interrupts
	INTCONbits.GIEL = 1; // Enable low priority interrupts
}

void DelayFor18TCY() {
    __delay_us(18);
}

void DelayPORXLCD() {
     __delay_ms(15);
}

void DelayXLCD() {
    __delay_ms(5);
}

void main() {
    init();
	
    // PWM pin
    TRISBbits.RB5 = 0;
    LATBbits.LATB5 = 1;
    
    OpenXLCD(FOUR_BIT & 0b00111000 & CURSOR_ON & BLINK_ON);
    while(1) {
        WriteCmdXLCD(SHIFT_DISP_LEFT);
        putrsXLCD("somssomssomssoms");
    }


	USBDeviceInit();

	while (1) {
		USBDeviceTasks();

		if (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)
			continue;

		if (packet_available && !HIDTxHandleBusy(USBInHandle)) {
			usbInBuffer[0] = 36;
			usbInBuffer[1] = 4;
			memcpy(usbInBuffer + 2, packet, 8);
			USBInHandle = HIDTxPacket(JOYSTICK_EP, (uint8_t*)&joystick_input, sizeof(joystick_input));
			packet_available = FALSE;
		}
		SCOPE_TEAL = packet_available;

	}
}

void high_priority interrupt isr_high() {
	 // interrupt latency is 3 or 4 cycles, so waste another 20 then sample
	w = buff; // 3 instructions
	Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
	Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
	*w = PORTC; // total 4 instr, samples at 4th

	w++; //
	while (!GCDAT);
	TMR0 = 150; // 1.5 bit wait
	TMR0IF = 0;
	
loop:
	if (TMR0IF) {
		handle_packet_gc();
		INTCONbits.IOCIF = 0;
		return;
	}
	else if (!GCDAT) {
		// getting from here to the point where PORTC is read takes 12 instructions,
		// so waste the other 11:
		Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
		Nop(); Nop(); Nop(); Nop(); Nop();

		*w = PORTC; // sample happens exactly 24 instructions after loop entry

		w++;
		while (!GCDAT);
		TMR0 = 150; // 1.5 bit wait
	}
	goto loop;
}

void low_priority interrupt isr_low() {
	Nop();
}

void handle_packet_gc() {
	UINT8 idx = w - buff;

	if (idx == 90) {
		BYTE* b = packet;
		// bits 0-23 are request, 24 is stop bit, 25-88 is data
		for (UINT8 i = 25; i < 88; i++) {
			*b <<= 1;
			*b |= (buff[i] >> 1) & 1;
			if ((i - 25) % 8 == 7) b++;
		}
		packet_available = TRUE;
	}
}


// ****************************************************************************
// ************** USB Callback Functions **************************************
// ****************************************************************************

void USBCBSuspend(void) {
}

void USBCBWakeFromSuspend() {
}

void USBCB_SOF_Handler() {
}

void USBCBErrorHandler() {
}

void USBCBStdSetDscHandler() {
}

void USBCheck_GetDescriptor_SerialNumber() {
    // pick up Get_Descriptor( STRING, serial number ) request
    if (SetupPkt.bmRequestType == 0x80
            && SetupPkt.bRequest == USB_REQUEST_GET_DESCRIPTOR
            && SetupPkt.bDescriptorType == USB_DESCRIPTOR_STRING
            && SetupPkt.bDscIndex == 3) {
        // here, read out serial number string desc to CtrlTrfData[]
        // The size of CtrlTrfData[] array is USB_EP0_BUFF_SIZE (default: 8)
        // if the desc size doesn't fit to this size, declare greater array and assign it instead.

        // copy to CtrlTrfData
        CtrlTrfData[0] = 22; // number of bytes
        CtrlTrfData[1] = USB_DESCRIPTOR_STRING;
        unsigned char i;
        for (i = 0; i < 10; i++) {
            CtrlTrfData[2 + i * 2] = 'x';
            CtrlTrfData[2 + i * 2 + 1] = '\0';
        }
        inPipes[0].info.bits.ctrl_trf_mem = USB_EP0_RAM; // Set memory type
        inPipes[0].pSrc.bRam = (BYTE*) CtrlTrfData; // Set Source
        inPipes[0].wCount.v[0] = CtrlTrfData[0]; // Set data count
        inPipes[0].info.bits.busy = 1;
    }
}


void USBCBCheckOtherReq() {
    USBCheckHIDRequest();
    USBCheck_GetDescriptor_SerialNumber(); // add custom Get_Descriptor( serial number ) handler
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
			//enable the HID endpoint
			USBEnableEndpoint(JOYSTICK_EP, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
			
			break;
        case EVENT_SET_DESCRIPTOR:
            USBCBStdSetDscHandler();
            break;
        case EVENT_EP0_REQUEST:
            USBCheckHIDRequest();
            USBCheck_GetDescriptor_SerialNumber();
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

