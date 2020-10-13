\ tick.fs ticks once a second

#require beep.fs

NVM

variable tickState

: tick.Init ( -- )
    beep.Init
    0 beep.Ctr !
    0 tickState !
;

: updateCounters ( -- )
    0 beep.Ctr @ < if
        -1 beep.Ctr +!
    then
;

: tickRun? ( -- )
    beep.Ctr @ 0=
;


: tickStateMach ( -- )
    tickState @ 0= if
        beep.On
        1 beep.Ctr ! \ turn off in n * 5ms
        1 tickState !
    else tickState @ 1 = if
        beep.Off
        199 beep.Ctr ! \ next run in n * 5ms
        0 tickState !
    then then
;

: tickerTask ( -- )
    updateCounters

    tickRun? if
        tickStateMach
    then
;

: startTickerTask ( -- )
    ." starting ticker task"
    tick.Init
    [ ' tickerTask ] literal BG !
    hi
;

' startTickerTask 'BOOT !

RAM
