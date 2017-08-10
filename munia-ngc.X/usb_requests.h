#ifndef USB_REQUESTS_H
#define USB_REQUESTS_H

#include <stdint.h>
#include "config.h"

void usb_tasks();

typedef struct {
    uint8_t command_id;
    uint8_t fw_minor : 4;
    uint8_t fw_major : 4;    
    uint8_t hw_revision : 4;
    uint8_t device_type : 4;        
    config_t config;
    union {
        struct {
            uint8_t devid1;
            uint8_t revision : 5;
            uint8_t devid2 : 3;
        };
        uint16_t device_id;
    };
} cfg_read_report_t;

typedef struct {
    uint8_t command_id;
    config_t config;
} cfg_write_report_t;



#endif