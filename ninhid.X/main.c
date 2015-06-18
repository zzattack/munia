#include "config_bits.h"
#include "system_config.h"
#include "gamepads.h"
#include "hardware.h"
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include <stdlib.h>
#include "lcd.h"


snes_packet joydata_snes @ 0x500;
n64_packet joydata_n64 @ 0x510;
ngc_packet joydata_ngc @ 0x520;
wii_packet joydata_wii @ 0x530;

USB_HANDLE USBInHandleSNES = 0;
USB_HANDLE USBInHandleN64 = 0;
USB_HANDLE USBInHandleNGC = 0;
USB_HANDLE USBInHandleWII = 0;

void init_random();
void init_pll();
void init_io();
void init_timers();
void init_interrupts();
void low_priority interrupt updatePwm();
void high_priority interrupt blahblah();

BOOL tick1khz = FALSE;
uint8_t timer100hz;
uint16_t timer1000hz;

void main() {
    // init_random();
    init_pll();
    init_io();
        
    init_interrupts();
    init_timers();
    usb_descriptors_check();
	USBDeviceInit();
    USBDeviceAttach();

    LED_SNES_GREEN = 0;
    LED_SNES_ORANGE = 0;
    LED_GC_ORANGE = 0;
    LED_GC_GREEN = 0;

    LED_SNES_GREEN = 1;
    LED_SNES_ORANGE = 1;
    LED_GC_ORANGE = 1;
    LED_GC_GREEN = 1;
        
    lcd_setup();
    lcd_setBacklight(1);
    lcd_home();
    lcd_clear();
    
    lcd_string("xXx#");
    
	while (1) {
        
        if (tick1khz) {
            tick1khz = FALSE;
            timer100hz++;
            timer1000hz++;
            lcd_process();
        }
        if (timer1000hz == 1000) {
            timer1000hz = 0;
            LED_GC_ORANGE = !LED_GC_ORANGE;
            LED_GC_GREEN = !LED_GC_GREEN;
            LED_SNES_GREEN = !LED_SNES_GREEN;
            LED_SNES_ORANGE = !LED_SNES_ORANGE;
            static int x = 0;
            lcd_char('0' + x);
            x++;
            if (x == 10) x = 0;
        }
        
        USBDeviceTasks();
              
		if (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)
			continue;
            
        if (!HIDTxHandleBusy(USBInHandleSNES)) {
            memset(&joydata_snes, 0x11, sizeof(joydata_snes));
            USBInHandleSNES = HIDTxPacket(HID_EP_SNES, (uint8_t*)&joydata_snes, sizeof(joydata_snes));
        }  
        if (!HIDTxHandleBusy(USBInHandleN64)) {
            memset(&joydata_n64, 0x22, sizeof(joydata_n64));
            USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64, sizeof(joydata_n64));
        }
        if (!HIDTxHandleBusy(USBInHandleNGC)) {
            memset(&joydata_ngc, 0x33, sizeof(joydata_ngc));
            USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(joydata_ngc));
        }  		
        if (!HIDTxHandleBusy(USBInHandleWII)) {
            memset(&joydata_wii, 0x44, sizeof(joydata_wii));
            //Send the packet over USB to the host.
            USBInHandleWII = HIDTxPacket(HID_EP_WII, (uint8_t*)&joydata_wii, sizeof(joydata_wii));
        }
    }
}
    
void init_pll() {
	OSCTUNE = 0x80; // 3X PLL ratio mode selected
	OSCCON = 0x70;  // Switch to 16MHz HFINTOSC
	OSCCON2 = 0x10; // Enable PLL, SOSC, PRI OSC drivers turned off
	while (!OSCCON2bits.PLLRDY); // Wait for PLL lock
	ACTCON = 0x90;  // Enable active clock tuning for USB operation
}

void init_io() {
    // analog ports
	ANSELA = 0b00000000; // all digital
	ANSELB = 0b00000000;
	ANSELC = 0b00000000;
    
    // io directions
    TRISA = 0b11000000; // RA0-4 for LCD, RA5 for led, RA6/7 snes LATCH/CLOCK
    TRISB = 0b00000000; // outputs for leds, LCD, fake signal, switches
    TRISC = 0b10000111; // usb sense, uart tx, snes/n64/gc input signals
}

void init_timers() {    
	// cfg timer
	/*INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 0; // Timer0 is configured as an 16-bit timer/counter
	T0CONbits.T0CS = 0; // Internal instruction cycle clock (FOSC/4)
	T0CONbits.PSA = 0; // Timer0 prescaler is NOT assigned. Timer0 clock input bypasses prescaler
    T0CONbits.T0PS = 0b100;*/
    
    //Timer1 Registers Prescaler= 1 - TMR1 Preset = 53536 - Freq = 1000.00 Hz - Period = 0.001000 seconds
    T1CONbits.TMR1CS = 0b00; // clock source is instruction clock (FOSC/4)
    T1CONbits.T1CKPS = 0b00; // 1:1 prescaler
    IPR1bits.TMR1IP = 0; // // Interrupt on low priority group
	PIE1bits.TMR1IE = 1; // Enables the TMR1 overflow interrupt
    T1CONbits.TMR1ON = 1; // start
    WRITETIMER1(53536);
}

void init_random() {
    // setup adc
    /* todo
    TRISAbits.TRISA1    = 0b1;  // set pin as input
    ANCON0bits.ANSEL1   = 0b1;  // set pin as analog
    ADCON1bits.VCFG     = 0b00; // set v+ reference to Vdd
    ADCON1bits.VNCFG    = 0b0;  // set v- reference to GND
    ADCON1bits.CHSN     = 0b000;// set negative input to GND
    ADCON2bits.ADFM     = 0b1;  // right justify the output
    ADCON2bits.ACQT     = 0b110;// 16 TAD
    ADCON2bits.ADCS     = 0b101;// use Fosc/16 for clock source
    ADCON0bits.ADON     = 0b1;  // turn on the ADC
    */
    // generate random serial, seeded with xor of 16 readings of ADC
    unsigned int seed = 0x3F07; // poly
    ADCON0bits.CHS = 3; // select analog input channel
    for (int i = 0; i < 16; i++) {
        ADCON0bits.GO = 0b1; // start the conversion
        while(ADCON0bits.DONE); // wait for the conversion to finish
        seed ^= (ADRESH << 8) | ADRESL;  // return the result
    }
    srand(seed); // seed with conversion result
}

void init_interrupts() {
	// setup interrupt on falling change of data on gamepad ports
	// INTCONbits.IOCIE = 0; // IOC interrupt enabled
	// INTCON2bits.IOCIP = 1; // High priority group      
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
            USBEnableEndpoint(HID_EP_SNES, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP_N64, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP_NGC, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);
            USBEnableEndpoint(HID_EP_WII, USB_IN_ENABLED | USB_HANDSHAKE_ENABLED | USB_DISALLOW_SETUP);            
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


// Low priority interrupt procedure
UINT8 pwmCycle = 0;
void low_priority interrupt updatePwm() {    
    WRITETIMER1(53536);
    PIR1bits.TMR1IF = 0;  // Clear the timer1 interrupt flag
    LCD_PWM = pwmCycle < lcd_backLightValue;
    pwmCycle++;
    if (pwmCycle == 10) pwmCycle = 0;
    tick1khz = 1;
}

void high_priority interrupt blahbla() { 
    // determine why
    Nop();
}