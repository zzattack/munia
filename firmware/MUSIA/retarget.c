#include "retarget.h"
#include <stdio.h>
#include <stdarg.h>
#include <stm32f0xx_hal.h>
#include <stm32f0xx_hal_uart.h>
#include <_ansi.h>
#include <errno.h>
#include <sys/time.h>
#include <sys/times.h>
#include <sys/stat.h>

UART_HandleTypeDef *gHuart;
#define STDIN_FILENO  0
#define STDOUT_FILENO 1
#define STDERR_FILENO 2

void RetargetInit(void* huart) {
	gHuart = (UART_HandleTypeDef*)huart ;

	/* Disable I/O buffering for STDOUT stream, so that
	 * chars are sent out as soon as they are printed. */
	setvbuf(stdout, NULL, _IONBF, 0);
}

int sync_printf(const char* format, ...) {
	va_list args;
	va_start(args, format);
	int v = 0;
	v = vprintf(format, args);
	
	va_end(args);
	return v;
}

int sync_printf_pfx(const char* prefix, const char* format, ...) {
	va_list args;
	va_start(args, format);
	int v = 0;
	v = printf("[%s] ", prefix);
	v += vprintf(format, args);

	va_end(args);
	return v;
}

EXTERNC void printf_payload(const char* x, int len) {	
	while (len--) sync_printf("%02x ", *x++);
}

#ifndef FAST_SEMIHOSTING_PROFILER_DRIVER
int _isatty(int fd) {
	if (fd >= STDIN_FILENO && fd <= STDERR_FILENO)
		return 1;

	errno = EBADF;
	return 0;
}

int _write(int fd, char* ptr, int len) {
	HAL_StatusTypeDef hstatus;

	if (fd == STDOUT_FILENO || fd == STDERR_FILENO) {
		hstatus = HAL_UART_Transmit(gHuart, (uint8_t*)ptr, len, HAL_MAX_DELAY);
		if (hstatus == HAL_OK)
			return len;
	}
	errno = EBADF;
	return -1;
}

int _close(int fd) {
	if (fd >= STDIN_FILENO && fd <= STDERR_FILENO)
		return 0;

	errno = EBADF;
	return -1;
}

int _lseek(int fd, int ptr, int dir) {
	(void) fd;
	(void) ptr;
	(void) dir;

	errno = EBADF;
	return -1;
}

int _read(int fd, char* ptr, int len) {
	HAL_StatusTypeDef hstatus;

	if (fd == STDIN_FILENO) {
		hstatus = HAL_UART_Receive(gHuart, (uint8_t *) ptr, 1, HAL_MAX_DELAY);
		if (hstatus == HAL_OK)
			return 1;
		else
			return EIO;
	}
	errno = EBADF;
	return -1;
}

int _fstat(int fd, struct stat* st) {
	if (fd >= STDIN_FILENO && fd <= STDERR_FILENO) {
		st->st_mode = S_IFCHR;
		return 0;
	}

	errno = EBADF;
	return 0;
}
#endif