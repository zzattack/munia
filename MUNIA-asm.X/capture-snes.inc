snes_sample
    ; called in interrupt: watches the SNES_CLK for 16 pulses
    ; The controllers serially shift the latched button states out SNES_DAT on every rising edge
    ; and the CPU samples the data on every falling edge.
    clrf TMR0 ; reset to 0 so latch has ample time to fall
    bcf INTCON, TMR0IF
    movlw 16
    movwf bit_count, 0
    
snes_wait_latch
    btfsc INTCON, TMR0IF ; timeout
    return    
    btfsc PORTC, 7 ; latch is RC7
    bra snes_wait_latch

    movlw 150
    movwf TMR0, 0
    
snes_sample_bit
    btfsc PORTC, 7 ; latch went high unexpectedly (RC7)
    return
    
snes_wait_fall
    btfsc INTCON, TMR0IF
    return
    btfsc PORTA, 7 ; wait for clock to go low (RA7)
    bra snes_wait_fall
   
    movf PORTC, w ; sample (RC2)
    movwf POSTINC0

    movlw 150 ; wait for clock to go high (RA7)
    movwf TMR0, 0
snes_wait_rise
    btfsc INTCON, TMR0IF
    return
    btfss PORTA, 7
    bra snes_wait_rise
    
    movwf TMR0, 0 ; reload
    decfsz bit_count	
    bra snes_sample_bit

snes_done
    bsf packet_state, 2, 0 ; mark packet available
    return
