#include "bootloader_helper.h"
#include "iwdg.h"
#include <stm32f0xx_hal.h>
#include <stdint.h>

#define BOOTLOADER_MAGIC_ADDR ((uint32_t*) ((uint32_t) 0x20001000)) //4k into SRAM (out of 6k)
#define BOOTLOADER_MAGIC_TOKEN 0xDEADBEEF  // :D
 
//Value taken from CD00167594.pdf page 35, system memory start.
#define BOOTLOADER_START_ADDR 0x1FFFC400 //for ST32F042

//This is the first thing the micro runs after startup_stm32f0xx.s
//Only the SRAM is initialized at this point
void bootloaderSwitcher() {
	uint32_t jumpaddr, tmp;
 
	tmp = *BOOTLOADER_MAGIC_ADDR;
	if (tmp == BOOTLOADER_MAGIC_TOKEN) {
		*BOOTLOADER_MAGIC_ADDR = 0; //Zero it so we don't loop by accident
 
		// For the LQFP48 of STM32F042, the BOOT0 pin is at PF11, 
		// force it high so bootloader doesnt skip to main app
		__HAL_RCC_GPIOF_CLK_ENABLE();
		GPIO_InitTypeDef GPIO_InitStruct;
		GPIO_InitStruct.Pin = GPIO_PIN_11;
		GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
		GPIO_InitStruct.Pull = GPIO_PULLUP;
		GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
		HAL_GPIO_Init(GPIOF, &GPIO_InitStruct);
		HAL_GPIO_WritePin(GPIOF, GPIO_PIN_11, GPIO_PIN_SET);

		// setup IWDG so reset occurs after DFU mode exit
		// RCC->CSR |= (1 << 0);                                           // LSI enable, necessary for IWDG
		hiwdg.Instance = IWDG;
		hiwdg.Init.Prescaler = IWDG_PRESCALER_32; // 32KHz/32 --> iwdg @ 1khz
		hiwdg.Init.Window = 0x0FFF; // no window
		hiwdg.Init.Reload = 255; // 255ms
		HAL_IWDG_Init(&hiwdg);
				
		// jump into bootloader ROM
		jumpaddr = *(__IO uint32_t*)(BOOTLOADER_START_ADDR + 4);
		void(*bootloader)(void) = (void(*)(void)) jumpaddr;
		__set_MSP(*(__IO uint32_t*) BOOTLOADER_START_ADDR); // bye bye
		bootloader();
 
		//this should never be hit, trap for debugging
		while (1) {}
	}
}

void RebootToBootloader() {
	//call this at any time to initiate a reboot into bootloader
	*BOOTLOADER_MAGIC_ADDR = BOOTLOADER_MAGIC_TOKEN;
	NVIC_SystemReset();
}