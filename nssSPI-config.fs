\ nssSPI-config.fs sets option byte 2 to enable the nSS pin (not Slave Select)

\res MCU: STM8S103
\res export OPT2

#require OPT!

$02 OPT2 OPT! \ AFR1 enables the nSS pin.
