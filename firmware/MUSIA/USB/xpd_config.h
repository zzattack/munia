/* TODO Include the proper CMSIS device header */
#include "stm32f042x6.h"

/* Replace type definitions */
#define USB_TypeDef                __USB_TypeDef
#define USB_OTG_INEndpointTypeDef  __USB_OTG_INEndpointTypeDef
#define USB_OTG_OUTEndpointTypeDef __USB_OTG_OUTEndpointTypeDef
#define USB_OTG_HostChannelTypeDef __USB_OTG_HostChannelTypeDef
#define USB_OTG_TypeDef            __USB_OTG_TypeDef

#undef  USB
#define USB ((__USB_TypeDef*)USB_BASE)

/** 
  * @brief Universal Serial Bus Full Speed Device
  */
typedef struct {
	union {
		struct {
			__IO uint16_t EA : 4;                           /*!<  EndPoint Address */
			__IO uint16_t STAT_TX : 2;                      /*!<  EndPoint TX Status */
			__IO uint16_t DTOG_TX : 1;                      /*!<  EndPoint Data TOGGLE TX */
			__IO uint16_t CTR_TX : 1;                       /*!<  EndPoint Correct TRansfer TX */
			__IO uint16_t KIND : 1;                         /*!<  EndPoint KIND */
			__IO uint16_t TYPE : 2;                         /*!<  EndPoint TYPE */
			__IO uint16_t SETUP : 1;                        /*!<  EndPoint SETUP */
			__IO uint16_t STAT_RX : 2;                      /*!<  EndPoint RX Status */
			__IO uint16_t DTOG_RX : 1;                      /*!<  EndPoint Data TOGGLE RX */
			__IO uint16_t CTR_RX : 1;                       /*!<  EndPoint Correct TRansfer RX */
		} b;
		__IO uint16_t w;
		uint32_t __RESERVED;
	} EPR[8];                                               /*!< USB endpoint register */
	uint32_t __RESERVED0[8];
	union {
		struct {
			__IO uint16_t FRES : 1;                         /*!< Force USB RESet */
			__IO uint16_t PDWN : 1;                         /*!< Power DoWN */
			__IO uint16_t LPMODE : 1;                       /*!< Low-power MODE */
			__IO uint16_t FSUSP : 1;                        /*!< Force SUSPend */
			__IO uint16_t RESUME : 1;                       /*!< RESUME request */
			__IO uint16_t L1RESUME : 1;                     /*!< LPM L1 Resume request */
			uint16_t __RESERVED0 : 1;
			__IO uint16_t L1REQM : 1;                       /*!< LPM L1 state request interrupt mask */
			__IO uint16_t ESOFM : 1;                        /*!< Expected Start Of Frame Mask */
			__IO uint16_t SOFM : 1;                         /*!< Start Of Frame Mask */
			__IO uint16_t RESETM : 1;                       /*!< RESET Mask   */
			__IO uint16_t SUSPM : 1;                        /*!< SUSPend Mask */
			__IO uint16_t WKUPM : 1;                        /*!< WaKe UP Mask */
			__IO uint16_t ERRM : 1;                         /*!< ERRor Mask */
			__IO uint16_t PMAOVRM : 1;                      /*!< DMA OVeR/underrun Mask */
			__IO uint16_t CTRM : 1;                         /*!< Correct TRansfer Mask */
		} b;
		__IO uint16_t w;
	} CNTR;                                                 /*!< Control register,                       Address offset: 0x40 */
	uint16_t __RESERVED1;
	union {
		struct {
			__IO uint16_t EP_ID : 4;                        /*!< EndPoint IDentifier (read-only bit)  */
			__IO uint16_t DIR : 1;                          /*!< DIRection of transaction (read-only bit)  */
			uint16_t __RESERVED0 : 2;
			__IO uint16_t L1REQ : 1;                        /*!< LPM L1 state request  */
			__IO uint16_t ESOF : 1;                         /*!< Expected Start Of Frame (clear-only bit) */
			__IO uint16_t SOF : 1;                          /*!< Start Of Frame (clear-only bit) */
			__IO uint16_t RESET : 1;                        /*!< RESET (clear-only bit) */
			__IO uint16_t SUSP : 1;                         /*!< SUSPend (clear-only bit) */
			__IO uint16_t WKUP : 1;                         /*!< WaKe UP (clear-only bit) */
			__IO uint16_t ERR : 1;                          /*!< ERRor (clear-only bit) */
			__IO uint16_t PMAOVR : 1;                       /*!< DMA OVeR/underrun (clear-only bit) */
			__IO uint16_t CTR : 1;                          /*!< Correct TRansfer (clear-only bit) */
		} b;
		__IO uint16_t w;
	} ISTR;                                                 /*!< Interrupt status register,              Address offset: 0x44 */
	uint16_t __RESERVED2;
	union {
		struct {
			__IO uint16_t FN : 11;                          /*!< Frame Number */
			__IO uint16_t LSOF : 2;                         /*!< Lost SOF */
			__IO uint16_t LCK : 1;                          /*!< LoCKed */
			__IO uint16_t RXDM : 1;                         /*!< status of D- data line */
			__IO uint16_t RXDP : 1;                         /*!< status of D+ data line */
		} b;
		__IO uint16_t w;
	} FNR;                                                  /*!< Frame number register,                  Address offset: 0x48 */
	uint16_t __RESERVED3;
	union {
		struct {
			__IO uint16_t ADD : 7;                          /*!< USB device address */
			__IO uint16_t EF : 1;                           /*!< USB device address Enable Function */
			uint16_t __RESERVED0 : 8;
		} b;
		__IO uint16_t w;
	} DADDR;                                                /*!< Device address register,                Address offset: 0x4C */
	uint16_t __RESERVED4;
	__IO uint16_t BTABLE;                                   /*!< Buffer Table address register,          Address offset: 0x50 */
	uint16_t __RESERVED5;
	union {
		struct {
			__IO uint16_t LMPEN : 1;                        /*!< LPM support enable  */
			__IO uint16_t LPMACK : 1;                       /*!< LPM Token acknowledge enable*/
			uint16_t __RESERVED0 : 1;
			__IO uint16_t REMWAKE : 1;                      /*!< bRemoteWake value received with last ACKed LPM Token */
			__IO uint16_t BESL : 4;                         /*!< BESL value received with last ACKed LPM Token  */
			uint16_t __RESERVED1 : 8;
		} b;
		__IO uint16_t w;
	} LPMCSR;                                               /*!< LPM Control and Status register,        Address offset: 0x54 */
	uint16_t __RESERVED6;
	union {
		struct {
			__IO uint16_t BCDEN : 1;                        /*!< Battery charging detector (BCD) enable */
			__IO uint16_t DCDEN : 1;                        /*!< Data contact detection (DCD) mode enable */
			__IO uint16_t PDEN : 1;                         /*!< Primary detection (PD) mode enable */
			__IO uint16_t SDEN : 1;                         /*!< Secondary detection (SD) mode enable */
			__IO uint16_t DCDET : 1;                        /*!< Data contact detection (DCD) status */
			__IO uint16_t PDET : 1;                         /*!< Primary detection (PD) status */
			__IO uint16_t SDET : 1;                         /*!< Secondary detection (SD) status */
			__IO uint16_t PS2DET : 1;                       /*!< PS2 port or proprietary charger detected */
			uint16_t __RESERVED0 : 7;
			__IO uint16_t DPPU : 1;                         /*!< DP Pull-up Enable */
		} b;
		__IO uint16_t w;
	} BCDR;                                                 /*!< Battery Charging detector register,     Address offset: 0x58 */
} USB_TypeDef;
