@echo off
cd %~dp0
rd /s /q cs 2>nul
echo :
echo : compile
echo : 
md cs
Haxe -p sys_cs -p hx  -m MainProgram --cs cs
echo : done
echo : 
echo : run
echo :
pause
cs\bin\MainProgram.exe
pause