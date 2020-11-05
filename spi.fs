\ spi.fs SPI routines

#require debug.fs

#require ]B!
#require ]B?

\res MCU: STM8S103
\res export SPI_CR1 SPI_CR2 SPI_SR
\res export SPI_DR

NVM

: spSSlMgt ( n -- ) \ software slave management
    0= if
        [ 0 SPI_CR2 1 ]B! \ 0=hardware nSS
    else
        [ 1 SPI_CR2 1 ]B! \ 1=software nSS
    then
;
: spSlSInt ( n -- ) \ slave select internal
    0= if
        [ 0 SPI_CR2 0 ]B! \ slave mode
    else
        [ 1 SPI_CR2 0 ]B! \ master mode
    then
;

: sp1WBiDi ( 1W/n2W ) \ one wire bidirectional mod
    0= if
        [ 0 SPI_CR2 7 ]B! 
    else
        [ 1 SPI_CR2 7 ]B! 
    then
;
: spRxOnly ( rxOnly/nTxRx -- )
    0= if
        [ 0 SPI_CR2 2 ]B! \ 1 = set Rx only, 0 = both Tx and Rx
    else
        [ 1 SPI_CR2 2 ]B!
    then
;

: spWrite ( b -- )
    SPI_DR C!
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
