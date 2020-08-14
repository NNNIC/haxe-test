@echo off
cd %~dp0
rd /s /q cs 2>nul
md cs
Haxe -p src -m Test --cs cs
cs\bin\Test.exe
pause