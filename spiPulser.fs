
\res MCU: STM8S103
\res export SPI_CR1 SPI_CR2 SPI_SR SPI_DR
\res export PA_ODR PA_DDR PA_CR1


#require spi.fs


NVM

: spDump ( -- )
    hex
    cr
    SPI_CR1 C@ . cr
    SPI_CR2 C@ . cr
    SPI_SR C@ . cr
    decimal
;

: SS ( n -- )
    0= if
        [ 1 PA_ODR 1 ]B! \ slave not selected
    else
        [ 0 PA_ODR 1 ]B! \ slave selected
    then
;
: SSInit ( -- )
    [ 1 PA_DDR 1 ]B! \ set PA1 to output
    [ 1 PA_CR1 1 ]B! \ set PA1 to push-pull output
    0 SS
;
: spInit ( -- )
    dbInit \ initialise debug
    \ 0 spRxOnly \ transmit only
    1 1 spCfg \ enable spi master
;

: txDly ( -- )
    2 for i drop next
    begin spTxE? until
;
variable btnSt
variable btnNR \ next run tick
: btnSM ( -- )
    btnSt @ 0= if
        dbPushed? if
            1 SS
            $a5 spWrite
            txDly
            $5a spWrite
            txDly
            dbH
            1 btnSt ! \ pushed
        then
    else btnSt @ 1 = if
        dbPushed?  if
        else
            2 btnSt ! \ may be released
        then
    else btnSt @ 2 = if
        dbPushed?  if
            1 btnSt ! \ still pushed
        else
            0 SS \ release slave
            dbL
            0 btnSt ! \ released
        then
    then then then
;

variable btnCnt
: updateBtnCnt ( -- )
    0 btnCnt @ < if
        -1 btnCnt +!
    then
;
: btnChkNow? ( -- flag )
    btnCnt @ 0= 
;
: btnChk ( -- )
    updateBtnCnt
    btnChkNow? if
        4 btnCnt !
        btnSM
    then
;
: main
    btnChk
;

: startup ( -- )
    cr ." start spi"
    spInit
    SSInit \ initialise slave select line
    [ ' main ] literal BG !
    hi
;

' startup 'BOOT !

RAM
