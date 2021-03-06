#include "lcd.h"
#include <stdlib.h>
#include <stdbool.h>
#include <stm32f0xx_hal.h>

void addByte(uint8_t b);
void lcd_pulseE();

bool lcd_commandMode = true;
uint16_t lcd_startupTimer;
CircularBuffer lcd_cb;

void sendPortLow(uint8_t b);
#define sendPortHigh(b) sendPortLow((b) >> 4)

void lcd_setup() {
	cbInit(&lcd_cb);
	lcd_startupTimer = 250;
	HAL_GPIO_WritePin(LCD_E_GPIO_Port, LCD_E_Pin, GPIO_PIN_RESET);
	HAL_GPIO_WritePin(LCD_RS_GPIO_Port, LCD_RS_Pin, GPIO_PIN_RESET);       
}

#define addByte(b) do { cbWrite(&lcd_cb, b); } while (0);

#pragma GCC push_options
#pragma GCC optimize ("O0")
void lcd_pulseE() { 
	// burn about 4us (scope aligned)
	volatile uint32_t counter = 100; while (counter--);
	HAL_GPIO_WritePin(LCD_E_GPIO_Port, LCD_E_Pin, GPIO_PIN_SET);
	counter = 100; while (counter--);
	HAL_GPIO_WritePin(LCD_E_GPIO_Port, LCD_E_Pin, GPIO_PIN_RESET);
}
#pragma GCC pop_options


void sendPortLow(uint8_t b) {
	HAL_GPIO_WritePin(LCD_D7_GPIO_Port, LCD_D7_Pin, (b & 8) ? GPIO_PIN_SET : GPIO_PIN_RESET);
	HAL_GPIO_WritePin(LCD_D6_GPIO_Port, LCD_D6_Pin, (b & 4) ? GPIO_PIN_SET : GPIO_PIN_RESET);
	HAL_GPIO_WritePin(LCD_D5_GPIO_Port, LCD_D5_Pin, (b & 2) ? GPIO_PIN_SET : GPIO_PIN_RESET);
	HAL_GPIO_WritePin(LCD_D4_GPIO_Port, LCD_D4_Pin, (b & 1) ? GPIO_PIN_SET : GPIO_PIN_RESET);
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
	while (*q)
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
	if (lcd_startupTimer > 0) {
		// timer is initialized at 200, which blocks lcd ops during the first 200ms
		if (lcd_startupTimer == 20) {
			sendPortHigh(0b00110000); // set interface to 8-bit for proper init
		} 
		else if (lcd_startupTimer == 15) { // 5 ms later just pulse E again
			lcd_pulseE();
		}
		else if (lcd_startupTimer == 12) { // 1 ms later again
			lcd_pulseE();
		} 
		else if (lcd_startupTimer == 6) { 
		    // 1 ms later, set to 4-bit mode
			sendPortHigh(LCD_INIT);
		} 
		else if (lcd_startupTimer == 4) {
		    // 4-bit, 2 lines, 5x8
			sendPortHigh(LCD_INIT); // confirm 4-bit mode
			sendPortLow(LCD_INIT);
		} 
		else if (lcd_startupTimer == 2) {
		    // display on, cursor off
			sendPortHigh(LCD_DISPLAY);
			sendPortLow(LCD_DISPLAY);
		}

		lcd_startupTimer--;
	} 
	else if (!cbIsEmpty(&lcd_cb)) {
		char c = cbRead(&lcd_cb);
		if (c == SPECIAL_TOCHR) {
			HAL_GPIO_WritePin(LCD_RS_GPIO_Port, LCD_RS_Pin, GPIO_PIN_SET);
		} 
		else if (c == SPECIAL_TOCMD) {
			HAL_GPIO_WritePin(LCD_RS_GPIO_Port, LCD_RS_Pin, GPIO_PIN_RESET);
		} 
		else {
			sendPortHigh(c);
			sendPortLow(c);
		}
	}
}
