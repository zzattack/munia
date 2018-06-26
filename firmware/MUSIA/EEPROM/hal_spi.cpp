#include "hal_spi.h"
#include "spi.h"

hal_spi_interface::hal_spi_interface(SPI_HandleTypeDef* hspi, GPIO_TypeDef* gpio, uint16_t gpio_pin)
	: hspi(hspi), gpio(gpio), gpio_pin(gpio_pin) {
}

void hal_spi_interface::setCS() {
	HAL_GPIO_WritePin(gpio, gpio_pin, GPIO_PIN_SET);
}

void hal_spi_interface::clearCS() {
	HAL_GPIO_WritePin(gpio, gpio_pin, GPIO_PIN_RESET);
}

uint8_t hal_spi_interface::writeSPI(uint8_t data) {
	uint8_t res = 0;
	HAL_SPI_TransmitReceive(hspi, &data, &res, 1, 200);
	return res;
}

void hal_spi_interface::writeSPI(const uint8_t* txBuff, uint8_t* rxBuff, uint16_t size) {
	bool txDel = false;
	uint8_t* tx, *rx;
	
	if (txBuff == nullptr) {
		tx = new uint8_t[size];
		txDel = true;
	}
	else 
		tx = const_cast<uint8_t*>(txBuff);
	
	bool rxDel = false;
	if (rxBuff == nullptr) {
		rx = new uint8_t[size];
		rxDel = true;
	}
	else rx = rxBuff;
	
	HAL_SPI_TransmitReceive(hspi, tx, rx, size, HAL_MAX_DELAY);
	if (txDel) delete[] tx;
	if (rxDel) delete[] rx;
}
