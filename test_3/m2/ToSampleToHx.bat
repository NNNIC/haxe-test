@echo off
cd /d %~dp0
set CS2HX=..\Cs2hx-master\trunk\Cs2hx\bin\Debug\Cs2hx.exe

"%CS2HX%" /sln:sample.sln /out:hx
cmd /K
