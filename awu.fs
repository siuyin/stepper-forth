RAM
#require :NVM

\res MCU: STM8S103
\res export AWU_APR AWU_TBR AWU_CSR1 INT_AWU

DECIMAL

:NVM              \ interrupt handler, "headerless" code
   SAVEC
   AWU_CSR1 C@
   IRET
;NVM ( xt ) INT_AWU !

: initawu  ( -- ) \ AWU period about 1s
  62 AWU_APR C!   12 AWU_TBR C!   16 AWU_CSR1 C!
;

: HALT ( -- )    \ encodes the STM8 `HALT` instruction
  [ $8E C, ]
;
RAM

\ Example
\ initawu HALT \ test - will return after the AWU period

