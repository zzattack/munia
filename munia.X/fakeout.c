#include "fakeout.h"
#include "uarts.h"
#include <stdlib.h>

#define NGC_JOY_DEADZONE 15
#define NGC_CSTICK_THRESHOLD 30

#define ngc_fakeout() do { portc_mask = 0b00000001; fakeout(); } while (0);
#define n64_fakeout() do { portc_mask = 0b00000010; fakeout(); } while (0);

void ngc_fakeout_test() {
    // Test if fake out needed. This is called from the interrupt so that
    // we can react as quickly as possible.
    uint8_t idx = (uint8_t)sample_w;
    if (idx == 8) {
        uint8_t cmd = pack_byte((int8_t*)sample_buff);
        if (cmd == 0x00) {
            // tell wii we're here
            fake_count = 24;
            pfake_out = ngc_id_packet;
            ngc_fakeout();
        }
        else if (cmd == 0x01) {
            fake_count = 80;
            pfake_out = ngc_init_packet;
            ngc_fakeout();
        }
        else {
            dbgs("unknown ngc command: "); dbgsval(cmd); dbgs("\n");
        }
    }
    else if (idx == 24) {
        fake_count = 64;
        pfake_out = fake_buffer;
        ngc_fakeout();
        TMR3 = 60000; // request next poll cycle real quickly
    }
    else {
        dbgs("unexpected ngc sample idx: "); dbgsval(idx); dbgs("\n");
    }
}

void n64_fakeout_test() {    
    uint8_t idx = (uint8_t)sample_w;
    if (idx == 8) {
        uint8_t cmd = pack_byte((int8_t*)sample_buff);
        if (cmd == 0x00) {
            fake_count = 24;
            pfake_out = n64_id_packet;
            n64_fakeout();
        }
        else if (cmd == 0x01) {
            fake_count = 32;
            pfake_out = fake_buffer;
            n64_fakeout();
        }
        else {
            dbgs("unknown n64 command: "); dbgsval(cmd); dbgs("\n");
        }
    }
    else {
        dbgs("unexpected n64 sample idx: "); dbgsval(idx); dbgs("\n");
    }
}

void fakeout() {
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    
    di(); // we have to disable interrupts, because we are toggling
    // the pins that have IOCC notification enabled
    
    CLR(); // set data pin to output, making the pin low
    uint8_t* r = pfake_out;
    while (fake_count) {
        CLR(); // use stopwatch to verify this happens either 1 or 3 µs after SET() call below)
        if (!*r) {
            // low
            r++; fake_count--; Nop();
            _delay(24);
            SET(); // use stopwatch to verify this happens 3µs after CLR())
        }
        else {
            _delay(3);
            SET(); // use stopwatch to verify this happens 3µs after CLR()
            r++; fake_count--;
            _delay(22);
        }
        _delay(4);
    }
        
    // stop bit, 2 us
    CLR(); 
    _delay(22);
    SET();// back set to open collector input with pull up
    LATC |= portc_mask; // reset pull up
    
    // clear the IOCC event we undoubtedly generated
    INTCONbits.IOCIF = 0;
    ei();
}


void fake_unpack(uint8_t* r, uint8_t n) {
    // unpack buffer bits to bytes
    uint8_t* w = fake_buffer;
    while (n--) {
        uint8_t b = *r++;
        for (uint8_t m = 0x80; m; m >>= 1)
            *w++ = (b & m) ? 1 : 0;
    }
}

void snes_create_ngc_fake() {
    joydata_ngc_raw.one = 1;
    joydata_ngc_raw.zero1 = 0;
    joydata_ngc_raw.zero2 = 0;
    joydata_ngc_raw.zero3 = 0;

    joydata_ngc_raw.a = joydata_snes_raw.b;
    joydata_ngc_raw.b = joydata_snes_raw.y;
    joydata_ngc_raw.x = joydata_snes_raw.a;
    joydata_ngc_raw.y = joydata_snes_raw.x;
    joydata_ngc_raw.start = joydata_snes_raw.start;
    joydata_ngc_raw.z = 0;
    joydata_ngc_raw.l = joydata_snes_raw.l;
    joydata_ngc_raw.r = joydata_snes_raw.r;
    joydata_ngc_raw.left_trig = joydata_snes_raw.l ? 255 : 0;
    joydata_ngc_raw.right_trig = joydata_snes_raw.r ? 255 : 0;

    if (!joydata_snes_raw.select) {
        joydata_ngc_raw.joy_x = 128;
        if (joydata_snes_raw.left)
            joydata_ngc_raw.joy_x -= 127;
        else if (joydata_snes_raw.right)
            joydata_ngc_raw.joy_x += 127;

        joydata_ngc_raw.joy_y = 128;
        if (joydata_snes_raw.up)
            joydata_ngc_raw.joy_y += 127;
        else if (joydata_snes_raw.down)
            joydata_ngc_raw.joy_y -= 127;

        joydata_ngc_raw.dup = 0;
        joydata_ngc_raw.ddown = 0;
        joydata_ngc_raw.dleft = 0;
        joydata_ngc_raw.dright = 0;

        joydata_ngc_raw.c_x = 128;
        joydata_ngc_raw.c_y = 128;
    }
    else {
        joydata_ngc_raw.dup = joydata_snes_raw.up;
        joydata_ngc_raw.ddown = joydata_snes_raw.down;
        joydata_ngc_raw.dleft = joydata_snes_raw.left;
        joydata_ngc_raw.dright = joydata_snes_raw.right;
        joydata_ngc_raw.joy_x = 128;
        joydata_ngc_raw.joy_y = 128;

        joydata_ngc_raw.c_x = 128;
        joydata_ngc_raw.c_y = 128;
        
        if (joydata_snes_raw.y) joydata_ngc_raw.c_x -= 127;
        if (joydata_snes_raw.a) joydata_ngc_raw.c_x += 127;
        if (joydata_snes_raw.b) joydata_ngc_raw.c_y -= 127;
        if (joydata_snes_raw.x) joydata_ngc_raw.c_y += 127;
        
        joydata_ngc_raw.a = false;
        joydata_ngc_raw.b = false;
        joydata_ngc_raw.x = false;
        joydata_ngc_raw.y = false;
    }

    // hid report fixup
    joydata_ngc_raw = joydata_ngc;
    joydata_ngc.joy_y = -joydata_ngc.joy_y;
    joydata_ngc.c_y = -joydata_ngc.c_y;
    joydata_ngc.hat = hat_lookup_ngc[joydata_ngc.hat];

    packets.ngc_avail = true;
}

void ngc_create_n64_fake() {
    joydata_n64_raw.dright = joydata_ngc_raw.dright;
    joydata_n64_raw.dleft = joydata_ngc_raw.dleft;
    joydata_n64_raw.ddown = joydata_ngc_raw.ddown;
    joydata_n64_raw.dup = joydata_ngc_raw.dup;

    joydata_n64_raw.a = joydata_ngc_raw.a;
    joydata_n64_raw.b = joydata_ngc_raw.b;
    
    joydata_n64_raw.start = joydata_ngc_raw.start;
    joydata_n64_raw.z = joydata_ngc_raw.z;
    joydata_n64_raw.l = joydata_ngc_raw.l > 50;
    joydata_n64_raw.r = joydata_ngc_raw.r > 50;
    
    
    // deadzone of 10 for center stick
    joydata_n64_raw.joy_x = 128 + joydata_ngc.joy_x;
    if (abs(joydata_n64_raw.joy_x) < NGC_JOY_DEADZONE) joydata_n64_raw.joy_x = 0;
    joydata_n64_raw.joy_y = 128 - joydata_ngc.joy_y;
    if (abs(joydata_n64_raw.joy_y) < NGC_JOY_DEADZONE) joydata_n64_raw.joy_y = 0;
    
    int8_t cx = joydata_ngc_raw.c_x - 128;
    int8_t cy = 128 - joydata_ngc_raw.c_y;
    joydata_n64_raw.cleft = cx < -NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cright = cx > +NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cdown = cy > +NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cup = cy < -NGC_CSTICK_THRESHOLD;
    
    
    // hid report fixup
    joydata_n64 = joydata_n64_raw;
    joydata_n64.joy_x -= 128;
    joydata_n64.joy_y = 128 - joydata_n64.joy_y;
    joydata_n64.dpad = hat_lookup_n64_snes[joydata_n64.dpad];
    // when l and r are pressed, the start button bit seems to shift to unused1
    joydata_n64.start |= joydata_n64.__unused1 & joydata_n64.l & joydata_n64.r;

        
    packets.n64_avail = true;
}