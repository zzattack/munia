snes_sample:
    ; called in interrupt: watches the SNES_CLK for 16 pulses
    ; The controllers serially shift the latched button states out SNES_DAT on every rising edge
    ; and the CPU samples the data on every falling edge.
    clrf TMR0, c ; reset to 0 so latch has ample time to fall
    bcf TMR0IF, c
    movlw 16
    movwf bit_count, c
    
snes_wait_latch:
    btfsc TMR0IF, c ; timeout
    return    
    btfsc PORTC, 7, c ; latch is RC7
    bra snes_wait_latch

    movlw 150
    movwf TMR0, c
    
snes_sample_bit:
    btfsc PORTC, 7, c ; latch went high unexpectedly (RC7)
    return
    
snes_wait_fall:
    btfsc TMR0IF, c
    return
    btfsc PORTA, 7, c ; wait for clock to go low (RA7)
    bra snes_wait_fall
   
    movf PORTC, w, c ; sample (RC2)
    movwf POSTINC0, c

    movlw 150 ; wait for clock to go high (RA7)
    movwf TMR0, c
snes_wait_rise:
    btfsc TMR0IF, c
    return
    btfss PORTA, 7, c
    bra snes_wait_rise
    
    movwf TMR0, c ; reload
    decfsz bit_count, c
    bra snes_sample_bit

snes_done:
    bsf packet_state, 2, c ; mark packet available
    return
