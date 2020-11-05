\ spiRcv.fs receives pulses from spiPulser.
\ Turn on (dbH) LED when data is recived.
\ The last byte is stored in variable 'dat' .



\res MCU: STM8S103
\res export SPI_CR1 SPI_CR2 SPI_SR SPI_DR
\res export SPI_ICR
\res export PA_DDR PA_CR1

#require spiRcvIntr.fs
#require ]B!

NVM

: spDump ( -- )
    hex
    cr
    SPI_CR1 C@ . cr
    SPI_CR2 C@ . cr
    SPI_SR C@ . cr
    cr
    SPI_DR C@ . cr
    decimal
;

: RxIntEn ( -- )
    [ 1 SPI_ICR 6 ]B!
;

: spInit ( -- )
    cr ." Initialising SPI"
    dbInit \ init debug

    \ good practice as recommended in the datasheet section 20.3.2 / note.
    [ 0 PA_DDR 3 ]B! \ set PA3 to input
    [ 1 PA_CR1 3 ]B! \ set PA3 to pull-up

    1 0 spCfg \ enabled slave

    RxIntEn \ enable receive interrupt

    hi
;

' spInit 'BOOT !

RAM
