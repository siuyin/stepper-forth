# Stepper motor forth code

Based on STM8 eForth (stm8ef) see: https://github.com/TG9541/stm8ef

## Hardware
I have a bi-polar 48-step per revolution stepper
connected to an STM8S103F3's ports PA1, PA2, PC3 and PC4.

The stepper is driven via an L293D quad half-bridge IC.

## Using the code

1. clone the repo.
1. change directory to project root.

```
e4thcom -t stm8ef -d ttyUSB0
```

At forth prompt:

```
#include stepper.fs
Stepper.init
2 48 1 * Stepper.step
```

The '2' above is the step-interval (2 * 5ms),
48 is the number of steps or each revolution of my stepper motor,
1 is the number of revolutions.

Thus the above will cause the stepper motor
to rotate 1 revolution.

## Other utility code

### beep.fs, beep-config.fs
Routines to enable the beeper pin.

### dly.fs
Provides a blocking delay by using stm8ef TIM counter.

### lsi-en.fs, lsi-meas.fs
Routines to enable and measure/calibrate
the low speed internal (LSI) oscillator.
That oscillator has a nominal frequency of 128kHz +/- 12.5% .

### meccabrain.fs, meccabrain_test.fs
Provides routines to drive Mecanno M.A.X robot components.

`meccabrain_test.fs` illustrates basic unit testing principles.

### tim1.fs
Timer 1 functions.
