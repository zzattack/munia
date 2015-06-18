/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "hardware.h"
#include "lcd.h"
#include <stdlib.h>



uint8_t lcd_outputBuffer[LCD_BUFFER_SIZE];
BOOL lcd_commandMode = TRUE;
uint8_t lcd_inputIndex;
uint8_t lcd_outputIndex;
uint8_t lcd_bufferInUse;
uint8_t lcd_timerCount = 0;
uint8_t lcd_startupTimer = 30;

void lcd_setup() {
    lcd_startupTimer = 30;
}

#define addByte(b) do { \
    lcd_outputBuffer[lcd_inputIndex++] = b; \
    if (lcd_inputIndex == LCD_BUFFER_SIZE) \
        lcd_inputIndex = 0; \
    lcd_bufferInUse++; \
} while(FALSE)

#define sendPortHigh(b) sendPortLow((b) >> 4)
void sendPortLow(uint8_t b) {
    LCD_D7 = (b >> 0) & 1;
    LCD_D6 = (b >> 1) & 1;
    LCD_D5 = (b >> 2) & 1;
    LCD_D4 = (b >> 3) & 1;
    LCD_E = 1;
    __delay_us(1);
    LCD_E = 0;
    __delay_us(1);
}

void lcd_command(uint8_t cmd) {
    if (lcd_bufferInUse > LCD_BUFFER_SIZE - 3)
        return;

    if (!lcd_commandMode) {
        addByte(SPECIAL_TOCMD);
        lcd_commandMode = TRUE;
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
    if (line == 3) {
        lcd_command(0x80 + 0x14 + pos);
    }
    if (line == 4) {
        lcd_command(0x80 + 0x54 + pos);
    }
}

void lcd_char(uint8_t chr) {
    if (lcd_bufferInUse > LCD_BUFFER_SIZE - 3)
        return;

    if (lcd_commandMode) {
        addByte(SPECIAL_TOCHR);
        lcd_commandMode = FALSE;
    }
    addByte(chr);
}

void lcd_string(const char *q) {
    while(*q)
        lcd_char(*q++);
}


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

// call me @ 1khz
void lcd_process() {
    if (lcd_timerCount < 255)
        lcd_timerCount++;
    
    if (lcd_startupTimer > 0) {
        LCD_E = 0;
        LCD_RS = 0;
        
        // timer is initialized at 30, which blocks lcd ops during the first 20ms
        if (lcd_startupTimer == 10) {
            sendPortHigh(0b00110000); // set interface to 8-bit for proper init
        } 
        else if (lcd_startupTimer == 5) { // 5 ms later just pulse E again
            __delay_us(1); LCD_E = 1;
            __delay_us(1); LCD_E = 0;
        } 
        else if (lcd_startupTimer == 4) { // 1 ms later again
            __delay_us(1); LCD_E = 1;
            __delay_us(1); LCD_E = 0;
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
            sendPortHigh(LCD_DISPL);
            sendPortLow(LCD_DISPL);
        }

        lcd_startupTimer--;
    } 
    else if (lcd_bufferInUse > 0 && lcd_timerCount == 255) {
        if (lcd_outputBuffer[lcd_outputIndex] < 4) {
            lcd_timerCount = 210;
        }
        if (lcd_outputBuffer[lcd_outputIndex] == SPECIAL_TOCHR) {
            LCD_RS = 1;
        } 
        else if (lcd_outputBuffer[lcd_outputIndex] == SPECIAL_TOCMD) {
            LCD_RS = 0;
        } 
        else {
            sendPortHigh(lcd_outputBuffer[lcd_outputIndex]);
            sendPortLow(lcd_outputBuffer[lcd_outputIndex]);
        }
        lcd_outputIndex++;
        if (lcd_outputIndex == LCD_BUFFER_SIZE)
            lcd_outputIndex = 0;
        lcd_bufferInUse--;
    }
}