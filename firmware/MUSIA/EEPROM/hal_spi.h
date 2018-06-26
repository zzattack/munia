#pragma once

#include "spi_interface.h"
#include <stm32f0xx_hal.h>

class hal_spi_interface : public spi_interface {
private:
	SPI_HandleTypeDef* hspi;
	GPIO_TypeDef* gpio;
	uint16_t gpio_pin;

public:
	hal_spi_interface(SPI_HandleTypeDef* hspi, GPIO_TypeDef* gpio, uint16_t gpio_pin);
	void setCS();
	void clearCS();
	uint8_t writeSPI(uint8_t c);
	void writeSPI(const uint8_t* txBuff, uint8_t* rxBuff, uint16_t size);
};
