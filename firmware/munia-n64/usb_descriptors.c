#include "system_config.h"
#include "report_descriptors.h"
#include <usb/usb.h>
#include <usb/usb_ch9.h>
#include <usb/usb_device_hid.h>
#include <stdio.h>

const USB_DEVICE_DESCRIPTOR device_dsc = {
    sizeof(USB_DEVICE_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_DEVICE,  // DEVICE descriptor type
    0x0200,                 // USB Spec Release Number in BCD format
    0x00,                   // Class Code
    0x00,                   // Subclass code
    0x00,                   // Protocol code
    USB_EP0_BUFF_SIZE,      // Max packet size for EP0, see usb_config.h
    0x1209,                 // Vendor ID (VID))
    0x8841,                 // Product ID (PID))
    0x0001,                 // Device release number in BCD format
    0x01,                   // Manufacturer string index
    0x02,                   // Product string index
    0x03,                   // Device serial number string index
    0x01                    // Number of possible configurations         
};

// Configuration 1 Descriptor
const uint8_t configDescriptor1[]={
    // Configuration Descriptor
    sizeof(USB_CONFIGURATION_DESCRIPTOR), // 9 Size of this descriptor in bytes
    USB_DESCRIPTOR_CONFIGURATION, // CONFIGURATION descriptor type
    DESC_CONFIG_WORD(0x0042), // Total length of data for this cfg
    USB_MAX_NUM_INT, // Number of interfaces in this cfg
    1, // Index value of this configuration
    0, // Configuration string index
    _DEFAULT, //| _SELF, // Attributes, see usb_device.h
    250, // Max power consumption (2X mA)
    
    
    // -------- Interface Descriptor 1: N64 ------------------------------------
    sizeof(USB_INTERFACE_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE,    // INTERFACE descriptor type
    HID_INTF_N64,           // Interface Number
    0,                      // Alternate Setting Number
    1,                      // Number of endpoints in this intf
    HID_INTF,               // Class code
    0,                      // Subclass code
    0,                      // Protocol code
    4,                      // Interface string index

    // HID Class-Specific Descriptor
    0x09,//sizeof(USB_HID_DSC)+3,    // Size of this descriptor in bytes
    DSC_HID,                   // HID descriptor type
    DESC_CONFIG_WORD(0x0111),  // HID Spec Release Number in BCD format (1.11)
    0x00,                      // Country Code (0x00 for Not supported)
    HID_NUM_OF_DSC,            // Number of class descriptors, see usbcfg.h
    DSC_RPT,                   // Report descriptor type
    DESC_CONFIG_WORD(HID_RPT_N64_SIZE), // Size of the report descriptor
    
    // Endpoint Descriptor
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,                // Endpoint Descriptor
    HID_EP_N64 | _EP_IN,                    // EndpointAddress
    _INTERRUPT,                             // Attributes
    DESC_CONFIG_WORD(HID_INT_IN_N64_SIZE),  // size
    0x01,                                   // Interval
    // -------------------------------------------------------------------------
    
        
    // -------- Interface Descriptor 2: Config exchange-------------------------
    sizeof(USB_INTERFACE_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE,    // INTERFACE descriptor type
    HID_INTF_CFG,                // Interface Number
    0,                           // Alternate Setting Number
    2,                           // Number of endpoints in this intf
    HID_INTF,                    // Class code
    0,                           // Subclass code
    0,                           // Protocol code
    5,                           // Interface string index

    /* HID Class-Specific Descriptor */
    0x09,//sizeof(USB_HID_DSC)+3,    // Size of this descriptor in bytes
    DSC_HID,                    // HID descriptor type
    0x11,0x01,                  // HID Spec Release Number in BCD format (1.11)
    0x00,                       // Country Code (0x00 for Not supported)
    HID_NUM_OF_DSC,             // Number of class descriptors, see usbcfg.h
    DSC_RPT,                    // Report descriptor type
    DESC_CONFIG_WORD(HID_RPT_CFG_SIZE), // Size of the report descriptor

    /* Endpoint Descriptor */
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,    // Endpoint Descriptor
    HID_EP_CFG | _EP_IN,        // EndpointAddress
    _INTERRUPT,                 // Attributes
    DESC_CONFIG_WORD(HID_INT_CFG_SIZE),  // size
    0x20,                       // Interval

    /* Endpoint Descriptor */
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,    // Endpoint Descriptor
    HID_EP_CFG | _EP_OUT,       // EndpointAddress
    _INTERRUPT,                 // Attributes
    DESC_CONFIG_WORD(HID_INT_CFG_SIZE),  // size
    0x20                        // Interval
    
};

//Language code string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[1];}sd000={
sizeof(sd000),USB_DESCRIPTOR_STRING,
{	0x0409
}};

//Manufacturer string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[8];}sd001={
sizeof(sd001),USB_DESCRIPTOR_STRING,
{'m','u','n','i','a','.','i','o'
}};

//Product string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[9];}sd002={
sizeof(sd002),USB_DESCRIPTOR_STRING,
{'M','U','N','I','A','-','N','6','4'
}};

//Product string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[1];}sd003={
sizeof(sd003),USB_DESCRIPTOR_STRING,
{'n',
}};

//Interface 1 string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[10];}sd004={
sizeof(sd004),USB_DESCRIPTOR_STRING,
{'N','i','n','H','I','D',' ','N','6','4'
}};

//Interface 2 string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[10];}sd005={
sizeof(sd005),USB_DESCRIPTOR_STRING,
{'N','i','n','H','I','D',' ','C','F','G'
}};

//Array of configuration descriptors
const uint8_t *const USB_CD_Ptr[]=
{(const uint8_t *const)&configDescriptor1
};

//Array of string descriptors
const uint8_t *const USB_SD_Ptr[]=
{
    (const uint8_t *const)&sd000,
    (const uint8_t *const)&sd001,
    (const uint8_t *const)&sd002,
    (const uint8_t *const)&sd003,
    (const uint8_t *const)&sd004,
    (const uint8_t *const)&sd005,
};


void usb_descriptors_check() {
#ifdef SIMUL
    int dds = sizeof(device_dsc);
    while (device_dsc.bLength != dds) {
        printf("device_dsc size should be %i", dds);
        __delay_ms(10);
    }
    int cds = sizeof(configDescriptor1);
    USB_CONFIGURATION_DESCRIPTOR* p = (USB_CONFIGURATION_DESCRIPTOR*)configDescriptor1;
    while (cds != p->wTotalLength) {
        printf("configDescriptor1 size should be %i", cds);
        __delay_ms(10);
    }
#endif
}