#include "usbd_customhid.h"
#include "usbd_desc.h"
#include "usbd_ctlreq.h"
#include  "report_descriptors.h"



static uint8_t  USBD_CUSTOM_HID_Init(USBD_HandleTypeDef *pdev, uint8_t cfgidx);
static uint8_t  USBD_CUSTOM_HID_DeInit(USBD_HandleTypeDef *pdev, uint8_t cfgidx);
static uint8_t  USBD_CUSTOM_HID_Setup(USBD_HandleTypeDef *pdev, USBD_SetupReqTypedef *req);

static uint8_t  *USBD_CUSTOM_HID_GetCfgDesc(uint16_t *length);
static uint8_t  *USBD_CUSTOM_HID_GetDeviceQualifierDesc(uint16_t *length);

static uint8_t  USBD_CUSTOM_HID_DataIn(USBD_HandleTypeDef *pdev, uint8_t epnum);

static uint8_t  USBD_CUSTOM_HID_DataOut(USBD_HandleTypeDef *pdev, uint8_t epnum);
static uint8_t  USBD_CUSTOM_HID_EP0_RxReady(USBD_HandleTypeDef  *pdev);


USBD_ClassTypeDef  USBD_CUSTOM_HID = {
	USBD_CUSTOM_HID_Init,
	USBD_CUSTOM_HID_DeInit,
	USBD_CUSTOM_HID_Setup,
	NULL,
	/*EP0_TxSent*/  
	USBD_CUSTOM_HID_EP0_RxReady,
	/*EP0_RxReady*/ /* STATUS STAGE IN */
	USBD_CUSTOM_HID_DataIn,
	/*DataIn*/
	USBD_CUSTOM_HID_DataOut,
	NULL,
	/*SOF */
	NULL,
	NULL,      
	USBD_CUSTOM_HID_GetCfgDesc,
	USBD_CUSTOM_HID_GetCfgDesc, 
	USBD_CUSTOM_HID_GetCfgDesc,
	USBD_CUSTOM_HID_GetDeviceQualifierDesc,
};

/* USB CUSTOM_HID device Configuration Descriptor */
__ALIGN_BEGIN static uint8_t USBD_CUSTOM_HID_CfgDesc[] __ALIGN_END = {
	/* 0 */	
	0x09, // bLength: Configuration Descriptor size
	USB_DESC_TYPE_CONFIGURATION, // bDescriptorType: Configuration
	34,
	0x00, // TODO: correct! wTotalLength: Bytes returned
	0x01, // bNumInterfaces: 1 interface
	0x01, // bConfigurationValue: Configuration value
	0x00, // iConfiguration: Index of string descriptor describing the configuration
	0xC0, // bmAttributes: bus powered
	250, // MaxPower 500 mA: this current is used for detecting Vbus
  	
	
		/************** Descriptor of 1st HID interface: MUSIA ****************/
		/* 09 */
		0x09, // bLength: Interface Descriptor size
		USB_DESC_TYPE_INTERFACE, // bDescriptorType: Interface descriptor type
		0x00, // bInterfaceNumber: Number of Interface
		0x00, // bAlternateSetting: Alternate setting
		0x01, // bNumEndpoints
		0x03, // bInterfaceClass: CUSTOM_HID
		0x00, // bInterfaceSubClass : 1=BOOT, 0=no boot
		0x00, // InterfaceProtocol : 0=none, 1=keyboard, 2=mouse
		0x04, // iInterface: Index of string descriptor
		// -------------------------------------------------------------------------

	
			/******************** Descriptor of MUSIA class *************************/
			/* 18 */
			0x09, // bLength: CUSTOM_HID Descriptor size
			CUSTOM_HID_DESCRIPTOR_TYPE, // bDescriptorType: CUSTOM_HID
			0x11, // CUSTOM_HIDUSTOM_HID: CUSTOM_HID Class Spec release number
			0x01,
			0x00, // bCountryCode: Hardware target country
			0x01, // bNumDescriptors: Number of CUSTOM_HID class descriptors to follow
			0x22, // bDescriptorType
			HID_RPT_PS2_SIZE, // wItemLength: Total length of Report descriptor
			0x00,
			// -------------------------------------------------------------------------
	
	
			/******************** Descriptor of only input endpoint ********************/
			/* 27 */
			0x07, // bLength: Endpoint Descriptor size
			USB_DESC_TYPE_ENDPOINT, // bDescriptorType:
			HID_EP_MUSIA | 0x80, // bEndpointAddress: Endpoint Address (IN)
			0x03, // bmAttributes: Interrupt endpoint
			HID_INT_IN_MUSIA_SIZE, // wMaxPacketSize
			0x00,
			0x01, //  bInterval: Polling Interval (1 ms)
			// -------------------------------------------------------------------------	
};

/* USB CUSTOM_HID device Configuration Descriptor */
__ALIGN_BEGIN static uint8_t USBD_CUSTOM_HID_Desc[] __ALIGN_END = {
	/* 18 */
	0x09, // bLength: CUSTOM_HID Descriptor size
	CUSTOM_HID_DESCRIPTOR_TYPE, // bDescriptorType: CUSTOM_HID
	0x11, // bCUSTOM_HIDUSTOM_HID: CUSTOM_HID Class Spec release number
	0x01,
	0x00, // bCountryCode: Hardware target country
	0x01, // bNumDescriptors: Number of CUSTOM_HID class descriptors to follow
	0x22, // bDescriptorType
	HID_RPT_PS2_SIZE, // wItemLength: Total length of Report descriptor
	0x00,
};

/* USB Standard Device Descriptor */
__ALIGN_BEGIN static uint8_t USBD_CUSTOM_HID_DeviceQualifierDesc[USB_LEN_DEV_QUALIFIER_DESC] __ALIGN_END = {
	USB_LEN_DEV_QUALIFIER_DESC,
	USB_DESC_TYPE_DEVICE_QUALIFIER,
	0x00,
	0x02,
	0x00,
	0x00,
	0x00,
	0x40,
	0x01,
	0x00,
};


/**
  * @brief  USBD_CUSTOM_HID_Init
  *         Initialize the CUSTOM_HID interface
  * @param  pdev: device instance
  * @param  cfgidx: Configuration index
  * @retval status
  */
static uint8_t  USBD_CUSTOM_HID_Init(USBD_HandleTypeDef *pdev, uint8_t cfgidx) {
	uint8_t ret = 0;
	USBD_CUSTOM_HID_HandleTypeDef *hhid;
	/* Open MUSIA EP IN */
	USBD_LL_OpenEP(pdev, MUSIA_HID_EPIN_ADDR, USBD_EP_TYPE_INTR, HID_INT_IN_MUSIA_SIZE);  
  
	/* Open Config EP OUT */ // todo
	// TODO: USBD_LL_OpenEP(pdev, CFG_HID_EPIN_ADDR, USBD_EP_TYPE_INTR, HID_INT_CFG_SIZE);
	
	/* Open Config EP OUT */
	// TODO: USBD_LL_OpenEP(pdev, CFG_HID_EPOUT_ADDR | 0x80, USBD_EP_TYPE_INTR, HID_INT_CFG_SIZE);
  
	pdev->pClassData = USBD_malloc(sizeof(USBD_CUSTOM_HID_HandleTypeDef));
  
	if (pdev->pClassData == NULL) {
		ret = 1; 
	}
	else {
		hhid = (USBD_CUSTOM_HID_HandleTypeDef*) pdev->pClassData;      
		hhid->state = CUSTOM_HID_IDLE;
		((USBD_CUSTOM_HID_ItfTypeDef *)pdev->pUserData)->Init();
		/* Prepare Out endpoint to receive 1st packet */ 
		// TODO: USBD_LL_PrepareReceive(pdev, CFG_HID_EPOUT_ADDR, hhid->Report_buf, USBD_CUSTOMHID_OUTREPORT_BUF_SIZE);
	}
    
	return ret;
}

/**
  * @brief  USBD_CUSTOM_HID_Init
  *         DeInitialize the CUSTOM_HID layer
  * @param  pdev: device instance
  * @param  cfgidx: Configuration index
  * @retval status
  */
static uint8_t  USBD_CUSTOM_HID_DeInit(USBD_HandleTypeDef *pdev, uint8_t cfgidx) {
	/* Close CUSTOM_HID EP IN */
	USBD_LL_CloseEP(pdev, MUSIA_HID_EPIN_ADDR);
	// TODO: USBD_LL_CloseEP(pdev, CFG_HID_EPIN_ADDR);
	// TODO: USBD_LL_CloseEP(pdev, CFG_HID_EPOUT_ADDR); 

	/* FRee allocated memory */
	if(pdev->pClassData != NULL) {
		((USBD_CUSTOM_HID_ItfTypeDef *)pdev->pUserData)->DeInit();
		USBD_free(pdev->pClassData);
		pdev->pClassData = NULL;
	}
	return USBD_OK;
}

/**
  * @brief  USBD_CUSTOM_HID_Setup
  *         Handle the CUSTOM_HID specific requests
  * @param  pdev: instance
  * @param  req: usb requests
  * @retval status
  */
static uint8_t  USBD_CUSTOM_HID_Setup(USBD_HandleTypeDef* pdev, USBD_SetupReqTypedef* req) {
	uint16_t len = 0;
	uint8_t* pbuf = NULL;
	USBD_CUSTOM_HID_HandleTypeDef* hhid = (USBD_CUSTOM_HID_HandleTypeDef*)pdev->pClassData;

	switch (req->bmRequest & USB_REQ_TYPE_MASK) {
	case USB_REQ_TYPE_CLASS :  
		switch (req->bRequest) {      
      
		case CUSTOM_HID_REQ_SET_PROTOCOL:
			hhid->Protocol = (uint8_t)(req->wValue);
			break;
      
		case CUSTOM_HID_REQ_GET_PROTOCOL:
			USBD_CtlSendData(pdev, (uint8_t *)&hhid->Protocol, 1);    
			break;
      
		case CUSTOM_HID_REQ_SET_IDLE:
			hhid->IdleState = (uint8_t)(req->wValue >> 8);
			break;
      
		case CUSTOM_HID_REQ_GET_IDLE:
			USBD_CtlSendData(pdev, (uint8_t *)&hhid->IdleState, 1);        
			break;      
    
		case CUSTOM_HID_REQ_SET_REPORT:
			hhid->IsReportAvailable = 1;
			USBD_CtlPrepareRx(pdev, hhid->Report_buf, (uint8_t)(req->wLength));      
			break;
			
		default:
			USBD_CtlError(pdev, req);
			return USBD_FAIL; 
		}
		break;
    
	case USB_REQ_TYPE_STANDARD:
		switch (req->bRequest) {
		case USB_REQ_GET_DESCRIPTOR: 
			if (req->wValue >> 8 == CUSTOM_HID_REPORT_DESC) {
				len = MIN(HID_RPT_PS2_SIZE, req->wLength);
				pbuf =  ((USBD_CUSTOM_HID_ItfTypeDef *)pdev->pUserData)->pReport;
			}
			else if (req->wValue >> 8 == CUSTOM_HID_DESCRIPTOR_TYPE) {
				pbuf = USBD_CUSTOM_HID_Desc;   
				len = MIN(sizeof(USBD_CUSTOM_HID_CfgDesc), req->wLength) ;
			}      
			USBD_CtlSendData(pdev, pbuf, len);      
			break;
      
		case USB_REQ_GET_INTERFACE :
			USBD_CtlSendData(pdev, (uint8_t *)&hhid->AltSetting, 1);
			break;
      
		case USB_REQ_SET_INTERFACE :
			hhid->AltSetting = (uint8_t)(req->wValue);
			break;
		}
	}
	return USBD_OK;
}

/**
  * @brief  USBD_CUSTOM_HID_SendReport 
  *         Send CUSTOM_HID Report
  * @param  pdev: device instance
  * @param  buff: pointer to report
  * @retval status
  */
uint8_t USBD_CUSTOM_HID_SendReport(USBD_HandleTypeDef  *pdev, uint8_t *report, uint16_t len) {
	USBD_CUSTOM_HID_HandleTypeDef *hhid = (USBD_CUSTOM_HID_HandleTypeDef*)pdev->pClassData;
  
	if (pdev->dev_state == USBD_STATE_CONFIGURED) {
		if (hhid->state == CUSTOM_HID_IDLE) {
			hhid->state = CUSTOM_HID_BUSY;
			USBD_LL_Transmit(pdev, MUSIA_HID_EPIN_ADDR, report, len);
		}
	}
	return USBD_OK;
}

/**
  * @brief  USBD_CUSTOM_HID_GetCfgDesc 
  *         return configuration descriptor
  * @param  speed : current device speed
  * @param  length : pointer data length
  * @retval pointer to descriptor buffer
  */
static uint8_t  *USBD_CUSTOM_HID_GetCfgDesc(uint16_t *length) {
	*length = sizeof(USBD_CUSTOM_HID_CfgDesc);
	return USBD_CUSTOM_HID_CfgDesc;
}

/**
  * @brief  USBD_CUSTOM_HID_DataIn
  *         handle data IN Stage
  * @param  pdev: device instance
  * @param  epnum: endpoint index
  * @retval status
  */
static uint8_t  USBD_CUSTOM_HID_DataIn(USBD_HandleTypeDef *pdev, uint8_t epnum) {
  
	/* Ensure that the FIFO is empty before a new transfer, this condition could 
	be caused by  a new transfer before the end of the previous transfer */
	((USBD_CUSTOM_HID_HandleTypeDef *)pdev->pClassData)->state = CUSTOM_HID_IDLE;

	return USBD_OK;
}

/**
  * @brief  USBD_CUSTOM_HID_DataOut
  *         handle data OUT Stage
  * @param  pdev: device instance
  * @param  epnum: endpoint index
  * @retval status
  */
static uint8_t  USBD_CUSTOM_HID_DataOut(USBD_HandleTypeDef *pdev, uint8_t epnum) {  
	USBD_CUSTOM_HID_HandleTypeDef     *hhid = (USBD_CUSTOM_HID_HandleTypeDef*)pdev->pClassData;    
	((USBD_CUSTOM_HID_ItfTypeDef *)pdev->pUserData)->OutEvent(hhid->Report_buf[0], hhid->Report_buf[1]);    
	// todo check
	USBD_LL_PrepareReceive(pdev, CFG_HID_EPOUT_ADDR, hhid->Report_buf, HID_RPT_PS2_SIZE);
	return USBD_OK;
}

/**
  * @brief  USBD_CUSTOM_HID_EP0_RxReady
  *         Handles control request data.
  * @param  pdev: device instance
  * @retval status
  */
uint8_t USBD_CUSTOM_HID_EP0_RxReady(USBD_HandleTypeDef *pdev) {
	USBD_CUSTOM_HID_HandleTypeDef     *hhid = (USBD_CUSTOM_HID_HandleTypeDef*)pdev->pClassData;  

	if (hhid->IsReportAvailable == 1) {
		((USBD_CUSTOM_HID_ItfTypeDef *)pdev->pUserData)->OutEvent(hhid->Report_buf[0], hhid->Report_buf[1]);
		hhid->IsReportAvailable = 0;      
	}

	return USBD_OK;
}

/**
* @brief  DeviceQualifierDescriptor 
*         return Device Qualifier descriptor
* @param  length : pointer data length
* @retval pointer to descriptor buffer
*/
static uint8_t  *USBD_CUSTOM_HID_GetDeviceQualifierDesc(uint16_t *length) {
	*length = sizeof(USBD_CUSTOM_HID_DeviceQualifierDesc);
	return USBD_CUSTOM_HID_DeviceQualifierDesc;
}

/**
* @brief  USBD_CUSTOM_HID_RegisterInterface
  * @param  pdev: device instance
  * @param  fops: CUSTOMHID Interface callback
  * @retval status
  */
uint8_t USBD_CUSTOM_HID_RegisterInterface(USBD_HandleTypeDef *pdev, USBD_CUSTOM_HID_ItfTypeDef *fops) {
	uint8_t  ret = USBD_FAIL;  
	if (fops != NULL) {
		pdev->pUserData = fops;
		ret = USBD_OK;    
	}  
	return ret;
}