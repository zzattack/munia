#ifndef CIRCULARBUFFER_H
#define	CIRCULARBUFFER_H

#include <stdint.h>

typedef struct {
    uint8_t buf[64];
    uint8_t* buf_end;
    uint8_t* w;
    uint8_t* r;
} CircularBuffer;

void cbInit(CircularBuffer* cb);
#define cbIsEmpty(cb) ((cb)->w==(cb)->r)
void cbWrite(CircularBuffer* cb, unsigned char data);
unsigned char cbRead(CircularBuffer* cb);
void cbClear(CircularBuffer* cb);


#endif	/* CIRCULARBUFFER_H */