#include "spi_sniffer.h"
#include "gpio.h"
#include <stm32f0xx_hal.h>

spi_sniffer* gSniffer;

spi_sniffer::spi_sniffer(SPI_HandleTypeDef* hspiCmd, SPI_HandleTypeDef* hspiData) {
	gSniffer = this;
	this->hspiCmd = hspiCmd;
	this->hspiData = hspiData;
}

void spi_sniffer::configure() {
	// ## configure switches to sniffer mode; master=console, slave=joystick; SPI1 & SPI2 configured as slave
	// ## console MOSI connected to SPI1 MOSI, joystick connected to SPI2 MOSI
		
	// configure SPI1 as receive-only slave
	hspiCmd->Init.Mode = SPI_MODE_SLAVE;
	hspiCmd->Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
	hspiCmd->Init.DataSize = SPI_DATASIZE_8BIT;
	hspiCmd->Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspiCmd->Init.CLKPhase = SPI_PHASE_2EDGE;
	hspiCmd->Init.NSS = SPI_NSS_SOFT;
	hspiCmd->Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspiCmd->Init.TIMode = SPI_TIMODE_DISABLE;
	hspiCmd->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspiCmd->Init.CRCPolynomial = 7;
	hspiCmd->Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspiCmd->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspiCmd);
		
	// configure SPI2 as receive-only slave
	hspiData->Init.Mode = SPI_MODE_SLAVE;
	hspiData->Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
	hspiData->Init.DataSize = SPI_DATASIZE_8BIT;
	hspiData->Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspiData->Init.CLKPhase = SPI_PHASE_2EDGE;
	hspiData->Init.NSS = SPI_NSS_SOFT;
	hspiData->Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspiData->Init.TIMode = SPI_TIMODE_DISABLE;
	hspiData->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspiData->Init.CRCPolynomial = 7;
	hspiData->Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspiData->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspiData);
			
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET);         // RESET = console connected to joystick
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET);        // SET = pull up disabled
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_RESET);      // RESET = vibr mosfet disabled	
	HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_SET);      // SET = vibr mosfet enabled
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET);     // RESET = sniffer mode
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_SET);      // SET = sniffer mode
		
	HAL_NVIC_EnableIRQ(EXTI0_1_IRQn);   // on ATT pin
	// setup DMA's but don't start them yet
	HAL_SPI_Receive_DMA(hspiCmd, buffCmd, sizeof(buffCmd));
	HAL_SPI_Receive_DMA(hspiData, buffData, sizeof(buffData));
	HAL_SPI_MspInit(hspiCmd);
	HAL_SPI_MspInit(hspiData);
}

void spi_sniffer::work() {
	if (captureAvailable) {		
		uint32_t pos = __HAL_DMA_GET_COUNTER(hspiCmd->hdmarx);
		int i = 0;
		while (dmaIdxCmd != sizeof(buffCmd) - pos) {
			pkt.data[i++] = buffCmd[dmaIdxCmd++];
			if (dmaIdxCmd == sizeof(buffCmd)) dmaIdxCmd = 0;
		}
		pkt.pktLength = i;
				
		pos = __HAL_DMA_GET_COUNTER(hspiData->hdmarx);
		i = 0;
		while (dmaIdxData != sizeof(buffData) - pos) {
			pkt.cmd[i++] = buffData[dmaIdxData++];
			if (dmaIdxData == sizeof(buffData)) dmaIdxData = 0;
		}
		// ASSERT(pkt.pktLength == i);
		pkt.isNew = i > 0;
		captureAvailable = false;
	}
}

ps2_packet* spi_sniffer::getNewPacket() {
	if (pkt.isNew) return &pkt;
	else return nullptr;
}

void spi_sniffer::captureStart() {
	HAL_SPI_DMAResume(hspiCmd);
	HAL_SPI_DMAResume(hspiData);
	captureAvailable = false;
}
void spi_sniffer::captureEnd() {
	HAL_SPI_DMAPause(hspiCmd);
	HAL_SPI_DMAPause(hspiData);
	captureAvailable = true;
}

EXTERNC void EXTI0_1_IRQHandler() {
	__HAL_GPIO_EXTI_CLEAR_IT(JPS2_ATT_Pin);
	
	// when ATT pin toggles, this indicates either start or end of packet
	bool att = HAL_GPIO_ReadPin(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
	
	static uint16_t idxDMA1 = 0;
	static uint16_t idxDMA2 = 0;
	
	if (!att) {
		// start DMA sampling
		gSniffer->captureStart();
	}
	else {
		// just completed
		gSniffer->captureEnd();
	}
}
