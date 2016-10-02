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
    
#include "isr.inc"
		    
    ORG 0x3a00
#include "capture-ngc.inc"
    ORG 0x3c00
#include "capture-n64.inc"
    ORG 0x3e00
#include "capture-snes.inc"
		
    END