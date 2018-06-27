#include "ps2_controller_if.h"
#include <usbd_hid.h>
#include "usbd_pid.h"
#include <stdint.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>
#include "hid_usage.h"

//Class specific descriptor - HID
__ALIGN_BEGIN static const uint8_t hid_rpt_ps2[] __ALIGN_END = {
	0x05, 0x01,					// USAGE_PAGE (Generic Desktop)
	0x09, 0x04,					// USAGE (Joystick)
	0xa1, 0x01,					// COLLECTION (Application)
    0x85, 0x01,						//    Report ID 1
	// 12 buttons (4x face, l1/l2/r1/r2, start, select, lstick, rstick)
	0x05,	0x09,					//   USAGE_PAGE (Button)
	0x19,	0x01,					//   USAGE_MINIMUM (Button 1)
	0x29,	0x0C,					//   USAGE_MAXIMUM (Button12)
	0x15,	0x00,					//   LOGICAL MINIMUM (0)
	0x25,	0x01,					//   LOGICAL_MAXIMUM (1)
	0x95,	0x0C,					//   REPORT_COUNT (12)
	0x75,	0x01,					//   REPORT_SIZE (1)
	0x81,	0x02,					//   INPUT (Data,Var,Abs)

	// hat
	0x05,	0x01,					//   USAGE_PAGE(Generic Desktop)
	0x09,	0x39,					//   USAGE(Hat Switch)
	0x15,	0x00,					//   LOGICAL MINIMUM (0)
	0x25,	0x07,					//   LOGICAL MAXIMUM (7)
	0x46,	0x3b,	0b01,			//   PHYSICAL MAXIMUM (315)
	0x65,	0x14,					//   UNIT (English,Rot,Ang.Pos))
	0x75,	0x04,					//   REPORT_SIZE(4)
	0x95,	0x01,					//   REPORT_COUNT(1)
	0x81,	0x42,					//   INPUT(Data,Var,Abs,Null)


	// 2 analog sticks
	0x05,	0x01,					//   USAGE_PAGE (Generic Desktop)
	0x09,	0x30,					//   USAGE (X)
	0x09,	0x31,					//   USAGE (Y)
	0x09,	0x33,					//   USAGE (Rx)
	0x09,	0x34,					//   USAGE (Ry)
	0x15,	0x00,					//   LOGICAL MINIMUM (0)
	0x26,	0xff,	0x00,			//   LOGICAL_MAXIMUM (255)
	0x75,	0x08,					//   REPORT_SIZE (8)
	0x95,	0x04,					//   REPORT_COUNT (4)
	0x81,	0x02,					//   INPUT (Data,Var,Abs)    
	

	// remainig huuge descriptor is for a 'PID' physical interface
	// so that we can have driverless force-feedback
	
	0x05,0x0F,			//    Usage Page Physical Interface
	0x09,0x92,			//    Usage ES Playing
	0xA1,0x02,			//    Collection Datalink
	0x85,0x02,				//    Report ID 2
	0x09,0x9F,				//    Usage DS Device is Reset
	0x09,0xA0,				//    Usage DS Device is Pause
	0x09,0xA4,				//    Usage Actuator Power
	0x09,0xA5,				//    Usage Undefined
	0x09,0xA6,				//    Usage Undefined
	0x15,0x00,				//    Logical Minimum 0
	0x25,0x01,				//    Logical Maximum 1
	0x35,0x00,				//    Physical Minimum 0
	0x45,0x01,				//    Physical Maximum 1
	0x75,0x01,				//    Report Size 1
	0x95,0x05,				//    Report Count 5
	0x81,0x02,				//    Input (Variable)
	0x95,0x03,				//    Report Count 3
	0x75,0x01,				//    Report Size 1
	0x81,0x03,				//    Input (Constant, Variable)
	0x09,0x94,				//    Usage PID Device Control
	0x15,0x00,					//    Logical Minimum 0
	0x25,0x01,					//    Logical Maximum 1
	0x35,0x00,					//    Physical Minimum 0
	0x45,0x01,					//    Physical Maximum 1
	0x75,0x01,					//    Report Size 1
	0x95,0x01,					//    Report Count 1
	0x81,0x02,					//    Input (Variable)
	0x09,0x22,					//    Usage Effect Block Index
	0x15,0x01,						//    Logical Minimum 1
	0x25,0x28,						//    Logical Maximum 28h (40d)
	0x35,0x01,						//    Physical Minimum 1
	0x45,0x28,						//    Physical Maximum 28h (40d)
	0x75,0x07,						//    Report Size 7
	0x95,0x01,						//    Report Count 1
	0x81,0x02,						//    Input (Variable)
	0xC0    ,		// End Collection


	0x09,0x21,		//    Usage Set Effect Report
	0xA1,0x02,		//    Collection Datalink
	0x85,0x01,		//    Report ID 1
	0x09,0x22,		//    Usage Effect Block Index
	0x15,0x01,		//    Logical Minimum 1
	0x25,0x28,		//    Logical Maximum 28h (40d)
	0x35,0x01,		//    Physical Minimum 1
	0x45,0x28,		//    Physical Maximum 28h (40d)
	0x75,0x08,		//    Report Size 8
	0x95,0x01,		//    Report Count 1
	0x91,0x02,		//    Output (Variable)
	0x09,0x25,		//    Usage Effect Type
	0xA1,0x02,		//    Collection Datalink
		0x09,0x26,    //    Usage ET Constant Force
		0x25,0x01,    //    Logical Maximum Ch (1d)
		0x15,0x01,    //    Logical Minimum 1
		0x35,0x01,    //    Physical Minimum 1
		0x45,0x01,    //    Physical Maximum Ch (1d)
		0x75,0x08,    //    Report Size 8
		0x95,0x01,    //    Report Count 1
		0x91,0x00,    //    Output
	0xC0    ,          //    End Collection

	0x09,0x50,         //    Usage Duration
	0x09,0x54,         //    Usage Trigger Repeat Interval
	0x09,0x51,         //    Usage Sample Period
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x7F,    //    Logical Maximum 7FFFh (32767d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0xFF,0x7F,    //    Physical Maximum 7FFFh (32767d)
	0x66,0x03,0x10,    //    Unit 1003h (4099d)
	0x55,0xFD,         //    Unit Exponent FDh (253d)
	0x75,0x10,         //    Report Size 10h (16d)
	0x95,0x03,         //    Report Count 3
	0x91,0x02,         //    Output (Variable)
	0x55,0x00,         //    Unit Exponent 0
	0x66,0x00,0x00,    //    Unit 0
	0x09,0x52,         //    Usage Gain
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x00,    //    Logical Maximum FFh (255d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x75,0x08,         //    Report Size 8
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x09,0x53,         //    Usage Trigger Button
	0x15,0x01,         //    Logical Minimum 1
	0x25,0x08,         //    Logical Maximum 8
	0x35,0x01,         //    Physical Minimum 1
	0x45,0x08,         //    Physical Maximum 8
	0x75,0x08,         //    Report Size 8
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x09,0x55,         //    Usage Axes Enable
	0xA1,0x02,         //    Collection Datalink
		0x05,0x01,    //    Usage Page Generic Desktop
		0x09,0x30,    //    Usage X
		0x09,0x31,    //    Usage Y
		0x15,0x00,    //    Logical Minimum 0
		0x25,0x01,    //    Logical Maximum 1
		0x75,0x01,    //    Report Size 1
		0x95,0x02,    //    Report Count 2
		0x91,0x02,    //    Output (Variable)
	0xC0     ,    // End Collection
	0x05,0x0F,    //    Usage Page Physical Interface
	0x09,0x56,    //    Usage Direction Enable
	0x95,0x01,    //    Report Count 1
	0x91,0x02,    //    Output (Variable)
	0x95,0x05,    //    Report Count 5
	0x91,0x03,    //    Output (Constant, Variable)
	0x09,0x57,    //    Usage Direction
	0xA1,0x02,    //    Collection Datalink
		0x0B,0x01,0x00,0x0A,0x00,    //    Usage Ordinals: Instance 1
		0x0B,0x02,0x00,0x0A,0x00,    //    Usage Ordinals: Instance 2
		0x66,0x14,0x00,              //    Unit 14h (20d)
		0x55,0xFE,                   //    Unit Exponent FEh (254d)
		0x15,0x00,                   //    Logical Minimum 0
		0x26,0xFF,0x00,              //    Logical Maximum FFh (255d)
		0x35,0x00,                   //    Physical Minimum 0
		0x47,0xA0,0x8C,0x00,0x00,    //    Physical Maximum 8CA0h (36000d)
		0x66,0x00,0x00,              //    Unit 0
		0x75,0x08,                   //    Report Size 8
		0x95,0x02,                   //    Report Count 2
		0x91,0x02,                   //    Output (Variable)
		0x55,0x00,                   //    Unit Exponent 0
		0x66,0x00,0x00,              //    Unit 0
	0xC0     ,         //    End Collection
	0x05,0x0F,         //    Usage Page Physical Interface
	0x09,0xA7,         //    Usage Undefined
	0x66,0x03,0x10,    //    Unit 1003h (4099d)
	0x55,0xFD,         //    Unit Exponent FDh (253d)
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x7F,    //    Logical Maximum 7FFFh (32767d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0xFF,0x7F,    //    Physical Maximum 7FFFh (32767d)
	0x75,0x10,         //    Report Size 10h (16d)
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x66,0x00,0x00,    //    Unit 0
	0x55,0x00,         //    Unit Exponent 0
	0xC0     ,    //    End Collection
	0x05,0x0F,    //    Usage Page Physical Interface
	0x09,0x5A,    //    Usage Set Envelope Report
	0xA1,0x02,    //    Collection Datalink
	0x85,0x02,         //    Report ID 2
	0x09,0x22,         //    Usage Effect Block Index
	0x15,0x01,         //    Logical Minimum 1
	0x25,0x28,         //    Logical Maximum 28h (40d)
	0x35,0x01,         //    Physical Minimum 1
	0x45,0x28,         //    Physical Maximum 28h (40d)
	0x75,0x08,         //    Report Size 8
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x09,0x5B,         //    Usage Attack Level
	0x09,0x5D,         //    Usage Fade Level
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x00,    //    Logical Maximum FFh (255d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x95,0x02,         //    Report Count 2
	0x91,0x02,         //    Output (Variable)
	0x09,0x5C,         //    Usage Attack Time
	0x09,0x5E,         //    Usage Fade Time
	0x66,0x03,0x10,    //    Unit 1003h (4099d)
	0x55,0xFD,         //    Unit Exponent FDh (253d)
	0x26,0xFF,0x7F,    //    Logical Maximum 7FFFh (32767d)
	0x46,0xFF,0x7F,    //    Physical Maximum 7FFFh (32767d)
	0x75,0x10,         //    Report Size 10h (16d)
	0x91,0x02,         //    Output (Variable)
	0x45,0x00,         //    Physical Maximum 0
	0x66,0x00,0x00,    //    Unit 0
	0x55,0x00,         //    Unit Exponent 0
	0xC0     ,            //    End Collection
	0x09,0x5F,    //    Usage Set Condition Report
	0xA1,0x02,    //    Collection Datalink
	0x85,0x03,    //    Report ID 3
	0x09,0x22,    //    Usage Effect Block Index
	0x15,0x01,    //    Logical Minimum 1
	0x25,0x28,    //    Logical Maximum 28h (40d)
	0x35,0x01,    //    Physical Minimum 1
	0x45,0x28,    //    Physical Maximum 28h (40d)
	0x75,0x08,    //    Report Size 8
	0x95,0x01,    //    Report Count 1
	0x91,0x02,    //    Output (Variable)
	0x09,0x23,    //    Usage Parameter Block Offset
	0x15,0x00,    //    Logical Minimum 0
	0x25,0x01,    //    Logical Maximum 1
	0x35,0x00,    //    Physical Minimum 0
	0x45,0x01,    //    Physical Maximum 1
	0x75,0x04,    //    Report Size 4
	0x95,0x01,    //    Report Count 1
	0x91,0x02,    //    Output (Variable)
	0x09,0x58,    //    Usage Type Specific Block Off...
	0xA1,0x02,    //    Collection Datalink
		0x0B,0x01,0x00,0x0A,0x00,    //    Usage Ordinals: Instance 1
		0x0B,0x02,0x00,0x0A,0x00,    //    Usage Ordinals: Instance 2
		0x75,0x02,                   //    Report Size 2
		0x95,0x02,                   //    Report Count 2
		0x91,0x02,                   //    Output (Variable)
	0xC0     ,         //    End Collection
	0x15,0x80,         //    Logical Minimum 80h (-128d)
	0x25,0x7F,         //    Logical Maximum 7Fh (127d)
	0x36,0xF0,0xD8,    //    Physical Minimum D8F0h (-10000d)
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x09,0x60,         //    Usage CP Offset
	0x75,0x08,         //    Report Size 8
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x36,0xF0,0xD8,    //    Physical Minimum D8F0h (-10000d)
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x09,0x61,         //    Usage Positive Coefficient
	0x09,0x62,         //    Usage Negative Coefficient
	0x95,0x02,         //    Report Count 2
	0x91,0x02,         //    Output (Variable)
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x00,    //    Logical Maximum FFh (255d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x09,0x63,         //    Usage Positive Saturation
	0x09,0x64,         //    Usage Negative Saturation
	0x75,0x08,         //    Report Size 8
	0x95,0x02,         //    Report Count 2
	0x91,0x02,         //    Output (Variable)
	0x09,0x65,         //    Usage Dead Band
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0xC0     ,    //    End Collection
	
	0x09,0x73,    //    Usage Set Constant Force Rep...
	0xA1,0x02,    //    Collection Datalink
	0x85,0x05,         //    Report ID 5
	0x09,0x22,         //    Usage Effect Block Index
	0x15,0x01,         //    Logical Minimum 1
	0x25,0x28,         //    Logical Maximum 28h (40d)
	0x35,0x01,         //    Physical Minimum 1
	0x45,0x28,         //    Physical Maximum 28h (40d)
	0x75,0x08,         //    Report Size 8
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0x09,0x70,         //    Usage Magnitude
	0x16,0x01,0xFF,    //    Logical Minimum FF01h (-255d)
	0x26,0xFF,0x00,    //    Logical Maximum FFh (255d)
	0x36,0xF0,0xD8,    //    Physical Minimum D8F0h (-10000d)
	0x46,0x10,0x27,    //    Physical Maximum 2710h (10000d)
	0x75,0x10,         //    Report Size 10h (16d)
	0x95,0x01,         //    Report Count 1
	0x91,0x02,         //    Output (Variable)
	0xC0     ,    //    End Collection

	
	0x05,0x0F,   //    Usage Page Physical Interface
	0x09,0x77,   //    Usage Effect Operation Report
	0xA1,0x02,   //    Collection Datalink
	0x85,0x0A,    //    Report ID Ah (10d)
	0x09,0x22,    //    Usage Effect Block Index
	0x15,0x01,    //    Logical Minimum 1
	0x25,0x28,    //    Logical Maximum 28h (40d)
	0x35,0x01,    //    Physical Minimum 1
	0x45,0x28,    //    Physical Maximum 28h (40d)
	0x75,0x08,    //    Report Size 8
	0x95,0x01,    //    Report Count 1
	0x91,0x02,    //    Output (Variable)
	0x09,0x78,    //    Usage Operation
	0xA1,0x02,    //    Collection Datalink
		0x09,0x79,    //    Usage Op Effect Start
		0x09,0x7A,    //    Usage Op Effect Start Solo
		0x09,0x7B,    //    Usage Op Effect Stop
		0x15,0x01,    //    Logical Minimum 1
		0x25,0x03,    //    Logical Maximum 3
		0x75,0x08,    //    Report Size 8
		0x95,0x01,    //    Report Count 1
		0x91,0x00,    //    Output
	0xC0     ,         //    End Collection
	0x09,0x7C,         //    Usage Loop Count
	0x15,0x00,         //    Logical Minimum 0
	0x26,0xFF,0x00,    //    Logical Maximum FFh (255d)
	0x35,0x00,         //    Physical Minimum 0
	0x46,0xFF,0x00,    //    Physical Maximum FFh (255d)
	0x91,0x02,         //    Output (Variable)
	0xC0     ,    //    End Collection
	0x09,0x90,    //    Usage PID State Report
	0xA1,0x02,    //    Collection Datalink
	0x85,0x0B,    //    Report ID Bh (11d)
	0x09,0x22,    //    Usage Effect Block Index
	0x25,0x28,    //    Logical Maximum 28h (40d)
	0x15,0x01,    //    Logical Minimum 1
	0x35,0x01,    //    Physical Minimum 1
	0x45,0x28,    //    Physical Maximum 28h (40d)
	0x75,0x08,    //    Report Size 8
	0x95,0x01,    //    Report Count 1
	0x91,0x02,    //    Output (Variable)
	0xC0     ,    //    End Collection
	0x09,0x96,    //    Usage DC Disable Actuators
	0xA1,0x02,    //    Collection Datalink
	0x85,0x0C,    //    Report ID Ch (12d)
	0x09,0x97,    //    Usage DC Stop All Effects
	0x09,0x98,    //    Usage DC Device Reset
	0x09,0x99,    //    Usage DC Device Pause
	0x09,0x9A,    //    Usage DC Device Continue
	0x09,0x9B,    //    Usage PID Device State
	0x09,0x9C,    //    Usage DS Actuators Enabled
	0x15,0x01,    //    Logical Minimum 1
	0x25,0x06,    //    Logical Maximum 6
	0x75,0x08,    //    Report Size 8
	0x95,0x01,    //    Report Count 1
	0x91,0x00,    //    Output
	0xC0     ,    //    End Collection
	

	
0xC0,	// END COLLECTION ()

	
	
};


static void PS2Controller_SetReport(uint8_t reportId, uint8_t * data, uint16_t length);
static void PS2Controller_GetReport(uint8_t reportId);

const USBD_HID_AppType ps2controllerApp = {
	.Name = "PS2 controller",
	.Report = {
		.Desc = hid_rpt_ps2,
		.Length = sizeof(hid_rpt_ps2),
		.IDs = 1,	
	},
	.SetReport = PS2Controller_SetReport,
	.GetReport = PS2Controller_GetReport,
};

USBD_HID_IfHandleType hps2controller_if = {
	.App = &ps2controllerApp,
	.Base.AltCount = 1,
	.Config.InEp.Size = sizeof(ps2_hid_packet),
	.Config.InEp.Interval = 1,
}, *const ps2controller_if = &hps2controller_if;







void USBSetDataEffect(uint8_t* hid_report_out, uint16_t length);
void USBSetEffect(uint8_t* hid_report_out, uint16_t length);


static void PS2Controller_SetReport(uint8_t reportId, uint8_t* data, uint16_t length) {
	usb_printf("PS2Controller_SetReport, reportId: %02X, len %d, payload: ", reportId, length);
	printf_payload(data, length); printf("\n");
	
  //I get the set report, then i get the DATA !
  // THE DEVICE NOW NEEDS TO ALLOCATE THE EFFECT
  // ONCE THE EFFECT IS ALLOCATED; THE HOST SENDS A GET REPORT, function above
  /*
  Offset Field Size Value Description
  0 bmRequestType 1 10100001b From device to host
  1 bRequest 1 0x01 Get_Report
  2 wValue 2 0x0203 Report ID (2) and Report Type (feature)
  4 wIndex 2 0x0000 Interface
  6 wLength 2 0x0500 Number of bytes to transfer in the data phase
  (5 bytes)
  */
  // DEVICE RESPONDS TO THE GET REPORT; WITH THE PID BLOCK LOAD REPORT
  
  //USBSetEffect();
  // I HAVE TO: 1: BE SURE I CHECK THE FEATURE, BECAUSE: REASONS
  
	switch (reportId) {
	case 0x01: // report ID: 1, SET NEW EFFECT,APPLICATION DATA, etc , and now ?
	   // NOW I CHECK THE REPORT TYPE ! INPUT? OUTPUT? FEATURE ? 
/*
		if ((setupValue >> 8) == 0x02) {
			// FEATURE: activate effect
			USBSetDataEffect(data, length);
		}
		if ((setupValue >> 8) == 0x03) {
			// SetReport: save effect data
			USBSetEffect(data, length);
		} */
		break;
	}
}
	

static void PS2Controller_GetReport(uint8_t reportId) {
	usb_printf("PS2Controller_GetReport for reportId %02X\n", reportId);
	uint8_t response[5];
	switch (reportId) {
	case 0x01:// INPUT REPORT
	   // IGNORE FOR NOW....
		break;
	case 0x02: // OUTPUT, the host wants to send me the data
		response[0] = 0;
		response[1] = 0x1;
		response[2] = 0x1;
		response[3] = 10;
		response[4] = 10;
		USBD_HID_ReportIn(ps2controller_if, response, 5);
		break;
	case 0x03:// FEATURE, the host wants ME to send the data
		response[0] = 0;
		response[1] = 0x1;
		response[2] = 0x1;
		response[3] = 10;
		response[4] = 10;
		USBD_HID_ReportIn(ps2controller_if, response, 5);
		
		break;					
	}	
}







SET_GET_EFFECT_STRUCTURE set_get_effect_structure;
bool CONFIGURED_EFFECT_NUMBER[12] = { 0 };

/*******************************************************************
 * Overview: This function is called from USER_SET_REPORT_HANDLER
 * to handle the PID class specific request: SET REPORT REQUEST
 * SET REPORT REQUEST is issued by the host application when it needs
 * to create a new effect. The output report, specifies wich
 * effect type to create.
 * The firmware creates the effect ( sets CONFIGURED_EFFECT_NUMBER[EFFECT TYPE] to 1 )
 * and prepares the get_report for the host.
 * The set report request is saved in hid_report_out[n]
 *******************************************************************/
void USBSetEffect(uint8_t* hid_report_out, uint16_t length) {
	set_get_effect_structure.SET_REPORT_REQUEST.report_id = hid_report_out[0]; // 1: based on the pid report descriptor
	set_get_effect_structure.SET_REPORT_REQUEST.effect_type = hid_report_out[1];// the type of effect, based on pid report descriptor
	set_get_effect_structure.SET_REPORT_REQUEST.byte_count = hid_report_out[2]; // this device does not support custom effects

	switch (set_get_effect_structure.SET_REPORT_REQUEST.effect_type) { 
	case 1:// Usage ET Constant Force 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 1; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[0] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.

		break;
	case 2:// Usage ET Ramp 1
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 2; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[1] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 3:// Usage ET Square 2
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 3; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[2] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 4:// Usage ET Sine 3
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 4; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated.

		CONFIGURED_EFFECT_NUMBER[3] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 5:// Usage ET Triangle 4
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 5; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[4] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 6:// Usage ET Sawtooth Up 5
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 6; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[5] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 7:// Usage ET Sawtooth Down 6
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 7; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this, because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[6] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 8:// Usage ET Spring 7
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 8; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this  because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated.

		CONFIGURED_EFFECT_NUMBER[7] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 9:// Usage ET Damper 8
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 9; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this , because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[8] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 10:// Usage ET Inertia 9
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 10; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this , because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[9] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 11:// Usage ET Friction 10
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 11; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 1; // ok, i can load this , because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0xFFFF; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[10] = 1; // I HAVE CONFIGURED CONSTANT FORCE EFFECT FOR THIS DEVICE
		// now, when the host issues a get_report blah blah, i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	case 12:// Usage ET Custom Force Data 11 ! NOT SUPPORTED BY THIS DEVICE !
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.report_id = 2; // 2= PID BLOCK LOAD REPORT
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.effect_block_index = 0; //0=i can't create effect, 1 = CONSTANT FORCE, index in the array = effect_block_index-1 : 0
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.block_load_status = 3; // ok, i can load this , because i have already memory for it, and because : reasons
		set_get_effect_structure.PID_BLOCK_LOAD_REPORT.ram_pool_available = 0x0000; // i have no ideea why i need this, however i have no more memory except for the effects already preallocated. 

		CONFIGURED_EFFECT_NUMBER[11] = 0; // 
		// now, when the host issues a get_report , i send the set_get_effect_structure.PID_BLOCK_LOAD_REPORT.
		break;
	} 
}

void USBSetDataEffect(uint8_t* hid_report_out, uint16_t length) {
	usb_printf("USBSetDataEffect()\n");	
}