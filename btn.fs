\ btn.fs configures push button on PC3

\res MCU: STM8S103
\res export PC_IDR PC_DDR PC_CR1
#require ]B!
#require led.fs

NVM

: BTN.Init ( -- )
    [ 0 PC_DDR 3 ]B! \ set PC3 to input
    [ 1 PC_CR1 3 ]B! \ set PCe pull-up high on open-drain
;

: BTN.Pushed? ( -- flag )
   PC_IDR C@
   8 and
   8 = not  \ active low
;

variable BTN.state
variable BTN.cnt \ next run tick. TIM ticks every 5ms

: resetBTNCnt ( ticks -- )
    BTN.cnt !
;

: ToggleLEDInit ( -- )
    BTN.Init
    LED.Init
    0 BTN.state !
    0 resetBTNCnt
;

: updateBTNCnt ( -- )
    0 BTN.cnt @ < if
        -1 BTN.cnt +!
    then
;

: btnRunNow? ( -- flag )
    BTN.cnt @ 0= 
;


: btnStateMachine ( -- )
    BTN.state @ 0= if \ released
        BTN.Pushed? if
            LED.Toggle
            1 BTN.state !
        else
        then
    else BTN.state @ 1 = if \ pushed
        BTN.Pushed? if
        else
            2 BTN.state !
        then
    else BTN.state @ 2 = if \ maybe released
        BTN.Pushed? if
            1 BTN.state !
        else
            0 BTN.state !
        then
    then then then
;
: ToggleLED ( -- )
    updateBTNCnt

    btnRunNow? if
        4 resetBTNCnt
        btnStateMachine
    then
;

RAM
