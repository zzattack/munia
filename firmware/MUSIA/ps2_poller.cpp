#include "ps2_poller.h"
#include "spi.h"
#include "dma.h"

void ps2_poller::configure() {
	hspi1.Instance = SPI1;
	hspi1.Init.Mode = SPI_MODE_MASTER;
	hspi1.Init.Direction = SPI_DIRECTION_2LINES;
	hspi1.Init.DataSize = SPI_DATASIZE_8BIT;
	hspi1.Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspi1.Init.CLKPhase = SPI_PHASE_2EDGE;
	hspi1.Init.NSS = SPI_NSS_SOFT;
	hspi1.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_64;
	hspi1.Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspi1.Init.TIMode = SPI_TIMODE_DISABLE;
	hspi1.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspi1.Init.CRCPolynomial = 7;
	hspi1.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspi1.Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(&hspi1);

	hspi1.Instance = SPI2;
	hspi2.Init.Mode = SPI_MODE_MASTER;
	hspi2.Init.Direction = SPI_DIRECTION_2LINES;
	hspi2.Init.DataSize = SPI_DATASIZE_8BIT;
	hspi2.Init.CLKPolarity = SPI_POLARITY_LOW;
	hspi2.Init.CLKPhase = SPI_PHASE_1EDGE;
	hspi2.Init.NSS = SPI_NSS_SOFT;
	hspi2.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_32;
	hspi2.Init.FirstBit = SPI_FIRSTBIT_MSB;
	hspi2.Init.TIMode = SPI_TIMODE_DISABLE;
	hspi2.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspi2.Init.CRCPolynomial = 7;
	hspi2.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspi2.Init.NSSPMode = SPI_NSS_PULSE_ENABLE;
	HAL_SPI_Init(&hspi2);

	// console SPI lines + ATT are disconnected from corresponding joystick lines
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET);   // SET = console disconnected
		
	// enable pull-up on MISO line in case joystick is not connected
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET);   // RESET = pull enabled
		
	// break connection between PS2 8V vibration motor power source
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_RESET);   // RESET = vibr mosfet disabled	
		
	// connect +5V from USB to joystick vibration motor
	HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_SET);   // SET = vibr mosfet enabled
		
	// connect joystick MISO to SPI1 MISO
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_RESET);  // RESET = driver mode
		
	// disconnect joystick clock from SPI2 clock
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_SET);  // SET = disconnected from ps2 clock
		
	HAL_NVIC_DisableIRQ(EXTI0_1_IRQn);   // on ATT pin
		
	HAL_SPI_DMAStop(&hspi1);
	HAL_SPI_DMAStop(&hspi2);
	HAL_SPI_MspDeInit(&hspi1);
	HAL_SPI_MspDeInit(&hspi2);	
}

void ps2_poller::work() {
}