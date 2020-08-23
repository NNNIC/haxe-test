@echo off
cd %~dp0
del js\Test.js 2>nul
echo :
echo : compile
echo : 
Haxe -p sys_js -p hx  -m MainProgram --js js\Test.js
echo : done
echo : 
echo : run
echo :
pause
start js\index.html
pause