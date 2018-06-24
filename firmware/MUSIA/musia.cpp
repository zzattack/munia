#include <stm32f0xx_hal.h>
#include "iwdg.h"
#include "usart.h"
#include "spi.h"
#include "dma.h"
#include "retarget.h"

bool dmaCaptureAvailable = false;
uint8_t circBuffSPI1[64];
uint8_t circBuffSPI2[64];
extern DMA_HandleTypeDef hdma_spi1_rx;
extern DMA_HandleTypeDef hdma_spi2_rx;

uint8_t buffSPI1[64];
uint8_t buffSPI2[64];
int dma1CaptureCount = 0;
int dma2CaptureCount = 0;

void configure(bool snifferMode) {	
	if (snifferMode) {
		// ## configure switches to sniffer mode; master=console, slave=joystick; SPI1 & SPI2 configured as slave
		// ## console MOSI connected to SPI1 MOSI, joystick connected to SPI2 MOSI
		
		// configure SPI1 as receive-only slave
		hspi1.Instance = SPI1;
		hspi1.Init.Mode = SPI_MODE_SLAVE;
		hspi1.Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
		hspi1.Init.DataSize = SPI_DATASIZE_8BIT;
		hspi1.Init.CLKPolarity = SPI_POLARITY_HIGH;
		hspi1.Init.CLKPhase = SPI_PHASE_2EDGE;
		hspi1.Init.NSS = SPI_NSS_SOFT;
		hspi1.Init.FirstBit = SPI_FIRSTBIT_LSB;
		hspi1.Init.TIMode = SPI_TIMODE_DISABLE;
		hspi1.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
		hspi1.Init.CRCPolynomial = 7;
		hspi1.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
		hspi1.Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
		HAL_SPI_Init(&hspi1);
		
		// configure SPI2 as receive-only slave
		hspi2.Instance = SPI2;
		hspi2.Init.Mode = SPI_MODE_SLAVE;
		hspi2.Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
		hspi2.Init.DataSize = SPI_DATASIZE_8BIT;
		hspi2.Init.CLKPolarity = SPI_POLARITY_HIGH;
		hspi2.Init.CLKPhase = SPI_PHASE_2EDGE;
		hspi2.Init.NSS = SPI_NSS_SOFT;
		hspi2.Init.FirstBit = SPI_FIRSTBIT_LSB;
		hspi2.Init.TIMode = SPI_TIMODE_DISABLE;
		hspi2.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
		hspi2.Init.CRCPolynomial = 7;
		hspi2.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
		hspi2.Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
		HAL_SPI_Init(&hspi2);
			
		HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET);       // RESET = console connected to joystick
		HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET);      // SET = pull up disabled
		HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_RESET);    // RESET = vibr mosfet disabled	
		HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_SET);    // SET = vibr mosfet enabled
		HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET);   // RESET = sniffer mode
		HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_SET);    // SET = sniffer mode
		
		HAL_NVIC_EnableIRQ(EXTI0_1_IRQn); // on ATT pin		
		HAL_SPI_Receive_DMA(&hspi1, circBuffSPI1, sizeof(circBuffSPI1));
		HAL_SPI_Receive_DMA(&hspi2, circBuffSPI2, sizeof(circBuffSPI2));
		HAL_SPI_MspInit(&hspi1);
		HAL_SPI_MspInit(&hspi2);
	}
	
	else {
		// ## configure switches to MCU driver mode; master=msu, slave=joystick; all on SPI1
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
		HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET);  // SET = console disconnected
		
		// enable pull-up on MISO line in case joystick is not connected
		HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET);  // RESET = pull enabled
		
		// break connection between PS2 8V vibration motor power source
		HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_RESET);  // RESET = vibr mosfet disabled	
		
		// connect +5V from USB to joystick vibration motor
		HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_SET);  // SET = vibr mosfet enabled
		
		// connect joystick MISO to SPI1 MISO
		HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_RESET); // RESET = driver mode
		
		// disconnect joystick clock from SPI2 clock
		HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_SET); // SET = disconnected from ps2 clock
		
		HAL_NVIC_DisableIRQ(EXTI0_1_IRQn);  // on ATT pin
		
		HAL_SPI_DMAStop(&hspi1);
		HAL_SPI_DMAStop(&hspi2);
		HAL_SPI_MspDeInit(&hspi1);
		HAL_SPI_MspDeInit(&hspi2);
	}	
}


EXTERNC int musia_main(void) {
#ifndef FAST_SEMIHOSTING_PROFILER_DRIVER
	RetargetInit(&huart1);
#endif

	// sys_printf("MUSIA started\n");
	HAL_GPIO_WritePin(EE_CS_GPIO_Port, EE_CS_Pin, GPIO_PIN_SET);   // SET = disabled

	configure(true);  // enter sniffer mode
	for(;;) {
		HAL_IWDG_Refresh(&hiwdg);
	
		if (dmaCaptureAvailable) {
			/*sys_printf("Captured %d (%d) bytes: \n", dma1CaptureCount, dma2CaptureCount);
			printf("\t MISO: ");
			for (int i = 0; i < dma1CaptureCount; i++)
				printf("%02X ", buffSPI1[i]);
			printf("\n\t MOSI: ");
			for (int i = 0; i < dma2CaptureCount; i++)
				printf("%02X ", buffSPI2[i]);
			printf("\n");*/
			// consume
			dmaCaptureAvailable = false;
		}
	}
}

EXTERNC void EXTI0_1_IRQHandler(void) {
	__HAL_GPIO_EXTI_CLEAR_IT(JPS2_ATT_Pin);
	
	// when ATT pin toggles, this indicates either start or end of packet
	bool att = HAL_GPIO_ReadPin(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
	
	static uint16_t idxDMA1 = 0;
	static uint16_t idxDMA2 = 0;
	
	if (!att) {
		// start DMA sampling
		dmaCaptureAvailable = false;
		dma1CaptureCount = dma2CaptureCount = 0;
		
		HAL_SPI_DMAResume(&hspi1);
		HAL_SPI_DMAResume(&hspi2);
	}
	else {		
		HAL_SPI_DMAPause(&hspi1);
		HAL_SPI_DMAPause(&hspi2);

		uint32_t pos = __HAL_DMA_GET_COUNTER(&hdma_spi1_rx);
		int i = 0;
		while (idxDMA1 != sizeof(circBuffSPI1) - pos) {
			buffSPI1[i++] = circBuffSPI1[idxDMA1++];
			if (idxDMA1 == sizeof(buffSPI1)) idxDMA1 = 0;
		}
		dma1CaptureCount = i;
				
		pos = __HAL_DMA_GET_COUNTER(&hdma_spi2_rx);
		i = 0;
		while (idxDMA2 != sizeof(circBuffSPI2) - pos) {
			buffSPI2[i++] = circBuffSPI2[idxDMA2++];
			if (idxDMA2 == sizeof(buffSPI2)) idxDMA2 = 0;
		}
		dma2CaptureCount = i;
		
		dmaCaptureAvailable = true;
	}
}