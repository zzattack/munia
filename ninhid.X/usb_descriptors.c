#include "system_config.h"
#include <usb/usb.h>
#include <usb/usb_ch9.h>
#include <usb/usb_device_hid.h>
#include <stdio.h>

const USB_DEVICE_DESCRIPTOR device_dsc = {
    0x12,    // Size of this descriptor in bytes
    USB_DESCRIPTOR_DEVICE,                // DEVICE descriptor type
    0x0200,                 // USB Spec Release Number in BCD format
    0x00,                   // Class Code
    0x00,                   // Subclass code
    0x00,                   // Protocol code
    USB_EP0_BUFF_SIZE,          // Max packet size for EP0, see usb_config.h
    0x04D8,                 // Vendor ID (VID))
    0x005E,                 // Product ID (PID))
    0x0001,                 // Device release number in BCD format
    0x01,                   // Manufacturer string index
    0x02,                   // Product string index
    0x00,                   // Device serial number string index
    0x01                    // Number of possible configurations         
};

// Configuration 1 Descriptor
const uint8_t configDescriptor1[]={
    // Configuration Descriptor
    sizeof(USB_CONFIGURATION_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_CONFIGURATION, // CONFIGURATION descriptor type
    DESC_CONFIG_WORD(0x0049), // Total length of data for this cfg
    2, // Number of interfaces in this cfg
    1, // Index value of this configuration
    0, // Configuration string index
    _DEFAULT, //| _SELF, // Attributes, see usb_device.h
    250, // Max power consumption (2X mA)
							
    // Interface Descriptor1----------------------------------------------------
    sizeof(USB_INTERFACE_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE,    // INTERFACE descriptor type
    HID_INTF_ID1,           // Interface Number
    0,                      // Alternate Setting Number
    2,                      // Number of endpoints in this intf
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
    DESC_CONFIG_WORD(HID_RPT01_SIZE), // Size of the report descriptor
    
    // Endpoint Descriptor
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,     // Endpoint Descriptor
    HID_EP1 | _EP_IN,            // EndpointAddress
    _INTERRUPT,                  // Attributes
    DESC_CONFIG_WORD(64),        // size
    0x01,                        // Interval

    // Endpoint Descriptor
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,     // Endpoint Descriptor
    HID_EP1 | _EP_OUT,           // EndpointAddress
    _INTERRUPT,                  // Attributes
    DESC_CONFIG_WORD(64),        // size
    0x01,                        // Interval
    // -------------------------------------------------------------------------
    
    // Interface Descriptor2
    sizeof(USB_INTERFACE_DESCRIPTOR), // Size of this descriptor in bytes
    USB_DESCRIPTOR_INTERFACE, // INTERFACE descriptor type
    HID_INTF_ID2,           // Interface Number
    0,                      // Alternate Setting Number
    2,                      // Number of endpoints in this intf
    HID_INTF,               // Class code
    0,                      // Subclass code
    0,                      // Protocol code
    5,                      // Interface string index

    // HID Class-Specific Descriptor
    0x09,//sizeof(USB_HID_DSC)+3,    // Size of this descriptor in bytes
    DSC_HID,                   // HID descriptor type
    DESC_CONFIG_WORD(0x0111),  // HID Spec Release Number in BCD format (1.11)
    0x00,                      // Country Code (0x00 for Not supported)
    HID_NUM_OF_DSC,            // Number of class descriptors, see usbcfg.h
    DSC_RPT,                   // Report descriptor type
    DESC_CONFIG_WORD(HID_RPT02_SIZE), // Size of the report descriptor
    
    // Endpoint Descriptor
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,     //Endpoint Descriptor
    HID_EP2 | _EP_IN,            //EndpointAddress
    _INTERRUPT,                  //Attributes
    DESC_CONFIG_WORD(64),        //size
    0x01,                        //Interval

    // Endpoint Descriptor
    sizeof(USB_ENDPOINT_DESCRIPTOR), 
    USB_DESCRIPTOR_ENDPOINT,     //Endpoint Descriptor
    HID_EP2 | _EP_OUT,           //EndpointAddress
    _INTERRUPT,                  //Attributes
    DESC_CONFIG_WORD(64),        //size
    0x01,                        //Interval
};


//Class specific descriptor - HID
const struct{uint8_t report[HID_RPT01_SIZE];}hid_rpt01={ {
    0x05, 0x01, // USAGE_PAGE (Generic Desktop)
    0x09, 0x04, // USAGE (Joystick)
    0xA1, 0x01, // COLLECTION (Application)
    0x15, 0x00, // LOGICAL_MINIMUM (0)
    0x26, 0xff, 0x03, // LOGICAL_MAXIMUM (1023)
    0x09, 0x30, // USAGE (X)
    0x09, 0x31, // USAGE (Y)
    0x09, 0x32, // USAGE (Z)
    0x09, 0x33, // USAGE (Rx)
    0x75, 0x10, // Report Size (16)
    0x95, 0x04, // Report Count (4)
    0x81, 0x02, // Input (Data, Variable, Absolute)
    0x05, 0x09, // Usage Page (Buttons)
    0x19, 0x01, // Usage Minimum (1)
    0x29, 0x20, // Usage Maximum (32)
    0x15, 0x00, // Logical Minimum (0)
    0x25, 0x01, // Logical Maximum (1)
    0x75, 0x01, // Report Size (1)
    0x95, 0x20, // Report Count (32)
    0x81, 0x02, // Input (Data, Variable, Absolute)
    0xC0 // End Collection 1 byte 
// 42 
} };

const struct{uint8_t report[HID_RPT02_SIZE];}hid_rpt02={ {
    0x05, 0x01, // USAGE_PAGE (Generic Desktop)
    0x09, 0x04, // USAGE (Joystick)
    0xA1, 0x01, // COLLECTION (Application)
    0x15, 0x00, // LOGICAL_MINIMUM (0)
    0x26, 0xff, 0x03, // LOGICAL_MAXIMUM (1023)
    0x09, 0x30, // USAGE (X)
    0x09, 0x31, // USAGE (Y)
    0x09, 0x32, // USAGE (Z)
    0x09, 0x33, // USAGE (Rx)
    0x75, 0x10, // Report Size (16)
    0x95, 0x04, // Report Count (4)
    0x81, 0x02, // Input (Data, Variable, Absolute)
    0x05, 0x09, // Usage Page (Buttons)
    0x19, 0x01, // Usage Minimum (1)
    0x29, 0x20, // Usage Maximum (32)
    0x15, 0x00, // Logical Minimum (0)
    0x25, 0x01, // Logical Maximum (1)
    0x75, 0x01, // Report Size (1)
    0x95, 0x20, // Report Count (32)
    0x81, 0x02, // Input (Data, Variable, Absolute)
    0xC0 // End Collection 1 byte 
// 42 
} };  

//Language code string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[1];}sd000={
sizeof(sd000),USB_DESCRIPTOR_STRING,
{	0x0409
}};

//Manufacturer string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[12];}sd001={
sizeof(sd001),USB_DESCRIPTOR_STRING,
{'z','z','a','t','t','a','c','k','.','o','r','g'
}};

//Product string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[21];}sd002={
sizeof(sd002),USB_DESCRIPTOR_STRING,
{'N','i','n','t','e','n','d','o',' ','I','n','p','u','t',' ','d','e','v','i','c','e'
}};

//Product string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[21];}sd003={
sizeof(sd003),USB_DESCRIPTOR_STRING,
{'Y','o','u',' ','s','h','o','u','l','d',' ','n','o','t',' ','s','e','e',' ','m','e'
}};

//Interface1 string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[10];}sd004={
sizeof(sd004),USB_DESCRIPTOR_STRING,
{'J','o','y','s','t','i','c','k',' ','1'
}};

//Interface2 string descriptor
const struct{uint8_t bLength;uint8_t bDscType;uint16_t string[10];}sd005={
sizeof(sd005),USB_DESCRIPTOR_STRING,
{'J','o','y','s','t','i','c','k',' ','2'
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


#define USB_ARRAYLEN(X) (sizeof(X)/sizeof(*X))
#define STATIC_SIZE_CHECK_EQUAL(X,Y) typedef char USB_CONCAT(STATIC_SIZE_CHECK_LINE_,__LINE__) [(X==Y)?1:-1]
#define USB_CONCAT(X,Y)  USB_CONCAT_HIDDEN(X,Y)
#define USB_CONCAT_HIDDEN(X,Y) X ## Y

STATIC_SIZE_CHECK_EQUAL(sizeof(hid_rpt01), HID_RPT01_SIZE);
STATIC_SIZE_CHECK_EQUAL(sizeof(hid_rpt02), HID_RPT02_SIZE);


void usb_descriptors_check() {
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
}