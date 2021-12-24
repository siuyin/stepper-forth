\res MCU: STM8S103
#require MARKER
#require ]B!

MARKER END\

\ The counter clock frequency is calculated as follows:
\ f CK_CNT = f CK_PSC /(PSCR[15:0]+1)
\res export TIM1_PSCRH   \ TIM1 prescaler register high
NVM
: TIM1.prescale ( n -- )
    TIM1_PSCRH 2C!
    ;
RAM

\res export TIM1_ARRH    \ TIM1 auto-reload register high
NVM
: TIM1.upperLimit ( n -- ) 
    TIM1_ARRH 2C!
    ;
RAM

\res export TIM1_CR1     \ TIM1 control register 1
NVM
: TIM1.clkEn ( -- )	\ timer 1 clock enable
    [ 1 TIM1_CR1 0 ]B!
    ;

: TIM1.clkDis ( -- )	\ timer 1 clock disable
  [ 0 TIM1_CR1 0 ]B!
  ;
    
: TIM1.init ( prescale interval -- )
    TIM1.upperLimit
    TIM1.prescale
    TIM1.clkEn
    ;
RAM

\res export TIM1_CCMR3   \ TIM1 capture/compare mode register 3
\res export TIM1_CCER2   \ TIM1 capture/compare enable register 2
\res export TIM1_BKR     \ TIM1 break register
NVM
: TIM1.pwm3.init ( -- )
    $68 TIM1_CCMR3 C! \ set timer 1 channel 3 to PWM mode 1
    [ 1 TIM1_CCER2 0 ]B! \ enable channel 3 output
    [ 1 TIM1_BKR 6 ]B! \ AOE: Automatic Output Enable set
    ;
RAM
    
\res export TIM1_CNTRH   \ TIM1 counter high
\res export TIM1_CCR3H   \ TIM1 capture/compare register 3 high
NVM
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

\res export TIM1_CCMR4   \ TIM1 capture/compare mode register 4
\res export TIM1_CCER2   \ TIM1 capture/compare enable register 2
\res export TIM1_BKR     \ TIM1 break register
NVM
: TIM1.pwm4.init ( -- )
    $68 TIM1_CCMR4 C! \ set timer 1 channel 4 to PWM mode 1
    [ 1 TIM1_CCER2 4 ]B! \ enable channel 4 output
    [ 1 TIM1_BKR 6 ]B! \ AOE: Automatic Output Enable set
    ;
RAM
    
\res export TIM1_CNTRH   \ TIM1 counter high
\res export TIM1_CCR4H   \ TIM1 capture/compare register 4 high
NVM
: TIM1.pwm4 ( dutyCycle -- )
    0 TIM1_CNTRH 2C! \ reset counter to zero
    TIM1_CCR4H 2C! \ set pwm duty cycle
    ;

: PWM4.Blink ( -- )
    16 9999 TIM1.init
    TIM1.pwm4.init
    1999 TIM1.pwm4
    ;
RAM
END\

\\ Content ignored below this line because of the starting \\.

: someExampleCode ( -- n ) 1 ;
