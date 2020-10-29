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
: acWake ( configByte -- ) \ wakeup the accelerometer
    i2cInit
    acCR1 acReg \ prepare to write CR1
    I2C_DR C! \ send configuration byte: fast read and active.
    begin i2cTxEmptyAndByteTransferred? until
    i2cStop
;

: acxyz ( n -- x y z ) \ n = 1 or 4 for 3 * 8-bit or 3 * 14-bit (MSB, LSB) outputs respectively
    1 acReg

    i2cStart
    begin i2cStartSent? until
    acAddr 1 + I2C_DR C! \ 0: LSB of address -> write, 1 -> read
    begin i2cAddrSent? until
    i2cClrAddr

    for \ 3-1-1 = 1 or 6-1-1 = 4
        i2cACK
        begin i2cRxNotEmpty? until
        I2C_DR C@
    next

    i2cNAK
    i2cStop
    begin i2cRxNotEmpty? until
    I2C_DR C@
;

variable x
variable y
variable z

: 14pack ( msb lsb -- n )
    2/ 2/ \ shift right 2 bits
    $3F and \ mask off bits [7:6]
    swap
    5 for 2* next \ shift left 6 bits
    +
;

: 14b ( xx yy zz -- )
    14pack z !
    14pack y !
    14pack x !
;

: 14sign ( n -- signed )
    dup $2000 and 0= not if
        $1fff and
        $2000 swap -
        negate 
    then
;
: show ( -- )
    x  @ 14sign .
    y  @ 14sign .
    z  @ 14sign .
;



: 14bitShow ( -- )
    4 acxyz
    14b
    show
;

: 8sign ( n -- signed )
    dup $80 and 0= not if
        $7f and
        $80 swap -
        negate 
    then
;
: 8bitShow ( -- )
    1 acxyz
    z !
    y !
    x !

    x @ 8sign .
    y @ 8sign .
    z @ 8sign .
;

: ac8Init ( -- )
    3 acWake
;

: ac14Init ( -- )
    1 acWake
;

RAM
