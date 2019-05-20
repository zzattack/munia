#include "ps2_controller_if.h"
#include <usbd_hid.h>
#include <stdint.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>


static void PS2Controller_Init();

//Class specific descriptor - HID
__ALIGN_BEGIN static const uint8_t hid_rpt_ps2[] __ALIGN_END = {
	0x05, 0x01,						// USAGE_PAGE (Generic Desktop)
	0x09, 0x04,						// USAGE (Joystick)
	0xa1, 0x01,						// COLLECTION (Application)
									// 12 buttons (4x face, l1/l2/r1/r2, start, select, lstick, rstick)

    // 0x85, 0x01,						// Report ID 1
	0x05, 0x09,						//   USAGE_PAGE (Button)
	0x19, 0x01,						//     USAGE_MINIMUM (Button 1)
	0x29, 0x0C,						//     USAGE_MAXIMUM (Button12)
	0x15, 0x00,						//     LOGICAL MINIMUM (0)
	0x25, 0x01,						//     LOGICAL_MAXIMUM (1)
	0x95, 0x0C,						//     REPORT_COUNT (12)
	0x75, 0x01,						//     REPORT_SIZE (1)
	0x81, 0x02,						//     INPUT (Data,Var,Abs)
									// hat
	0x05, 0x01,						//   USAGE_PAGE(Generic Desktop)
	0x09, 0x39,						//     USAGE(Hat Switch)
	0x15, 0x00,						//     LOGICAL MINIMUM (0)
	0x25, 0x07,						//     LOGICAL MAXIMUM (7)
	0x46, 0x3b, 0b01,				//     PHYSICAL MAXIMUM (315)
	0x65, 0x14,						//     UNIT (English,Rot,Ang.Pos))
	0x75, 0x04,						//     REPORT_SIZE(4)
	0x95, 0x01,						//     REPORT_COUNT(1)
	0x81, 0x42,						//     INPUT(Data,Var,Abs,Null)

	// 2 analog sticks
	0x05, 0x01,						//   USAGE_PAGE (Generic Desktop)
	0x09, 0x30,						//     USAGE (X)
	0x09, 0x31,						//     USAGE (Y)
	0x09, 0x33,						//     USAGE (Rx)
	0x09, 0x34,						//     USAGE (Ry)
	0x15, 0x00,						//     LOGICAL MINIMUM (0)
	0x26, 0xff, 0x00,				//     LOGICAL_MAXIMUM (255)
	0x75, 0x08,						//     REPORT_SIZE (8)
	0x95, 0x04,						//     REPORT_COUNT (4)
	0x81, 0x02,						//     INPUT (Data,Var,Abs)

	// 1 byte indicating pressure sensor presence + 18 bytes of pressure data
	0x05, 0x01,						//   USAGE_PAGE (Generic Desktop)
	0x09, 0x00,						//     USAGE (UNDEFINED)
	0x15, 0x00,						//     LOGICAL MINIMUM (0)
	0x26, 0xff, 0x00,				//     LOGICAL_MAXIMUM (255)
	0x75, 0x08,						//     REPORT_SIZE (8)
	0x95, 0x13,						//     REPORT_COUNT (19)
	0x81, 0x02,						//     INPUT (Data,Var,Abs)
	
	0xC0,    //    End Collection (Application)
};


const USBD_HID_AppType ps2controllerApp = {
	.Name = "MUSIA PS2 controller",
	.Report = {
		.Desc = hid_rpt_ps2,
		.Length = sizeof(hid_rpt_ps2),
	},
	.Init = PS2Controller_Init,
};

USBD_HID_IfHandleType hps2controller_if = {
	.App = &ps2controllerApp,
	.Base.AltCount = 1,
	.Config.InEp.Size = sizeof(ps2_hid_packet),
	.Config.InEp.Interval_ms = 1,
}, *const ps2controller_if = &hps2controller_if;



uint8_t ps2Buff[32];
static void PS2Controller_Init() {
	USBD_HID_ReportOut(&hps2controller_if, ps2Buff, 16);
}
