#pragma once

#include "spi_interface.h"

enum class M25xx080Instruction {
	Read = 0x03,
	Write = 0x02,
	WriteDisable = 0x04,
	WriteEnable = 0x06,
	ReadStatusRegister = 0x05,
	WriteStatusRegister = 0x01,
};

typedef struct __attribute__((__packed__)) {
	uint8_t WIP : 1; // 0=device in standby, 1=device busy with program or erase,
	uint8_t WEL : 1; // 0=write disabled, cannot program or erase, 1=write unlocked
	uint8_t BP0 : 1; // range of sectors 0 protected from program/erase
	uint8_t BP1 : 1; // range of sectors 1 protected from program/erase
	uint8_t Unused : 3;
	uint8_t WPEN : 1; // 
} M25xx080StatusRegister; // status register 1 non-volatile


class m25xx080 {
private:
	spi_interface* _spi;
	bool isBusy();
	M25xx080StatusRegister getStatus() const;
public:
	m25xx080(spi_interface* spi);
	bool selfTest();
	void read(uint16_t address, uint8_t* buffer, uint16_t length);
	void write(uint16_t address, const uint8_t* buffer, uint16_t length);
	void setWriteEnable(bool enable) const;

	spi_interface* getInterface() const { return _spi; }
};
