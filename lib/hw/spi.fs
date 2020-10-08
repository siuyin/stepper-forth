\ STM8S103 SPI
\ derived from al177/stm8ef/HC12/boardcore.inc
\ refer to github.com/TG9541/stm8ef/blob/master/LICENSE.md

#require ]C!
#require ]B!

\ Init and enable SPI
: SPIon ( baud -- )
  [
    $5C C,              \  INCW    X         ; pull baud
    $F6 C,              \  LD      A,(X)
    $5C C,              \  INCW    X
    $A407 ,             \  AND     A,#7      ; CPOL=CPHA=0
    $4E C,              \  SWAP    A         ; 16 *
    $47 C,              \  SRA     A         ; 2 /
    $AA04 ,             \  OR      A,#4      ; set master mode
    $C7 C, SPI_CR1 , ]  \  LD      SPI_CR1,A
  [ $01 SPI_CR2 ]C!     \ no NSS, FD, no CRC
  [ 1 SPI_CR1 6 ]B!    \ SPI enable
;

\ disable SPI
: SPIoff ( -- )
  [ 0 SPI_CR1 ]C!    \ disable SPI
;

\ Perform SPI byte cycle with result c
: SPI ( c -- c)
  [ $E601 ,                 \ LD A,(1,X)
    $C7 C,  SPI_DR ,        \ LD SPI_DR,A
    $7201 , SPI_SR , $FB C, \ BTJF SPI_SR,#SPIRXNE_WAIT (0)
    $C6 C,  SPI_DR ,        \ LD A,SPI_DR
    $E701 ,                 \ LD (1,X),A
    $7F C, ]                \ CLR  (X)
;

\\ Example:

\ interactive SPI test

\ non-e4thcom / codeload.py: remove the following lines...
\res MCU: STM8S103
\res export SPI_CR1
\res export SPI_CR2
\res export SPI_SR
\res export SPI_DR

\ ... and uncomment the CONSTANT definitions
\ $5200 CONSTANT SPI_CR1
\ $5201 CONSTANT SPI_CR2
\ $5203 CONSTANT SPI_SR
\ $5204 CONSTANT SPI_DR

NVM

\ non-e4thcom / codeload.py: include file contents here
\#include hw/spi.fs

RAM

1 SPIon   \  clock SPI = click core / 2

\ loopback: connect pins PC6/MOSI and PC7/MISO)
165 SPI . \ 165
SPIoff
