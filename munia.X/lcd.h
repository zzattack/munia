#ifndef LCD_H
#define	LCD_H

#include <stdint.h>
#include "circular_buffer.h"

#define LCD_LINES 2
#define LCD_INIT 0b00101000 // for 2 lines
#define LCD_DISPLAY_OFF 0x08
#define LCD_DISPLAY_ON_CURSOR_OFF 0x0C
#define LCD_DISPLAY LCD_DISPLAY_OFF // display off initially

#define SPECIAL_TOCMD 251
#define SPECIAL_TOCHR 250

void lcd_setup();
void lcd_command(uint8_t cmd);
void lcd_goto(uint8_t line, uint8_t pos);
#define lcd_clear() lcd_command(0x01)
#define lcd_home() lcd_command(0x80)

void lcd_char(uint8_t chr);
void lcd_string(const char *q);
void lcd_process();

uint8_t lcd_backLightValue = 0;

#endif	/* LCD_H */
