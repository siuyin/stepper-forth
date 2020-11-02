\ i2cRxPulse.fs receives a pulse from an I2C master.

\res MCU: STM8S103
\res export I2C_CR1 I2C_CR2 I2C_OARL I2C_OARH
\res export I2C_DR


NVM

: startup
    cr ." running startup"
    $20 i2cSetOwnAddr
    i2cInit
    i2cStop \ relese I2C bus
    i2cACK
    i2cItEvEn
    i2cItBufEn
    hi
;


' startup  'BOOT !

RAM

