#include "uarts.h"
#include <string.h>
#include <stdio.h>
#include <xc.h>

#ifndef UART_DISABLE_PORT1

void U1Init(uint32_t baudrate, bool read, bool write) {
    if (write) TRISCbits.TRISC6 = 0; // TX UART1
    if (read) TRISCbits.TRISC7 = 1; // RX UART1

    // setup TXSTA/RCSTA for UART1
    TXSTA1 = 0;
    TXSTA1bits.TX9 = 0;  // Selects 8-bit transmission
    TXSTA1bits.TXEN = write; // Transmit enabled

    RCSTA1bits.OERR = 0; // Clear errors
    RCSTA1bits.FERR = 0;
    RCSTA1bits.SPEN = 1; // Serial Port Enable bit
    RCSTA1bits.RX9 = 0;  // Selects 8-bit reception
    RCSTA1bits.CREN = read; // Enables continuous receive

    // setup interrupts
	PIE1bits.RC1IE = 0;
    
    U1SetBaudrate(baudrate);
}

void U1SetBaudrate(uint32_t baudrate) {
    TXSTA1bits.SYNC = 0;    // Asynchronous
    TXSTA1bits.BRGH = 1;    // High speed
    BAUDCON1bits.BRG16 = 1; // 16-Bit Baud Rate Register
    uint16_t baud = _XTAL_FREQ / (4 * (baudrate + 1));
    SPBRG1 = baud & 0xFF;    // SPBRG 416 --> baud 38400
    SPBRGH1 = baud >> 8;     // SPBRG 138 --> baud 115200
}

// synchronously data exchange
void U1TxSync(char ch) {
    while (!TX1IF);
    TXREG1 = ch;
}
void U1putsSync(const char* msg) {
    while (*msg != '\0') U1TxSync(*msg++);
}

#endif
