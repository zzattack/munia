#include <xc.inc>
#include "hardware.h"
    
; keep this in sync with declarations in asm_routines.h
PSECT vars,class=RAM,space=1,abs,ovrld
    ORG 2h
    WREGCPY: ds 1
    STATUSCPY: ds 1
    BSRCPY: ds 1

    packet_state: ds 1
    bit_count: ds 1
    PWMCycle: ds 1
    
    GMEM: ds 2
    sample_w: ds 2  ; sample buffer ptr
    
pSMEM		equ		0x700   ; sample buffer memory
    
#include "isrs.inc"    
#include "capture-n64.inc"
    
    
    end