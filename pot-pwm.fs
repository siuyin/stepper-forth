\ pot-pwm links potentiometer on ADC 3 to PWM on TIM1 channel 4

#require tim1.fs
#require conv.fs
#require led.fs

NVM

: init ( -- )
    16 1023 TIM1.Init
    TIM1.PWM4.Init
    LED.Init
;

: task ( -- )
    3 conv dup TIM1.PWM4
    dup 512 < not if
        LED.On
    else dup 507 < if
        LED.Off
    then then
    drop
;

: startPotPWM ( -- )
    ." linking pot on ADC 3 to TIM.PWM ch 4"
    init
    [ ' task ] literal BG !
    hi
;

' startPotPWM 'BOOT !

RAM
