#pragma once

#include <stdint.h>

#define USBD_VID     1155
#define USBD_LANGID_STRING     1033
#define USBD_MANUFACTURER_STRING     "munia.io"
#define USBD_PID_FS     22352
#define USBD_PRODUCT_STRING_FS     "MUSIA"
#define USBD_SERIALNUMBER_STRING_FS     "00000000001A"
#define USBD_CONFIGURATION_STRING_FS     "Custom HID Config"
#define USBD_INTERFACE_STRING_FS     "PS2 controller"




#define CFG_CMD_READ  0x47        // if return length
#define CFG_CMD_WRITE 0x44
#define CFG_CMD_WRITE_LEGACY 0x46 // used with old menu structure
#define CFG_CMD_ENTER_BL  0x48
#define CFG_CMD_REPORT_SIZE 0x08

const uint8_t hid_rpt_cfg[] = {
	0x06,
	0x00,
	0xFF,              // Usage Page = 0xFF00 (Vendor Defined Page 1)
	0x09,
	0x01,                    // USAGE (Vendor Usage 1)
	0xa1,
	0x01,                    // COLLECTION (Application)
	0x05,
	0x01,                    //   USAGE_PAGE (Generic Desktop)
	0x15,
	0x00,                    //     LOGICAL_MINIMUM (0)
	0x26,
	0xff,
	0x00,              //     LOGICAL_MAXIMUM (255)

	// write config command
	0x09,
	0x00,                    //     USAGE (Undefined)
	0x95,
	CFG_CMD_REPORT_SIZE,     //     REPORT_COUNT
	0x29,
	CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
	0x19,
	0x01,                    //     USAGE_MINIMUM
	0x75,
	0x08,                    //     REPORT_SIZE (8)
	0x91,
	0x00,                    //     OUTPUT (Data,Ary,Abs)
	0x29,
	CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
	0x19,
	0x01,                    //     USAGE_MINIMUM
	0x81,
	0x00,                    //     INPUT (Data,Ary,Abs)

	0xc0                           // END_COLLECTION    
};


#define HID_RPT_CFG_SIZE          sizeof(hid_rpt_cfg)

#define USB_ARRAYLEN(X) (sizeof(X)/sizeof(*X))
#define STATIC_SIZE_CHECK_EQUAL(X,Y) typedef char USB_CONCAT(STATIC_SIZE_CHECK_LINE_,__LINE__) [(X==Y)?1:-1]
#define USB_CONCAT(X,Y)  USB_CONCAT_HIDDEN(X,Y)
#define USB_CONCAT_HIDDEN(X,Y) X ## Y
