#ifndef HARDWARE_H
#define	HARDWARE_H

#include <xc.h>
#include <stdint.h>

#define _XTAL_FREQ 48000000

#define SNES_DAT PORTCbits.RC2
#define SNES_LATCH PORTAbits.RA6
#define SNES_CLK PORTAbits.RA7
#define N64_DAT PORTCbits.RC1
#define NGC_DAT PORTCbits.RC0

#define SNES_DAT_TRIS TRISCbits.TRISC2
#define SNES_LATCH_TRIS TRISAbits.TRISA6
#define SNES_CLK_TRIS TRISAbits.TRISA7
#define N64_DAT_TRIS TRISCbits.TRISC1
#define NGC_DAT_TRIS TRISCbits.TRISC0

#define LED_SNES_GREEN LATAbits.LATA5
#define LED_SNES_ORANGE LATCbits.LATC6
#define LED_GC_GREEN LATBbits.LATB6
#define LED_GC_ORANGE LATBbits.LATB7

#define FAKE_DAT LATBbits.LATB3
#define SWITCH1 LATBbits.LATB2
#define SWITCH2 LATBbits.LATB1
#define SWITCH3 LATBbits.LATB0

#define LCD_RS LATBbits.LATB4
#define LCD_PWM LATBbits.LATB5
#define LCD_E LATAbits.LATA4
#define LCD_D4 LATAbits.LATA0
#define LCD_D5 LATAbits.LATA1
#define LCD_D6 LATAbits.LATA2
#define LCD_D7 LATAbits.LATA3


#endif	/* HARDWARE_H */
