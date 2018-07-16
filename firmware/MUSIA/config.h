#pragma once

#include "main.h"

 // no PF11 on STM32F072 for MENU_ALT_PIN for STM32F072, or MENU_PIN otherwise
#define READ_MENU_PIN() (((DBGMCU->IDCODE & 0xFFF) == 0x448) \
			? HAL_GPIO_ReadPin(MENU_ALT_GPIO_Port, MENU_ALT_Pin) \
			: HAL_GPIO_ReadPin(MENU_GPIO_Port, MENU_Pin))

#define BUTTON_COUNT 1
#define BUTTON_0 READ_MENU_PIN()
