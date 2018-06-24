#pragma once

#include  "usbd_ioreq.h"

#define MUSIA_EPIN_SIZE                 0x20
#define MUSIA_EPOUT_SIZE                0x20

#define CUSTOM_HID_DESCRIPTOR_TYPE           0x21
#define CUSTOM_HID_REPORT_DESC               0x22

#define CUSTOM_HID_REQ_SET_PROTOCOL          0x0B
#define CUSTOM_HID_REQ_GET_PROTOCOL          0x03

#define CUSTOM_HID_REQ_SET_IDLE              0x0A
#define CUSTOM_HID_REQ_GET_IDLE              0x02

#define CUSTOM_HID_REQ_SET_REPORT            0x09
#define CUSTOM_HID_REQ_GET_REPORT            0x01
	 
#define HID_INTF_MUSIA          0
#define HID_INTF_CFG            1

#define HID_EP_MUSIA			1
#define HID_EP_CFG              2

#define MUSIA_HID_EPIN_ADDR HID_EP_MUSIA | 0x80
#define CFG_HID_EPIN_ADDR HID_EP_CFG | 0x80
#define CFG_HID_EPOUT_ADDR HID_EP_CFG | 0x00

#define HID_INT_IN_MUSIA_SIZE   32 // Valid Options: 8, 16, 32, or 64 bytes.
#define HID_INT_CFG_SIZE        9 // one extra for report id


typedef enum {
	CUSTOM_HID_IDLE = 0,
	CUSTOM_HID_BUSY,
}
CUSTOM_HID_StateTypeDef; 

typedef struct _USBD_CUSTOM_HID_Itf {
	uint8_t* pReport;
	int8_t(* Init)(void);
	int8_t(* DeInit)(void);
	int8_t(* OutEvent)(uint8_t, uint8_t);
} USBD_CUSTOM_HID_ItfTypeDef;

typedef struct {
	uint8_t              Report_buf[32];
	uint32_t             Protocol;   
	uint32_t             IdleState;  
	uint32_t             AltSetting;
	uint32_t             IsReportAvailable;  
	CUSTOM_HID_StateTypeDef     state;  
} USBD_CUSTOM_HID_HandleTypeDef; 

extern USBD_ClassTypeDef  USBD_CUSTOM_HID;
#define USBD_CUSTOM_HID_CLASS    &USBD_CUSTOM_HID
uint8_t USBD_CUSTOM_HID_SendReport(USBD_HandleTypeDef *pdev, uint8_t *report, uint16_t len);
uint8_t  USBD_CUSTOM_HID_RegisterInterface(USBD_HandleTypeDef   *pdev, USBD_CUSTOM_HID_ItfTypeDef *fops);
