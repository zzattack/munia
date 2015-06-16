#ifndef LCD_H
#define	LCD_H

UINT8  lcd_backLightValue;

void lcd_command(UINT8 cmd);
void lcd_goto(UINT8 line, UINT8 pos);
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

void lcd_char(UINT8 chr);
void lcd_string(const char *q);
void lcd_setBacklight(UINT8 value);   // 0-10
void lcd_process();


#endif	/* LCD_H */

