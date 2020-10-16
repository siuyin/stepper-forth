\ conv.fs 64 sample ADC

NVM

: adc.ChSel ( c -- )
    ADC!
;


\ conv samples adcInp 64 times and averages the samples
\ leaving the result n on the data stack.
: conv ( adcInp -- n )
    adc.ChSel ( c -- )
    ADC@ ( -- n )

    \ sample 63 more times and sum
    62 for
        ADC@ ( n0 -- n0 n1 )
        + ( n0 n1 -- ns )
    next

    \ divide by 64 -- 2^6
    5 for
        2/
    next

    \ make it unsigned
    $3ff and
;

RAM
