#define N64_TIMEOUT 100
GLOBAL _n64_sample
_n64_sample:
    movlw N64_TIMEOUT
    movwf TMR0, c
    bcf TMR0IF, c
    
    ; a bit begins on the falling edge, then wait for rise
n64_wait_rise:
    btfsc TMR0IF, c
    bra n64_timeout
    btfss PORTC, 1, c
    bra n64_wait_rise

    ; save the period spent low
    movf TMR1L, w, c
    clrf TMR1L, c
n64_wait_fall:
    btfsc TMR0IF, c
    bra n64_timeout
    btfsc PORTC, 1, c
    bra n64_wait_fall
    
    subwf TMR1L, w, c ; bit has ended, compare timers
    clrf TMR1L, c ; restart asap for next bit
        
    movwf POSTINC0, c ; interpret stored value as 2's complement integer for comparison
    movlw N64_TIMEOUT ; reload tmr0
    movwf TMR0, c
    
    bra n64_wait_rise ; sample next bit
    
n64_timeout:
    bsf packet_state, 1, c ; mark packet available    
    return