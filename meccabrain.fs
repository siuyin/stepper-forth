\ meccabrain.fs is code to drive the meccano
\ meccadroid smart devices.

: rightShift ( n pos -- n )
    dup 1 < if
        drop
    else
        1-  \ loop pos times rather than pos+1 times
        for 2/ next
        $ff and \ discard high-byte for unsigned shift right
    then
;

: leftShift ( n pos -- n )
    dup 1 < if
        drop
    else
        1-  \ loop pos times rather than pos+1 times
        for 2* next
    then
;

\ checksum is the high nibble of the high byte.
\ m is the module ID [0,3]
: checksum ( m b1 b2 b3 b4 -- cs )
    + + +           \ sum b4 through b1
    dup 8 rightShift +
    dup 4 leftShift +
    $f0 and         \ select upper-nibble
    or              \ module ID is low-nibble
;
