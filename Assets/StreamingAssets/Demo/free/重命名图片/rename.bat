@echo off

set a=901

setlocal EnableDelayedExpansion

for %%n in (*.png) do (

ren "%%n" "!a!.png"
set /A a+=1

)