\ FIXME rename this scratch pad file
#require MARKER
#require ]B!

MARKER  END\	\ say END\ to remove from RAM

\res MCU: STM8S103
\res export CLK_ICKR     \ Internal clock control register
NVM
: lsiEn ( 1|0 --) \ Enable the Low Speed Internal oscillator.
  IF
    [ 1 CLK_ICKR 3 ]B!
  ELSE
    [ 0 CLK_ICKR 3 ]B!
  THEN
;
RAM

\res export AWU_CSR1     \ AWU control/status register 1
NVM
: lsiMeasEn ( 1|0 -- ) \ Enable measuring the low speed osc by timer 1
  IF   [ 1 AWU_CSR1 0 ]B!
  ELSE [ 0 AWU_CSR1 0 ]B!
  THEN
;
RAM

END\
