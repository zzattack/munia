#pragma once


#define LOOP_MAX	0xFFFF
static unsigned int _loop_count;
static unsigned char vibration_on = 0;
static unsigned char constant_force = 0;
static unsigned char magnitude = 0;

void gamepadVibrate(int i) {
	sys_printf("gamepadVibrate(%d)\n", i);
}

static void decideVibration(void) {
#ifdef NONSTOP_VIBRATION
	gamepadVibrate(1);
	return;
#endif

	if (!_loop_count)
		vibration_on = 0;

	if (!vibration_on) {
		gamepadVibrate(0);
	}
	else {
		if (constant_force > 0x7f) {
			gamepadVibrate(1);
		}
		if (magnitude > 0x7f) {
			gamepadVibrate(1);
		}
	}
}
