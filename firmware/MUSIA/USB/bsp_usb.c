#include "bsp_usb.h"
#include <xpd_usb.h>

extern USB_HandleType *const UsbDevice;

#include <core_cm0.h>

void USB_IRQHandler(void);
void RCC_USB_CLK_ENABLE();
void RCC_USB_CLK_DISABLE();

	
/* USB dependencies initialization */
static void BSP_USB_Init(void* handle) {
	RCC_USB_CLK_ENABLE();
	NVIC_SetPriority(USB_IRQn, 0);
	NVIC_EnableIRQ(USB_IRQn);
}

static void BSP_USB_Deinit(void * handle) {
	RCC_USB_CLK_DISABLE();
	NVIC_DisableIRQ(USB_IRQn);
}

void BSP_USB_Bind(void) {
	USB_INST2HANDLE(UsbDevice, USB);
	UsbDevice->Callbacks.DepInit = BSP_USB_Init;
	UsbDevice->Callbacks.DepDeinit = BSP_USB_Deinit;
}

void USB_IRQHandler(void) {
	USB_vIRQHandler(UsbDevice);
}


void RCC_USB_CLK_ENABLE() {	
	volatile uint32_t tmpreg; 
	((((RCC_TypeDef *)((((uint32_t)0x40000000U) + 0x00020000) + 0x00001000))->APB1ENR) |= ((0x1U << (23U))));
	tmpreg = ((((RCC_TypeDef *)((((uint32_t)0x40000000U) + 0x00020000) + 0x00001000))->APB1ENR) & ((0x1U << (23U)))); 
	((void)(tmpreg)); 
}

void RCC_USB_CLK_DISABLE() { 
	(((RCC_TypeDef *)((((uint32_t)0x40000000U) + 0x00020000) + 0x00001000))->APB1ENR &= ~((0x1U << (23U))));
}