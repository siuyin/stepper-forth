\ meccabrain.fs is code to drive the meccano
\ meccadroid smart devices.
\ 
\ Each Meccano Smart Module must be initialized and operational
\ before it will relay commands down the daisy-chain.
\ 
\ $FE (hex FE) is the discovery command.
\ $FC is the report device type command.
\ 
\ $FA and below are user commands.
\ Eg. $FA for a Smart Servo commands it into Learned Intelligent Movement mode.

\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1 PA_IDR

#require ]B!
#require ]B?

NVM

\ debugging routines
: dbInit ( -- )
    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull output
;
: dbH ( -- ) \ set pin High
    [ 1 PA_ODR 2 ]B!
;
: dbL [ 0 PA_ODR 2 ]B! ;


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
: pinH? ( -- flag )
    [ PA_IDR 1 ]B?
;
: highDone ( -- )
     tim ( -- t1 )
    begin
        dup ( t1 -- t1 t1 )
        pinH? not ( t1 -- t1 flag )
        swap ( t1 flag -- flag t1 )
        tim swap - 3 < not \ true if t2 - t1 >= n x 5ms
        or
    until
    drop
;
: lowDone ( -- )
    tim ( -- t1 )
    begin
        dup ( t1 -- t1 t1 )
        pinH? ( t1 -- t1 flag )
        swap ( t1 flag -- flag t1 )
        tim swap - 3 < not \ true if t2 - t1 >= n x 5ms ( flag1 t1 -- flag1 flag2 ) 
        or
    until
    drop
;


251 constant bitDelay

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

: >= 
    < not
;
: ifTimeoutReplaceWith0 ( n1 -- n2 )
    dup 1000 >= if drop 0 then
;

\ pulseMeas measures the duration of a high-pulse
\ in 5ms ticks from the sytem timer, TIM.
: pulseMeas ( -- n )
    highDone
    lowDone
    -1 \ push initial loop count
    begin \ pin is now high
        1+ ( t n -- t n+1 )
        dup 1000 >=
        pinH? not \ until pin is low
        or
    until
    ifTimeoutReplaceWith0
    \ the loop count is left on the stack as output.
;

\ highPulse? returns true if it detects is a high-pulse with duration >= 400 microseconds
\ WARNING: The following only applies to a tight loop: This corresponds to 400 / 7.625 = 53  loop counts approximately.
\ With a check the number should be the average of 9 and 32 approx 21.
: highPulse? ( -- flag )
    pulseMeas
    21 >=
;

\ ReadByte reads a byte from PA1 and puts it on the stack.
variable tmp
: ReadByte ( -- n )
    pinIn
    \ 942 dly \ delay for 1.5ms
    0 tmp !
    1
    7 for
        dup ( 1 -- 1 1 )
        highPulse? if
            tmp @ or tmp ! ( 1 1 -- 1 )
        else
            drop ( 1 1 -- 1 )
        then
        2* ( 1 -- 2 )
    next
    drop
    tmp @ \ push value of tmp on to the stack as output
;


variable outBytes 1 2* allot
: setDiscover ( -- )
    4 0 do
        $fe outBytes I + c!
    loop
;
: sob ( byte offset -- ) \ set output byte
    outBytes + c!
;
: sendOutBytes ( -- )
    4 0 do
        outBytes I + C@ sendByte
    loop
;
: pushOutBytes ( -- b4 b3 b2 b1 )
    4 0 do
        outBytes I + C@
    loop
;


: computeChecksum ( n -- n )
    pushOutBytes
    checksum
;
: cs1 ( n -- n )
    pushOutBytes + + +
    dup 8 rightShift +
    dup 4 leftShift +
    $f0 and
    or
;

: targetNode ( n -- )
    $ff sendByte \ header
    sendOutBytes
    cs1 sendByte
;
: targetNodeReadByte ( n -- n ) \ target node and read byte
    targetNode
    ReadByte
;

: commonSetup
    dbInit
    initPin
    pinH
    setDiscover
;

\ discover initializes a Smart Module at position n.
\ returns module type 1 for Server 2 for LED.
: discover ( n -- n )
    dup $FE swap sob \ put $FE in byte n
    dup targetNodeReadByte
    $FE = if
        dup $FC swap sob \ put $FC in byte n
        dup targetNodeReadByte
    then
;

RAM
