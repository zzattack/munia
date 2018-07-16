#pragma once

#include <cstdint>
#include "ps2_state.h"

enum class MENU_PAGE {
	OUTPUT,
	POLL_FREQ,
	CONFIRM,
};
enum class MENU_COMMAND {
	LEFT,
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
void menu_tasks();
void menu_packet(ps2_state* packet);

