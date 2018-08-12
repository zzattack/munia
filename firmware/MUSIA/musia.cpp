// #define WITHOUT_POLLER
// #define WITHOUT_LCD
#define WITHOUT_UART

#include "musia.h"
#include "ps2_poller.h"
#include "ps2_state.h"
#include "spi_sniffer.h"
#include "usb_joystick.h"
#include "hal_spi.h"
#include <stm32f0xx_hal.h>
#include <algorithm>
#include <cstring>
#include "tim.h"

#include "usb_musia_device.h"
#include "ps2_controller_if.h"
#include "configurator_if.h"
#include "iwdg.h"
#include "usart.h"
#include "eeprom.h"

#ifndef WITHOUT_LCD
#include "lcd.h"
#include "menu.h"
#include "buttonchecker.h"
#endif


SPI_HandleTypeDef hspi1;
SPI_HandleTypeDef hspi2;
DMA_HandleTypeDef hdma_spi1_rx;
DMA_HandleTypeDef hdma_spi2_rx;

hal_spi_interface spi_ps2(&hspi1, JPS2_ATT_GPIO_Port, JPS2_ATT_Pin); // in driver mode
hal_spi_interface spi_ee(&hspi2, EE_CS_GPIO_Port, EE_CS_Pin);
m25xx080 m25xx080(&spi_ee); // eeprom for configuration retaining
eeprom ee(&m25xx080);

spi_sniffer sniffer(&hspi2, &hspi1, &hdma_spi2_rx, &hdma_spi1_rx); // sniff on both spi lines configured as slave inputs

#ifndef WITHOUT_POLLER
ps2_poller poller(&spi_ps2); // polls on spi1 when not in sniffer mode
#endif

ps2_state ps2State; // tracks controller state, read by USB and updated either by sniffer or poller
usb_joystick usbJoy; // transmits USB HID packets containing ps2 state

bool configUpdatePending = false;
extern TIM_HandleTypeDef htim3;
uint8_t tim1Expired;
bool packetObserved = false;
extern bool in_menu;

void sysInit();
void applyConfig();
bool validateConfig();
void handleSnifferPacket();
void handlePollerPacket();
void eepromSelect(spi_interface* spi_if);
void eepromDeselect(spi_interface* spi_if);

volatile bool tick1Khz;

EXTERNC int musia_main(void) {
	HAL_IWDG_Refresh(&hiwdg);

	sysInit();
	sys_printf("MUSIA started\n");
	
	spi_ee.setSelectHandler(eepromSelect);
	ee.init(); // load eeprom
	validateConfig();
	applyConfig();

	for (;;) {
		HAL_IWDG_Refresh(&hiwdg);

		if (configUpdatePending) {
			validateConfig();
			ee.sync();
			applyConfig();
			configUpdatePending = false;
		}
		else if (ee.data->mode == MODE_SNIFFER) {
			sniffer.work();
			handleSnifferPacket();
		}
		else {
#ifndef WITHOUT_POLLER
			poller.work();
			handlePollerPacket();
#endif
		}

		if (tim1Expired) {
			HAL_GPIO_WritePin(LED_ORANGE_GPIO_Port, LED_ORANGE_Pin, packetObserved ? GPIO_PIN_SET : GPIO_PIN_RESET);
			packetObserved = false;
			tim1Expired = false;
		}
		
#ifndef WITHOUT_LCD
		if (tick1Khz) {
			static uint16_t counter = 0;
			tick1Khz = false;
			if (bcCheck()) {
				if (bcPressed(0) && bcTick(0)) {
					if (!in_menu) menu_enter();
					else menu_exit(false); 
				}
			}
			
			lcd_process();
			menu_tick1000hz();
		}
#endif
		
		__WFI();
		// USB is fully based off interrupts,
		// poller is based off timer interrupt,
		// sniffer is based off EXTI interrupt
	}
}

void sysInit() {
	__HAL_DBGMCU_UNFREEZE_IWDG();
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
	
	// start timer and interrupts
	HAL_TIM_Base_Start_IT(&htim1);

	// set green led
	HAL_GPIO_WritePin(LED_GREEN_GPIO_Port, LED_GREEN_Pin, GPIO_PIN_SET);

#ifndef WITHOUT_LCD
	lcd_setup();
#endif

#ifndef WITHOUT_UART
	MX_USART1_UART_Init();
	RetargetInit(&huart1);
#endif
}

bool validateConfig() {
	ee.data->mode = (musia_mode)((int)(ee.data->mode) & 0x01);
	if (ee.data->pollFreq != 25  && ee.data->pollFreq != 30 &&
		ee.data->pollFreq != 50  && ee.data->pollFreq != 60 &&
		ee.data->pollFreq != 100 && ee.data->pollFreq != 120) {
		
		ee.data->pollFreq = 60;
		return true;
	}

	return false;
}

void applyConfig() {
	// eeprom config was updated
	sys_printf("Applying new configuration\n");
	
	static musia_mode current_mode = MODE_INVALID;
	if (current_mode == MODE_SNIFFER)
		sniffer.deInit();

#ifndef WITHOUT_POLLER
	if (current_mode == MODE_POLLER)
		poller.deInit();
#endif

		
	if (ee.data->mode == MODE_SNIFFER) {
		sniffer.init();
		sniffer.start();
		current_mode = MODE_SNIFFER;
	}
	else {
#ifndef WITHOUT_POLLER
		poller.init();
		poller.start(static_cast<polling_freq>(ee.data->pollFreq));
		current_mode = MODE_POLLER;
#endif
	}
}

void handleSnifferPacket() {
	static uint8_t resyncDetect = 0, resyncFail = 0;
	static bool prevWasValid = false;
	
	auto* upd = sniffer.getNewPacket();			
	if (upd == nullptr) return;
			
	bool validPacket = ps2State.update(upd->cmd, upd->data, upd->pktLength);
	packetObserved |= validPacket;

	if (!validPacket) {
		sys_printf("INVALID PACKET RECEIVED, RESYNC CTR: %d\n", resyncDetect);
		ps2_state::print_packet(upd->cmd, upd->data, upd->pktLength);
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
#ifndef WITHOUT_LCD
		menu_packet(&ps2State);
#endif
	}
	
	upd->isNew = false;
	prevWasValid = validPacket;
}

#ifndef WITHOUT_POLLER
void handlePollerPacket() {
	static uint8_t resyncDetect = 0, resyncFail = 0;
	static bool prevWasValid = false;

	auto* upd = poller.getNewPacket();			
	if (upd == nullptr) return;
			
	bool validPacket = ps2State.update(upd->cmd, upd->data, upd->pktLength);
	packetObserved |= validPacket;

	if (!validPacket) {
		sys_printf("INVALID PACKET RECEIVED, RESYNC CTR: %d\n", 0);
		ps2_state::print_packet(upd->cmd, upd->data, upd->pktLength);
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
		menu_packet(&ps2State);
	}
	upd->isNew = false;
	prevWasValid = validPacket;
}
#endif

void eepromSelect(spi_interface* spi_if) {
	hal_spi_interface* hintf = (hal_spi_interface*)spi_if;
	auto intf = hintf->getHandle();
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
	
	intf->Init.Mode = SPI_MODE_MASTER;
	intf->Init.Direction = SPI_DIRECTION_2LINES;
	intf->Init.DataSize = SPI_DATASIZE_8BIT;
	intf->Init.CLKPolarity = SPI_POLARITY_LOW;
	intf->Init.CLKPhase = SPI_PHASE_1EDGE;
	intf->Init.NSS = SPI_NSS_SOFT;
	intf->Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_64;
	intf->Init.FirstBit = SPI_FIRSTBIT_MSB;
	intf->Init.TIMode = SPI_TIMODE_DISABLE;
	intf->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	intf->Init.CRCPolynomial = 7;
	intf->Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	intf->Init.NSSPMode = SPI_NSS_PULSE_ENABLE;
	HAL_SPI_Init(intf);
}

void eepromDeselect(spi_interface* spi_if) {	
	hal_spi_interface* hintf = (hal_spi_interface*)spi_if;
	auto intf = hintf->getHandle();
	HAL_SPI_DeInit(intf);
}

EXTERNC void setEEPROMConfig(const uint8_t* buffer) {
	memcpy(ee.eepBuf, buffer, EEPROM_SIZE);
	configUpdatePending = true;
}
EXTERNC void getEEPROMConfig(uint8_t* buffer, uint8_t buffSize) {
	memcpy(buffer, ee.eepBuf, std::min((uint8_t)EEPROM_SIZE, buffSize));
}

EXTERNC void HAL_SYSTICK_Callback() {
	tick1Khz = true;
}

EXTERNC void lcd_backlight(uint8_t pwm) {
	// lcd backlight on, maybe PWM
	HAL_GPIO_WritePin(LCD_PWM_GPIO_Port, LCD_PWM_Pin, pwm ? GPIO_PIN_SET : GPIO_PIN_RESET);
}