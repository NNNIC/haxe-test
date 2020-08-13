@echo off
cd %~dp0
Haxe -p src -m Test --js a.js
pause