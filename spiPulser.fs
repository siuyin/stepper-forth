
\res MCU: STM8S103
\res export SPI_CR1 SPI_CR2 SPI_SR SPI_DR

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

: spInit ( -- )
    dbInit \ initialise debug
    0 spRxOnly \ transmit only
    1 1 spCfg \ enable spi master
;

: spWrite ( b -- )
    SPI_DR C!
;

variable btnSt
variable btnNR \ next run tick
: btnSM ( -- )
    btnSt @ 0= if
        dbPushed? if
            $a5 spWrite
            begin spTxE? until
            $5a spWrite
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
    [ ' main ] literal BG !
    hi
;

' startup 'BOOT !

RAM
