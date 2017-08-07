#include "menu.h"
#include "lcd.h"
#include "globals.h"
#include "asm_decl.h"
#include "snes.h"
#include "n64.h"
#include "gamecube.h"
#include "gamepad.h"
#include <stdbool.h>
#include <stdlib.h>

// [*] = multi select, checked and focused
// [o] = multi select, checked but not focused
// [.] = multi select, focused but not checked
// [ ] = not checked or focused


enum __menu_pages {
    MENU_PAGE_OUTPUT, // PC, NGC, N64, SNES
    MENU_PAGE_PC_INPUTS, // toggle select NGC/N64/SNES
    MENU_PAGE_INPUT,     // radio select either NGC or N64 or SNES
    MENU_PAGE_CONFIRM,
    MENU_PAGE_COUNT,
};
enum __menu_command {
    mc_left, mc_right, mc_select, mc_confirm, mc_cancel, mc_exit
};

// this is where the input is redirected to
const char* menu_sub_items[][5] = {
    {"NGC ", "N64 ", "SNES", "PC ", NULL},  // MENU_PAGE_NGC,
    {"NGC ", "N64 ", "SNES", NULL },        // MENU_PAGE_PC_INPUTS,
    {"NGC ", "N64",  "SNES", NULL },        // MENU_PAGE_INPUT,
    {"A:Ok ", " B:Cancel", NULL },          // MENU_PAGE_CONFIRM,
};

uint8_t submenu_idx = 0;
uint8_t submenu_mask = 0;
bool menu_leftalign = true;
const uint8_t num_menu_pages = MENU_PAGE_COUNT;
uint16_t menu_next_press_delay;
const char** menu_current_items;
uint8_t submenu_count = 0;

void menu_press(uint8_t command);
void menu_display_setting();

void menu_enter() {
    lcd_command(LCD_DISPLAY_ON_CURSOR_OFF);
    lcd_backlight(10);
    in_menu = true;
    
    // copy config
    memcpy(&config_backup, &config, sizeof(config_t));
    memcpy(&config_edit, &config, sizeof(config_t));
    
    config.output_mode = output_pc;
    config.input_sources = input_ngc | input_n64 | input_snes;
    apply_config();
    
    menu_page(MENU_PAGE_OUTPUT);
}
void menu_exit(bool save_settings) {
    lcd_command(LCD_DISPLAY_OFF);
    lcd_backlight(0);
    in_menu = false;
    
    if (save_settings) {
        memcpy(&config, &config_edit, sizeof(config_t));
        save_config();
    }
    else {
        memcpy(&config, &config_backup, sizeof(config_t));
    }
    
    apply_config();
}

void menu_page(uint8_t page) {
    current_menu_page = page;
    menu_current_items = menu_sub_items[current_menu_page];
    submenu_idx = 0;
    menu_leftalign = true;
    submenu_count = 0;
    while (menu_current_items[submenu_count] != NULL) submenu_count++;

    lcd_clear();
    lcd_goto(1, 0);

    if (page == MENU_PAGE_OUTPUT) {
        lcd_string("Output device");
        submenu_idx = config_edit.output_mode;
    }
    else if (page == MENU_PAGE_PC_INPUTS) {
        lcd_string("PC inputs"); 
        submenu_idx = 0;
        submenu_mask = config_edit.input_sources;
    }
    else if (page == MENU_PAGE_INPUT && config_edit.output_mode == output_ngc) {
        lcd_string("NGC input");
        submenu_idx = 0; // todo: restore selection
    }
    else if (page == MENU_PAGE_INPUT && config_edit.output_mode == output_n64) {
        lcd_string("N64 input");
        submenu_idx = 0; // todo: restore selection
    }
    else if (page == MENU_PAGE_INPUT && config_edit.output_mode == output_snes) {
        lcd_string("SNES input");
        submenu_idx = 0; // todo: restore selection
    }
    else if (page == MENU_PAGE_CONFIRM) {
        lcd_string("Save");
        submenu_idx = 0;
    }
    
    menu_display_setting();
}

void menu_display_setting() {    
    uint8_t left_display = min(submenu_count - 2, max(0, menu_leftalign  ? submenu_idx - 1 : submenu_idx));
    uint8_t right_display = left_display + 1;
        
    lcd_goto(2, 0);
    lcd_char(left_display != 0 ? '<' : ' ');
    lcd_goto(2, 15);
    lcd_char(right_display < submenu_count - 1 ? '>' : ' ');    
    lcd_goto(2, 1);
    
    for (uint8_t i = left_display; i <= right_display; i++) {
        lcd_string(menu_current_items[i]);
        if (current_menu_page == MENU_PAGE_PC_INPUTS) {
            // multiple checkboxes
            lcd_char('[');
            bool marked = submenu_mask & (1<<i);
            bool focused = submenu_idx == i;
            if (marked && focused) lcd_char('*');
            else if (marked) lcd_char('o');
            else if (focused) lcd_char('.');
            else lcd_char(' ');
            lcd_string("] ");
        }
        else if (current_menu_page != MENU_PAGE_CONFIRM) {
            lcd_char('[');
            lcd_char(submenu_idx == i ? '*' : ' ');
            lcd_string("] ");
        }
    }
    
}


void menu_tasks() {
    if (!in_menu) return;
    if (menu_next_press_delay > 0) { 
        menu_next_press_delay--; 
        return; 
    }
    if (packets.snes_avail) {
        if (joydata_snes_raw.left) menu_press(mc_left);
        else if (joydata_snes_raw.right) menu_press(mc_right);
        if (joydata_snes_raw.start) menu_press(mc_exit);
        else if (joydata_snes_raw.a) menu_press(mc_select);
        else if (joydata_snes_raw.b) menu_press(mc_cancel);
        packets.snes_avail = false;
    }
    else if (packets.n64_avail) {
        if (joydata_n64_raw.dleft) menu_press(mc_left);
        else if (joydata_n64_raw.dright) menu_press(mc_right);
        if (joydata_n64_raw.start) menu_press(mc_exit);
        else if (joydata_n64_raw.a) menu_press(mc_select);
        else if (joydata_n64_raw.b) menu_press(mc_cancel);
        packets.n64_avail = false;
    }
    else if (packets.ngc_avail) {
        if (joydata_ngc_raw.dleft) menu_press(mc_left);
        else if (joydata_ngc_raw.dright) menu_press(mc_right);
        if (joydata_ngc_raw.start) menu_press(mc_exit);
        else if (joydata_ngc_raw.a) menu_press(mc_select);
        else if (joydata_ngc_raw.b) menu_press(mc_cancel);
        packets.ngc_avail = false;
    }
}

void menu_press(uint8_t command) {
    menu_next_press_delay = 300; // 300ms
    if (command == mc_exit)
        menu_page(MENU_PAGE_CONFIRM);
    else if (command == mc_confirm || command == mc_select) {
        if (current_menu_page == MENU_PAGE_CONFIRM) menu_exit(true);
        else if (current_menu_page == MENU_PAGE_OUTPUT) {
            config_edit.output_mode = submenu_idx;
            if (submenu_idx == output_ngc) menu_page(MENU_PAGE_INPUT);
            else if (submenu_idx == output_n64) menu_page(MENU_PAGE_INPUT);
            else if (submenu_idx == output_snes) menu_page(MENU_PAGE_INPUT);
            else if (submenu_idx == output_pc) menu_page(MENU_PAGE_PC_INPUTS);
        }
        else if (current_menu_page == MENU_PAGE_PC_INPUTS) {
            if (command == mc_select) {
                submenu_mask ^= (1 << submenu_idx);
                menu_display_setting();
            }
            else if (submenu_mask != 0) menu_page(MENU_PAGE_CONFIRM);
            else { /* no source selected */ }
        }
        else {
            // select only one input source, lookup from table
            uint8_t in = submenu_idx;
            config_edit.input_sources = in;
            menu_page(MENU_PAGE_CONFIRM);
        }
    }
    else if (command == mc_cancel) {
        // todo: navigate to previous menu
        menu_exit(false);
    }
    
    else if (command == mc_left) {
        if (submenu_idx == submenu_count - 2 && !menu_leftalign) 
            menu_leftalign = true;
        else {
            submenu_idx = max(0, submenu_idx - 1);
        }
        menu_display_setting();
    }
    else if (command == mc_right) {
        if (submenu_idx == 1 && menu_leftalign) 
            menu_leftalign = false;
        else {
            submenu_idx = min(submenu_idx + 1, submenu_count - 1);
        }        
        menu_display_setting();    
    }
}
