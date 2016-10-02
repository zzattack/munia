    ORG 0x1008
    goto    ISR

    ORG 0x3fa0
ISR
    movf PORTC, w ; determine which port got triggered
    
    ; no need to check IF flag because IOCIF is the only enabled interrupt
    movff FSR0L, GMEM+0 ; backup FSR
    movff FSR0H, GMEM+1
    lfsr FSR0, pSMEM ; setup FSR0

    xorlw 0xff ; invert bits 0 and 1 for next test (but not 7)
    andwf IOCC, w, 0
    movwf test, 0
    
    btfsc test, 1 ; RC1 is n64
    call n64_sample
    
    btfsc test, 0 ; RC0 is ngc
    call ngc_sample
    
    btfsc test, 7 ; RC7 is snes lat
    call snes_sample
    
    movff FSR0L, pSW+0 ; update sample_w
    movff FSR0H, pSW+1

    movff GMEM+0, FSR0L    ; restore FSR
    movff GMEM+1, FSR0H
    
    bcf INTCON, IOCIF ; clear interrupt flag, late reset is fine
    ; as if we missed something in the mean time it's permanently lost anyway
    
    retfie 1