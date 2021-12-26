\ beep-config.fs sets option byte 2 to enable beeper
\res MCU: STM8S103

\res export OPT2    \ Options 2 Alternate function remapping
NVM
#require OPT!
: setBeepFunction ( -- ) \ edit lib/TARGET to NVM to move this to NVM
    $80 OPT2 OPT!
;
RAM
