\ Interrupt vector installation

NVM
variable pingDat
variable pingAddr
RAM

#require :NVM
#require ]B?
#require ]B!

\res MCU: STM8S103
\res export INT_I2C
\res export I2C_SR1 I2C_SR3
\res export I2C_CR2
\res export I2C_DR


:NVM
    savec
    
    i2cStartSent? if
        pingAddr @ 2* 0 + I2C_DR C!
    then

    [ I2C_SR1 1 ]B? \ address sent or matched interrupt?
    if
        i2cClrAddr
        pingDat @ I2C_DR C!
        i2cStop
    then

    iret
;NVM INT_I2C !


RAM
