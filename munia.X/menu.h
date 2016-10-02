#ifndef MENU_H
#define	MENU_H

#include <stdint.h>
#include "globals.h"

bool in_menu = false;
uint8_t current_menu_page;

enum __menu_pages {
    MENU_PAGE_NGC,
    MENU_PAGE_N64,
    MENU_PAGE_SNES,
    MENU_PAGE_EXIT,
    MENU_PAGE_COUNT,
};
enum __menu_command {
    mc_next_page, mc_prev_page, mc_left, mc_right, mc_confirm, mc_cancel, mc_exit
};

// this is where the input is redirected to
const char* menu_sub_items[][4] = {
    {"NGC ", "PC ", NULL},         // MENU_PAGE_NGC,
    {"N64 ", "PC ", NULL },        // MENU_PAGE_N64,
    {"SNES", "PC ", "NGC", NULL }, // MENU_PAGE_SNES,
    {"A:Ok ", " B:Cancel", NULL }, // MENU_PAGE_EXIT,
};
enum __snes_modes { SNES_MODE_SNES, SNES_MODE_PC, SNES_MODE_NGC };
enum __n64_modes  { N64_MODE_N64,   N64_MODE_PC  };
enum __ngc_modes  { NGC_MODE_NGC,   NGC_MODE_PC };

typedef struct {
    uint8_t snes_mode;
    uint8_t n64_mode;
    uint8_t ngc_mode;
} config_t;
volatile config_t config @ 0x003;

config_t config_edit, config_backup;

void menu_enter();
void menu_exit(bool save_settings);
void menu_page(uint8_t page);
void menu_tasks();

#endif	/* MENU_H */
