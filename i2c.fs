\ i2c.fs i2c routines

#require ]B!
#require ]B?

\res MCU: STM8S103
\res export I2C_FREQR I2C_CR1 I2C_CR2 I2C_DR
\res export I2C_SR1 I2C_SR2 I2C_SR3
\res export I2C_TRISER I2C_CCRL

\res export PA_ODR PA_DDR PA_CR1 PA_IDR

NVM

\ debugging routines
: dbInit ( -- )
    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull output
;
: dbH ( -- ) \ set pin High
    [ 1 PA_ODR 2 ]B!
;
: dbL [ 0 PA_ODR 2 ]B! ;

: i2cEn ( -- )
    [ 1 I2C_CR1 0 ]B! \ enable I2C
;
: i2cRiseTimeSet ( -- )
    17 I2C_TRISER C! \ Given a 16MHz peripheral clock, configure I2C rise time for a maximum of 1000ns. (17 = 16+1)
;
: i2cSpeedSet ( -- )
    $50 I2C_CCRL C! \ Given a 16MHz peripheral clock, configure I2C to Standard mode, 100kHz SCL.
;
: i2cPeriphClkInit ( -- )
    16 I2C_FREQR C! \ tell the I2C system that it clocked from a 16MHz master clock source.
;

: i2cInit ( -- )
    dbInit
    i2cPeriphClkInit
    i2cSpeedSet
    i2cRiseTimeSet
    i2cEn
;

: i2cStart ( -- )
    [ 1 I2C_CR2 0 ]B! \ triggers Start condition when in Slave mode (default after reset). Triggers Restart condition in Master mode (default after Start issued). This bit is cleared by hardware when Start is sent or when PE = 0 (Peripheral Enable = 0).
;
: i2cACK ( -- ) \ set Acknowlege bit
    [ 1 I2c_CR2 2 ]B!
;
: i2cNAK ( -- ) \ set Not Acknowlege bit
    [ 0 I2c_CR2 2 ]B!
;
: i2cStop ( -- )
    [ 1 I2C_CR2 1 ]B! \ triggers Stop condition after current byte is transferred or after Start condition is sent. Cleared by hardware when a Stop condition is detected.
;
: i2cTxEmpty? ( -- flag )
    [ I2C_SR1 7 ]B?
;
: i2cRxNotEmpty? ( -- flag )
    [ I2C_SR1 6 ]B?
;
: i2cByteTransferred? ( -- flag )
    [ I2C_SR1 2 ]B?
;
: i2cTxEmptyAndByteTransferred? ( -- flag )
    I2C_SR1 C@ $84 and $84 =
;
: i2cTxDone? ( -- flag )
    [ I2C_SR1 2 ]B?
;
: i2cBusy? ( -- flag )
    [ I2C_SR3 1 ]B?
;
: i2cAddrSent? ( -- flag )
    [ I2C_SR1 1 ]B?
;
: i2cClrAddr ( -- )
    I2C_SR3 C@ drop \ to clear ADDR, first read SR1 (see above) then read SR3.
;
: i2cStartSent? ( -- flag )
    [ I2C_SR1 0 ]B?
;
: i2cStopped? ( -- flag )
    [ I2C_SR1 4 ]B?
;


RAM


