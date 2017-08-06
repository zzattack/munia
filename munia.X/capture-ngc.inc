ngc_sample:
    movlw 150
    movwf TMR0, c
    bcf TMR0IF, c
    
    ; a bit begins on the falling edge, then wait for rise
ngc_wait_rise:
    btfsc TMR0IF, c
    bra ngc_timeout
    btfss PORTC, 0, c
    bra ngc_wait_rise

    ; save the period spent low
    movf TMR2, w, c
    clrf TMR2, c
ngc_wait_fall:
    btfsc TMR0IF, c
    bra ngc_timeout
    btfsc PORTC, 0, c
    bra ngc_wait_fall
	
    subwf TMR2, w, c ; bit has ended, compare timers
    clrf TMR2, c ; restart asap for next bit
        
    movwf POSTINC0, c ; interpret stored value as 2's complement integer for comparison
    movlw 150 ; reload tmr0
    movwf TMR0, c

    bra ngc_wait_rise ; sample next bit
    
ngc_timeout:
    bsf packet_state, 0, c ; mark packet available    
    return