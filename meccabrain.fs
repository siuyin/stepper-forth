\ meccabrain.fs is code to drive the meccano
\ meccadroid smart devices.

\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1

#require ]B!

NVM
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

\ InitPin initialises PA1 (pin 5) to be a push-pull output.
: initPin ( -- )
    [ 0 PA_CR1 1 ]B! \ set PA1 to pseudo open-drain on output, no pull-up on input
;
: pinH ( -- )
    [ 1 PA_ODR 1 ]B! \ set idle state to high
;
: pinL ( -- )
    [ 0 PA_ODR 1 ]B!
;
: pinOut ( -- )
    [ 1 PA_DDR 1 ]B! \ set PA1 to output
    pinH
;
: pinIn ( -- )
    [ 0 PA_DDR 1 ]B! \ set PA1 to input
;


262 constant bitDelay

: dly ( n -- ) \ simple busy loop delay
    for next
;

: sendBit ( n -- ) 
    0= if
        pinL
    else
        pinH
    then
    bitDelay dly
;

: sendByte ( n -- )
    pinOut
    0 sendBit \ low start bit
    7 for
        dup
        1 and
        sendBit
        2/
    next
    drop 
    1 sendBit \ two high stop bits
    1 sendBit
;

RAM
