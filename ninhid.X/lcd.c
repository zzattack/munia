/******************************************************************************/
/* Files to Include                                                           */
/******************************************************************************/

#include "hardware.h"
#include "lcd.h"

/******************************************************************************/
/* Private variables                                                          */
/******************************************************************************/

#ifndef LCD_PRIVATE_BANK
  #define LCD_PRIVATE_BANK bank0
#endif

#if LCD_LINES==1
  #define LCD_INIT 0b00100000
#else
  #define LCD_INIT 0b00101000
#endif

#if LCD_CURSOR==1
  #if LCD_CURSORBLINK==1
    #define LCD_DISPL 0b00001111
  #else
    #define LCD_DISPL 0b00001110
  #endif
#else
  #define LCD_DISPL 0b00001100
#endif

#ifndef LCD_BUFFER_SIZE
  #define LCD_BUFFER_SIZE (LCD_LINES)*20
#endif

#define SPECIAL_TOCMD 251
#define SPECIAL_TOCHR 250

uint8_t LCD_PRIVATE_BANK lcd_outputBuffer[LCD_BUFFER_SIZE];
bool LCD_PRIVATE_BANK lcd_commandMode = true;
uint8_t LCD_PRIVATE_BANK lcd_inputIndex;
uint8_t LCD_PRIVATE_BANK lcd_outputIndex;
uint8_t LCD_PRIVATE_BANK lcd_bufferInUse;

uint8_t LCD_PRIVATE_BANK lcd_timerCount;
uint8_t LCD_PRIVATE_BANK lcd_startupTimer = 100;

#ifdef LCD_BACKLIGHT
uint8_t LCD_PRIVATE_BANK lcd_pwmTimer = 0;
#endif

#define addByte(b) do { \
    lcd_outputBuffer[lcd_inputIndex++] = b; \
    if (lcd_inputIndex == LCD_BUFFER_SIZE) \
        lcd_inputIndex = 0; \
    lcd_bufferInUse++; \
} while(false)

void sendPortHigh(uint8_t b) {
    uint8_t shadow = LCD_DATAPORT;
    shadow &= LCD_DATAPORTMASK;
    if (testbit(b, 4)) setbit(shadow, LCD_DATAPIN0);
    if (testbit(b, 5)) setbit(shadow, LCD_DATAPIN1);
    if (testbit(b, 6)) setbit(shadow, LCD_DATAPIN2);
    if (testbit(b, 7)) setbit(shadow, LCD_DATAPIN3);
    LCD_DATAPORT = shadow;
    LCD_ENABLE = 1;
    __delay_us(1);
    LCD_ENABLE = 0;
}

void sendPortLow(uint8_t b) {
    uint8_t shadow = LCD_DATAPORT;
    shadow &= LCD_DATAPORTMASK;
    if (testbit(b, 0)) setbit(shadow, LCD_DATAPIN0);
    if (testbit(b, 1)) setbit(shadow, LCD_DATAPIN1);
    if (testbit(b, 2)) setbit(shadow, LCD_DATAPIN2);
    if (testbit(b, 3)) setbit(shadow, LCD_DATAPIN3);
    LCD_DATAPORT = shadow;
    LCD_ENABLE = 1;
    __delay_us(1);
    LCD_ENABLE = 0;
}

void lcd_command(uint8_t cmd)
{
    if (lcd_bufferInUse > LCD_BUFFER_SIZE-3)
        return;

    if (!lcd_commandMode) {
        addByte(SPECIAL_TOCMD);
        lcd_commandMode = true;
    }
    addByte(cmd);
}

void lcd_goto(uint8_t line, uint8_t pos)
{
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

void lcd_char(uint8_t chr)
{
    if (lcd_bufferInUse > LCD_BUFFER_SIZE-3)
        return;

    if (lcd_commandMode) {
        addByte(SPECIAL_TOCHR);
        lcd_commandMode = false;
    }
    addByte(chr);
}

void lcd_string(const char *q)
{
    while(*q)
        lcd_char(*q++);
}

void lcd_process() {
    if (lcd_timerCount < 255)
        lcd_timerCount++;
    
#ifdef LCD_BACKLIGHT
    lcd_pwmTimer++;
    if (lcd_pwmTimer == 20) {
        if (lcd_backLightValue > 0) LCD_BACKLIGHT = 1;
        lcd_pwmTimer = 0;
    } else {
        if (lcd_backLightValue <= lcd_pwmTimer) LCD_BACKLIGHT = 0;
    }
#endif

    if (lcd_startupTimer > 0) {
        LCD_ENABLE = 0;
        LCD_RS = 0;
        if (lcd_timerCount >= 39) {
            lcd_timerCount = 0;
            if (lcd_startupTimer == 10) {
                sendPortHigh(0b00110000);
            } else if (lcd_startupTimer == 9) {
                sendPortHigh(0b00110000);
            } else if (lcd_startupTimer == 8) {
                sendPortHigh(0b00110000);
            } else if (lcd_startupTimer == 7) {
                sendPortHigh(LCD_INIT);
                sendPortLow(LCD_INIT);
            } else if (lcd_startupTimer == 6) {
                sendPortHigh(LCD_DISPL);
                sendPortLow(LCD_DISPL);
            } else if (lcd_startupTimer == 5) {
                sendPortHigh(0b00000110);
                sendPortLow(0b00000110);
            } else if (lcd_startupTimer == 3) {
                sendPortHigh(0b00000001);
                sendPortLow(0b00000001);
            }

            lcd_startupTimer--;
        }
    } else if (lcd_bufferInUse > 0 && lcd_timerCount == 255) {
        if (lcd_outputBuffer[lcd_outputIndex] < 4) {
            lcd_timerCount = 210;
        }
        if (lcd_outputBuffer[lcd_outputIndex] == SPECIAL_TOCHR) {
            LCD_RS = 1;
        } else if (lcd_outputBuffer[lcd_outputIndex] == SPECIAL_TOCMD) {
            LCD_RS = 0;
        } else {
            sendPortHigh(lcd_outputBuffer[lcd_outputIndex]);
            sendPortLow(lcd_outputBuffer[lcd_outputIndex]);
        }
        lcd_outputIndex++;
        if (lcd_outputIndex == LCD_BUFFER_SIZE)
            lcd_outputIndex = 0;
        lcd_bufferInUse--;
    }
}