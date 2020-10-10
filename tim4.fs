\ tim4.fs STM8 basic timer 4 routines

\res MCU: STM8S103
\res export TIM4_PSCR TIM4_CR1 TIM4_ARR TIM4_CNTR

#require ]B!

: TIM4.prescale ( n -- ) 
    TIM4_PSCR C!
;

: TIM4.upperLimit ( n -- )
    TIM4_ARR C!
;

: TIM4.enableClock ( -- )
    [ 1 TIM4_CR1 0 ]B!
;

: TIM4.init ( prescale interval -- ) \ 2**prescale, eg 4 -> prescale by 2**4 = 16, max 2**7 = 128
    TIM4.upperLimit
    TIM4.prescale
    TIM4.enableClock
;
