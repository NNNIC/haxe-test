@echo off
cd %~dp0
path=G:\HaxeToolkit\haxe;%path%
echo :
echo : Compile 
echo :
pause
haxe --main Test --cpp cpp
echo : 
echo : Run
echo :  
pause
cpp\Test.exe
pause