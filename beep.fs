\ beep.fs beeper routines

#require ]B!

NVM

variable beep.Ctr

: beep.Init ( -- )
    $0e $50f3 c!
;


: beep.Off 
    [ 0 $50f3 5 ]B!
;

: beep.On 
    [ 1 $50f3 5 ]B! 
;

RAM
