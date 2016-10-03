#include "p18f24k50.inc"

bit_count	equ		0x002
snes_mode	equ		0x003
n64_mode	equ		0x004
ngc_mode	equ		0x005
packet_state	equ		0x006
test		equ		0x007
GMEM		equ		0x008
pSW		equ		0x010   ; sample buffer ptr
pSMEM		equ		0x700   ; sample buffer memory
    
    ORG 0x1008
    goto    ISR
    
; program memory layout:
; isr: 3f00-3f42 --> 66
; snes: 3f50-3f88 --> 56
; n64: 3f90-3fb8  --> 40
; ngc: 3fc0-3fe8 --> 40
    ORG 0x3f00
#include "isr.inc"
    ORG 0x3f50
#include "capture-ngc.inc"
    ORG 0x3f90
#include "capture-n64.inc"
    ORG 0x3fc4
#include "capture-snes.inc"
		
    END