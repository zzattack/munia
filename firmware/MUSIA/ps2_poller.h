#pragma once

#include "hal_spi.h"
#include "ps2_packet.h"

enum class polling_freq {
	poll25Hz = 25,
	poll30Hz = 30,
	poll50Hz = 50,
	poll60Hz = 60,
	poll100Hz = 100,
	poll120Hz = 120,
};

enum class config_state : int {
	notInitialized = 0,
	enterConfigMode,
	turnOnAnalog,
	setupMotorMapping,
	enablePressureMappings,
	exitConfig,
	completed
};

class ps2_poller {
private:
	ps2_packet pkt;
	polling_freq freq;
	config_state state = config_state::notInitialized;
	uint8_t configFailCount = 0;
	hal_spi_interface* spi;
	
	void poll();
	void spiExchange(const uint8_t* w, uint8_t* r, uint16_t len, bool doPrint = false);
	void nextConfigState();
	
public:	
	ps2_poller(hal_spi_interface* spi);

	void configure(); // todo remove
	void init();
	void deInit();
	void start(polling_freq freq);
	void stop();
	void work();

	ps2_packet* getNewPacket();
	void resync(bool hard);
};