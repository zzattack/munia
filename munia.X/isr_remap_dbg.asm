// This file is only included only in debug builds
// Non-debug builds contain the bootloader which also defines this
    PSECT HiVector,class=CODE,delta=1,abs
    org 0x08
    goto 0x1008
    
    end