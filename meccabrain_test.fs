\ meccabrain_test.fs -- unit tests for meccabrain.fs
\ #include meccabrain_test.fs to run the tests

#require meccabrain.fs

NVM
#require utils/tester.fs
RAM

\ rightShift tests
T{ 1 0 rightShift -> 1 }T
T{ 1 1 rightShift -> 0 }T
T{ 1 -1 rightShift -> 1 }T \ same effect as 0 rightShift
T{ %1000 3 rightShift -> 1 }T
T{ %1000 2 rightShift -> %10 }T
T{ %1000 1 rightShift -> %100 }T

\ leftshift tests
T{ 1 0 leftShift -> 1 }T
T{ 1 1 leftShift -> 2 }T
T{ 1 2 leftShift -> 4 }T
T{ 1 3 leftShift -> 8 }T
T{ 1 -1 leftShift -> 1 }T

\ checksum tests
T{ 0 1 2 3 4 checksum -> 160 }T
T{ 1 1 2 3 4 checksum -> 161 }T
