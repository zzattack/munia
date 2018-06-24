#ifndef CONFIG_H
#define CONFIG_H

#include <stdint.h>

typedef enum { output_ngc, output_n64, output_snes, output_pc } output_t;
typedef enum { input_ngc = 1, input_n64 = 2, input_snes = 4 } input_t;
typedef struct {
    output_t output_mode;
    union {
        struct {
            input_t input_sources;
        };
        struct {
            uint8_t input_ngc : 1;
            uint8_t input_n64 : 1;
            uint8_t input_snes : 1;
        };
    };
    uint8_t spare; // old config had 3 bytes, so this pads it
} config_t;

#endif