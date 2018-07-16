#pragma once

#include <cstdint>
#include "ps2_state.h"

enum class MENU_PAGE : uint8_t {
	OUTPUT = 0,
	POLL_FREQ,
	CONFIRM,
};
enum class MENU_COMMAND : uint8_t {
	LEFT = 0,
	RIGHT,
	SELECT,
	CONFIRM,
	CANCEL,
	EXIT
};

void menu_enter();
void menu_exit(bool save_settings);
void menu_page(MENU_PAGE page);
void menu_cmd(MENU_COMMAND cmd);
void menu_packet(ps2_state* packet);
void menu_tick1000hz();
