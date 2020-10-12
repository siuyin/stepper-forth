\ tim4.fs STM8 basic timer 4 routines

RAM

\res MCU: STM8S103
\res export TIM4_PSCR TIM4_CR1 TIM4_ARR TIM4_CNTR

#require ]B!

NVM

: TIM4.prescale ( n -- ) 
    TIM4_PSCR C!
;

: TIM4.upperLimit ( n -- )
    TIM4_ARR C!
;

: TIM4.enableClock ( -- )
    [ 1 TIM4_CR1 0 ]B!
;

\ The counter clock frequency is calculated as follows:
\ f CK_CNT = f CK_PSC /2 ** (PSCR[2:0])
\ 
\ Max interval is 255.
: TIM4.init ( prescale interval -- )
    TIM4.upperLimit
    TIM4.prescale
    TIM4.enableClock
;

\ tick is a 1ms tick counter
variable tick

RAM

#require :NVM

\res export INT_TIM4 TIM4_IER TIM4_SR


:NVM
    savec
    [ 0 TIM4_SR 0 ]B! \ clear update interrupt flag
    1 tick +!
    iret
;NVM ( xt ) INT_TIM4 !

\ TIM4.intrEn enables timer 4 interrupts.
\ 1ms tick.
: TIM4.intrEn ( -- )
    6 250 TIM4.init
    $41 TIM4_IER C!
;



RAM
