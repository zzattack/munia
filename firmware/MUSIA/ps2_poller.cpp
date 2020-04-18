#include "ps2_poller.h"
#include "dma.h"
#include <stm32f0xx_hal.h>
#include <cstring>

TIM_HandleTypeDef htim3;

ps2_poller::ps2_poller(hal_spi_interface* spi) {
	this->spi = spi;
}

void ps2_poller::init() {
	__HAL_RCC_SPI1_CLK_ENABLE();
	
	/**SPI1 GPIO Configuration    
	PA5     ------> SPI1_SCK
	PA6     ------> SPI1_MISO
	PA7     ------> SPI1_MOSI 
	*/
	GPIO_InitTypeDef GPIO_InitStruct;
	GPIO_InitStruct.Pin = GPIO_PIN_5 | GPIO_PIN_6 | GPIO_PIN_7;
	GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
	GPIO_InitStruct.Alternate = GPIO_AF0_SPI1;
	HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);	
	
	// console SPI lines + ATT are disconnected from corresponding joystick lines
	HAL_GPIO_WritePin(SW_CONSOLE_DISCONNECT_GPIO_Port, SW_CONSOLE_DISCONNECT_Pin, GPIO_PIN_SET); // SET = console disconnected
		
	// enable pull-up on MISO line in case joystick is not connected
	HAL_GPIO_WritePin(SW_PULLUP_ENABLE_GPIO_Port, SW_PULLUP_ENABLE_Pin, GPIO_PIN_RESET);
		
	// disconnect PS2 8V vibration motor power source
	HAL_GPIO_WritePin(PS2_VIBR_EN_GPIO_Port, PS2_VIBR_EN_Pin, GPIO_PIN_RESET);
		
	// connect joystick MISO to SPI1 MISO
	HAL_GPIO_WritePin(SW_SNIFFER_CONSOLE_GPIO_Port, SW_SNIFFER_CONSOLE_Pin, GPIO_PIN_RESET);
		
	// disconnect joystick clock from SPI2 clock
	HAL_GPIO_WritePin(SW_SPI_CLK_GPIO_Port, SW_SPI_CLK_Pin, GPIO_PIN_SET); 
	
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
	hspi->Init.NSSPMode = SPI_NSS_PULSE_DISABLE;
	HAL_SPI_Init(hspi);
	__HAL_SPI_ENABLE(hspi);
	
	// setup polling interval timer
	this->freq = freq;
	htim3.Init.Prescaler = 199; // handy divisor works for all freqs
	htim3.Init.CounterMode = TIM_COUNTERMODE_DOWN;
	htim3.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
	htim3.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_ENABLE;
	htim3.Init.RepetitionCounter = 0;

	__HAL_RCC_TIM3_CLK_ENABLE();
	HAL_NVIC_SetPriority(TIM3_IRQn, 0, 0);
	HAL_NVIC_EnableIRQ(TIM3_IRQn);
	HAL_TIM_Base_Init(&htim3);
	
	TIM_ClockConfigTypeDef sClockSourceConfig;
	sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
	HAL_TIM_ConfigClockSource(&htim3, &sClockSourceConfig);
	
	// configure ATT and ACK pins
	GPIO_InitStruct.Pin = JPS2_ATT_Pin;
	GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
	GPIO_InitStruct.Pull = GPIO_NOPULL;
	HAL_GPIO_Init(JPS2_ATT_GPIO_Port, &GPIO_InitStruct);
	
	GPIO_InitStruct.Pin = JPS2_ACK_Pin;
	GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
	GPIO_InitStruct.Pull = GPIO_PULLUP;
	HAL_GPIO_Init(JPS2_ACK_GPIO_Port, &GPIO_InitStruct);
}

void ps2_poller::deInit() {
	stop();
	__HAL_RCC_TIM3_CLK_DISABLE();
	__HAL_RCC_SPI1_CLK_DISABLE();
	HAL_NVIC_DisableIRQ(TIM3_IRQn);
	HAL_TIM_Base_DeInit(&htim3);	
	
	HAL_GPIO_DeInit(JPS2_ATT_GPIO_Port, JPS2_ATT_Pin);
	HAL_GPIO_DeInit(JPS2_ACK_GPIO_Port, JPS2_ACK_Pin);
	HAL_SPI_DeInit(spi->getHandle());
}

void ps2_poller::start(polling_freq freq) {	
	this->freq = freq;
	this->state = config_state::notInitialized;
	htim3.Init.Period = 240000 / (int)freq;
	HAL_TIM_Base_Init(&htim3);
	// start timer and interrupts
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


#pragma GCC push_options
#pragma GCC optimize ("O0")
void ps2_poller::spiExchange(const uint8_t* w, uint8_t* r, uint16_t len, bool doPrint) {
	const uint8_t* payload = w;
	const uint8_t* received = r;
	uint16_t c = len;
	
	auto hspi = spi->getHandle();
	
	// Wait for pending transmission to complete
	SPI_WAIT_TX(hspi->Instance);
	__IO uint8_t discard = hspi->Instance->DR; // discard if pending read
	
	// make controller attentive
	spi->clearCS();
		
	// burn about 80us (scope aligned)
	volatile uint32_t counter = 58;
	while (counter--);	
	
	while (len--) {
		// Fill output buffer with data
		*(__IO uint8_t *)&hspi->Instance->DR = *w++;
	
		// Wait for transmission to complete, then read was was simultaneously received
		SPI_WAIT_TX(hspi->Instance);
		// this should always clear right away
		SPI_WAIT_RX(hspi->Instance);
		
		// update receive buffer
		*r++ = hspi->Instance->DR;
		
		// burn about 12us (scope aligned)
		volatile uint32_t counter = 32;
		while (counter--);	
	}
	spi->setCS();	
		
	if (doPrint) {
		ps2_printf("spi_exchange exchange:\n");
		sync_printf("\t --> "); printf_payload((char*)payload, c);
		sync_printf("\t <-- "); printf_payload((char*)received, c);
	}
}
#pragma GCC pop_options


volatile bool pollNeeded = false;

void ps2_poller::nextConfigState() {
	state = (config_state)((int)state + 1);
	configFailCount = 0;
}

void ps2_poller::work() {
	if (!pollNeeded) return;	
	
	if (state < config_state::completed) {
		
		if (state == config_state::notInitialized) {
			uint8_t payload[] = { 0x01, 0x42, 0x00, 0xFF, 0xFF };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, 5);			
			if (rcvBuff[2] == 0x5a) {
				ps2_printf("state notInitialized --> enterConfigMode\n");
				nextConfigState();
			}
			else {
				configFailCount++;
				ps2_printf("enterConfigMode failed, return payload=");
				printf_payload((char*)rcvBuff, sizeof(rcvBuff));
			}
		}
		
		else if (state == config_state::enterConfigMode) {
			uint8_t payload[] = { 0x01, 0x43, 0x00, 0x01, 0x00 };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, sizeof(payload));
			
			if (rcvBuff[2] == 0x5A) {
				nextConfigState();
				ps2_printf("state enterConfigMode --> turnOnAnalog\n");
			}
			else {
				configFailCount++;
				ps2_printf("turnOnAnalog failed, return payload=");
				printf_payload((char*)rcvBuff, sizeof(rcvBuff));
			}
		}
		
		else if (state == config_state::turnOnAnalog) {
			uint8_t payload[9] = { 0x01, 0x44, 0x00, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00 };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, sizeof(payload));
			
			if (rcvBuff[1] == 0xF3 && rcvBuff[2] == 0x5A) {
				nextConfigState();
				ps2_printf("state turnOnAnalog --> setupMotorMapping\n");
			}
			else {
				configFailCount++;
				ps2_printf("setupMotorMapping failed, return payload=");
				printf_payload((char*)rcvBuff, sizeof(rcvBuff));
				if (configFailCount > 2) {
					// no big deal, probably a PS1 controller
					nextConfigState();
				}
			}
		}
		
		else if (state == config_state::setupMotorMapping) {
			uint8_t payload[9] = { 0x01, 0x4D, 0x00, 0x00, 0x01, 0xFF, 0xFF, 0xFF, 0xFF };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, sizeof(payload));
			
			if (rcvBuff[1] == 0xF3 && rcvBuff[2] == 0x5a && rcvBuff[4] == 0x01) {
				nextConfigState();
				ps2_printf("state setupMotorMapping --> enablePressureMappings\n");
			}
			else {
				configFailCount++;
				ps2_printf("\nenablePressureMappings failed, return payload=");
				printf_payload((char*)rcvBuff, sizeof(rcvBuff));
				if (configFailCount > 2) {
					// no big deal, probably a PS1 controller
					nextConfigState();
				}
			}
		}
		
		else if (state == config_state::enablePressureMappings) {
			uint8_t payload[9] = { 0x01, 0x4F, 0x00, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00 };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, sizeof(payload), true);
			
			if (rcvBuff[1] & 0x73) {
				nextConfigState();
				ps2_printf("state enablePressureMappings --> exitConfig\n");
			 }
			else {
				configFailCount++;
				ps2_printf("enablePressureMappings failed, return payload=");
				printf_payload((char*)rcvBuff, sizeof(rcvBuff));
			}
		}
		
		else if (state == config_state::exitConfig) {
			uint8_t payload[9] = { 0x01, 0x43, 0x00, 0x00, 0x5a, 0x5a, 0x5a, 0x5a, 0x5a };
			uint8_t rcvBuff[sizeof(payload)];
			spiExchange(payload, rcvBuff, sizeof(payload));
			
			if (rcvBuff[1] == 0xF3 && rcvBuff[2] == 0x5a) {
				// show be completed now
				nextConfigState();
				ps2_printf("state exitConfig --> fully initialized\n");
			}
			else configFailCount++;
		}
		
		if (configFailCount >= 5) {
			resync(true); 
			configFailCount = 0;
		 }
	}
	
	else {
		poll();
	 }
	
	pollNeeded = false;
}

void ps2_poller::poll() {
	uint8_t motor_small = 0x00;
	uint8_t motor_large = 0x00;
	uint8_t payload[] = { 0x01, 0x42, 0x00, motor_small, motor_large };
	
	memcpy(this->pkt.cmd, payload, sizeof(payload));
	spiExchange(pkt.cmd, pkt.data, 21);
	pkt.pktLength = 21;
	pkt.isNew = true;
}

void ps2_poller::resync(bool hard) {
	if (hard) {
		deInit();
		HAL_Delay(5);
		init();
		start(this->freq);
	}
	else {	
		// simply reinitializing should be enough
		this->state = config_state::notInitialized;
	}
	ps2_printf("poller resync attempted!\n");
}

ps2_packet* ps2_poller::getNewPacket() {
	if (pkt.isNew) return &pkt;
	else return nullptr;
}

EXTERNC void TIM3_IRQHandler() {
	__HAL_TIM_CLEAR_IT(&htim3, TIM_IT_UPDATE);
	pollNeeded = true;
}