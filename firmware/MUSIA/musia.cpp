#include "musia.h"
#include "ps2_poller.h"
#include "ps2_state.h"
#include "spi_sniffer.h"
#include "usb_joystick.h"
#include "hal_spi.h"
#include <stm32f0xx_hal.h>

#include "usb_musia_device.h"
#include "ps2_controller_if.h"
#include "configurator_if.h"

#include "iwdg.h"
#include "usart.h"
#include "eeprom.h"


SPI_HandleTypeDef hspi1;
SPI_HandleTypeDef hspi2;
DMA_HandleTypeDef hdma_spi1_rx;
DMA_HandleTypeDef hdma_spi2_rx;

hal_spi_interface spi_ps2(&hspi1, JPS2_ATT_GPIO_Port, JPS2_ATT_Pin); // in driver mode
hal_spi_interface spi_ee(&hspi2, EE_CS_GPIO_Port, EE_CS_Pin);
m25xx080 m25xx080(&spi_ee); // eeprom for configuration retaining
eeprom ee(&m25xx080);

spi_sniffer sniffer(&hspi2, &hspi1, &hdma_spi2_rx, &hdma_spi1_rx); // sniff on both spi lines configured as slave inputs
ps2_poller poller(&spi_ps2); // polls on spi1 when not in sniffer mode
ps2_state ps2State; // tracks controller state, read by USB and updated either by sniffer or poller
usb_joystick usbJoy; // transmits USB HID packets containing ps2 state

extern TIM_HandleTypeDef htim3;

void sysInit();
void applyConfig();
void handleSnifferPacket();
void handlePollerPacket();
void eepromSelect(spi_interface* spi_if);

EXTERNC int musia_main(void) {
	sysInit();
	sys_printf("MUSIA started\n");
	
	spi_ee.setSelectHandler(eepromSelect);
	ee.init(); // load eeprom
	applyConfig();
	
	for (;;) {
		HAL_IWDG_Refresh(&hiwdg);
		
		if (ee.data->mode == musia_mode::sniffer) {
			sniffer.work();
			handleSnifferPacket();
		}
		else {
			poller.work();
			handlePollerPacket();
		}
		
		__WFI();
		// USB is fully based off interrupts,
		// poller is based off timer interrupt,
		// sniffer is based off EXTI interrupt
	}
}

void sysInit() {
	// initialize HAL instances that we elected for CubeMX not to initialize
	hspi1.Instance = SPI1;
	hspi2.Instance = SPI2;
	htim3.Instance = TIM3;
	hdma_spi1_rx.Instance = DMA1_Channel2;
	hdma_spi2_rx.Instance = DMA1_Channel4;
	
	HAL_NVIC_DisableIRQ(USB_IRQn); 
	__HAL_RCC_USB_FORCE_RESET();
	__HAL_RCC_USB_RELEASE_RESET();
	__HAL_RCC_USB_CLK_ENABLE();
	NVIC_SetPriority(USB_IRQn, 0);
	NVIC_EnableIRQ(USB_IRQn);
	UsbDevice_Init();
}

void applyConfig() {
	if (ee.data->mode == musia_mode::sniffer) {
		poller.deInit();
		sniffer.init();
		sniffer.start();
	}
	else {
		sniffer.deInit();
		poller.init();
		poller.start(polling_interval::poll25Hz);
	}
}

void handleSnifferPacket() {
	static uint8_t resyncDetect = 0, resyncFail = 0;
	static bool prevWasValid = false;
	
	auto* upd = sniffer.getNewPacket();			
	if (upd == nullptr) return;
			
	bool validPacket = ps2State.update(upd->cmd, upd->data, upd->pktLength);
	if (!validPacket) {
		// sys_printf("INVALID PACKET RECEIVED, RESYNC CTR: %d\n", resyncDetect);
		// ps2_state::print_packet(upd->cmd, upd->data, upd->pktLength);
		resyncDetect++;
		if (resyncDetect == 10) {
			sniffer.resync(resyncFail > 10); 
		}
		else if (resyncDetect > 10) {
			resyncDetect = 0; 
			resyncFail++;
		}
	}
	else {
		if (!prevWasValid) sys_printf("Sniffer resynched successfully\n");
		resyncDetect = 0;
		resyncFail = 0;
		usbJoy.updateState(&ps2State);
	}
	
	upd->isNew = false;
	prevWasValid = validPacket;
}

void handlePollerPacket() {
	static uint8_t resyncDetect = 0, resyncFail = 0;
	static bool prevWasValid = false;

	auto* upd = poller.getNewPacket();			
	if (upd == nullptr) return;
			
	bool validPacket = ps2State.update(upd->cmd, upd->data, upd->pktLength);
	if (!validPacket) {
		// sys_printf("INVALID PACKET RECEIVED, RESYNC CTR: %d\n", 0);
		// ps2_state::print_packet(upd->cmd, upd->data, upd->pktLength);
		resyncDetect++;
		if (resyncDetect == 10) {
			poller.resync(resyncFail > 10); 
		}
		else if (resyncDetect > 10) {
			resyncDetect = 0; 
			resyncFail++;
		}
	}
	else {
		if (!prevWasValid) sys_printf("Poller resynched successfully\n");
		resyncDetect = 0;
		resyncFail = 0;
		usbJoy.updateState(&ps2State);
	}
	upd->isNew = false;
	prevWasValid = validPacket;
}

void eepromSelect(spi_interface* spi_if) {
	__HAL_RCC_SPI2_CLK_ENABLE();
	
	// configure SPI2 as master
    /**SPI2 GPIO Configuration    
    PB10     ------> SPI2_SCK
    PB14     ------> SPI2_MISO
    PB15     ------> SPI2_MOSI */
	GPIO_InitTypeDef GPIO_InitStruct;
	GPIO_InitStruct.Pin = GPIO_PIN_10;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF5_SPI2;
	HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

	GPIO_InitStruct.Pin = GPIO_PIN_14 | GPIO_PIN_15;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF0_SPI2;
	HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);    
	
	hspi2.Init.Mode = SPI_MODE_MASTER;
	hspi2.Init.Direction = SPI_DIRECTION_2LINES;
	hspi2.Init.DataSize = SPI_DATASIZE_8BIT;
	hspi2.Init.CLKPolarity = SPI_POLARITY_LOW;
	hspi2.Init.CLKPhase = SPI_PHASE_1EDGE;
	hspi2.Init.NSS = SPI_NSS_SOFT;
	hspi2.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_64;
	hspi2.Init.FirstBit = SPI_FIRSTBIT_MSB;
	hspi2.Init.TIMode = SPI_TIMODE_DISABLE;
	hspi2.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspi2.Init.CRCPolynomial = 7;
	hspi2.Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspi2.Init.NSSPMode = SPI_NSS_PULSE_ENABLE;
	HAL_SPI_Init(&hspi2);
}

