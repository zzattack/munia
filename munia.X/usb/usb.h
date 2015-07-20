/*******************************************************************************
 Module for Microchip USB Library

  Company:
    Microchip Technology Inc.

  File Name:
    usb.h

  Summary:
    This header file exposes the core library APIs and definitions for the USB
    library.

  Description:
    This header file exposes the core library APIs and definitions for the USB
    library.  The user is responsible for also including the header file for
    the specific driver they will be using.
*******************************************************************************/

// DOM-IGNORE-BEGIN
/*******************************************************************************
Copyright (c) 2014 released Microchip Technology Inc.  All rights reserved.

Microchip licenses to you the right to use, modify, copy and distribute
Software only when embedded on a Microchip microcontroller or digital signal
controller that is integrated into your product or third party product
(pursuant to the sublicense terms in the accompanying license agreement).

You should refer to the license agreement accompanying this Software for
additional information regarding your rights and obligations.

SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF
MERCHANTABILITY, TITLE, NON-INFRINGEMENT AND FITNESS FOR A PARTICULAR PURPOSE.
IN NO EVENT SHALL MICROCHIP OR ITS LICENSORS BE LIABLE OR OBLIGATED UNDER
CONTRACT, NEGLIGENCE, STRICT LIABILITY, CONTRIBUTION, BREACH OF WARRANTY, OR
OTHER LEGAL EQUITABLE THEORY ANY DIRECT OR INDIRECT DAMAGES OR EXPENSES
INCLUDING BUT NOT LIMITED TO ANY INCIDENTAL, SPECIAL, INDIRECT, PUNITIVE OR
CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, COST OF PROCUREMENT OF
SUBSTITUTE GOODS, TECHNOLOGY, SERVICES, OR ANY CLAIMS BY THIRD PARTIES
(INCLUDING BUT NOT LIMITED TO ANY DEFENSE THEREOF), OR OTHER SIMILAR COSTS.
*******************************************************************************/
//DOM-IGNORE-END

#ifndef _USB_H_
//DOM-IGNORE-BEGIN
#define _USB_H_
//DOM-IGNORE-END

// *****************************************************************************
// *****************************************************************************
// Section: All necessary USB Library headers
// *****************************************************************************
// *****************************************************************************

#include "system.h"
#include "system_config.h"          // Must be defined by the application

#include "usb_common.h"         // Common USB library definitions
#include "usb_ch9.h"            // USB device framework definitions

#if defined( USB_SUPPORT_DEVICE )
    #include "usb_device.h"     // USB Device abstraction layer interface
#endif

#if defined( USB_SUPPORT_HOST )
    #include "usb_host.h"       // USB Host abstraction layer interface
#endif

#include "usb_hal.h"            // Hardware Abstraction Layer interface

/* USB Library version number.  This can be used to verify in an application 
   specific version of the library is being used.
 */
#define USB_MAJOR_VER   2        // Firmware version, major release number.
#define USB_MINOR_VER   11       // Firmware version, minor release number.
#define USB_DOT_VER     0        // Firmware version, dot release number.


#endif // _USB_H_



