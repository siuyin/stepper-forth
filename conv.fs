\ conv.fs 64 sample ADC

: adc.sample ( adcInp -- n )
    ADC! ADC@
;

\ conv samples adcInp 64 times and averages the samples
\ leaving the result n on the data stack.
: conv ( adcInp -- n )
    dup ( c -- c c )
    adc.sample ( c c -- c n )

    \ sample 63 more times and sum
    62 for
        swap ( c n -- n c )
        dup ( n c -- n c c )
        adc.sample ( n c c -- n c ns )
        rot ( n c ns -- c ns n )
        + ( c ns n -- c ns )
    next

    \ divide by 64 ( c ns -- c nd )
    5 for
        2/
    next

    \ make it unsigned ( c nd -- c nu )
    $3ff and

    swap drop ( c nu -- nu )
;
