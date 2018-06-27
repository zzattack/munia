#include "eeprom.h"
#include "crc.h"
#include <string.h>


eeprom::eeprom(m25xx080* interface) : _if(interface) {
	this->data = (EELayout*)this->eepBuf;
}

void eeprom::init() {
	_if->getInterface()->select();

	// first 2 uint8_ts is EEPROM checksum
	uint16_t stored_crc;
	_if->read(0x000, reinterpret_cast<uint8_t*>(&stored_crc), 2);
	// rest is data
	_if->read(0x002, eepBuf, EEPROM_SIZE);
	
	uint16_t calculated_crc = crc16(eepBuf, 0, EEPROM_SIZE);

	if (stored_crc != calculated_crc) {
		sys_printf("EEPROM reset\n");
		reset();
	}
	else sys_printf("EEPROM has valid CRC 0x%04X\n", calculated_crc);
}

void eeprom::sync() {
	sys_printf("eeprom::sync();\n");
	
	_if->getInterface()->select();
	uint16_t stored_crc;
	_if->read(0x000, reinterpret_cast<uint8_t*>(&stored_crc), 2);

	uint16_t crc = crc16(eepBuf, 0, EEPROM_SIZE);
	if (crc != stored_crc) {
		// update if needed only
		_if->write(0x000, reinterpret_cast<uint8_t*>(&crc), 2);
		_if->write(0x002, eepBuf, EEPROM_SIZE);
	}
}

void eeprom::fixChecksum() {
	_if->getInterface()->select();
	uint8_t payload[EEPROM_SIZE];
	_if->read(0x002, payload, EEPROM_SIZE);
	uint16_t crc = crc16(payload, 0, EEPROM_SIZE);
	_if->write(0x000, reinterpret_cast<uint8_t*>(&crc), 2);
}

void eeprom::reset() {
	memcpy(this->eepBuf, &FactoryEEPROM, sizeof(FactoryEEPROM));
	sync();
}
