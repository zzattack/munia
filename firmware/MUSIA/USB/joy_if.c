#include "joy_if.h"
#include <usbd_hid.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

//Class specific descriptor - HID
__ALIGN_BEGIN static const uint8_t hid_rpt_ps2[] __ALIGN_END = {
	0x05,
	0x01,                    // USAGE_PAGE (Generic Desktop)
	0x09,
	0x04,                    // USAGE (Joystick)
	0xa1,
	0x01,                    // COLLECTION (Application)

	// a b x y start
	0x05,
	0x09,                    //   USAGE_PAGE (Button)
	0x19,
	0x01,                    //   USAGE_MINIMUM (Button 1)
	0x29,
	0x0C,                    //   USAGE_MAXIMUM (Button12)
	0x15,
	0x00,                    //   LOGICAL MINIMUM (0)
	0x25,
	0x01,                    //   LOGICAL_MAXIMUM (1)
	0x95,
	0x0C,                    //   REPORT_COUNT (12)
	0x75,
	0x01,                    //   REPORT_SIZE (1)
	0x81,
	0x02,                    //   INPUT (Data,Var,Abs)
    
    
	// hat
	0x05,
	0x01,                    //   USAGE_PAGE(Generic Desktop)
	0x09,
	0x39,                    //   USAGE(Hat Switch)
	0x15,
	0x00,                    //   LOGICAL MINIMUM (0)
	0x25,
	0x07,                    //   LOGICAL MAXIMUM (7)
	0x46,
	0x3b,
	0b01,              //   PHYSICAL MAXIMUM (315)
	0x65,
	0x14,                    //   UNIT (English,Rot,Ang.Pos))
	0x75,
	0x04,                    //   REPORT_SIZE(4)
	0x95,
	0x01,                    //   REPORT_COUNT(1)
	0x81,
	0x42,                    //   INPUT(Data,Var,Abs,Null)

    
	// 2 analog sticks
	0x05,
	0x01,                    //   USAGE_PAGE (Generic Desktop)
	0x09,
	0x30,                    //   USAGE (X)
	0x09,
	0x31,                    //   USAGE (Y)
	0x09,
	0x33,                    //   USAGE (Rx)
	0x09,
	0x34,                    //   USAGE (Ry)
	0x15,
	0x00,                    //   LOGICAL MINIMUM (0)
	0x26,
	0xff,
	0x00,              //   LOGICAL_MAXIMUM (255)
	0x75,
	0x08,                    //   REPORT_SIZE (8)
	0x95,
	0x04,                    //   REPORT_COUNT (4)
	0x81,
	0x02,                    //   INPUT (Data,Var,Abs)
    

	0xc0,                          // END_COLLECTION
};

const USBD_HID_AppType joyApp = {
	.Name = "PS2 controller",
	.Report = {
		.Desc = hid_rpt_ps2,
		.Length = sizeof(hid_rpt_ps2),
		.IDs = 0,	
	},
};

USBD_HID_IfHandleType hjoy_if = {
	.App = &joyApp,
	.Base.AltCount = 1,
	.Config.InEp.Size = sizeof(ps2_hid_packet),
	.Config.InEp.Interval = 1,
}, *const joy_if = &hjoy_if;
