\res MCU: STM8S103
\res export PA_ODR PA_DDR PA_CR1
\res export PC_ODR PC_DDR PC_CR1
#require ]B!
#require dly.fs

NVM

variable st \ stepper state: 0 to 3

: Stepper.init ( -- )
    0 st !    \ sequence is 0,2,1,3

    [ 1 PA_DDR 1 ]B! \ set PA1 to output
    [ 1 PA_CR1 1 ]B! \ set PA1 to push-pull

    [ 1 PA_DDR 2 ]B! \ set PA2 to output
    [ 1 PA_CR1 2 ]B! \ set PA2 to push-pull

    [ 1 PC_DDR 3 ]B! \ set PC3 to output
    [ 1 PC_CR1 3 ]B! \ set PC3 to push-pull

    [ 1 PC_DDR 4 ]B! \ set PC4 to output
    [ 1 PC_CR1 4 ]B! \ set PC4 to push-pull
;

: Stepper.run ( -- )
    st @ 0 = if
        2 st !
        [ 0 PA_ODR 1 ]B!
        [ 1 PC_ODR 3 ]B!
    else st @ 1 = if
        3 st !
        [ 0 PA_ODR 2 ]B!
        [ 1 PC_ODR 4 ]B!
    else st @ 2 = if
        1 st !
        [ 0 PC_ODR 3 ]B!
        [ 1 PA_ODR 2 ]B!
    else st @ 3 = if
        0 st !
        [ 0 PC_ODR 4 ]B!
        [ 1 PA_ODR 1 ]B!
    then then then then
;

: Stepper.step ( stepInterval n -- )
    1- \ loop n times rather than n+1 times.
    for
        Stepper.run
        dup dly
    next
    drop
;    

RAM

