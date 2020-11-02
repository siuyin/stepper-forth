\ Interrupt vector installation

RAM

#require i2c.fs
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
    [ I2C_SR1 1 ]B? \ address matched interrupt?
    if
        i2cClrAddr
    then

    i2cRxNotEmpty? if \ byte received
        i2cACK
        I2C_DR C@  0= if
            dbL
        else
            dbH
        then
    then

    i2cStopped? if
        [ 1 I2C_CR2 1 ]B! \ set STOP to release SDA and SCL
    then
    iret
;NVM INT_I2C !


RAM
