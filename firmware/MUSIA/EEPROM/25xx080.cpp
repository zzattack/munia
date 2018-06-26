#include "25xx080.h"
#include <string.h>
#include <algorithm>

m25xx080::m25xx080(spi_interface* spi) : _spi(spi) {
}

bool m25xx080::selfTest() {
	bool ret = true;
	setWriteEnable(false);
	ret &= getStatus().WEL == false;
	setWriteEnable(true);
	ret &= getStatus().WEL == true;
	return ret;
}

M25xx080StatusRegister m25xx080::getStatus() const {
	_spi->clearCS();
	_spi->writeSPI(static_cast<uint8_t>(M25xx080Instruction::ReadStatusRegister));
	char c = _spi->writeSPI(0);
	M25xx080StatusRegister sr = *reinterpret_cast<M25xx080StatusRegister*>(&c);
	_spi->setCS();
	return sr;
}

bool m25xx080::isBusy() {
	return getStatus().WIP;
}

void m25xx080::read(uint16_t address, uint8_t* buffer, uint16_t length) {
	setWriteEnable(false);
	_spi->clearCS();
	_spi->writeSPI(static_cast<uint8_t>(M25xx080Instruction::Read));
	_spi->writeSPI(address >> 8);
	_spi->writeSPI(address >> 0);

	// entire memory can be read by single read instruction
	auto* write = new uint8_t[length];
	memset(write, 0, length);
	_spi->writeSPI(write, buffer, length);
	delete[] write;

	_spi->setCS();
}

void m25xx080::write(uint16_t address, const uint8_t* buffer, uint16_t length) {
	// must write in blocks of 16b at most, and be aligned on the same page
	unsigned int r = 0;
	for (int i = 0; i < length; ) {
		setWriteEnable(true); // must be repeated
		_spi->clearCS();
		_spi->writeSPI(static_cast<uint8_t>(M25xx080Instruction::Write));
		_spi->writeSPI(address >> 8);
		_spi->writeSPI(address >> 0);

		int cb;
		if ((address & 0x0F) != 0)
			// write is not page-aligned
			cb = std::min<uint16_t>(length, 0x10 - (address & 0x0F));
		else
			cb = length - i > 16 ? 16 : length - i;

		_spi->writeSPI(&buffer[r], nullptr, cb);
		r += cb;
		address += cb;
		i += cb;

		_spi->setCS();
		while (isBusy()) {}
	}
}

void m25xx080::setWriteEnable(bool enable) const
{
	_spi->clearCS();
	_spi->writeSPI(static_cast<uint8_t>(enable ? M25xx080Instruction::WriteEnable : M25xx080Instruction::WriteDisable));
	_spi->setCS();
}
