\ debug.fs debug routines using pins PA2 (LED active high) and PD3 (button input active low)

#require ]B!
#require ]B?

\res MCU: STM8S103

\res export PA_ODR PA_DDR PA_CR1 PA_IDR
\res export PD_IDR PD_DDR PD_CR1


NVM

\ debugging routines
: dbInit ( -- )
    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull output

    [ 0 PD_DDR 3 ]B! \ set PD3 to input
    [ 1 PD_CR1 3 ]B! \ set PD3 to pull-up
;
: dbH ( -- ) \ set pin High
    [ 1 PA_ODR 2 ]B!
;
: dbL [ 0 PA_ODR 2 ]B! ;
: dbTgl ( -- ) PA_ODR C@ 4 XOR PA_ODR C! ; \ 4 is 1 << 2
: dbPushed? ( -- flag )
    [ PD_IDR 3 ]B? not
;

RAM
