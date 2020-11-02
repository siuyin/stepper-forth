\ i2cRxPulse.fs receives a pulse from an I2C master.

\res MCU: STM8S103
\res export I2C_CR1 I2C_CR2 I2C_OARL I2C_OARH
\res export I2C_DR

#require i2cRxIntr.fs

NVM

: startup
    cr ." running startup"
    $20 i2cSetOwnAddr
    i2cInit
    i2cItEvEn
    i2cItBufEn
    i2cACK
    hi
;


' startup  'BOOT !

RAM

