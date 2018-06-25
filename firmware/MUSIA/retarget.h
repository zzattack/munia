#pragma once

#ifdef __cplusplus
#define EXTERNC extern "C"
#else
#define EXTERNC
#endif

EXTERNC int sync_printf_pfx(const char* prefix, const char* format, ...);
EXTERNC int sync_printf(const char* format, ...);
EXTERNC void RetargetInit(void* huart);

#ifndef FAST_SEMIHOSTING_PROFILER_DRIVER
EXTERNC int _isatty(int fd);
EXTERNC int _write(int fd, char* ptr, int len);
EXTERNC int _close(int fd);
EXTERNC int _lseek(int fd, int ptr, int dir);
EXTERNC int _read(int fd, char* ptr, int len);
// EXTERNC int _fstat(int fd, struct stat* st);
#endif

#ifndef RELEASE
#define gps_printf(x, ...) sync_printf_pfx("GPS", x, ##__VA_ARGS__)
#define rpd_printf(x, ...) sync_printf_pfx("RPD", x, ##__VA_ARGS__)
#define era_printf(x, ...) sync_printf_pfx("ERA", x, ##__VA_ARGS__)
#define fla_printf(x, ...) sync_printf_pfx("FLASH", x, ##__VA_ARGS__)
#define can_printf(x, ...) sync_printf_pfx("CANBUS", x, ##__VA_ARGS__)
#define img_printf(x, ...) sync_printf_pfx("IMG", x, ##__VA_ARGS__)
#define spi_printf(x, ...) sync_printf_pfx("SPI", x, ##__VA_ARGS__)
#define hal_printf(x, ...) sync_printf_pfx("HAL", x, ##__VA_ARGS__)
#define sys_printf(x, ...) sync_printf_pfx("SYS", x, ##__VA_ARGS__)
#define cv_printf(x, ...) sync_printf_pfx("Canvas", x, ##__VA_ARGS__)
#define pwr_printf(x, ...) sync_printf_pfx("POWER", x, ##__VA_ARGS__)
#else
#define gps_printf(x, ...)
#define rpd_printf(x, ...)
#define era_printf(x, ...)
#define fla_printf(x, ...)
#define can_printf(x, ...)
#define img_printf(x, ...)
#define spi_printf(x, ...)
#define hal_printf(x, ...)
#define sys_printf(x, ...)
#define cv_printf(x, ...) 
#define pwr_printf(x, ...)
#endif