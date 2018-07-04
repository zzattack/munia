#include "configurator_if.h"

#include <usbd_hid.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

#define CFG_CMD_WRITE 0x44
#define CFG_CMD_INFO  0x46
#define CFG_CMD_READ  0x47
#define HID_DETACH 0x55
#define CFG_CMD_REPORT_SIZE sizeof(cfg_read_report_t)


extern void rebootToBootloader();
extern void setEEPROMConfig(const uint8_t* buffer); 
extern void getEEPROMConfig(uint8_t* buffer, uint8_t buffSize);

uint8_t cfgBuff[CFG_CMD_REPORT_SIZE ];

void Configurator_Init();
void Configurator_SetReport(uint8_t* data, uint16_t length);
void Configurator_GetReport(uint8_t reportId);

//Class specific descriptor - HID
__ALIGN_BEGIN static const uint8_t hid_rpt_cfg[] __ALIGN_END = {
	0x06, 0x00, 0xFF,				// Usage Page = 0xFF00 (Vendor Defined Page 1)
	0x09, 0x01,						// USAGE (Vendor Usage 1)

	0xa1, 0x01,						// COLLECTION (Application)
	0x05, 0x01,						//   USAGE_PAGE (Generic Desktop)
	0x15, 0x00,						//     LOGICAL_MINIMUM (0)
	0x26, 0xff, 0x00,				//     LOGICAL_MAXIMUM (255)
    0x75, 0x08,						//     REPORT_SIZE (8)

									// write config command
	0x85, CFG_CMD_WRITE,			//     REPORT ID 0x44
    0x09, 0x01,						//       USAGE (Vendor defined usage 1)
    0x95, CFG_CMD_REPORT_SIZE,		//       REPORT_COUNT
    0xB1, 0x00,						//       FEATURE (Data,Ary,Abs)

										// read config command
	0x85, CFG_CMD_INFO,				//     REPORT ID 0x46
    0x09, 0x02,						//       USAGE (Vendor defined usage 2)
    0x95, CFG_CMD_REPORT_SIZE,		//       REPORT_COUNT
    0xB1, 0x00,						//       FEATURE (Data,Ary,Abs)

									// read info command
	0x85, CFG_CMD_READ,				//     REPORT ID 0x47
    0x09, 0x03,						//       USAGE (Vendor defined usage 3)
    0x95, CFG_CMD_REPORT_SIZE,		//       REPORT_COUNT
    0xB1, 0x00,						//       FEATURE (Data,Ary,Abs)

									// HID detach
	0x06, 0x00, 0xFF,				//   Usage Page = 0xFF00 (Vendor Defined Page 1)
	0x85, HID_DETACH,				//     REPORT ID 0x55
    0x09, HID_DETACH,				//       USAGE (HID Detach)
    0x95, 0x01,						//       REPORT_COUNT (1)
    0xB1, 0x82,						//       FEATURE (Data,Var,Abs,Vol)
	

    0xc0							// END_COLLECTION     
};

const USBD_HID_AppType cfgApp = {
	.Name = "MUSIA config",
	.Report = {
		.Desc = hid_rpt_cfg,
		.Length = sizeof(hid_rpt_cfg),
		.IDs = 0,//1,
	},
	.Init = Configurator_Init,
	.SetReport = Configurator_SetReport,
	.GetReport = Configurator_GetReport,
};


USBD_HID_IfHandleType hconfigurator_if = {
	.App = &cfgApp,
	.Base.AltCount = 1,
	.Config.InEp.Size = CFG_CMD_REPORT_SIZE,
	.Config.InEp.Interval = 20,
	.Config.OutEp.Size = CFG_CMD_REPORT_SIZE,
	.Config.OutEp.Interval = 20,
}, *const configurator_if = &hconfigurator_if;

void Configurator_Init() {
	USBD_HID_ReportOut(&hconfigurator_if, cfgBuff, sizeof(cfgBuff));
}

void Configurator_SetReport(uint8_t* data, uint16_t length) {
	sys_printf("Configurator_SetReport\n");
	
	uint8_t reportId = data[0];
	if (reportId == CFG_CMD_WRITE) {
		setEEPROMConfig(&data[1]);
	}
	if (reportId == HID_DETACH) {
		// no point in confirming success, DFuSeDemo doesn't listen for it anyway
		rebootToBootloader();
	}

	// re-arm endpoint
	USBD_HID_ReportOut(configurator_if, cfgBuff, sizeof(cfgBuff));
}

void Configurator_GetReport(uint8_t reportId) {
	sys_printf("Configurator_GetReport\n");

	uint8_t resp[9];
	resp[0] = reportId;

	if (reportId == CFG_CMD_READ) {
		getEEPROMConfig(&resp[1], sizeof(cfg_read_report_t));
	}
	else if (reportId == CFG_CMD_INFO) {
		cfg_info_report_t* rep = (cfg_info_report_t*)&resp[1];
		rep->device_code = DBGMCU->IDCODE.w;
		rep->fw_ver_major = FW_MAJOR;
		rep->fw_ver_minor = FW_MINOR;
		rep->hw_rev = HW_REVISION;
	}

	USBD_HID_ReportIn(configurator_if, resp, sizeof(resp));

}
