#ifndef MENU_H
#define	MENU_H

#include <stdint.h>
#include "globals.h"
#include "config.h"

bool in_menu = false;
uint8_t current_menu_page;

config_t
    config, // active config, modified to redirect to menu        
    config_edit, // the config as we're editing   it,
    config_backup; // backup of config before entering menu

void menu_enter();
void menu_exit(bool save_settings);
void menu_page(uint8_t page);
void menu_tasks();

#endif	/* MENU_H */
