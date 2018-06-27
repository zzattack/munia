#pragma once


#include "eeprom_layout.h"
#include "25xx080.h"

class eeprom {
private:
	m25xx080* _if;
public:
	uint8_t eepBuf[EEPROM_SIZE];
	EELayout* data;
	
	eeprom(m25xx080* interface);
	void init();
	void sync();
	void fixChecksum();
	void reset();
};
