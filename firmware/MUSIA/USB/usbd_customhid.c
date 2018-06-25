#include "report_descriptors.h"

/*
uint8_t USBD_CUSTOM_HID_CfgDesc[] = {
	0x09, // bLength: Configuration Descriptor size
	USB_DESC_TYPE_CONFIGURATION, // bDescriptorType: Configuration
	34,
	0x00, // TODO: correct! wTotalLength: Bytes returned
	0x01, // bNumInterfaces: 1 interface
	0x01, // bConfigurationValue: Configuration value
	0x00, // iConfiguration: Index of string descriptor describing the configuration
	0xC0, // bmAttributes: bus powered
	250, // MaxPower 500 mA: this current is used for detecting Vbus
  	
	
		// ************* Descriptor of 1st HID interface: MUSIA ***************
		// count so far: 9
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

	
			// ******************* Descriptor of MUSIA class *************************
			// count so far: 18
			0x09, // bLength: CUSTOM_HID Descriptor size
			CUSTOM_HID_DESCRIPTOR_TYPE, // bDescriptorType: CUSTOM_HID
			0x11, // CUSTOM_HIDUSTOM_HID: CUSTOM_HID Class Spec release number
			0x01,
			0x00, // bCountryCode: Hardware target country
			0x01, // bNumDescriptors: Number of CUSTOM_HID class descriptors to follow
			0x22, // bDescriptorType
			HID_RPT_PS2_SIZE, // wItemLength: Total length of Report descriptor // TODO: this varies!!
			0x00,
			// -------------------------------------------------------------------------
	
	
			// ******************* Descriptor of only input endpoint ********************
			// count so far: 27
			0x07, // bLength: Endpoint Descriptor size
			USB_DESC_TYPE_ENDPOINT, // bDescriptorType:
			PS2_HID_EPIN_ADDR, // bEndpointAddress: Endpoint Address (IN)
			0x03, // bmAttributes: Interrupt endpoint
			HID_INT_IN_PS2_SIZE, // wMaxPacketSize
			0x00,
			0x01, //  bInterval: Polling Interval (1 ms)
			// -------------------------------------------------------------------------	
};
*/