\ timing.fs time measurement experiments
\ Experimental results: for 500 us pulse, I got pulseMeas of 64 or 65.
\ This is running from flash.
\ With pulseMeas running from RAM, I got 52. Running from RAM is slower.
\ Each loop is approximately 122 16MHz periods or 7.625us.

#require beep.fs
\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1 PA_IDR

#require ]B!
#require ]B?

NVM

: init ( -- )
    beep.Init
    beep.On
    [ 0 PA_DDR 1 ]B! \ set PA1 to input
    [ 1 PA_CR1 1 ]B! \ set PA1 pull-up high on open-drain
;

: pinH? ( -- flag )
    [ PA_IDR 1 ]B?
;

: highDone ( -- )
    begin
        pinH? not
    until
;

: lowDone ( -- )
    begin
        pinH?
    until
;

: pulseMeas ( -- n )
    -1 \ push initial count onto the stack
    highDone 
    lowDone
    begin \ pin is how high
        1+ \ increment number on stack
        pinH? not \ until pin is low
    until
;

RAM

