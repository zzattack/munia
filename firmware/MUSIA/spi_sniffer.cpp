#include "spi_sniffer.h"
#include "gpio.h"

spi_sniffer* gSniffer;

spi_sniffer::spi_sniffer(SPI_HandleTypeDef* hspiCmd, SPI_HandleTypeDef* hspiData, DMA_HandleTypeDef* hdmaCmd, DMA_HandleTypeDef* hdmaData) {
	gSniffer = this;
	this->hspiCmd = hspiCmd;
	this->hspiData = hspiData;
	this->hdmaCmd = hdmaCmd;
	this->hdmaData = hdmaData;
}

void spi_sniffer::init() {
	// ## configure switches to sniffer mode; master=console, slave=joystick; SPI1 & SPI2 configured as slave
	// ## console MOSI connected to SPI1 MOSI, joystick connected to SPI2 MOSI
		
	
    /**SPI1 GPIO Configuration    
    PA5     ------> SPI1_SCK
    PA6     ------> SPI1_MISO
    PA7     ------> SPI1_MOSI 
    */
	GPIO_InitTypeDef GPIO_InitStruct;
	GPIO_InitStruct.Pin = GPIO_PIN_5 | GPIO_PIN_6 | GPIO_PIN_7;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF0_SPI1;
	HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);
	
	
	// setup SPI1 peripheral
	hspiCmd->Init.Mode = SPI_MODE_SLAVE;
	hspiCmd->Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
	hspiCmd->Init.DataSize = SPI_DATASIZE_8BIT;
	hspiCmd->Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspiCmd->Init.CLKPhase = SPI_PHASE_2EDGE;
	hspiCmd->Init.NSS = SPI_NSS_SOFT;
	hspiCmd->Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspiCmd->Init.TIMode = SPI_TIMODE_DISABLE;
	hspiCmd->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspiCmd->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspiCmd);
	
		
	// setup DMA
	hdmaCmd->Init.Direction = DMA_PERIPH_TO_MEMORY;
	hdmaCmd->Init.PeriphInc = DMA_PINC_DISABLE;
	hdmaCmd->Init.MemInc = DMA_MINC_ENABLE;
	hdmaCmd->Init.PeriphDataAlignment = DMA_PDATAALIGN_BYTE;
	hdmaCmd->Init.MemDataAlignment = DMA_MDATAALIGN_BYTE;
	hdmaCmd->Init.Mode = DMA_CIRCULAR;
	hdmaCmd->Init.Priority = DMA_PRIORITY_HIGH;
	HAL_DMA_Init(hdmaCmd);
	__HAL_LINKDMA(hspiCmd, hdmarx, *hdmaCmd);
		
		
	// configure SPI2 as receive-only slave
    /**SPI2 GPIO Configuration    
    PB10     ------> SPI2_SCK
    PB14     ------> SPI2_MISO
    PB15     ------> SPI2_MOSI */
	GPIO_InitStruct.Pin = GPIO_PIN_10;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF5_SPI2;
	HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

	GPIO_InitStruct.Pin = GPIO_PIN_14 | GPIO_PIN_15;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF0_SPI2;
	HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);    
    
	// setup SPI peripheral
	hspiData->Init.Mode = SPI_MODE_SLAVE;
	hspiData->Init.Direction = SPI_DIRECTION_2LINES_RXONLY;
	hspiData->Init.DataSize = SPI_DATASIZE_8BIT;
	hspiData->Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspiData->Init.CLKPhase = SPI_PHASE_2EDGE;
	hspiData->Init.NSS = SPI_NSS_SOFT;
	hspiData->Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspiData->Init.TIMode = SPI_TIMODE_DISABLE;
	hspiData->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspiData->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspiData);
	
	
	// setup DMA
	hdmaData->Init.Direction = DMA_PERIPH_TO_MEMORY;
	hdmaData->Init.PeriphInc = DMA_PINC_DISABLE;
	hdmaData->Init.MemInc = DMA_MINC_ENABLE;
	hdmaData->Init.PeriphDataAlignment = DMA_PDATAALIGN_BYTE;
	hdmaData->Init.MemDataAlignment = DMA_MDATAALIGN_BYTE;
	hdmaData->Init.Mode = DMA_CIRCULAR;
	hdmaData->Init.Priority = DMA_PRIORITY_HIGH;
	HAL_DMA_Init(hdmaData);
	__HAL_LINKDMA(hspiData, hdmarx, *hdmaData);
	
	
	// console SPI lines + ATT are connected to joystick lines
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET); // RESET = console connected 
	
	// no pull-up on MISO line, console already has this
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET); // SET = pull up disabled
	
	// disconnect USB +5v from joystick
	HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_RESET); // RESET = vibr mosfet disabled	

	// connect PS2 8V vibration motor power source
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_SET); // SET = vibr mosfet enabled
	
	// connect joystick MISO to SPI2 MOSI
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET); // RESET = sniffer mode
	
	// connect joystick clock to SPI2 clock
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_SET); // SET = sniffer mode
	
	
	// configure ATT pin to interrupt
	GPIO_InitStruct.Pin = JPS2_ATT_Pin;
	GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING_FALLING;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	HAL_GPIO_Init(JPS2_ATT_GPIO_Port, &GPIO_InitStruct);
	
	// configure ACK pin as input
	GPIO_InitStruct.Pin = JPS2_ACK_Pin;
	GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	HAL_GPIO_Init(JPS2_ACK_GPIO_Port, &GPIO_InitStruct);
}

void spi_sniffer::deInit() {
	stop();
	HAL_GPIO_DeInit(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
	HAL_GPIO_DeInit(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin);

	__HAL_RCC_SPI1_CLK_DISABLE();	
	HAL_GPIO_DeInit(GPIOA, GPIO_PIN_5 | GPIO_PIN_6 | GPIO_PIN_7);
	HAL_DMA_DeInit(hspiCmd->hdmarx);
	__HAL_RCC_SPI2_CLK_DISABLE();
	HAL_GPIO_DeInit(GPIOB, GPIO_PIN_10 | GPIO_PIN_14 | GPIO_PIN_15);
	HAL_DMA_DeInit(hspiData->hdmarx);
}

void spi_sniffer::start() {
	__HAL_RCC_SPI1_CLK_ENABLE();
	__HAL_RCC_SPI2_CLK_ENABLE();
	__HAL_RCC_GPIOA_CLK_ENABLE();
	__HAL_RCC_GPIOB_CLK_ENABLE();
	HAL_NVIC_EnableIRQ(EXTI0_1_IRQn);   // on ATT pin
	// setup DMA's but don't start them yet
	HAL_SPI_Receive_DMA(hspiCmd, buffCmd, sizeof(buffCmd));
	HAL_SPI_Receive_DMA(hspiData, buffData, sizeof(buffData));
}

void spi_sniffer::stop() {
	HAL_NVIC_DisableIRQ(EXTI0_1_IRQn);	
	HAL_SPI_DMAStop(hspiCmd);	
	HAL_SPI_DMAStop(hspiData);
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

	__HAL_GPIO_EXTI_CLEAR_IT(JPS2_ATT_Pin);
}
