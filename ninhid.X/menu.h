#ifndef MENU_H
#define	MENU_H

#include <stdint.h>
#include "globals.h"

enum __mode { console, pc /*, fake_snes, fake_n64, fake_gc*/ };
enum __menu_pages {
    MENU_PAGE_NGC,
    MENU_PAGE_N64,
    MENU_PAGE_SNES,
    MENU_PAGE_EXIT,
    MENU_PAGE_COUNT,
};
enum __menu_command {
    next_page, prev_page, left, right, confirm, cancel, exit,
};

bool in_menu = false;

typedef struct {
    uint8_t snes_mode : 1;
    uint8_t n64_mode : 1;
    uint8_t ngc_mode : 1;
} config_t;
config_t config;

void menu_enter();
void menu_exit(bool save_settings);
void menu_tasks();

#endif	/* MENU_H */
