#include "config_bits.h"
#include "system_config.h"
#include "gamepads.h"
#include "hardware.h"
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include <stdlib.h>
#include "lcd.h"

void init_random();
void init_pll();
void init_io();
void init_timers();
void init_interrupts();
void low_priority interrupt isr_low();
void high_priority interrupt blahblah();

void snes_tasks();
void n64_tasks();
void ngc_tasks();
void wii_tasks();

void snes_poll();
void n64_poll();
void ngc_poll();
void wii_poll();

void snes_sample();
void n64_sample();
void ngc_sample();
void wii_sample();

void handle_packet_snes();
void handle_packet_n64();
void ngc_handle_packet();
void handle_packet_wii();

enum __mode { real, pc, fake_snes, fake_n64, fake_gc, fake_wii };
enum __sampleSource { snes, n64, ngc, wii };
uint8_t sampleSource;
uint8_t snes_mode;
uint8_t n64_mode;
uint8_t ngc_mode;
uint8_t wii_mode;

snes_packet_t joydata_snes @ 0x500;
n64_packet_t joydata_n64 @ 0x510;
ngc_packet_t joydata_ngc @ 0x520;
wii_packet_t joydata_wii @ 0x530;

BOOL tick1khz = FALSE;
uint8_t timer100hz;
uint16_t timer1000hz;
uint8_t lcd_backLightValue;
BOOL pollNeeded = FALSE;

BYTE sample_buff[90] = {0};
BYTE* sample_w = sample_buff;

BOOL snes_snoop_packet_available = FALSE;
BOOL n64_snoop_packet_available = FALSE;
BOOL ngc_packet_available = FALSE;
BOOL wii_snoop_packet_available = FALSE;

USB_HANDLE USBInHandleSNES = 0;
USB_HANDLE USBInHandleN64 = 0;
USB_HANDLE USBInHandleNGC = 0;
USB_HANDLE USBInHandleWII = 0;

#define USB_READY (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)

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
    
    snes_real();
    n64_real();
    ngc_real();
    
    lcd_setup();
    lcd_backLightValue = 3;
    lcd_home();
    lcd_clear();
    lcd_string("dutchj noob");
    
    ngc_fake();
    ngc_mode = pc;
    
	while (1) {
        USBDeviceTasks();
        
        if (tick1khz) {
            tick1khz = FALSE;
            timer100hz++;
            timer1000hz++;
            lcd_process();
        }
        
        if (PIR2bits.TMR3IF) {
            PIR2bits.TMR3IF = 0;
            WRITETIMER3(15536);
            pollNeeded = TRUE;
        }
        
        snes_tasks();
        n64_tasks();        
        ngc_tasks();
        wii_tasks();
        pollNeeded = FALSE;
        
		if (!USB_READY)
			continue;
        
        if (!HIDTxHandleBusy(USBInHandleSNES)) {
            memset(&joydata_snes, 0x11, sizeof(joydata_snes));
            USBInHandleSNES = HIDTxPacket(HID_EP_SNES, (uint8_t*)&joydata_snes, sizeof(joydata_snes));
        }  
        if (!HIDTxHandleBusy(USBInHandleN64)) {
            memset(&joydata_n64, 0x22, sizeof(joydata_n64));
            USBInHandleN64 = HIDTxPacket(HID_EP_N64, (uint8_t*)&joydata_n64, sizeof(joydata_n64));
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
	// Timer0 as 8-bit 1:1 on instruction clock
	INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 1; // Timer0 is configured as an 8-bit timer/counter
	T0CONbits.T0CS = 0; // Internal instruction cycle clock (FOSC/4)
	T0CONbits.PSA = 1; // Timer0 prescaler is NOT assigned. Timer0 clock input bypasses prescaler
    
    //Timer1 Registers Prescaler= 1 - TMR1 Preset = 53536 - Freq = 1000.00 Hz - Period = 0.001000 seconds
    T1CONbits.TMR1CS = 0b00; // clock source is instruction clock (FOSC/4)
    T1CONbits.T1CKPS = 0b00; // 1:1 prescaler
    IPR1bits.TMR1IP = 0; // // Interrupt on low priority group
	PIE1bits.TMR1IE = 1; // Enables the TMR1 overflow interrupt
    T1CONbits.TMR1ON = 1; // start
    WRITETIMER1(53536);
    
    // Timer3 Registers Prescaler= 4 - TMR1 Preset = 15536 - Freq = 60.00 Hz - Period = 0.016667 seconds
    T3CONbits.TMR3CS = 0b00; // clock source is instruction clock (FOSC/4)
    T3CONbits.T3CKPS = 0b10; // 1:1 prescaler
    PIE2bits.TMR3IE = 0; // Disable interrupt
    T3CONbits.TMR3ON = 1; // Start timer
    WRITETIMER3(15536);
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
	INTCONbits.IOCIE = 0; // IOC interrupt enabled
	INTCON2bits.IOCIP = 1; // High priority group
	IOCCbits.IOCC0 = 0; // enable IOC on RC0 (ngc))
	IOCCbits.IOCC1 = 0; // enable IOC on RC1 (n64))
	IOCCbits.IOCC2 = 0; // enable IOC on RC2 (snes dat))
    
	RCONbits.IPEN = 1; // Enable priority levels on interrupts
	INTCONbits.GIEH = 1; // Enable high priority interrupts
	INTCONbits.GIEL = 1; // Enable low priority interrupts
}

void snes_tasks() {
    
}

void n64_tasks() {
    
}

void ngc_tasks() {
    if (ngc_mode == pc && pollNeeded && USB_READY) {
        di();
        ngc_poll();
        sample_w = sample_buff + 25;
        // waste about 10 instructions before sampling
        Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
        ngc_sample();
        ei();
    }

    if (ngc_packet_available && (ngc_mode == pc || ngc_mode == real) && USB_READY && !HIDTxHandleBusy(USBInHandleNGC)) {
        // hid tx
        USBInHandleNGC = HIDTxPacket(HID_EP_NGC, (uint8_t*)&joydata_ngc, sizeof(joydata_ngc));
        ngc_packet_available = FALSE;
    }
}

void wii_tasks() {
    
}

// open drain, so configure lat = 0 and toggle on tris
#define CLR_NGC() TRISC &= 0b11111110;
#define SET_NGC() TRISC |= 0b00000001;
#define HIGH_NGC() CLR_NGC(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET_NGC(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
#define LOW_NGC() CLR_NGC(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); SET_NGC(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 

void ngc_poll() {
    LATC &= 0b11111110; // make sure we never pull this high
    
    // set data pin to output
    NGC_DAT_TRIS = 0;
    SET_NGC();
    // send 01000000
    //      00000011    
    //      00000010

    LOW_NGC(); HIGH_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); 
    LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); HIGH_NGC(); HIGH_NGC(); 
    LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); LOW_NGC(); 
    
    // stop bit, 2 us
    CLR_NGC(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); Nop(); 
    
    NGC_DAT_TRIS = 1;// set to open collector input
}

void ngc_handle_packet() {
	UINT8 idx = sample_w - sample_buff;

	if (idx == 90) {
		BYTE* b = (BYTE*)&joydata_ngc;
		// bits 0-23 are request, 24 is stop bit, 25-88 is data
		for (UINT8 i = 25; i < 88; i++) {
			*b <<= 1;
			*b |= (sample_buff[i] >> 0) & 1; // use bit0 of every PORTC reading
			if ((i - 25) % 8 == 7) b++;
		}
		ngc_packet_available = TRUE;
	}
}

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
void low_priority interrupt isr_low() {    
    WRITETIMER1(53536);
    PIR1bits.TMR1IF = 0;  // Clear the timer1 interrupt flag
    LCD_PWM = pwmCycle < lcd_backLightValue;
    pwmCycle++;
    if (pwmCycle == 4) pwmCycle = 0;
    tick1khz = 1;
}

void high_priority interrupt isr_high() {
    if (INTCONbits.IOCIF) {
        sample_w = sample_buff; // 3 instructions
        if (sampleSource == snes) snes_sample();
        if (sampleSource == n64) n64_sample();
        if (sampleSource == ngc) ngc_sample();
        if (sampleSource == wii) wii_sample();
    }
    INTCONbits.IOCIF = 0;
}

void snes_sample() {}
void n64_sample() {}
void ngc_sample() {    
    // latency from interrupt to calling this should be about 12 cycles
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
    *sample_w = PORTC; // total 4 instr, samples at 4th

    sample_w++; //
    while (!NGC_DAT);
    TMR0 = 255 - 60; // 1.5 bit wait
    INTCONbits.TMR0IF = 0;

    loop:
    if (INTCONbits.TMR0IF) { // timeout - no bit received
        ngc_handle_packet();
        return;
    }
    else if (!NGC_DAT) {
        // getting from here to the point where PORTC is read takes 12 instructions,
        // so waste the other 11:
        Nop(); Nop(); Nop(); Nop(); Nop(); Nop();
        Nop(); Nop(); Nop(); Nop(); Nop();

        *sample_w = PORTC; // sample happens exactly 24 instructions after loop entry

        sample_w++;
        while (!NGC_DAT);
        TMR0 = 255 - 60; // 1.5 bit wait
    }
    goto loop;
}
void wii_sample() {}