\ spiRcvIntr.fs SPI slave receiver interrupt vector initialisation

RAM

#require spi.fs
#require :NVM
#require ]B?
#require ]B!

\res MCU: STM8S103
\res export INT_SPI SPI_DR

NVM
variable dat
RAM

:NVM
    savec

    dbH
    SPI_DR C@ dat !

    iret
;NVM INT_SPI !

RAM
