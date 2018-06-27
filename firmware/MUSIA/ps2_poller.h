#pragma once

#include "hal_spi.h"

enum class polling_interval {
	poll25Hz = 25,
	poll30Hz = 30,
	poll50Hz = 50,
	poll60Hz = 60,
	poll100Hz = 100,
	poll120Hz = 120,
};

enum class config_state {
	notInitialized,
	enterConfigMode,
	turnOnAnalog,
	setupMotorMapping,
	enablePressureMappings,
	exitConfig,
	completed
};

class ps2_poller {
private:
	polling_interval freq;
	config_state state = config_state::notInitialized;
	hal_spi_interface* spi;
	void poll();
	void spi_exchange(const uint8_t* w, uint8_t* r, uint16_t len);
	
public:	
	ps2_poller(hal_spi_interface* spi);

	void configure(); // todo remove
	void init();
	void deInit();
	void start(polling_interval freq);
	void stop();

	void work();
};