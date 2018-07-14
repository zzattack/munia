#include "fakeout.h"
#include "asm_decl.h"
#include "uarts.h"
#include <stdlib.h>
#include <xc.h>

#define NGC_JOY_DEADZONE 25
#define NGC_CSTICK_THRESHOLD 30

#define ngc_fakeout() do { portc_mask = 0b00000001; fakeout_ngc64(); } while (0);
#define n64_fakeout() do { portc_mask = 0b00000010; fakeout_ngc64(); } while (0);

void ngc_fakeout_test() {
    // Test if fake out needed. This is called from the interrupt so that
    // we can react as quickly as possible.
    uint8_t idx = (uint8_t)sample_w;
    if (idx == 8) {
        uint8_t cmd = pack_byte((int8_t*)sample_buff);
        if (cmd == 0x00) {
            // tell wii we're here
            fake_count = sizeof(ngc_id_packet) * 8;
            fake_unpack(ngc_id_packet, sizeof(ngc_id_packet));
            ngc_fakeout();
        }
        else if (cmd == 0x41) {
            fake_count = sizeof(ngc_init_packet) * 8;
            fake_unpack(ngc_init_packet, sizeof(ngc_init_packet));
            ngc_fakeout();
        }
        else {
            dbgs("unknown ngc command: "); dbgsval(cmd); dbgs("\n");
        }
    }
    else if (idx == 24) {
        fake_count = 64;
        ngc_fakeout();
    }
    else {
        // dbgs("unexpected ngc sample idx: "); dbgsval(idx); dbgs("\n");
    }
}

void n64_fakeout_test() {    
    uint8_t idx = (uint8_t)sample_w;
    if (idx == 8) {
        uint8_t cmd = pack_byte((int8_t*)sample_buff);
        if (cmd == 0x00) {
            fake_count = 8*sizeof(n64_id_packet);
            fake_unpack(n64_id_packet, sizeof(n64_id_packet));
            n64_fakeout();
        }
        else if (cmd == 0x01) {
            fake_count = 32;
            n64_fakeout();
            // dbgs("n64_fakeout() completed\n");
        }
        else {
            dbgs("unknown n64 command: "); dbgsval(cmd); dbgs("\n");
        }
    }
    else if (idx != 41) { // 41 for fake poll
        dbgs("unexpected n64 sample idx: "); dbgsval(idx); dbgs("\n");
    }
}

void snes_fakeout_test() {
    // not yet implemented
}


void fakeout_ngc64() {
    LATC &= ~portc_mask; // pull down - always call this before CLR() calls
    
    di(); // we have to disable interrupts, because we are toggling
    // the pins that have IOCC notification enabled
    
    CLR(); // set data pin to output, making the pin low
    uint8_t* r = fake_buffer;
    while (fake_count) {
        CLR(); // use stopwatch to verify this happens either 1 or 3 µs after SET() call below)
        if (!*r) {
            // low
            r++; fake_count--; Nop();
            _delay(24);
            SET(); // use stopwatch to verify this happens 3�s after CLR())
        }
        else {
            _delay(3);
            SET(); // use stopwatch to verify this happens 3�s after CLR()
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

void fakeout_snes() { 

}



// Create N64 packet from SNES data
void snes_to_n64() {
    joydata_n64_raw.start = joydata_snes_raw.start;
    joydata_n64_raw.l = joydata_snes_raw.l;
    joydata_n64_raw.r = joydata_snes_raw.r;
    
    if (!joydata_snes_raw.select) {
        // face buttons map directly
        joydata_n64_raw.a = joydata_snes_raw.b;
        joydata_n64_raw.b = joydata_snes_raw.y;
        joydata_n64_raw.z = joydata_snes_raw.a || joydata_snes_raw.x;
    
        // control stick based on d-pad input
        joydata_n64_raw.joy_x = 0;
        if (joydata_snes_raw.left)
            joydata_n64_raw.joy_x -= 127;
        else if (joydata_snes_raw.right)
            joydata_n64_raw.joy_x += 127;
        
        joydata_n64_raw.joy_y = 0;
        if (joydata_snes_raw.up)
            joydata_n64_raw.joy_y += 127;
        else if (joydata_snes_raw.down)
            joydata_n64_raw.joy_y -= 127;

        // d-pad unused
        joydata_n64_raw.dpad = 0;

        // c-stick unused
        joydata_n64_raw.cdown = 0;
        joydata_n64_raw.cleft = 0;
        joydata_n64_raw.cright = 0;
        joydata_n64_raw.cup = 0;
    }
    else {
        // face buttons disabled
        joydata_n64_raw.a = false;
        joydata_n64_raw.b = false;
        joydata_n64_raw.z = 0;
        
        // control stick disabled 
        joydata_n64_raw.joy_x = 0;
        joydata_n64_raw.joy_y = 0;
        
        // d-pad maps to d-pad
        joydata_n64_raw.dpad = joydata_snes_raw.dpad;

        // face buttons map to c-stick
        joydata_n64_raw.cleft = joydata_snes_raw.y;
        joydata_n64_raw.cright = joydata_snes_raw.a;
        joydata_n64_raw.cdown = joydata_snes_raw.b;
        joydata_n64_raw.cup = joydata_snes_raw.x;
    }
    
    packets.n64_avail = true;
}

// Create NGC packet from SNES data
void snes_to_ngc() {
    joydata_ngc_raw.one = 1;
    joydata_ngc_raw.zero1 = 0;
    joydata_ngc_raw.zero2 = 0;
    joydata_ngc_raw.zero3 = 0;

    joydata_ngc_raw.z = 0;
    joydata_ngc_raw.start = joydata_snes_raw.start;
    joydata_ngc_raw.l = joydata_snes_raw.l;
    joydata_ngc_raw.r = joydata_snes_raw.r;
    
    joydata_ngc_raw.left_trig = joydata_snes_raw.l ? 255 : 0;
    joydata_ngc_raw.right_trig = joydata_snes_raw.r ? 255 : 0;

    if (!joydata_snes_raw.select) {
        // face buttons map directly
        joydata_ngc_raw.a = joydata_snes_raw.b;
        joydata_ngc_raw.b = joydata_snes_raw.y;
        joydata_ngc_raw.x = joydata_snes_raw.a;
        joydata_ngc_raw.y = joydata_snes_raw.x;
    
        // control stick based on d-pad input
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

        // d-pad unused
        joydata_ngc_raw.pad = 0;

        // c-stick unused
        joydata_ngc_raw.c_x = 128;
        joydata_ngc_raw.c_y = 128;
        
        // z-button unavailable
        joydata_ngc_raw.z = 0;
    }
    else {
        // facebuttons disabled
        joydata_ngc_raw.a = false;
        joydata_ngc_raw.b = false;
        joydata_ngc_raw.x = false;
        joydata_ngc_raw.y = false;
        
        // control stick disabled 
        joydata_ngc_raw.c_x = 128;
        joydata_ngc_raw.c_y = 128;
        
        // d-pad maps to d-pad
        joydata_ngc_raw.dup = joydata_snes_raw.up;
        joydata_ngc_raw.ddown = joydata_snes_raw.down;
        joydata_ngc_raw.dleft = joydata_snes_raw.left;
        joydata_ngc_raw.dright = joydata_snes_raw.right;
        joydata_ngc_raw.joy_x = 128;
        joydata_ngc_raw.joy_y = 128;

        // face buttons map to c-stick
        if (joydata_snes_raw.y) joydata_ngc_raw.c_x -= 127;
        if (joydata_snes_raw.a) joydata_ngc_raw.c_x += 127;
        if (joydata_snes_raw.b) joydata_ngc_raw.c_y -= 127;
        if (joydata_snes_raw.x) joydata_ngc_raw.c_y += 127;
        
        // when holding select and L + R, then press Z on gamecube side
        joydata_ngc_raw.z = joydata_snes_raw.l && joydata_snes_raw.r;
    }

    packets.ngc_avail = true;
}

void n64_to_snes() {
    joydata_snes_raw.a = 0;
    packets.snes_avail = true;
}

// Create NGC packet from N64 data
void n64_to_ngc() {
    joydata_ngc_raw.one = 1;
    joydata_ngc_raw.zero1 = 0;
    joydata_ngc_raw.zero2 = 0;
    joydata_ngc_raw.zero3 = 0;

    joydata_ngc_raw.start = joydata_n64_raw.start;
    joydata_ngc_raw.joy_x = joydata_n64_raw.joy_x - 128;
    joydata_ngc_raw.joy_y = joydata_n64_raw.joy_y - 128;

    joydata_ngc_raw.dup = joydata_n64_raw.dup;
    joydata_ngc_raw.ddown = joydata_n64_raw.ddown;
    joydata_ngc_raw.dleft = joydata_n64_raw.dleft;
    joydata_ngc_raw.dright = joydata_n64_raw.dright;

    joydata_ngc_raw.c_x = 128;
    joydata_ngc_raw.c_y = 128;
    
    if (joydata_n64_raw.l && joydata_n64_raw.r && joydata_n64_raw.z) {
        // disable normal buttons
        joydata_ngc_raw.z = 0;
        joydata_ngc_raw.l = 0;
        joydata_ngc_raw.r = 0;
        joydata_ngc_raw.a = 0;
        joydata_ngc_raw.b = 0;
        
        if (joydata_n64_raw.cright && joydata_n64_raw.cup) joydata_ngc_raw.y = 1;
        if (joydata_n64_raw.cleft && joydata_n64_raw.cdown) joydata_ngc_raw.x = 1;
        if (joydata_n64_raw.a && joydata_n64_raw.b) joydata_ngc_raw.z = 1;        
    }
    else {
        joydata_ngc_raw.l = joydata_n64_raw.l;
        joydata_ngc_raw.r = joydata_n64_raw.r;
        joydata_ngc_raw.a = joydata_n64_raw.a;
        joydata_ngc_raw.b = joydata_n64_raw.b;
        joydata_ngc_raw.z = 0;
        if (joydata_n64_raw.cleft) joydata_ngc_raw.c_x -= 128;
        if (joydata_n64_raw.cright) joydata_ngc_raw.c_x += 127;
        if (joydata_n64_raw.cdown) joydata_ngc_raw.c_y -= 128;
        if (joydata_n64_raw.cup) joydata_ngc_raw.c_y += 127;
    }

    joydata_ngc_raw.left_trig = joydata_ngc_raw.l ? 255 : 0;
    joydata_ngc_raw.right_trig = joydata_ngc_raw.r ? 255 : 0;
    
    packets.ngc_avail = true;    
}

void ngc_to_snes() {
    joydata_snes_raw.a = 0;
    packets.snes_avail = true;
}

// Create N64 packet from NGC data
void ngc_to_n64() {
    joydata_n64_raw.dright = joydata_ngc_raw.dright;
    joydata_n64_raw.dleft = joydata_ngc_raw.dleft;
    joydata_n64_raw.ddown = joydata_ngc_raw.ddown;
    joydata_n64_raw.dup = joydata_ngc_raw.dup;

    joydata_n64_raw.a = joydata_ngc_raw.a;
    joydata_n64_raw.b = joydata_ngc_raw.b;
    
    joydata_n64_raw.start = joydata_ngc_raw.start;
    joydata_n64_raw.z = joydata_ngc_raw.z;
    joydata_n64_raw.l = joydata_ngc_raw.left_trig > 50 || joydata_ngc_raw.l;
    joydata_n64_raw.r = joydata_ngc_raw.right_trig > 50 || joydata_ngc_raw.r;
        
    // deadzone of 20 for center stick
    joydata_n64_raw.joy_x = joydata_ngc_raw.joy_x - 128;
    if (abs(joydata_n64_raw.joy_x) < NGC_JOY_DEADZONE) joydata_n64_raw.joy_x = 0;
    joydata_n64_raw.joy_y = joydata_ngc_raw.joy_y - 128;
    if (abs(joydata_n64_raw.joy_y) < NGC_JOY_DEADZONE) joydata_n64_raw.joy_y = 0;
    
    int8_t cx = joydata_ngc_raw.c_x - 128;
    int8_t cy = joydata_ngc_raw.c_y - 128;
    joydata_n64_raw.cleft = cx < -NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cright = cx > +NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cup = cy > +NGC_CSTICK_THRESHOLD;
    joydata_n64_raw.cdown = cy < -NGC_CSTICK_THRESHOLD;    
        
    packets.n64_avail = true;
}




