#ifndef MENU_H
#define	MENU_H

#include <stdint.h>
#include "globals.h"

bool in_menu = false;
uint8_t current_menu_page;

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
