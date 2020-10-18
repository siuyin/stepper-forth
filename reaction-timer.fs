\ reaction timer uses btn.fs and tim4.fs

#require btn.fs
#require tim4.fs
#require dly.fs

NVM

: init ( -- )
    BTN.Init
    TIM4.IntrEn
;


\ rand produces a random number between 0 and 999
: rand ( -- n )
    tick @ 1000 mod
;


: randDly ( -- )
    rand 5 / \ random int between 0 and 999 / 5 to get number of 5ms ticks
    200 + \ and 1 second
    dly
;

: measure ( -- )
    LED.On
    tick @
    begin
        BTN.Pushed?
    until
    tick @
    swap - .
    LED.Off
;

: instr
    CR
    ." Press button when you see the 2nd flash" CR
;

: cheatCheck 
    BTN.Pushed? if
        CR
        ABORT" Release button. Press only when you see the second flash."
    then
;

\ start starts reaction time sequence by flasing the led for 1 second (200 5ms ticks).
: start ( -- )
    instr

    LED.Init 
    init


    LED.On
    200 dly
    LED.Off

    cheatCheck

    randDly

    measure
;

RAM
