#pragma once

#include <stm32f0xx_hal.h>
#include "ps2_packet.h"

class spi_sniffer {
private:
	bool captureAvailable;
	ps2_packet pkt;	
	SPI_HandleTypeDef* hspiCmd;
	SPI_HandleTypeDef* hspiData;
	DMA_HandleTypeDef* hdmaCmd;
	DMA_HandleTypeDef* hdmaData;
	
	uint8_t buffData[64];
	uint8_t buffCmd[64];
	int dmaIdxCmd = 0, dmaIdxData = 0;
public:
	spi_sniffer(SPI_HandleTypeDef* hspiCmd, SPI_HandleTypeDef* hspiData, DMA_HandleTypeDef* hdmaCmd, DMA_HandleTypeDef* hdmaData);
	void init();
	void deInit();
	void start();
	void stop();
	void work();
	ps2_packet* getNewPacket();
	
	void captureStart();
	void captureEnd();
	void resync(bool hard);
};