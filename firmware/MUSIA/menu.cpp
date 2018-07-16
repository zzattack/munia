#include "menu.h"
#include "lcd.h"
#include <cstdlib>
#include <cstring>
#include "eeprom.h"
#include <algorithm>

// [*] = multi select, checked and focused
// [o] = multi select, checked but not focused
// [.] = multi select, focused but not checked
// [ ] = not checked or focused


void menu_press(MENU_COMMAND command);
void menu_display_setting(); 
extern void applyConfig();


// this is where the input is redirected to
const char* menu_sub_items[][6] = {
	{ "PS2 ", "PC ", NULL },								   // MENU_PAGE::OUTPUT,
	{ "120Hz ", "100Hz", "60Hz", "50Hz", "30Hz", "25Hz" },  // MENU_PAGE::POLL_FREQ,
	{ "A:Ok ", " B:Cancel", NULL },						   // MENU_PAGE::CONFIRM,
};

uint8_t pollFreqs[] = { 120, 100, 60, 50, 30, 25 };
EELayout config_backup, config_edit;
extern eeprom ee;
uint8_t submenu_idx = 0;
uint8_t submenu_mask = 0;
bool menu_leftalign = true;
bool in_menu;
MENU_PAGE current_menu_page;

uint16_t menu_next_press_delay;
const char** menu_current_items;
uint8_t submenu_count = 0;


void menu_enter() {
	lcd_command(LCD_DISPLAY_ON_CURSOR_OFF);
	// lcd_backlight(10);
	in_menu = true;
    
	// copy config
	memcpy(&config_backup, ee.data, sizeof(EELayout));
	memcpy(&config_edit, ee.data, sizeof(EELayout));
    
	ee.data->mode = MODE_POLLER;
	applyConfig();
    
	menu_page(MENU_PAGE::OUTPUT);
}
void menu_exit(bool save_settings) {
	lcd_command(LCD_DISPLAY_OFF);
	// lcd_backlight(0);
	in_menu = false;
    
	if (save_settings) {
		memcpy(ee.data, &config_edit, sizeof(EELayout));
		ee.sync();
	}
	else {
		memcpy(ee.data, &config_backup, sizeof(EELayout));
	}
    
	applyConfig();
}

unsigned int log2(unsigned int x) {
	unsigned int ans = 0;
	while (x >>= 1) ans++;
	return ans ;
}

void menu_page(MENU_PAGE page) {
	current_menu_page = page;
	menu_current_items = menu_sub_items[(int)page];
	submenu_idx = 0;
	menu_leftalign = true;
	submenu_count = 0;
	while (menu_current_items[submenu_count] != NULL) submenu_count++;

	lcd_clear();
	lcd_goto(1, 0);

	if (page == MENU_PAGE::OUTPUT) {
		lcd_string("Output device");
		submenu_idx = config_edit.mode;
	}
	else if (page == MENU_PAGE::POLL_FREQ) {
		lcd_string("Poll frequency");
		submenu_idx = 0;
		for (int i = 0; i < sizeof(pollFreqs) / sizeof(pollFreqs[0]); i++)
			if (pollFreqs[i] = ee.data->pollFreq)
				submenu_idx  = i;
	}
	else if (page == MENU_PAGE::CONFIRM) {
		lcd_string("Save");
		submenu_idx = 0;
	}    
	menu_display_setting();
}

void menu_display_setting() {    
	uint8_t left_display = std::min(submenu_count - 2, std::max(0, menu_leftalign  ? submenu_idx - 1 : submenu_idx));
	uint8_t right_display = left_display + 1;
        
	lcd_goto(2, 0);
	lcd_char(left_display != 0 ? '<' : ' ');
	lcd_goto(2, 15);
	lcd_char(right_display < submenu_count - 1 ? '>' : ' ');    
	lcd_goto(2, 1);
    
	for (uint8_t i = left_display; i <= right_display; i++) {
		lcd_string(menu_current_items[i]);
		if (current_menu_page != MENU_PAGE::CONFIRM) {
			lcd_char('[');
			lcd_char(submenu_idx == i ? '*' : ' ');
			lcd_string("] ");
		}
	}
    
}

void menu_packet(ps2_state* packet) {
	if (!in_menu) return;
	if (menu_next_press_delay > 0) { 
		menu_next_press_delay--; 
		return; 
	}
	if (packet->dpad_left) menu_press(MENU_COMMAND::LEFT);
	else if (packet->dpad_right) menu_press(MENU_COMMAND::RIGHT);
	if (packet->start) menu_press(MENU_COMMAND::EXIT);
	else if (packet->cross || packet->select) menu_press(MENU_COMMAND::SELECT);
	else if (packet->circle) menu_press(MENU_COMMAND::CANCEL);
}

void menu_press(MENU_COMMAND command) {
	menu_next_press_delay = 300; // 300ms

	if (command == MENU_COMMAND::EXIT) {
		menu_page(MENU_PAGE::CONFIRM);
	 }

	else if (command == MENU_COMMAND::CONFIRM || command == MENU_COMMAND::SELECT) {
		if (current_menu_page == MENU_PAGE::OUTPUT) {
			config_edit.mode = (musia_mode)submenu_idx;
			if (config_edit.mode == MODE_POLLER) menu_page(MENU_PAGE::POLL_FREQ);
			else menu_page(MENU_PAGE::CONFIRM);
		}
		else if (current_menu_page == MENU_PAGE::POLL_FREQ) {
			menu_page(MENU_PAGE::CONFIRM);
		} 
		else if (current_menu_page == MENU_PAGE::CONFIRM) {
			menu_exit(true);
		}	     
	}

	else if (command == MENU_COMMAND::CANCEL) {
	    // todo: navigate to previous menu
		menu_exit(false);
	}
    
	else if (command == MENU_COMMAND::LEFT) {
		if (submenu_idx == submenu_count - 2 && !menu_leftalign) 
			menu_leftalign = true;
		else {
			submenu_idx = std::max(0, submenu_idx - 1);
		}
		menu_display_setting();
	}
	else if (command == MENU_COMMAND::RIGHT) {
		if (submenu_idx == 1 && menu_leftalign) 
			menu_leftalign = false;
		else {
			submenu_idx = std::min(submenu_idx + 1, submenu_count - 1);
		}        
		menu_display_setting();    
	}
}
