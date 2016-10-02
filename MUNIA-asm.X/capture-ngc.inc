ngc_sample
    movf PORTC, w
    movwf POSTINC0
    ; bcf TRISA,7
    ; bcf LATA,7
    
ngc_wait_bit0
    btfss PORTC, 0
    bra ngc_wait_bit0
    
ngc_preloop
    movlw 255-60 ; 1.5 bit wait
    movwf TMR0L
    bcf INTCON, TMR0IF
    movlw 0

ngc_loop
    btfsc INTCON, TMR0IF ; timeout while waiting for falling edge
    bra ngc_timeout
    btfsc PORTC, 0 ; wait for falling edge
    bra ngc_loop
        
ngc_samplebit
    ; we have time to sample during at most 3µs, which gives us 3e-6 / (1/12e6)=36 cycles
    ; sampling takes only 2 instructions, we've lost at most 4 in ngc_loop, and need 
    ; leaving us time to perform at least (36-4-2)/2=15 samples
    
    ; bsf LATA,7

    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    btfsc PORTC, 0
    incf WREG, w, 0
    
    btfsc PORTC, 0
    incf WREG, w, 0
    
    ; bcf LATA,7
    
ngc_sampledone
    movwf POSTINC0
    bra ngc_preloop ; prepare for next bit
    
ngc_timeout
    ; packet is done sampling, if we're in SNES2NGC mode we must react quickly
    btfss snes_mode, 2
    bra ngc_done
    
ngc_done
    bsf packet_state, 0, 0 ; mark packet available
    ; bcf LATA,7
    return
