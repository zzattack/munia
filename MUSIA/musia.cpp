#include <stm32f0xx_hal.h>
#include "iwdg.h"

extern "C" int musia_main(void) {

	// reset console switch /OE
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET); // RESET = console connected to joystick, SET = disconnected
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET); // SET = pull up disabled, RESET = disabled
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_SET); // SET = vibr mosfet enabled, RESET = disabled	
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_RESET); // RESET = sniffer mode, SET = driver mode
	HAL_GPIO_WritePin(EE_CS_GPIO_Port, EE_CS_Pin, GPIO_PIN_SET); // SET = disabled, RESET = enabled

	
	
	for (;;) {
		HAL_IWDG_Refresh(&hiwdg);
	}
}
