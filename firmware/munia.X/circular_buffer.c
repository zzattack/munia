#include "circular_buffer.h"


void cbInit(CircularBuffer* cb) {
    cb->r = cb->w = cb->buf;
    cb->buf_end = cb->buf + sizeof(cb->buf);
}

// Write an element, overwriting oldest element if buffer is full-> App can
//   choose to avoid the overwrite by checking cbIsFull()
void cbWrite(CircularBuffer* cb, unsigned char data) {
    *(cb->w++) = data;
    if (cb->w == cb->buf_end)
        cb->w = cb->buf;
}

unsigned char cbRead(CircularBuffer* cb) {
    unsigned char data = *(cb->r++);
    if (cb->r == cb->buf_end)
        cb->r = cb->buf;
    return data;
}

void cbClear(CircularBuffer* cb) {
    cb->r = cb->buf;
    cb->w = cb->buf;
}
