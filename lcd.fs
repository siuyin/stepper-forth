\ lcd1602.fs provides routines for 16 char x 2 line hitachi based LCD.
\ 
\ PA1 RS (register select: 0=Instruction 1=Data)
\ PA2 En 
\ PC4..7 D4..7
\ R/nW set to write by grounding pin


\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1
\res export PC_ODR PC_DDR PC_CR1

#require ]B!

NVM

: _portInit ( -- )
    [ 1 PA_DDR 1 ]B! \ set PA1 to output
    [ 1 PA_CR1 1 ]B! \ set PA1 to push-pull

    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull

    $f0 PC_DDR C! \ sets PC4..7 to output
    $f0 PC_CR1 C! \ sets PC4..7 to push-pull
;
: _en ( n -- ) \ enable
    0= if
        [ 0 PA_ODR 2 ]B!
    else
        [ 1 PA_ODR 2 ]B!
    then
;
: _rs ( n -- ) \ Register Select: 0=Instruction, 1=Data
    0= if
        [ 0 PA_ODR 1 ]B!
    else
        [ 1 PA_ODR 1 ]B!
    then
;
: dly for i drop next ; \ each loop is about 6 microseconds
: _enPulse ( -- ) \ pulse enable line 0 to 1 to 0
    0 _en 1 dly
    1 _en 1 dly
    0 _en 9 dly \ requires > 37 microseconds to settle
;
: _wNbl ( nb -- ) \ writes nybble d4..7
    $f0 and PC_ODR C!
    _enPulse
;
: _uNbl ( b -- nb ) \ select uppber nybble
    $f0 and
;
: _lNbl ( b -- nb ) \ places lower nybble in bit4..7
    $0f and
    3 for 2* next
;
: _w2Nbl ( b -- ) \ write byte upper nybble first
    dup _uNbl
     _wNbl

    _lNbl
    _wNbl
;
: _wByt ( b -- ) \ write byte
    PC_ODR C!
    _enPulse
;

20 constant wt
: lc1602 ( -- ) \ initialize 4 bit mode 2 line display
    0 _rs \ instruction
    0 _en
    $20 _wNbl \ set 4 bit mode
    wt dly

    $28 _w2Nbl \ set >1 line, 5x8 font.
    wt dly

    $0e _w2Nbl \ display on, cursor on
    wt dly

    $06 _w2Nbl \ increment address, shift cursor, display not shifted
    wt dly

;
: >= < not ;
: _wt5ms (  -- )
        tim
        begin
            dup
            tim swap - 1 >=  \ wait at least 1 tim tick (5ms) which is > required 4.2ms
        until
        drop
;
: lcRst ( -- ) \ software reset the LCD
    0 _rs
    0 _en
    2 for \ do this 3 times
        $30 _wByt
        _wt5ms
    next
;
: lcClr ( -- ) \ clears display
    0 _rs
    1 _w2Nbl
    _wt5ms
;
: lcWrite ( b -- ) 
    1 _rs
    _w2Nbl
;

RAM
