#include "menu.h"
#include "lcd.h"
#include "globals.h"
#include "gamecube.h"
#include "gamepad.h"
#include <stdbool.h>

config_t config_edit, config_backup;
uint8_t current_menu_page;
const uint8_t num_menu_pages = MENU_PAGE_COUNT;
uint8_t menu_next_press_delay;

void menu_page(uint8_t page);
void menu_press(uint8_t command);
void menu_display_setting();

void menu_enter() {
    lcd_command(LCD_DISPLAY_ON_CURSOR_OFF);
    lcd_backLightValue = 10;
    in_menu = true;
    
    // copy config
    config_backup = config;
    config_edit = config;
    
    config.ngc_mode = pc;
    config.n64_mode = pc;
    config.snes_mode = pc;
    apply_config();
    
    menu_page(MENU_PAGE_NGC);
}
void menu_exit(bool save_settings) {
    lcd_command(LCD_DISPLAY_OFF);
    lcd_backLightValue = 0;
    in_menu = false;
    
    if (save_settings) {
        config = config_edit;
        save_config();
    }
    else {
        config = config_backup;
    }
    
     apply_config();
}

void menu_page(uint8_t page) {
    current_menu_page = page;
    lcd_clear();
    lcd_home();
    
    if (page == MENU_PAGE_NGC) 
        lcd_string("NGC config");    
    else if (page == MENU_PAGE_N64)
        lcd_string("N64 config");
    else if (page == MENU_PAGE_SNES)
        lcd_string("SNES config");
    else if (page == MENU_PAGE_EXIT) 
        lcd_string("Save config?");
    
    menu_display_setting();
}

void menu_display_setting() {
    lcd_goto(2, 0);
    if (current_menu_page == MENU_PAGE_NGC) {
        lcd_string("NGC [");
        lcd_char(config_edit.ngc_mode == console ? '*' : ' ');
        lcd_string("]  PC [");
        lcd_char(config_edit.ngc_mode == pc ? '*' : ' ');
        lcd_string("]");
    }
    else if (current_menu_page == MENU_PAGE_N64) {
        lcd_string("N64 [");
        lcd_char(config_edit.n64_mode == console ? '*' : ' ');
        lcd_string("]  PC[");
        lcd_char(config_edit.n64_mode == pc ? '*' : ' ');
        lcd_string("]");
    }
    else if (current_menu_page == MENU_PAGE_SNES) {
        lcd_string("SNES[");
        lcd_char(config_edit.snes_mode == console ? '*' : ' ');
        lcd_string("]  PC [");
        lcd_char(config_edit.snes_mode == pc ? '*' : ' ');
        lcd_string("]");
    }
    else if (current_menu_page == MENU_PAGE_EXIT) {
        lcd_string("A:Ok    B:Cancel");
    }
}


void menu_tasks() {
    if (!in_menu) return;
    if (menu_next_press_delay > 0) { 
        menu_next_press_delay--; 
        return; 
    }
    if (n64_packet_available) {
        
    }
    else if (ngc_packet_available) {
        if (joydata_ngc.hat == HAT_SWITCH_WEST) menu_press(left);
        else if (joydata_ngc.hat == HAT_SWITCH_EAST) menu_press(right);
        if (joydata_ngc.l) menu_press(prev_page);
        else if (joydata_ngc.r) menu_press(next_page);
        else if (joydata_ngc.start) menu_press(exit);
        else if (joydata_ngc.a) menu_press(confirm);
        else if (joydata_ngc.b) menu_press(cancel);
    }
}

void menu_press(uint8_t command) {
    menu_next_press_delay = 250; // 250ms
    if (command == prev_page) 
        menu_page((current_menu_page + MENU_PAGE_COUNT - 1) % MENU_PAGE_COUNT);
    else if (command == next_page) 
        menu_page((current_menu_page + 1) % MENU_PAGE_COUNT);
    else if (command == exit)
        menu_page(MENU_PAGE_EXIT);
    else if (command == confirm && current_menu_page == MENU_PAGE_EXIT)
        menu_exit(true);
    else if (command == cancel && current_menu_page == MENU_PAGE_EXIT)
        menu_exit(false);
    
    else if (command == left) {
        if (current_menu_page == MENU_PAGE_NGC) config_edit.ngc_mode = console;
        if (current_menu_page == MENU_PAGE_N64) config_edit.n64_mode = console;
        if (current_menu_page == MENU_PAGE_SNES) config_edit.snes_mode = console;
        menu_display_setting();
    }
    else if (command == right) {
        if (current_menu_page == MENU_PAGE_NGC) config_edit.ngc_mode = pc;
        if (current_menu_page == MENU_PAGE_N64) config_edit.n64_mode = pc;
        if (current_menu_page == MENU_PAGE_SNES) config_edit.snes_mode = pc;
        menu_display_setting();    
    }
}
