\ conv.fs 64 sample ADC

: adc.sample ( adcInp -- n )
    ADC! ADC@
;

variable adcCh
: conv ( adcInp -- n )
    adcCh !
    adcCh @ adc.sample

    \ sample 63 more times and sum
    62 for
        adcCh @ adc.sample
        +
    next

    \ divide by 64
    5 for
        2/
    next

    \ make it unsigned
    $3ff and
;
