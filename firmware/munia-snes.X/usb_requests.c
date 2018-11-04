#include "usb_requests.h"
#include "report_descriptors.h"
#include "globals.h"
#include <usb/usb.h>
#include <usb/usb_device_hid.h>
#include "config.h"

extern config_t config;

void usb_tasks() {
    // User Application USB tasks
    if (USBDeviceState < CONFIGURED_STATE || USBSuspendControl == 1)
        return;
    
    // Check if we have received an OUT data packet from the host, and nothing is left in buffer
    if (!HIDRxHandleBusy(USBOutHandleCfg) && !HIDTxHandleBusy(USBInHandleCfg)) {
        // We just received a packet of data from the USB host.
        // Check the first byte of the packet to see what command the host
        // application software wants us to fulfill.
        uint8_t cmd = usbOutBuffer[0];
        if (cmd == CFG_CMD_READ) {
            // format response
            cfg_read_report_t* report = (cfg_read_report_t*)usbInBuffer;
            report->command_id = CFG_CMD_READ;
            report->fw_major = FW_MAJOR;
            report->fw_minor = FW_MINOR;
            report->hw_revision = HW_REVISION;
            report->device_type = MUNIA_DEVICETYPE;

            uint8_t* r = &report->devid1;
            TBLPTR = 0x3FFFFF;
            asm("tblrd*-");
            *r++ = TABLAT;
            asm("tblrd*");
            *r = TABLAT;
            TBLPTRU = 0;

            memcpy(&report->config, &config, sizeof(config_t));
            memset(usbInBuffer + sizeof(cfg_read_report_t), 0, sizeof(usbInBuffer) - sizeof(cfg_read_report_t));       
            
            USBInHandleCfg = HIDTxPacket(HID_EP_CFG, usbInBuffer, CFG_CMD_REPORT_SIZE);
        }
        else if (cmd == CFG_CMD_WRITE) {
            // extract config
            cfg_write_report_t* report = (cfg_write_report_t*)usbOutBuffer;
            memcpy(&config, &report->config, sizeof(config_t));
            save_config();
            apply_config();
            
            // response
            usbInBuffer[0] = CFG_CMD_WRITE;
            usbInBuffer[1] = 1; // signifies ok
            memset(usbInBuffer + 2, 0, sizeof(usbInBuffer) - 2);
            
            USBInHandleCfg = HIDTxPacket(HID_EP_CFG, usbInBuffer, CFG_CMD_REPORT_SIZE);
        }
        
        else if (cmd == CFG_CMD_ENTER_BL) {
            // dbgs("jumping to bootloader\n");
            asm("goto 0x001C");
        }
        
        // re-arm
        USBOutHandleCfg = HIDRxPacket(HID_EP_CFG, usbOutBuffer, sizeof (usbOutBuffer));
    }
}