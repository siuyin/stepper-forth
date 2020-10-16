\ thr demonstrates threshold with hysterisis.
: thr ( -- )
    3 conv ( -- n )
    dup ( n -- n n )
    255 < not if ( n n -- n )
        led.on
    else 
        dup ( n -- n n )
        245 < if ( n n -- n )
            led.off
        then
    then
    drop
;
