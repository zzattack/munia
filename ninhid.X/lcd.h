#ifndef LCD_H
#define	LCD_H

#include <stdint.h>

#define LCD_LINES 2
#define LCD_INIT 0b00101000 // for 2 lines
#if LCD_CURSOR==1
  #if LCD_CURSORBLINK==1
    #define LCD_DISPL 0b00001111
  #else
    #define LCD_DISPL 0b00001110
  #endif
#else
  #define LCD_DISPL 0b00001100 // display on, cursor off
#endif

#ifndef LCD_BUFFER_SIZE
  #define LCD_BUFFER_SIZE (LCD_LINES)*20
#endif

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

#endif	/* LCD_H */
