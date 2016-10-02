n64_sample
    movf PORTC, w
    movwf POSTINC0
    ; bcf TRISA,7
    ; bcf LATA,7
    
n64_wait_rise
    btfss PORTC, 1
    bra n64_wait_rise
    
n64_preloop
    movlw 255-60 ; 1.5 bit wait
    movwf TMR0L
    bcf INTCON, TMR0IF
    movlw 0

n64_loop
    btfsc INTCON, TMR0IF
    bra n64_timeout
    btfsc PORTC, 1
    bra n64_loop
        
n64_samplebit
    ; we have time to sample during at most 3µs, which gives us 3e-6 / (1/12e6)=36 cycles
    ; sampling takes only 2 instructions, we've lost at most 4 in ngc_loop, and need 
    ; leaving us time to perform at least (36-4-2)/2=15 samples
    
    ; bsf LATA,7

    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    btfsc PORTC, 1
    incf WREG, w, 0
    
    btfsc PORTC, 1
    incf WREG, w, 0
        
    ; bcf LATA,7
    
n64_sampledone
    movwf POSTINC0
    bra n64_preloop ; prepare for next bit
    
n64_timeout
    bsf packet_state, 1, 0 ; mark packet available    
    ; bcf LATA,7
    return
