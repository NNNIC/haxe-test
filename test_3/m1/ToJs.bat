@echo off
cd %~dp0
rd /s /q cs 2>nul
echo :
echo : compile
echo : 
md cs
Haxe -p sys_cs\system -p hx -p hx\sample   -m sample.Program --js a.js
echo : done
echo : 
echo : run
echo :
pause
cs\bin\Test.exe
pause