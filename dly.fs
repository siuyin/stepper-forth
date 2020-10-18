NVM

variable nx

\ dly is a blocking delay of n * 5ms ticks.
: dly ( n -- )
    tim + nx !
    begin
        tim nx @ =
    until
;

RAM
