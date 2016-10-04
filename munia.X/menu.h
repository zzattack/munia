#ifndef MENU_H
#define	MENU_H

#include <stdint.h>
#include "globals.h"

bool in_menu = false;
uint8_t current_menu_page;

enum __menu_pages {
    MENU_PAGE_OUTPUT, // PC, NGC, N64, SNES
    MENU_PAGE_PC_INPUTS, // toggle boxes
    MENU_PAGE_INPUT_NGC, // select either NGC or SNES
    MENU_PAGE_INPUT_N64, // select either N64 or NGC
    MENU_PAGE_INPUT_SNES, // only SNES available
    MENU_PAGE_CONFIRM,
    MENU_PAGE_COUNT,
};
enum __menu_command {
    mc_left, mc_right, mc_select, mc_confirm, mc_cancel, mc_exit
};

// this is where the input is redirected to
const char* menu_sub_items[][5] = {
    {"NGC ", "N64 ", "SNES", "PC ", NULL}, // MENU_PAGE_NGC,
    {"NGC ", "N64 ", "SNES", NULL },        // MENU_PAGE_PC_INPUTS,
    {"NGC ", "SNES", NULL },             // MENU_PAGE_INPUT_NGC,
    {"N64 ", "NGC ", NULL },              // MENU_PAGE_INPUT_N64,
    {"SNES", NULL },                    // MENU_PAGE_INPUT_SNES,
    {"A:Ok ", " B:Cancel", NULL },       // MENU_PAGE_CONFIRM,
};

typedef enum { output_ngc, output_n64, output_snes, output_pc } output_t;
typedef enum { input_ngc = 1, input_n64 = 2, input_snes = 4 } input_t;
typedef struct {
    output_t output_mode;
    union {
        struct {
            input_t input_sources;
        };
        struct {
            uint8_t input_ngc : 1;
            uint8_t input_n64 : 1;
            uint8_t input_snes : 1;
        };
    };
    uint8_t spare; // old config had 3 bytes, so this pads it
} config_t;

config_t
    config, // active config, modified to redirect to menu        
    config_edit, // the config as we're editing   it,
    config_backup; // backup of config before entering menu

void menu_enter();
void menu_exit(bool save_settings);
void menu_page(uint8_t page);
void menu_tasks();

#endif	/* MENU_H */
