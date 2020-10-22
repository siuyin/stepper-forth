\ beep.fs beeper routines

#require ]B!

NVM

variable beep.Ctr

: beep.Init ( -- )
    $0e $50f3 c!
;
: 1k $0e $50f3 C! ;
: 2k $4e $50f3 C! ;
: 4k $8e $50f3 C! ;


: beep.Off 
    [ 0 $50f3 5 ]B!
;

: beep.On 
    [ 1 $50f3 5 ]B! 
;

RAM
