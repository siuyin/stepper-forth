\ udly.fs microsecond range blocking delay

#require tim4.fs

NVM

variable uNext

: udly ( n -- )
    TIM4_CNTR C@ + uNext C!
    begin
        TIM4_CNTR C@ uNext C@ < not
    until
;

RAM
