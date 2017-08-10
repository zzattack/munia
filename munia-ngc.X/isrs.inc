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
    
    goto    isr_low_end 
        
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

isr_test_ngc:
    btfss IOCC, 1, c ; RC1 is ngc
    call _ngc_sample
    bra isr_cleanup

isr_cleanup:
    movff FSR0L, sample_w+0 ; update sample_w
    movff FSR0H, sample_w+1

    movff GMEM+0, FSR0L    ; restore FSR
    movff GMEM+1, FSR0H
    
    bcf IOCIF, c; clear interrupt flag, late reset is fine
    ; as if we missed something in the mean time it's permanently lost anyway
    
    retfie F

