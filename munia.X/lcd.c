/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "hardware.h"
#include "lcd.h"
#include <stdlib.h>
#include <stdbool.h>

void addByte(uint8_t b);
void lcd_pulseE();

bool lcd_commandMode = true;
uint8_t lcd_timerCount = 0;
uint8_t lcd_startupTimer = 50;
CircularBuffer lcd_cb;

void lcd_setup() {
    cbInit(&lcd_cb);
    lcd_startupTimer = 100;
}

#define addByte(b) do { cbWrite(&lcd_cb, b); } while (0);

void lcd_pulseE() {            
    __delay_us(4); LCD_E = 1;
    __delay_us(4); LCD_E = 0;
}

#define sendPortHigh(b) sendPortLow((b) >> 4)
void sendPortLow(uint8_t b) {
    LATA &= 0xF0;
    LATA |= b;
    lcd_pulseE();
}

void lcd_command(uint8_t cmd) {
    if (!lcd_commandMode) {
        addByte(SPECIAL_TOCMD);
        lcd_commandMode = true;
    }
    addByte(cmd);
}

void lcd_goto(uint8_t line, uint8_t pos) {
    if (line == 1) {
        lcd_command(0x80 + pos);
    }
    if (line == 2) {
        lcd_command(0x80 + 0x40 + pos);
    }
}

void lcd_char(uint8_t chr) {
    if (lcd_commandMode) {
        addByte(SPECIAL_TOCHR);
        lcd_commandMode = false;
    }
    addByte(chr);
}

void lcd_string(const char *q) {
    while(*q)
        lcd_char(*q++);
}

/*
char itoaBuffer[8];
void align(char* buffer, uint8_t width, char fill, BOOL right) {
    
}
void lcd_uint(unsigned value, uint8_t width, char fill, BOOL right) {
    utoa(itoaBuffer, value, 10);
    if (width > 1)
        align(itoaBuffer, width, fill, right);
    lcd_string(itoaBuffer);
}

void lcd_int(int value, uint8_t width, char fill, BOOL right) {
    itoa(itoaBuffer, value, 10);
    if (width > 1)
        align(itoaBuffer, width, fill, right);
    lcd_string(itoaBuffer);
}
*/
// call me @ 1khz

void lcd_process() {
    if (lcd_timerCount < 255)
        lcd_timerCount++;
    
    if (lcd_startupTimer > 0) {
        LCD_E = 0;
        LCD_RS = 0;
        
        // timer is initialized at 100, which blocks lcd ops during the first 100ms
        if (lcd_startupTimer == 12) {
            sendPortHigh(0b00110000); // set interface to 8-bit for proper init
        } 
        else if (lcd_startupTimer == 5) { // 5 ms later just pulse E again
            lcd_pulseE();
        }
        else if (lcd_startupTimer == 4) { // 1 ms later again
            lcd_pulseE();
        } 
        else if (lcd_startupTimer == 3) { 
            // 1 ms later, set to 4-bit mode
            sendPortHigh(LCD_INIT);
        } 
        else if (lcd_startupTimer == 2) {
            // 4-bit, 2 lines, 5x8
            sendPortHigh(LCD_INIT); // confirm 4-bit mode
            sendPortLow(LCD_INIT);
        } 
        else if (lcd_startupTimer == 1) {
            // display on, cursor off
            sendPortHigh(LCD_DISPLAY);
            sendPortLow(LCD_DISPLAY);
        }

        lcd_startupTimer--;
    } 
    else if (!cbIsEmpty(&lcd_cb) && lcd_timerCount == 255) {
        char c = cbRead(&lcd_cb);
        if (c == SPECIAL_TOCHR) {
            LCD_RS = 1;
        } 
        else if (c == SPECIAL_TOCMD) {
            LCD_RS = 0;
        } 
        else {
            sendPortHigh(c);
            sendPortLow(c);
        }
    }
}