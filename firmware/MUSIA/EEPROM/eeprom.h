#pragma once


#include "eeprom_layout.h"
#include "25xx080.h"
extern uint8_t eepBuf[EEPROM_SIZE];
extern EELayout* eepData;

class eeprom {
private:
	m25xx080* _if;
public:
	eeprom(m25xx080* interface);
	void init();
	void sync();
	void fixChecksum();
	void reset();
};
