n64_sample:
    movlw 100
    movwf TMR0, c
    bcf TMR0IF, c
    
    ; a bit begins on the falling edge, then wait for rise
n64_wait_rise:
    btfsc TMR0IF, c
    bra n64_timeout
    btfss PORTC, 1, c
    bra n64_wait_rise

    ; save the period spent low
    movf TMR2, w, c
    clrf TMR2, c
n64_wait_fall:
    btfsc TMR0IF, c
    bra n64_timeout
    btfsc PORTC, 1, c
    bra n64_wait_fall
    
    subwf TMR2, w, c ; bit has ended, compare timers
    clrf TMR2, c ; restart asap for next bit
        
    movwf POSTINC0, c ; interpret stored value as 2's complement integer for comparison
    movlw 100 ; reload tmr0
    movwf TMR0, c
    
    bra n64_wait_rise ; sample next bit
    
n64_timeout:
    bsf packet_state, 1, c ; mark packet available    
    return