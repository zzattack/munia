#include "system_config.h"
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include <stdlib.h>
#include "hardware.h"
#include "globals.h"
#include "lcd.h"
#include "snes.h"
#include "n64.h"
#include "gamecube.h"
#include "gamepad.h"
#include "menu.h"
#include "buttonchecker.h"
#include "memory.h"
#include "report_descriptors.h"
#include "usb_requests.h"
#include "uarts.h"
#include "fakeout.h"

void init_pll();
void init_io();
void init_timers();
void init_interrupts();
void low_priority interrupt isr_low();

void load_config();
void apply_config();
void save_config();
void load_device_id();

volatile uint8_t EasyTimerTick;
uint8_t counter60hz = 0;
void tick1000Hz();

void usb_tasks();

void main() {
    init_io();
    
#ifdef DEBUG_PRINT
    U1Init(115200, false, true);
#endif
    
    LED_SNES_GREEN = 0;
    LED_SNES_ORANGE = 0;
    LED_GC_ORANGE = 0;
    LED_GC_GREEN = 0;
    lcd_backlight(0);
    init_pll();
        
    init_timers();
    usb_descriptors_check();
	USBDeviceInit();
    USBDeviceAttach();
    bcInit();
    lcd_setup();
    
    load_config();
    
    apply_config();
    init_interrupts();    
    
    dbgs("MUNIA Initialized\n");
    memset(usbOutBuffer, 0, sizeof(usbOutBuffer));
    memset(usbInBuffer, 0, sizeof(usbInBuffer));
    
	while (1) {
        ClrWdt();                
        USBDeviceTasks();
        
        LED_SNES_GREEN = USBDeviceState >= CONFIGURED_STATE;
        LED_SNES_ORANGE = !USBSuspendControl;
        
        if (EasyTimerTick > 0) {
            EasyTimerTick--;         
            tick1000Hz();   
        }
        
        if (PIR2bits.TMR3IF) {
            WRITETIMER3(15536);
            PIR2bits.TMR3IF = 0;
            pollNeeded = true;
        }
        
        snes_tasks();
        n64_tasks();        
        ngc_tasks();
        usb_tasks();
        pollNeeded = false;
    }
}

void init_pll() {
	OSCTUNE = 0x80; // 3X PLL ratio mode selected
	OSCCON = 0x70;  // Switch to 16MHz HFINTOSC
	OSCCON2 = 0x10; // Enable PLL, SOSC, PRI OSC drivers turned off
#ifndef SIMUL    
	while (!OSCCON2bits.PLLRDY); // Wait for PLL lock
#endif
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

    // pull ups on the inputs
    LATA |= 0b11000000;
    LATC |= 0b10000111;

    ADCON0bits.ADON = 0;
}

void init_timers() {
    // Timer 0: used for packet capture timeout detection
    //   Speed is FOSC
    //   Reset when starting packet sample, or new bit received
    //   Once IF set, packet sample has finished
    // Timer 1: count packet length during NGC/N64 sampling
    // Timer 2: source at 1KHz, for EasyTimer
    // Timer 3: polling indicator, runs either at 50Hz or 60Hz
    
    
	// Timer0 as 8-bit 1:1 on instruction clock
	INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 1; // Timer0 is configured as an 8-bit timer/counter
	T0CONbits.T0CS = 0; // Instruction clock (FOSC/4)
	T0CONbits.PSA = 1; // Timer0 prescaler is NOT assigned. 
    
    // Timer 1 counts during n64/ngc sampling
    T1CONbits.TMR1CS = 0b00; // clock source is instruction clock (FOSC/4)
    T1CONbits.T1CKPS = 0b00; // 1:1 prescaler
    T1CONbits.SOSCEN = 0; // 0 = Dedicated secondary oscillator circuit disabled
    T1CONbits.RD16 = 0; // 0 = Dedicated secondary oscillator circuit disabled
	PIE1bits.TMR1IE = 0; // Disables the TMR1 overflow interrupt
    T1CONbits.TMR1ON = 1; // start
    
    // Timer2 Registers Prescaler=16 - PostScaler=15 - PR2=50 - Freq =~ 1000.00 Hz
    T2CONbits.T2CKPS = 0b11; // Prescaler = 1:16
    T2CONbits.T2OUTPS = 0b1110; // Postscaler = 1:15
    PR2 = 50;         // Period register
    IPR1bits.TMR2IP = 0; // Low priority group
    PIE1bits.TMR2IE = 1; // Interrupt enabled
    T2CONbits.TMR2ON = 1;  // Enable timer

    // Timer3 Registers Prescaler= 4 - TMR3 Preset = 15536 - Freq = 60.00 Hz - Period = 0.016667 seconds
    T3CONbits.TMR3CS = 0b00; // clock source is instruction clock (FOSC/4)
    T3CONbits.T3CKPS = 0b10; // 1:1 prescaler
    PIE2bits.TMR3IE = 0; // Disable interrupt
    T3CONbits.TMR3ON = 1; // Start timer
    WRITETIMER3(15536);
}

void init_interrupts() {
	// setup interrupt on falling change of data on gamepad ports
	INTCONbits.IOCIE = 1; // IOC interrupt enabled
	INTCON2bits.IOCIP = 1; // High priority group
    
	RCONbits.IPEN = 1; // Enable priority levels on interrupts
	INTCONbits.GIEH = 1; // Enable high priority interrupts
	INTCONbits.GIEL = 1; // Enable low priority interrupts
}


void load_config() {
    uint8_t* w = (uint8_t*)&config;
    for (uint8_t i = 0; i < sizeof(config); i++)
        *w++ = ee_read(i);
}

void save_config() {
    uint8_t* r = (uint8_t*)&config;
    for (uint8_t i = 0; i < sizeof(config); i++)
        ee_write(i, *r++);
}

void apply_config() {
    // validate
    if (config.output_mode > output_pc) config.output_mode = output_pc;
    config.input_sources &= 0b111;
    // only take interrupt event from the console we're outputting to
    IOCCbits.IOCC0 = config.output_mode == output_ngc;
    IOCCbits.IOCC1 = config.output_mode == output_n64;
    IOCCbits.IOCC7 = config.output_mode == output_snes;
    
    // redirect joysticks we're not sampling to their console
    // switch position = 0 --> connected to our fake output,
    //                   1 --> connected to joystick
    SWITCH1 = IOCCbits.IOCC0 || !config.input_ngc;
    SWITCH2 = IOCCbits.IOCC1 || !config.input_n64;
    SWITCH3 = IOCCbits.IOCC7 || !config.input_snes;
}

void randomize_device_id() {    
    // setup adc
    ADCON1bits.PVCFG    = 0b00; // set v+ reference to Vdd
    ADCON1bits.NVCFG    = 0b0;  // set v- reference to GND
    ADCON2bits.ADFM     = 0b1;  // right justify the output
    ADCON2bits.ACQT     = 0b110;// 16 TAD
    ADCON2bits.ADCS     = 0b101;// use Fosc/16 for clock source
    ADCON0bits.ADON     = 0b1;  // turn on the ADC

    // generate random serial, seeded with xor of 16 readings of ADC
    // (Performing a conversion on unimplemented channels will return random values.)
    unsigned int seed = 0x3F07 ^ TMR0; // poly
    ADCON0bits.CHS = 0b11011; // select analog input channel 27 (unimplemented))
    
    for (int i = 0; i < 8; i++) {
        ADCON0bits.GO = 0b1; // start the conversion
        while(ADCON0bits.DONE); // wait for the conversion to finish
        seed ^= (ADRESH << 8) | ADRESL;  // return the result
    }
    ADCON0bits.ADON = 0;  // turn off the ADC
    srand(seed); // seed with conversion result

    uint8_t* w = (uint8_t*)&deviceID;
    *w++ = rand();
    *w++ = rand();
    *w++ = rand();
    *w++ = rand();
    ADCON0bits.ADON = 0;  // turn off the ADC
}

void load_device_id() {
    uint8_t* r = (uint8_t*)&deviceID;
    for (uint8_t i = 0x20; i < 0x24 ; i++)
        *r++ = ee_read(i);

    if (deviceID == 0 || deviceID == 0xFFFFFFFF) {
        // generate random device id, and save it
        randomize_device_id();
        uint8_t* w = (uint8_t*)&deviceID;   
        for (uint8_t i = 0x20; i < 0x24 ; i++){            
            ee_write(i, *w++);
        }
    }
}

void tick1000Hz() {
    lcd_process();
    menu_tasks();

#ifndef DEBUG
    if (bcCheck()) {
        if (bcTick() && bcPressed()) {
            if (in_menu) menu_exit(false);
            else menu_enter();
        }
    }
#endif
}