\ spi.fs SPI routines

#require debug.fs

#require ]B!
#require ]B?

\res MCU: STM8S103
\res export SPI_CR1 SPI_CR2 SPI_SR

NVM

: spRxOnly ( rxOnly/nTxOnly -- )
    \ [ 0 SPI_CR2 7 ]B! \ non-biredirectional mode (default after reset)
    0= if
        [ 0 SPI_CR2 2 ]B! \ 1 = set rx only, 0 = tx only
    else
        [ 1 SPI_CR2 2 ]B!
    then
;

: spRxnE? ( -- flag ) \ recieve buffer not empty?
    [ SPI_SR 0 ]B?
;
: spTxE? ( -- flag ) \ transmit buffer empty?
    [ SPI_SR 1 ]B?
;
: spBsy? ( -- flag ) \ SPI busy?
    [ SPI_SR 7 ]B?
;
: spIdle? ( -- flag )
    spRxnE?
    spTxE?
    spBsy? and and
;
: spMst ( master/nSlave -- )
    0= if
        [ 0 SPI_CR1 2 ]B! \ configure for slave
    else
        [ 1 SPI_CR1 2 ]B! \ configure for master
    then
;
: spEn ( enable/nDisable -- )
    0= if
        [ 0 SPI_CR1 6 ]B! \ disable SPI
    else
        [ 1 SPI_CR1 6 ]B! \ enable SPI
    then
;
: spCfg ( enable/nDisable master/nSlave -- ) \ configure SPI
    $30 SPI_CR1 C! \ MSB first, 125kHz clock, CPol=0,CPha=0, disabled, slave
    spMst
    spEn
;

RAM
