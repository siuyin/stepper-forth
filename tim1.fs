\res MCU: STM8S103
\res export TIM1_PSCRH TIM1_ARRH TIM1_CR1 TIM1_CCMR3 TIM1_CCER2 TIM1_BKR TIM1_CCR3H TIM1_CNTRH
#require ]B!

NVM

\ The counter clock frequency is calculated as follows:
\ f CK_CNT = f CK_PSC /(PSCR[15:0]+1)
: TIM1.prescale ( n -- )
    TIM1_PSCRH 2C!
    ;

: TIM1.upperLimit ( n -- ) 
    TIM1_ARRH 2C!
    ;

: TIM1.enableClock ( -- ) 
    [ 1 TIM1_CR1 0 ]B!
    ;
    
: TIM1.init ( prescale interval -- )
    TIM1.upperLimit
    TIM1.prescale
    TIM1.enableClock
    ;

: TIM1.pwm3.init ( -- )
    $68 TIM1_CCMR3 C! \ set timer 1 channel 3 to PWM mode 1
    [ 1 TIM1_CCER2 0 ]B! \ enable channel 3 output
    [ 1 TIM1_BKR 6 ]B! \ AOE: Automatic Output Enable set
    ;
    
: TIM1.pwm3 ( dutyCycle -- )
    0 TIM1_CNTRH 2C! \ reset counter to zero
    TIM1_CCR3H 2C! \ set pwm duty cycle
    ;

: PWM3.Blink ( -- )
    16 9999 TIM1.init
    TIM1.pwm3.init
    1999 TIM1.pwm3
    ;
    
RAM
