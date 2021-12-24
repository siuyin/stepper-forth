\ dly is a blocking delay of n * 5ms ticks.
: dly ( n -- )
    tim + >R	\ push TIM + n to return stack
    begin
        tim R@ =
    until
    R> DROP	\ remove TIM + n from return stack
;
