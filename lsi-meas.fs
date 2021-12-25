\res MCU: STM8S103

#require MARKER
#require ]B!
#require ]B?

MARKER END\

\res export TIM1_CCER1   \ TIM1 capture/compare enable register 1
NVM
: tim1.cc1.en ( -- ) 
  [ 1 TIM1_CCER1 0 ]B! ;

: tim1.cc1.dis ( -- )
  [ 0 TIM1_CCER1 0 ]B! ;
RAM

\res export TIM1_CCMR1   \ TIM1 capture/compare mode register 1
NVM
: tim1.cc1.inp ( -- ) \ timer 1 input capture enable
  [ 1 TIM1_CCMR1 0 ]B!
  [ 0 TIM1_CCMR1 1 ]B! ;

: tim1.cc1.out ( -- ) \ timer 1 input capture disabled, configured as output
  [ 0 TIM1_CCMR1 0 ]B!
  [ 0 TIM1_CCMR1 1 ]B! ;

\res export TIM1_CCR1H   \ TIM1 capture/compare register 1 high
: tim1.cc1.reg ( -- u )
  TIM1_CCR1H 2C@ ;

: lsiMeasInit ( -- ) \ setup to measure low speed internal oscillator
  1 lsiEn
  1 lsiMeasEn
  TIM1.clkEn
  tim1.cc1.dis
  tim1.cc1.inp
  ;
RAM

\res export TIM1_SR1     \ TIM1 status register 1
NVM
: measLSI ( -- u ) \ measure low speed internal oscillator period
  lsiMeasInit
  tim1.cc1.en
  
  [ 0 TIM1_SR1 1 ]B!  \ clear capture interrupt flag
  BEGIN [ TIM1_SR1 1 ]B? UNTIL \ wait until interrupt occurs
  tim1.cc1.reg 
  BEGIN [ TIM1_SR1 1 ]B? UNTIL \ wait until interrupt occurs
  tim1.cc1.reg
  SWAP -
  tim1.cc1.dis 
  ;
RAM
