\ intr.fs interrupt experiments


\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1
\res export PC_IDR PC_DDR PC_CR1 PC_CR2
\res export TIM4_PSCR TIM4_CR1 TIM4_ARR TIM4_CNTR
\res export EXTI_CR1

#require ]B!

NVM

: LED.on ( -- ) [ 1 PA_ODR 2 ]B! ;
: LED.off ( -- ) [ 0 PA_ODR 2 ]B! ;
: LED.toggle ( -- ) PA_ODR C@ 4 XOR PA_ODR C! ; \ 4 is 1 << 2

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

: init ( -- )
    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull

    [ 0 PC_DDR 3 ]B! \ set PC3 to input
    [ 1 PC_CR1 3 ]B! \ set PC3 pull-up high on open-drain
    [ 1 PC_CR2 3 ]B! \ enable interrupt on PC3
    $20 EXTI_CR1 C!  \ configure interrupt on port c for falling edge

    6 $FF TIM4.init \ run timer 4 with a 4 microsecond clock period, auto-reload at 255 .
;

RAM

#require :NVM
#require ]B?

\res export INT_EXTI2


:NVM
    savec
    [ PC_IDR 3 ]B? not if
        LED.On
    then
    iret
;NVM INT_EXTI2 !


RAM
