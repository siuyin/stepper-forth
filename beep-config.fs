\ beep-config.fs sets option byte 2 to enable beeper
\res MCU: STM8S103

\res export OPT2    \ Options 2 Alternate function remapping
#require OPT!
: setBeepFunction ( -- )
    $80 OPT2 OPT!
;

