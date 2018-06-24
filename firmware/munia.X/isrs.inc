PSECT ISR_jumps,local,class=CODE,delta=1,reloc=2,abs
#ifdef BOOTLOADER_INCLUDED
    org 0x1008
    goto isr_high
    
    org 0x1018
    goto isr_low
#else
    org 0x008
    goto isr_high
    
    org 0x018
    goto isr_low    
#endif
    
PSECT ISR_routines,local,class=CODE,delta=1,reloc=2
isr_low:    
    movff   WREG,WREGCPY
    movff   BSR,BSRCPY
    movff   STATUS,STATUSCPY   
    
    ; TMR2 is only interrupt source
    bcf	    TMR2IF,c ; tmr2 uses period, no need to reload, just increment 100Hz timer
    
    banksel _EasyTimerTick
    incf    BANKMASK(_EasyTimerTick),f,b    
    
    ; 4-bit/16-step PWM cycle
    incf    PWMCycle, f, c
    btfsc   PWMCycle, 4, c ; cycle is 16 steps
    goto    pwm_reset
    movf    PWMCycle, w, c
    
    banksel _LcdBacklightValue
    cpfsgt  BANKMASK(_LcdBacklightValue), b
    bcf	    LATB, 5, c ; LATB5 is backlight

    goto    isr_low_end 
    
pwm_reset:
    clrf    PWMCycle, c
    banksel _LcdBacklightValue
    movf    BANKMASK(_InitLATB), w, b
    iorwf   LATB, f, c
    
isr_low_end:    
    movff   BSRCPY,BSR
    movff   STATUSCPY,STATUS
    movff   WREGCPY,WREG
    retfie

    
    
isr_high:
    movf PORTC, w, c ; determine which port got triggered    
    clrf TMR1L, c ; reset timer 1, used to determine how long current bit is low
    
    ; no need to check IF flag because IOCIF is the only enabled interrupt
    movff FSR0L, GMEM+0 ; backup FSR
    movff FSR0H, GMEM+1
    lfsr 0, pSMEM ; setup FSR0

    xorlw 0x03 ; invert bits 0 and 1 for next test (but not 7)
    andwf IOCC, w, c
    
    btfss WREG, 1, c ; RC1 is n64
    bra isr_test_ngc
    call _n64_sample
    bra isr_cleanup
    
isr_test_ngc:
    btfss WREG, 0, c ; RC0 is ngc
    bra isr_test_snes
    call _ngc_sample
    bra isr_cleanup
    
isr_test_snes:
    ; btfsc test, 7 ; RC7 is snes lat, which is normally high on super famicom
    ; and normally low on snes, so toggle direction test is useless
    call _snes_sample

isr_cleanup:
    movff FSR0L, sample_w+0 ; update sample_w
    movff FSR0H, sample_w+1

    movff GMEM+0, FSR0L    ; restore FSR
    movff GMEM+1, FSR0H
    
    bcf IOCIF, c; clear interrupt flag, late reset is fine
    ; as if we missed something in the mean time it's permanently lost anyway
    
    retfie F

