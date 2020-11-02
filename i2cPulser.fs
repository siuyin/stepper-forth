\ i2cPulser - targets address 0x20 when button on PD3 is pushed.

cold

\res MCU: STM8S103
\res export I2C_DR

RAM

variable pingSt
: pingStTgl
    pingSt @ 0= if
        1 pingSt !
        dbH
    else
        0 pingSt !
        dbL
    then
;

: wait 50 for i drop next ;
: ping ( n addr -- )
    begin i2cBusy? not until
    i2cStart
    2* 0 + I2C_DR C!
    wait
    i2cAddrSent? if
        i2cClrAddr
        I2C_DR C!
        wait
    else
        drop
    then
    i2cStop
;

variable btnSt
variable btnNR \ next run tick
: btnSM ( -- )
    btnSt @ 0= if
        dbPushed? if
            pingStTgl
            pingSt @ $20 ping
            1 btnSt ! \ pushed
        then
    else btnSt @ 1 = if
        dbPushed?  if
        else
            2 btnSt ! \ may be released
        then
    else btnSt @ 2 = if
        dbPushed?  if
            1 btnSt ! \ still pushed
        else
            0 btnSt ! \ released
        then
    then then then
;

variable btnCnt
: updateBtnCnt ( -- )
    0 btnCnt @ < if
        -1 btnCnt +!
    then
;
: btnChkNow? ( -- flag )
    btnCnt @ 0= 
;
: btnChk ( -- )
    updateBtnCnt
    btnChkNow? if
        4 btnCnt !
        btnSM
    then
;

variable pingCnt
: main ( -- )
    btnChk
;


: startup ( -- )
    ." starting main"
    i2cInit
    [ ' main ] literal BG !
    hi
;

' startup 'BOOT !

RAM
