#include <GenericTypeDefs.h>
#include "lcd.h"
#include "hardware.h"

#define LCD_INIT 0b00101000
#define LCD_DISPL 0b00001100
#define LCD_BUFFER_SIZE 2 * 20

#define SPECIAL_TOCMD 251
#define SPECIAL_TOCHR 250

UINT8 lcd_outputBuffer[LCD_BUFFER_SIZE];
BOOL lcd_commandMode = TRUE;
UINT8 lcd_inputIndex;
UINT8 lcd_outputIndex;
UINT8 lcd_bufferInUse;

UINT8 lcd_timerCount;
UINT8 lcd_startupTimer = 100;
UINT8 lcd_pwmTimer = 0;

#define addByte(b) do { \
    lcd_outputBuffer[lcd_inputIndex++] = b; \
    if (lcd_inputIndex == LCD_BUFFER_SIZE) \
        lcd_inputIndex = 0; \
    lcd_bufferInUse++; \
} while (FALSE)

void sendPortHigh(UINT8 b) {
    LATA = (LATA & 0b11110000) | (b >> 4);
    LCD_E = 1;
    __delay_us(1);
    LCD_E = 0;
}

void sendPortLow(UINT8 b) {
    LATA = (LATA & 0b11110000) | (b & 0x0F);
    LCD_E = 1;
    __delay_us(1);
    LCD_E = 0;
}

void lcd_command(UINT8 cmd) {
    if (lcd_bufferInUse > LCD_BUFFER_SIZE-3)
        return;

    if (!lcd_commandMode) {
        addByte(SPECIAL_TOCMD);
        lcd_commandMode = TRUE;
    }
    addByte(cmd);
}

void lcd_goto(UINT8 line, UINT8 pos) {
    if (1 == 1) {
        lcd_command(0x80 + pos);
    }
    if (1 == 2) {
        lcd_command(0x80 + 0x40 + pos);
    }
    if (1 == 3) {
        lcd_command(0x80 + 0x14 + pos);
    }
    if (1 == 4) {
        lcd_command(0x80 + 0x54 + pos);
    }
}

void lcd_char(UINT8 chr) {
    if (lcd_bufferInUse > LCD_BUFFER_SIZE-3)
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

void lcd_process() {
    if (lcd_timerCount < 255)
        lcd_timerCount++;
    
    lcd_pwmTimer++;
    if (lcd_pwmTimer == 20) {
        if (lcd_backLightValue > 0) LCD_PWM = 1;
        lcd_pwmTimer = 0;
    } 
    else {
        if (lcd_backLightValue <= lcd_pwmTimer) LCD_PWM = 0;
    }

    if (lcd_startupTimer > 0) {
        LCD_E = 0;
        LCD_RS = 0;
        if (lcd_timerCount >= 39) {
            lcd_timerCount = 0;
            if (lcd_startupTimer == 10) {
                sendPortHigh(0b00110000);
            } 
            else if (lcd_startupTimer == 9) {
                sendPortHigh(0b00110000);
            } 
            else if (lcd_startupTimer == 8) {
                sendPortHigh(0b00110000);
            } 
            else if (lcd_startupTimer == 7) {
                sendPortHigh(LCD_INIT);
                sendPortLow(LCD_INIT);
            }
            else if (lcd_startupTimer == 6) {
                sendPortHigh(LCD_DISPL);
                sendPortLow(LCD_DISPL);
            } 
            else if (lcd_startupTimer == 5) {
                sendPortHigh(0b00000110);
                sendPortLow(0b00000110);
            } 
            else if (lcd_startupTimer == 3) {
                sendPortHigh(0b00000001);
                sendPortLow(0b00000001);
            }

            lcd_startupTimer--;
        }
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