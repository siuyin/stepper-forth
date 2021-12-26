\ beep.fs beeper routines

#require beep-config.fs
#require ]B!
#require MARKER

MARKER END\
\res MCU: STM8S103
\res export BEEP_CSR     \ BEEP control/status register          (0x1F)

NVM
: beep.Init ( -- )
    setBeepFunction	\ Configure option work to enable BEEP pin.
    $0e BEEP_CSR c!
;
: 1k $0e BEEP_CSR C! ;
: 2k $4e BEEP_CSR C! ;
: 4k $8e BEEP_CSR C! ;


: beep.Off 
    [ 0 BEEP_CSR 5 ]B!
;

: beep.On 
    [ 1 BEEP_CSR 5 ]B! 
;

RAM

END\

\\ Examples:

2k \ cause the beeper to sound at 2kHz.
