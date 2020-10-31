\ Interrupt vector installation

RAM

\res MCU: STM8S103
#require :NVM
#require ]B?
#require ]B!

\res export INT_I2C
\res export I2C_SR1 I2C_SR3
\res export I2C_CR2

:NVM
    savec
    [ I2C_SR1 1 ]B? \ address matched interrupt?
    if
        dbH
        i2cClrAddr
    then

    i2cStopped? if
        dbL
        [ 1 I2C_CR2 1 ]B! \ set STOP to release SDA and SCL
    then
    iret
;NVM INT_I2C !


RAM
