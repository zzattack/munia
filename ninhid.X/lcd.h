#ifndef LCD_H
#define	LCD_H

/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "basic.h"              /* For the basic definitions */

/******************************************************************************/
/* Public variables                                                           */
/******************************************************************************/

#ifdef LCD_BACKLIGHT
uint8_t LCD_PRIVATE_BANK lcd_backLightValue;
#endif

/******************************************************************************/
/* Function Prototypes                                                        */
/******************************************************************************/

void lcd_command(uint8_t cmd);
void lcd_goto(uint8_t line, uint8_t pos);
#define lcd_clear() lcd_command(0x01)
#define lcd_home() lcd_command(0x80)

#define LCD_GOTO(line, pos) do { \
    if ((line) == 1) { \
        lcd_command(0x80 + (pos)); \
    } \
    if ((line) == 2) { \
        lcd_command(0x80 + 0x40 + (pos)); \
    } \
    if ((line) == 3) { \
        lcd_command(0x80 + 0x14 + (pos)); \
    } \
    if ((line) == 4) { \
        lcd_command(0x80 + 0x54 + (pos)); \
    } \
} while (false)

void lcd_char(uint8_t chr);
void lcd_string(const char *q);

void lcd_setBacklight(uint8_t value);   // 0-10

void lcd_process();


#endif	/* LCD_H */

