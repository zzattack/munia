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
	
	__HAL_RCC_SPI1_CLK_ENABLE();
	__HAL_RCC_SPI2_CLK_ENABLE();
	
	
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
	HAL_SPI_Init(hspiCmd);

	
	
	
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
	HAL_SPI_Init(hspiData);
	
	
	// console SPI lines + ATT are connected to joystick lines
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET); // RESET = console connected 
	
	// no pull-up on MISO line, console already has this
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_SET); // SET = pull up disabled

	// connect PS2 8V vibration motor power source
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_SET); // SET = vibr mosfet enabled
	
	// connect joystick MISO to SPI2 MOSI
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_SET); // SET = sniffer mode
	
	// connect joystick clock to SPI2 clock
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET); // RESET = sniffer mode
	
	
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

	HAL_GPIO_DeInit(GPIOA, GPIO_PIN_5 | GPIO_PIN_6 | GPIO_PIN_7);
	HAL_DMA_DeInit(hspiCmd->hdmarx);
	HAL_SPI_DeInit(hspiCmd);
	
	HAL_GPIO_DeInit(GPIOB, GPIO_PIN_10 | GPIO_PIN_14 | GPIO_PIN_15);
	HAL_DMA_DeInit(hspiData->hdmarx);
	HAL_SPI_DeInit(hspiData);
	
	__HAL_RCC_SPI1_CLK_DISABLE();
	__HAL_RCC_SPI2_CLK_DISABLE();
}


void spi_sniffer::start() {	
	HAL_NVIC_EnableIRQ(EXTI0_1_IRQn);   // on ATT pin
	// setup DMA's but don't start them yet
	HAL_SPI_Receive_DMA(hspiCmd, buffCmd, sizeof(buffCmd));
	HAL_SPI_Receive_DMA(hspiData, buffData, sizeof(buffData));
}

void spi_sniffer::stop() {
	HAL_NVIC_DisableIRQ(EXTI0_1_IRQn);
	captureEnd();
	captureAvailable = false;
	pkt.isNew = false;
	HAL_SPI_DMAStop(hspiCmd);	
	HAL_SPI_DMAStop(hspiData);	
}

void spi_sniffer::work() {
	if (captureAvailable) {
		uint32_t posCmd  = __HAL_DMA_GET_COUNTER(hspiCmd->hdmarx);
		uint32_t posData = __HAL_DMA_GET_COUNTER(hspiData->hdmarx);

		// if too much data in buffer, discard
		int tgtIdxCmd = sizeof(buffCmd) - posCmd;
		int dataCount = dmaIdxCmd < tgtIdxCmd ? tgtIdxCmd - dmaIdxCmd : tgtIdxCmd + sizeof(buffCmd) - dmaIdxCmd;
		if (dataCount > sizeof(pkt.cmd)) 
			dmaIdxCmd = tgtIdxCmd;
		
		uint i = 0;
		while (dmaIdxCmd != tgtIdxCmd) {
			pkt.cmd[i++] = buffCmd[dmaIdxCmd++];
			if (dmaIdxCmd == sizeof(buffCmd)) dmaIdxCmd = 0;
		}
		pkt.pktLength = i;
		// spi_printf("CMD:  pos=%d, tgt=%d, cnt=%d, pkt.pktLength=%d\n", pos, tgtIdxCmd, dataCount, i);
				
		// if too much data in buffer, discard
		i = 0;
		int tgtIdxData = sizeof(buffData) - posData;
		dataCount = dmaIdxData < tgtIdxData ? tgtIdxData - dmaIdxData : tgtIdxData + sizeof(buffData) - dmaIdxData;
		if (dataCount > sizeof(pkt.data)) 
			dmaIdxData = tgtIdxData;

		while (dmaIdxData != tgtIdxData) {
			pkt.data[i++] = buffData[dmaIdxData++];
			if (dmaIdxData == sizeof(buffData)) dmaIdxData = 0;
		}
		// minimum packet length is 5 and both cmd and data should be of equal length
		pkt.isNew = i >= 5 && pkt.pktLength == i;

		// sys_printf("captureAvailable=true, pkt.isNew=%d, data len=%d, cmd len=%d\n", pkt.isNew ? 1 : 0, pkt.pktLength, i);
		captureAvailable = false;
	}
}

ps2_packet* spi_sniffer::getNewPacket() {
	if (pkt.isNew) return &pkt;
	else return nullptr;
}

void spi_sniffer::captureStart() {
	dmaIdxCmd = sizeof(buffCmd) - __HAL_DMA_GET_COUNTER(hspiCmd->hdmarx);
	dmaIdxData = sizeof(buffData) - __HAL_DMA_GET_COUNTER(hspiData->hdmarx);
	captureAvailable = false;
}
void spi_sniffer::captureEnd() {
	captureAvailable = true;
}

#define WAIT_EDGE(PORT,PIN,TIMEOUT, EDGE) do { \
	int to = TIMEOUT + HAL_GetTick(); \
	while (HAL_GetTick() < to && HAL_GPIO_ReadPin(PORT, PIN) != EDGE); \
	} while (0); 

#define WAIT_RISING_EDGE(PORT,PIN,TIMEOUT)   WAIT_EDGE(PORT,PIN,TIMEOUT,GPIO_PIN_SET)
#define WAIT_FALLING_EDGE(PORT,PIN,TIMEOUT)  WAIT_EDGE(PORT,PIN,TIMEOUT,GPIO_PIN_RESET)

void spi_sniffer::resync(bool hard) {
	ps2_printf("resync attempt\n");
	stop();
	
	// wait for ATT to toggle, ending high, indicating console clock pauses
	WAIT_RISING_EDGE(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin, 10);
	WAIT_FALLING_EDGE(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin, 10);
	WAIT_RISING_EDGE(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin, 10);
	// now is a good time to resync
	
	// disconnect SPI1 clock from console
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_SET);
	// disconnect SPI2 clock from joystick
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_SET);
	
	// should have plenty of time to restart now
	if (hard) {
		// hard resync tells us to fully disable all of the SPI peripherals,
		deInit();
		init();
	}
	else {
		// but usually just reconnecting the clocks is already enough
		HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_RESET);
		HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET);
	}
	start();

	ps2_printf("resync attempted!\n");
}

EXTERNC void EXTI0_1_IRQHandler() {
	// when ATT pin toggles, this indicates either start or end of packet
	bool att = HAL_GPIO_ReadPin(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
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
