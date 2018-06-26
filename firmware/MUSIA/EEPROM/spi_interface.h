#pragma once

#include <cstdint>

class spi_interface;
typedef void(*select_handler)(spi_interface* spi);

class spi_interface {
private:
	select_handler _selectHandler = nullptr;
public:
	virtual void setCS() = 0;
	virtual void clearCS() = 0;
	inline uint8_t next() { return writeSPI(0x00); }
	inline uint32_t next4() { uint32_t ret; writeSPI(nullptr, (uint8_t*)&ret, 4); return ret; }
	virtual uint8_t writeSPI(uint8_t c) = 0;
	virtual void writeSPI(const uint8_t* txBuff, uint8_t* rxBuff, uint16_t size) = 0;
	virtual void select() { if (_selectHandler != nullptr) _selectHandler(this); }
	virtual void setSelectHandler(select_handler handler) { _selectHandler = handler; }
};
