#pragma once

static void printchar(char **str, int c);
static int prints(char **out, const char *string, int width, int pad);
static int printi(char **out, int i, int b, int sg, int width, int pad, int letbase);
int print(char **out, const char *format, va_list args);
int printf(const char *format, ...);
int sprintf(char *out, const char *format, ...);
int snprintf(char *buf, unsigned int count, const char *format, ...);