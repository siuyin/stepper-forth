\ reaction timer uses btn.fs and tim4.fs

#require btn.fs
#require tim4.fs
#require dly.fs


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

\ start starts reaction time sequence by flasing the led for 1 second (200 5ms ticks).
: start ( -- )
    LED.Init 
    init

    LED.On
    200 dly
    LED.Off

    randDly

    measure
;
