#include "ps2_state.h"
#include <cstdio>

void ps2_state::print_packet(uint8_t* cmd, uint8_t* data, uint8_t pkt_len) {
	sys_printf("PS2 packet capture, len: %d:\n", pkt_len);
	printf("\t CMD:  ");
	for (int i = 0; i < pkt_len; i++)
		printf("%02X ", cmd[i]);
	printf("\n\t DATA: ");
	for (int i = 0; i < pkt_len; i++)
		printf("%02X ", data[i]);
	printf("\n");
}

bool ps2_state::update(uint8_t* cmd, uint8_t* data, uint8_t pkt_len) {
	if (pkt_len < 5) {
		ps2_printf("Capture empty packet, len=%d\n", pkt_len);
		return false;
	}
	else if (cmd[0] != 0x01) {
		ps2_printf("Invalid packet: cmd[0] != 0x01\n", pkt_len);
		return false;
	}
	else if (cmd[2] > 0x01 || data[2] != 0x5A) {
		ps2_printf("Invalid packet: cmd[2] > 0x01 || data[2] != 0x5A\n");
		return false; // not understood
	}
	
	uint8_t numWords = data[1] & 0x0F;
	if (pkt_len < 3 + numWords * 2) {
		ps2_printf("Invalid packet length: len %d should be %d\n", pkt_len, 3 + numWords * 2);
		return false; // verify pkt len
	}
	uint8_t cmdId = cmd[1];
	if (cmdId == 0x42) return updatePoll(&data[3], numWords * 2);
	else if (cmdId == 0x43) return updateConfig(cmd, data, numWords * 2);
}

bool ps2_state::updatePoll(uint8_t* data, int len) {
	device_mode deviceMode = static_cast<device_mode>(data[1] >> 4);

	uint8_t buttons1 = ~data[0];
	uint8_t buttons2 = ~data[1];
	
	if (len >= 2) {
		analog1_x = 0;
		analog1_y = 0;
		analog2_y = 0;
		analog2_y = 0;
		l_stick = false;
		r_stick = false;
		
		select = buttons1 & 0x01;
		start = buttons1 & 0x08;
		dpad_up = buttons1 & 0x10;
		dpad_right = buttons1 & 0x20;
		dpad_down = buttons1 & 0x40;
		dpad_left = buttons1 & 0x80;
		
		l2 = buttons2 & 0x01;
		r2 = buttons2 & 0x02;
		l1 = buttons2 & 0x04;
		r1 = buttons2 & 0x08;
		triangle = buttons2 & 0x10;
		circle = buttons2 & 0x20;
		cross = buttons2 & 0x40;
		square = buttons2 & 0x80;
	}
	
	int idx = 2;
	if (len >= 6) {
		l_stick = buttons1 & 0x02;
		r_stick = buttons1 & 0x04;		
		analog2_x = data[idx++] - 128;
		analog2_y = data[idx++] - 128;
		analog1_x = data[idx++] - 128;
		analog1_y = data[idx++] - 128;
	}
	else {
		analog2_x = 0;
		analog2_y = 0;
		analog1_x = 0;
		analog1_y = 0;
		idx += 4;
	}
	
	if (len == 18) {
		// pressures
		pressures.dpad_right    = data[idx++];
		pressures.dpad_left     = data[idx++];
		pressures.dpad_up		= data[idx++];
		pressures.dpad_down     = data[idx++];
		pressures.triangle      = data[idx++];
		pressures.circle		= data[idx++];
		pressures.cross			= data[idx++];
		pressures.square		= data[idx++];
		pressures.l1			= data[idx++];
		pressures.r1			= data[idx++];
		pressures.l2			= data[idx++];
		pressures.r2			= data[idx++];
	}
	pressures.available = len == 18;
	return len == 2 || len == 6 || len == 18;
}

bool ps2_state::updateConfig(uint8_t* cmd, uint8_t* data, int len) {
	sys_printf("updateConfig, with \n");
	printf("\t CMD:  ");
	for (int i = 0; i < len; i++)
		printf("%02X ", cmd[i]);
	printf("\n\t DATA: ");
	for (int i = 0; i < len; i++)
		printf("%02X ", data[i]);
	printf("\n");

	return true;
}















