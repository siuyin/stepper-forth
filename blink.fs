#require tim4.fs
#require led.fs

TIM4.IntrEn
LED.Init

: dly ( n -- ) 
    tick @ + ( tick+n )
    begin
        dup ( tick+n -- tick+n tick+n )
        tick @ swap ( tick+n tick+n -- tick+n tick tick+n )
        < not ( tick+n tick tick+n -- tick+n tick>=tick+n )
    until
    drop ( tick+n -- )
;

: blink ( interval n -- )
    0
    do
        LED.On
        dup dly
        LED.Off
        dup dly
    loop
    drop
;
