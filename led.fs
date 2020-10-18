\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1
#require ]B!

NVM
\ LED on/off at PA2 (pin 6)
\ LED PWM at PC4 (pin 14)
: LED.init ( -- )
    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull
;

: LED.on ( -- ) [ 1 PA_ODR 2 ]B! ;
: LED.off ( -- ) [ 0 PA_ODR 2 ]B! ;
: LED.toggle ( -- ) PA_ODR C@ 4 XOR PA_ODR C! ; \ 4 is 1 << 2

RAM
