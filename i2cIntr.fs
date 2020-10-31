\ Interrupt vector installation

RAM

\res MCU: STM8S103
#require :NVM
#require ]B?

\res export INT_I2C
\res export I2C_SR1 I2C_SR3

:NVM
    savec
    [ I2C_SR1 1 ]B? \ address matched interrupt?
    if
        i2cClrAddr
    then

    i2cStopped? if
        dbH \ turn on LED
    then
    iret
;NVM INT_I2C !


RAM
