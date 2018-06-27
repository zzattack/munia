#include "musia.h"
#include "ps2_poller.h"
#include "ps2_state.h"
#include "spi_sniffer.h"
#include "usb_joystick.h"
#include "spi.h"
#include "hal_spi.h"
#include <stm32f0xx_hal.h>

#include "usb_musia_device.h"
#include "ps2_controller_if.h"
#include "configurator_if.h"

#include "iwdg.h"
#include "usart.h"
#include "eeprom.h"

spi_sniffer sniffer(&hspi1, &hspi2);
ps2_state ps2State;
// ps2_poller poller(&hspi1);
usb_joystick usbJoy(&ps2State);
// eeprom ee;

void applyConfig();

EXTERNC void musia_init() {
#ifndef FAST_SEMIHOSTING_PROFILER_DRIVER
	RetargetInit(&huart1);
#endif
	hspi1.Instance = SPI1;
	hspi2.Instance = SPI2;
	// htim3.Instance = TIM3;
		
	__HAL_RCC_USB_CLK_ENABLE();
	NVIC_SetPriority(USB_IRQn, 0);
	NVIC_EnableIRQ(USB_IRQn);
	UsbDevice_Init();
}

EXTERNC int musia_main(void) {
	musia_init();
	sys_printf("MUSIA started\n");
	HAL_GPIO_WritePin(EE_CS_GPIO_Port, EE_CS_Pin, GPIO_PIN_SET);   // SET = disabled
	
	sniffer.configure();

	for (;;) {
		HAL_IWDG_Refresh(&hiwdg);
		//if (ee.data->mode == musia_mode::sniffer) {
			sniffer.work();
			auto* upd = sniffer.getNewPacket();			
			if (upd != nullptr) {
				ps2State.update(upd->cmd, upd->data, upd->pktLength);
				usbJoy.updateState();
				upd->isNew = false;
			}
		//}
		//else {
			// poller.work();
		//}
	}
}

void eeprom_select(spi_interface* spi_if) {
	hspi2.Instance = SPI2;
	hspi2.Init.Mode = SPI_MODE_MASTER;
	hspi2.Init.Direction = SPI_DIRECTION_2LINES;
	hspi2.Init.DataSize = SPI_DATASIZE_8BIT;
	hspi2.Init.CLKPolarity = SPI_POLARITY_LOW;
	hspi2.Init.CLKPhase = SPI_PHASE_1EDGE;
	hspi2.Init.NSS = SPI_NSS_SOFT;
	hspi2.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_64;
	hspi2.Init.FirstBit = SPI_FIRSTBIT_MSB;
	hspi2.Init.TIMode = SPI_TIMODE_DISABLE;
	hspi2.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspi2.Init.CRCPolynomial = 7;
	hspi2.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspi2.Init.NSSPMode = SPI_NSS_PULSE_ENABLE;
	HAL_SPI_Init(&hspi2);
}


EXTERNC void Configurator_SetReport(uint16_t setupValue, uint8_t * data, uint16_t length) {
	sys_printf("Configurator_SetReport\n");
}
EXTERNC void Configurator_GetReport(uint16_t setupValue) {
	sys_printf("Configurator_GetReport\n");
}