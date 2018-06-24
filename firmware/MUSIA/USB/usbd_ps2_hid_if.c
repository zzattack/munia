#include "usbd_customhid.h"

extern USBD_HandleTypeDef hUsbDeviceFS;

static int8_t MUSIA_HID_Init_FS(void);
static int8_t MUSIA_HID_DeInit_FS(void);
static int8_t MUSIA_HID_OutEvent_FS(uint8_t event_idx, uint8_t state);

extern uint hid_rpt_ps2[];

USBD_CUSTOM_HID_ItfTypeDef USBD_CustomHID_PS2_fops_FS = {
	hid_rpt_ps2,
	MUSIA_HID_Init_FS,
	MUSIA_HID_DeInit_FS,
	MUSIA_HID_OutEvent_FS
};

static int8_t MUSIA_HID_Init_FS() {
  return USBD_OK;
}

/**
  * @brief  DeInitializes the CUSTOM HID media low layer
  * @retval USBD_OK if all operations are OK else USBD_FAIL
  */
static int8_t MUSIA_HID_DeInit_FS() {
  return USBD_OK;
}

/**
  * @brief  Manage the CUSTOM HID class events
  * @param  event_idx: Event index
  * @param  state: Event state
  * @retval USBD_OK if all operations are OK else USBD_FAIL
  */
static int8_t MUSIA_HID_OutEvent_FS(uint8_t event_idx, uint8_t state) {
  /* USER CODE BEGIN 6 */
  return USBD_OK;
  /* USER CODE END 6 */
}
