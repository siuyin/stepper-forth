\ i2cPulser - targets address 0x20 when button on PD3 is pushed.

\res MCU: STM8S103
\res export I2C_DR

NVM

: ping ( addr -- )
    begin i2cBusy? not until
    i2cStart
    2* 0 + I2C_DR C!
    \ i2cAddrSent? drop
    \ i2cClrAddr
    i2cStop
;

: dbChk ( -- )
    dbPushed? not if
        dbH
        $20 ping
    else
        dbL
    then
;

: startup ( -- )
    ." starting dbChk"
    i2cInit
    [ ' dbChk ] literal BG !
    hi
;

' startup 'BOOT !

RAM
