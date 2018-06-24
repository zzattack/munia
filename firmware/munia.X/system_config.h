#ifndef _SYSTEM_CONFIG_H_
#define _SYSTEM_CONFIG_H_

#include <xc.h>
#include <stdbool.h>
#include "hardware.h"
#include "usb/usb_ch9.h"

/** DEFINITIONS ****************************************************/

#define USB_EP0_BUFF_SIZE  16   // Valid Options: 8, 16, 32, or 64 bytes.
#define USB_MAX_NUM_INT     4   // Set this number to match the maximum interface number used in the descriptors for this firmware project
#define USB_MAX_EP_NUMBER	4   // Set this number to match the maximum endpoint number used in the descriptors for this firmware project
#define HID_NUM_OF_DSC 1
#define USB_NUM_STRING_DESCRIPTORS 8

#define USB_USER_DEVICE_DESCRIPTOR &device_dsc
#define USB_USER_DEVICE_DESCRIPTOR_INCLUDE extern const USB_DEVICE_DESCRIPTOR device_dsc

//Configuration descriptors - if these two definitions do not exist then
//  a const BYTE *const variable named exactly USB_CD_Ptr[] must exist.
#define USB_USER_CONFIG_DESCRIPTOR USB_CD_Ptr
#define USB_USER_CONFIG_DESCRIPTOR_INCLUDE extern const uint8_t *const USB_CD_Ptr[]

#define USB_PING_PONG_MODE USB_PING_PONG__FULL_PING_PONG
#define USB_POLLING
//#define USB_INTERRUPT

#define USB_SPEED_OPTION USB_FULL_SPEED
#define USB_PULLUP_OPTION USB_PULLUP_ENABLE
#define USB_TRANSCEIVER_OPTION USB_INTERNAL_TRANSCEIVER

#define self_power 0

#define USB_ENABLE_STATUS_STAGE_TIMEOUTS    //Comment this out to disable this feature.
#define USB_STATUS_STAGE_TIMEOUT     (uint8_t)45   //Approximate timeout in milliseconds, except when
                                                //USB_POLLING mode is used, and USBDeviceTasks() is called at < 1kHz
                                                //In this special case, the timeout becomes approximately:
//Timeout(in milliseconds) = ((1000 * (USB_STATUS_STAGE_TIMEOUT - 1)) / (USBDeviceTasks() polling frequency in Hz))
//------------------------------------------------------------------------------------------------------------------
#define USB_SUPPORT_DEVICE
#define USB_ENABLE_ALL_HANDLERS

#define USB_USE_HID

/** ENDPOINTS ALLOCATION *******************************************/


/* HID */
#define HID_INTF_SNES           0
#define HID_INTF_N64            1
#define HID_INTF_NGC            2
#define HID_INTF_CFG            3

#define HID_EP_SNES				1
#define HID_EP_N64				2
#define HID_EP_NGC				3
#define HID_EP_CFG              4

#define HID_INT_IN_SNES_SIZE    16
#define HID_INT_IN_N64_SIZE     16
#define HID_INT_IN_NGC_SIZE     16
#define HID_INT_CFG_SIZE        9 // one extra for report id

void usb_descriptors_check();

#endif // _SYSTEM_CONFIG_H