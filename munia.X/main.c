#include "config_bits.h"
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
#include "uarts.h"

void init_random();
void init_pll();
void init_io();
void init_timers();
void init_interrupts();
void low_priority interrupt isr_low();
void high_priority interrupt isr_high();

void load_config();
void apply_config();
void save_config();

bool tick1khz = false;
uint8_t timer100hz;
uint16_t timer1000hz;
uint8_t counter60hz = 0;

void usb_tasks();

void main() {
    // init_random();
    init_io();
    
    LED_SNES_GREEN = 0;
    LED_SNES_ORANGE = 0;
    LED_GC_ORANGE = 0;
    LED_GC_GREEN = 0;    
    LCD_PWM = 0;
    lcd_backLightValue = 0;
#ifndef SIMUL    
    init_pll(); // disable on simulator!
#endif
    
    init_timers();
    usb_descriptors_check();
	USBDeviceInit();
    USBDeviceAttach();
    bcInit();
    lcd_setup();
    
    load_config();
    apply_config();
    init_interrupts();    
    
#ifdef DEBUG
    menu_enter();
    U1Init(115200, true, true);
#endif
    
	while (1) {
        ClrWdt();                
        USBDeviceTasks();
        
        LED_SNES_GREEN = USBDeviceState >= CONFIGURED_STATE;
#ifndef DEBUG
        LED_SNES_ORANGE = !USBSuspendControl;
#endif
        
        if (tick1khz) {
            tick1khz = false;
            timer100hz++;
            timer1000hz++;
            lcd_process();

            if (timer100hz == 10) {
                timer100hz = 0;
                
#ifndef DEBUG
                if (bcCheck()) {
                    if (BTN_MENU_PRESSED) {
                        if (in_menu) menu_exit(false);
                        else menu_enter();
                    }
                    BTN_MENU_PRESSED = false;
                }
#endif
            }
            
            menu_tasks();
        }
        if (timer1000hz == 1000)
            timer1000hz = 0;
        
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

    // pull ups on the inputs
    LATA |= 0b11000000;
    LATC |= 0b10000111;
}

void init_timers() {    
	// Timer0 as 8-bit 1:1 on instruction clock
	INTCONbits.TMR0IE = 0; // Disables the TMR0 overflow interrupt
	T0CONbits.TMR0ON = 1; // Enables Timer0
	T0CONbits.T08BIT = 1; // Timer0 is configured as an 8-bit timer/counter
	T0CONbits.T0CS = 0; // Instruction clock (FOSC/4)
	T0CONbits.PSA = 1; // Timer0 prescaler is NOT assigned. 
    
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
	INTCONbits.IOCIE = 1; // IOC interrupt enabled
	INTCON2bits.IOCIP = 1; // High priority group
    
	RCONbits.IPEN = 1; // Enable priority levels on interrupts
	INTCONbits.GIEH = 1; // Enable high priority interrupts
	INTCONbits.GIEL = 1; // Enable low priority interrupts
}


// Low priority interrupt procedure
uint8_t pwmCycle = 0;
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
        // see who went low
        uint8_t mask = ~PORTC & IOCC;
        if (mask & 0b00000010) n64_sample();
        if (mask & 0b00000001) ngc_sample();
        if (PORTC & IOCC & 0b10000000) snes_sample();
    }
    INTCONbits.IOCIF = 0;
}


void load_config() {
    uint8_t* w = (uint8_t*)&config;
    for (uint8_t i = 0; i < sizeof(config); i++)
        *w++ = DATAEE_ReadByte(i);
    }
void save_config() {
    uint8_t* r = (uint8_t*)&config;
    for (uint8_t i = 0; i < sizeof(config); i++)
        DATAEE_WriteByte(i, *r++);
}

void apply_config() {    
    if (config.ngc_mode == NGC_MODE_PC) {
        SWITCH1 = 0;
        IOCCbits.IOCC0 = 0; // disable IOC on RC0 (ngc)
    }
    else {
        // pull up, in case no device attached
        LATC |= 0b00000001;
        SWITCH1 = 1;
        IOCCbits.IOCC0 = 1; // enable IOC on RC0 (ngc)
    }
    
    if (config.n64_mode == N64_MODE_PC) {
        SWITCH2 = 0;
        IOCCbits.IOCC1 = 0; // disable IOC on RC1 (n64)
    }
    else { 
        // pull up, in case no device attached
        LATC |= 0b00000010;
        SWITCH2 = 1;
        IOCCbits.IOCC1 = 1; // enable IOC on RC1 (n64)
    }
    
    if (config.snes_mode == SNES_MODE_PC) {
        SWITCH3 = 0;
        IOCCbits.IOCC7 = 0; // disable IOC on RC7 (snes latch)
    }
    else {
        SWITCH3 = 1;
        IOCCbits.IOCC7 = 1; // enable IO7 on RC7 (snes latch)
    }
}


typedef struct {
    uint8_t report_id;
    
    uint8_t fw_minor : 4;
    uint8_t fw_major : 4;
    
    uint8_t hw_revision : 4;
    uint8_t : 4;
        
    config_t config;
} cfg_read_report_t;

typedef struct {
    uint8_t report_id;
    config_t config;
} cfg_write_report_t;


void usb_tasks() {
    // User Application USB tasks
    if (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)
        return;
    
    // Check if we have received an OUT data packet from the host, and nothing is left in buffer
    if (!HIDRxHandleBusy(USBOutHandleCfg) && !HIDTxHandleBusy(USBInHandleCfg)) {
        // We just received a packet of data from the USB host.
        // Check the first byte of the packet to see what command the host
        // application software wants us to fulfill.
        dbgs("packet received on endpoint 4, report id=0x"); dbgsvalx(usbOutBuffer[0]); dbgs("\n");
        
        if (usbOutBuffer[0] == REPORT_CFG_READ_ID) {
            // format response
            cfg_read_report_t* report = (cfg_read_report_t*)usbInBuffer;
            report->report_id = REPORT_CFG_READ_ID;
            report->fw_major = FW_MAJOR;
            report->fw_minor = FW_MINOR;
            report->hw_revision = HW_REVISION;
            memcpy(&report->config, &config, sizeof(config_t));
            memset(usbInBuffer + sizeof(cfg_read_report_t), 0, sizeof(usbInBuffer) - sizeof(cfg_read_report_t));
            
            USBInHandleCfg = HIDTxPacket(HID_EP_CFG, usbInBuffer, REPORT_CFG_SIZE + 1);
        }
        else if (usbOutBuffer[0] == REPORT_CFG_WRITE_ID) {
            // extract config
            cfg_write_report_t* report = (cfg_write_report_t*)usbOutBuffer;
            memcpy(&config, &report->config, sizeof(config_t));
            apply_config();
            if (in_menu) {
                memcpy(&config_edit, &config, sizeof(config_t));
                memcpy(&config_backup, &config, sizeof(config_t));
                menu_page(current_menu_page);
            }
            
            // response
            usbInBuffer[0] = REPORT_CFG_WRITE_ID;
            usbInBuffer[1] = 1; // signifies ok
            memset(usbInBuffer + 2, 0, sizeof(usbInBuffer) - 2);
            
            USBInHandleCfg = HIDTxPacket(HID_EP_CFG, usbInBuffer, REPORT_CFG_SIZE + 1);
        }
        
        // re-arm
        USBOutHandleCfg = HIDRxPacket(HID_EP_CFG, usbOutBuffer, sizeof (usbOutBuffer));
    }
}