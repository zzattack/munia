#include "usbd_customhid.h"

extern USBD_HandleTypeDef hUsbDeviceFS;

static int8_t CFG_HID_Init_FS(void);
static int8_t CFG_HID_DeInit_FS(void);
static int8_t CFG_HID_OutEvent_FS(uint8_t event_idx, uint8_t state);

extern uint8_t hid_rpt_cfg[];

USBD_CUSTOM_HID_ItfTypeDef USBD_CustomHID_CFG_fops_FS = {
	hid_rpt_cfg,
	CFG_HID_Init_FS,
	CFG_HID_DeInit_FS,
	CFG_HID_OutEvent_FS
};

static int8_t CFG_HID_Init_FS() {
	return USBD_OK;
}

/**
  * @brief  DeInitializes the CUSTOM HID media low layer
  * @retval USBD_OK if all operations are OK else USBD_FAIL
  */
static int8_t CFG_HID_DeInit_FS() {
	return USBD_OK;
}

/**
  * @brief  Manage the CUSTOM HID class events
  * @param  event_idx: Event index
  * @param  state: Event state
  * @retval USBD_OK if all operations are OK else USBD_FAIL
  */
static int8_t CFG_HID_OutEvent_FS(uint8_t event_idx, uint8_t state) {
  /* USER CODE BEGIN 6 */
	return USBD_OK;
	/* USER CODE END 6 */
}
