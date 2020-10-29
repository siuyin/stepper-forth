\ accel.fs MMA8451Q accelerometer routines.

#require i2c.fs
\res MCU: STM8S103
\res export I2C_DR

NVM

$1D 2* constant acAddr \ accelerometer address, left-shifted 1 bit to allow for R/W LSB.
$2A constant acCR1 \ control register 1

: acReg ( n -- ) \ prepare to read or write to accelerometer register n.
    begin i2cBusy? not until
    i2cStart
    begin i2cStartSent? until
    acAddr 0 + I2C_DR C! \ 0: LSB of address -> write, 1 -> read
    begin i2cAddrSent? until
    i2cClrAddr
    begin i2cTxEmpty? until

    I2C_DR C! \ send register address
    begin i2cTxEmpty? until
;
: acWake ( -- ) \ wakeup the accelerometer
    i2cInit
    acCR1 acReg \ prepare to write CR1
    3 I2C_DR C! \ send configuration byte: fast read and active.
    begin i2cTxEmptyAndByteTransferred? until
    i2cStop
;

: acxyz ( -- x y z )
    1 acReg

    i2cStart
    begin i2cStartSent? until
    acAddr 1 + I2C_DR C! \ 0: LSB of address -> write, 1 -> read
    begin i2cAddrSent? until
    i2cClrAddr

    i2cACK
    begin i2cRxNotEmpty? until
    I2C_DR C@

    i2cACK
    begin i2cRxNotEmpty? until
    I2C_DR C@

    i2cNAK
    i2cStop
    begin i2cRxNotEmpty? until
    I2C_DR C@
;

RAM
