cold
RAM
: abc ( n -- )
    0 do
        \ $61 i + lcWrite \ abc
        $41 i + lcWrite \ ABC
        \ $30 i + lcWrite \ 012
    loop
;



RAM

lcInit
3 abc
