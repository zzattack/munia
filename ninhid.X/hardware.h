#ifndef HARDWARE_H
#define	HARDWARE_H

#define SNES_DAT PORTCbits.RC2
#define SNES_LATCH PORTAbits.RA6
#define SNES_CLK PORTAbits.RA7
#define N64_DAT PORTCbits.RC1
#define GC_DAT PORTCbits.RC0

#define LED_SNES_ORANGE LATAbits.LATA5
#define LED_SNES_GREEN LATCbits.LATC6
#define LED_GC_ORANGE LATBbits.LATB6
#define LED_GC_GREEN LATBbits.LATB7

#define FAKE_OUT LATBbits.LATB3
#define SWITCH1 LATBbits.LATB2
#define SWITCH2 LATBbits.LATB1
#define SWITCH3 LATBbits.LATB0

#define snes_fake() do { SWITCH1 = 0; } while(0);
#define snes_real() do { SWITCH1 = 1; } while(0);
#define n64_fake()  do { SWITCH2 = 0; } while(0);
#define n64_real()  do { SWITCH2 = 1; } while(0);
#define gc_fake()   do { SWITCH3 = 0; } while(0);
#define gc_real()   do { SWITCH3 = 1; } while(0);

#define LCD_RS LATBbits.LATB4
#define LCD_PWM LATBbits.LATB5
#define LCD_PWM LATBbits.LATB5
#define LCD_E LATAbits.RA4        
#define LCD_D4 LATAbits.RA3
#define LCD_D5 LATAbits.RA2
#define LCD_D6 LATAbits.RA1
#define LCD_D7 LATAbits.RA0

#endif	/* HARDWARE_H */

