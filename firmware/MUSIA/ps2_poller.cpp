#include "ps2_poller.h"
#include "dma.h"
#include <stm32f0xx_hal.h>

TIM_HandleTypeDef htim3;

ps2_poller::ps2_poller(hal_spi_interface* spi) {
	this->spi = spi;
}

void ps2_poller::init() {
	// console SPI lines + ATT are disconnected from corresponding joystick lines
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_SET); // SET = console disconnected
		
	// enable pull-up on MISO line in case joystick is not connected
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_RESET); // RESET = pull enabled
		
	// disconnect PS2 8V vibration motor power source
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_RESET); // RESET = vibr mosfet disabled	
		
	// connect +5V from USB to joystick vibration motor
	HAL_GPIO_WritePin(USB_VIBR_EN_GPIO_Port, USB_VIBR_EN_Pin, GPIO_PIN_SET); // SET = vibr mosfet enabled
		
	// connect joystick MISO to SPI1 MISO
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_SET);  // SET = poller mode
		
	// disconnect joystick clock from SPI2 clock
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_RESET);  // RESET = disconnected from ps2 clock
		
	// setup SPI as master
	auto hspi = spi->getHandle();
	hspi->Init.Mode = SPI_MODE_MASTER;
	hspi->Init.Direction = SPI_DIRECTION_2LINES;
	hspi->Init.DataSize = SPI_DATASIZE_8BIT;
	hspi->Init.CLKPolarity = SPI_POLARITY_HIGH;
	hspi->Init.CLKPhase = SPI_PHASE_2EDGE;
	hspi->Init.NSS = SPI_NSS_SOFT;
	hspi->Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_128;
	hspi->Init.FirstBit = SPI_FIRSTBIT_LSB;
	hspi->Init.TIMode = SPI_TIMODE_DISABLE;
	hspi->Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
	hspi->Init.CRCPolynomial = 7;
	hspi->Init.CRCLength = SPI_CRC_LENGTH_DATASIZE;
	hspi->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspi);
	HAL_SPI_MspInit(hspi);
	
	// setup polling interval timer
	this->freq = freq;
	htim3.Instance = TIM3;
	htim3.Init.Prescaler = 199; // handy divisor works for all freqs
	htim3.Init.CounterMode = TIM_COUNTERMODE_DOWN;
	htim3.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
	htim3.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_ENABLE;
	htim3.Init.RepetitionCounter = 0;

	// this scales nicely between all choosable frequencies
	htim3.Init.Period = 240000 / (int)freq;

	__HAL_RCC_TIM3_CLK_ENABLE();
	HAL_NVIC_SetPriority(TIM3_IRQn, 0, 0);
	HAL_NVIC_EnableIRQ(TIM3_IRQn);
	HAL_TIM_Base_Init(&htim3);
	
	TIM_ClockConfigTypeDef sClockSourceConfig;
	sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
	HAL_TIM_ConfigClockSource(&htim3, &sClockSourceConfig);
	
	// configure ATT and ACK pins as outputs
	GPIO_InitTypeDef GPIO_InitStruct;
	GPIO_InitStruct.Pin = JPS2_ATT_Pin | JPS2_ACK_Pin;
	GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
	HAL_GPIO_Init(JPS2_ATT_GPIO_Port, &GPIO_InitStruct);
}

void ps2_poller::deInit() {
	stop();
	__HAL_RCC_TIM3_CLK_DISABLE();
	HAL_NVIC_DisableIRQ(TIM3_IRQn);
	HAL_TIM_Base_DeInit(&htim3);	
	
	HAL_GPIO_DeInit(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
	HAL_GPIO_DeInit(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin);
}

void ps2_poller::start(polling_interval freq) {	
	// start timer and interrupts
	spi->setCS();
	HAL_GPIO_WritePin(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin, GPIO_PIN_SET);
	HAL_TIM_Base_Start_IT(&htim3);
}

void ps2_poller::stop() {	
	HAL_TIM_Base_Stop_IT(&htim3);
}

#define SPI_WAIT_TX(SPIx)                   while ((SPIx->SR & SPI_FLAG_TXE) == 0 || (SPIx->SR & SPI_FLAG_BSY))
#define SPI_WAIT_RX(SPIx)                   while ((SPIx->SR & SPI_FLAG_RXNE) == 0 || (SPIx->SR & SPI_FLAG_BSY))
#define SPI_IS_BUSY(SPIx)                   (((SPIx)->SR & (SPI_SR_TXE | SPI_SR_RXNE)) == 0)
#define SPI_CHECK_ENABLED(SPIx)             if (!((SPIx)->CR1 & SPI_CR1_SPE)) {return;}
#define SPI_CHECK_ENABLED_RESP(SPIx, val)   if (!((SPIx)->CR1 & SPI_CR1_SPE)) {return (val);}


#define delayUS_ASM(us) do {\
	asm volatile (	"MOV R0,%[loops]\n\t"\
			"1: \n\t"\
			"SUB R0, #1\n\t"\
			"CMP R0, #0\n\t"\
			"BNE 1b \n\t" : : [loops] "r" (16*us) : "memory"\
		      );\
} while(0);


void ps2_poller::spi_exchange(const uint8_t* w, uint8_t* r, uint16_t len) {
	auto hspi = spi->getHandle();

	// Ensure SPI is enabled
	__HAL_SPI_ENABLE(hspi);
	
	// make controller attentive
	spi->clearCS();
	
	while (len--) {
		// Fill output buffer with data
		// *(__IO uint8_t *)&hspi->Instance->DR = *w++;
	
		// Wait for transmission to complete
		// SPI_WAIT_RX(hspi->Instance);
	
		// update receive buffer
		// *r++ = hspi->Instance->DR;

		// this should always clear right away
		// SPI_WAIT_TX(hspi->Instance);
		
		// delayUS_ASM(8);
		HAL_GPIO_WritePin(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin, GPIO_PIN_RESET);
		delayUS_ASM(4);
		HAL_GPIO_WritePin(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin, GPIO_PIN_SET);
		delayUS_ASM(4);
	}
	spi->setCS();	
}

static bool pollNeeded = false;
void ps2_poller::work() {
	if (!pollNeeded) return;
	
	
	if (state < config_state::completed) {

		if (state == config_state::notInitialized) {
			uint8_t payload[] = { 0x01, 0x43, 0x00, 0x01, 0x00 };
			uint8_t rcvBuff[5];
			spi_exchange(payload, rcvBuff, sizeof(payload));
						
			ps2_printf("enterConfigMode exchange:\n");
			printf("\t --> "); printf_payload((char*)payload, 5);
			printf("\n\t <-- "); printf_payload((char*)rcvBuff, 5);
			printf("\n");			
		}
		
	}
	
	else if (pollNeeded) {
		poll();
		pollNeeded = false;
	}
}

void ps2_poller::poll() {
	
}

EXTERNC void TIM3_IRQHandler() {
	__HAL_TIM_CLEAR_IT(&htim3, TIM_IT_UPDATE);
	pollNeeded = true;
}