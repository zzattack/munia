#pragma once

#ifdef __cplusplus
#define EXTERNC extern "C"
#else
#define EXTERNC
#endif

EXTERNC int sync_printf_pfx(const char* prefix, const char* format, ...);
EXTERNC int sync_printf(const char* format, ...);
EXTERNC void RetargetInit(void* huart);
EXTERNC void printf_payload(const char* x, int len);

#ifndef FAST_SEMIHOSTING_PROFILER_DRIVER
EXTERNC int _isatty(int fd);
EXTERNC int _write(int fd, char* ptr, int len);
EXTERNC int _close(int fd);
EXTERNC int _lseek(int fd, int ptr, int dir);
EXTERNC int _read(int fd, char* ptr, int len);
#endif

#ifndef RELEASE
#define spi_printf(x, ...) sync_printf_pfx("SPI", x, ##__VA_ARGS__)
#define hal_printf(x, ...) sync_printf_pfx("HAL", x, ##__VA_ARGS__)
#define sys_printf(x, ...) sync_printf_pfx("SYS", x, ##__VA_ARGS__)
#define ps2_printf(x, ...) // sync_printf_pfx("PS2", x, ##__VA_ARGS__)
#define usb_printf(x, ...) sync_printf_pfx("USB", x, ##__VA_ARGS__)
#else
#define spi_printf(x, ...)
#define hal_printf(x, ...)
#define sys_printf(x, ...)
#define ps2_printf(x, ...)
#define usb_printf(x, ...)
#endif