#pragma once

struct ps2_packet {
	bool isNew;
	uint8_t pktLength;
	uint8_t cmd[64];
	uint8_t data[64];
};

class spi_sniffer {
private:
	bool captureAvailable;
	ps2_packet pkt;	
	SPI_HandleTypeDef* hspiCmd;
	SPI_HandleTypeDef* hspiData;
	
	uint8_t buffData[64];
	uint8_t buffCmd[64];
	int dmaIdxCmd = 0, dmaIdxData = 0;
public:
	spi_sniffer(SPI_HandleTypeDef* hspiCmd, SPI_HandleTypeDef* hspiData);
	void configure();
	void work();
	ps2_packet* getNewPacket();
	
	void captureStart();
	void captureEnd();
};