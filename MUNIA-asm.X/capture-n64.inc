n64_sample
    movlw 150
    movwf TMR0
    bcf INTCON, TMR0IF
    
    ; a bit begins on the falling edge, then wait for rise
n64_wait_rise
    btfsc INTCON, TMR0IF
    bra n64_timeout
    btfss PORTC, 1
    bra n64_wait_rise

    ; save the period spent low
    movf TMR2, w
    clrf TMR2
n64_wait_fall
    btfsc INTCON, TMR0IF
    bra n64_timeout
    btfsc PORTC, 1
    bra n64_wait_fall
    
    subwf TMR2, w ; bit has ended, compare timers
    clrf TMR2 ; restart asap for next bit
        
    movwf POSTINC0 ; interpret stored value as 2's complement integer for comparison
    movlw 150 ; reload tmr0
    movwf TMR0
    
    bra n64_wait_rise ; sample next bit
    
n64_timeout
    bsf packet_state, 1, 0 ; mark packet available    
    return