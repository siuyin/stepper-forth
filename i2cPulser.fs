\ i2cPulser - targets address 0x20 when button on PD3 is pushed.


\res MCU: STM8S103
\res export I2C_DR
\res export I2C_CR1 I2C_CR2 I2C_SR1 I2C_SR2 I2C_SR3

#require i2cTxIntr.fs

NVM

: i2cdump
    hex
    cr ." CR" cr
    I2C_CR1 C@ . cr
    I2C_CR2 C@ . CR

    ." SR" cr
    I2C_SR1 C@ . cr
    I2C_SR2 C@ . cr
    I2C_SR3 C@ . cr

    decimal
;

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

: ping ( n addr -- )
    pingAddr !
    pingDat !
    i2cStart
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
    ." start main"
    i2cInit
    i2cItEvEn
    i2cItBufEn
    i2cItErrEn
    [ ' main ] literal BG !
    hi
;

' startup 'BOOT !

RAM
