\ Interrupt vector installation

NVM
variable pingDat
variable pingAddr
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

    i2cAckFErr? if
        i2cClrAF
        i2cStop
    then
    
    i2cStartSent? if
        pingAddr @ 2* 0 + I2C_DR C!
    then

    i2cAddrSent? if \ address sent or matched interrupt?
        i2cClrAddr
        pingDat @ I2C_DR C!
        i2cStop
    then

    i2cStopped? if
        I2C_CR2 C@ I2C_CR2 C! \ clear stop flag
    then


    iret
;NVM INT_I2C !


RAM
