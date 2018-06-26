#include "configurator_if.h"

#include <usbd_hid.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

#define CFG_CMD_READ  0x47        // if return length
#define CFG_CMD_WRITE 0x44
#define CFG_CMD_WRITE_LEGACY 0x46 // used with old menu structure
#define CFG_CMD_ENTER_BL  0x48
#define CFG_CMD_REPORT_SIZE 0x08


//Class specific descriptor - HID
__ALIGN_BEGIN static const uint8_t hid_rpt_cfg[] __ALIGN_END = {
	0x06,	0x00,	0xFF,              // Usage Page = 0xFF00 (Vendor Defined Page 1)
	0x09,	0x01,                    // USAGE (Vendor Usage 1)
	0xa1,	0x01,                    // COLLECTION (Application)
	0x05,	0x01,                    //   USAGE_PAGE (Generic Desktop)
	0x15,	0x00,                    //     LOGICAL_MINIMUM (0)
	0x26,	0xff,	0x00,            //     LOGICAL_MAXIMUM (255)

	// write config command
	0x09,	0x00,                    //     USAGE (Undefined)
	0x95,	CFG_CMD_REPORT_SIZE,     //     REPORT_COUNT
	0x29,	CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
	0x19,	0x01,                    //     USAGE_MINIMUM
	0x75,	0x08,                    //     REPORT_SIZE (8)
	0x91,	0x00,                    //     OUTPUT (Data,Ary,Abs)
	0x29,	CFG_CMD_REPORT_SIZE,     //     USAGE_MAXIMUM
	0x19,	0x01,                    //     USAGE_MINIMUM
	0x81,	0x00,                    //     INPUT (Data,Ary,Abs)

	0xc0                             // END_COLLECTION   
};

const USBD_HID_AppType cfgApp = {
	.Name = "MUSIA config",
	.Report = {
		.Desc = hid_rpt_cfg,
		.Length = sizeof(hid_rpt_cfg),
		.IDs = 1, // IDs are used
	},
};


USBD_HID_IfHandleType hconfigurator_if = {
	.App = &cfgApp,
	.Base.AltCount = 1,
	.Config.InEp.Size = sizeof(cfg_write_report_t),
	.Config.InEp.Interval = 20,
	.Config.OutEp.Size = sizeof(cfg_read_report_t),
	.Config.OutEp.Interval = 20,
}, *const configurator_if = &hconfigurator_if;

