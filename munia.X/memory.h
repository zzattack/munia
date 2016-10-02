#ifndef _MEMORY_H
#define _MEMORY_H

#include <stdbool.h>
#include <stdint.h>

void ee_write(uint16_t bAdd, uint8_t bData);
uint8_t ee_read(uint8_t bAdd);

#endif // _MEMORY_H