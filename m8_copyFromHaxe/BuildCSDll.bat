call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
@cd /d %~dp0
@echo off

echo ::
echo :: clean
echo ::

rd /s /q cs\src 2>nul
rd /s /q TestApp1\TestDll\src 2>nul

echo ::
echo :: Build Haxe to CS
echo ::
pause

echo . | call ToCSharp.bat

echo ::
echo :: copy from haxe's cs files.
echo ::
pause
robocopy cs\src TestApp1\TestDll\src /MIR

echo ::
echo :: Build
echo ::
pause
cd TestApp1
msbuild TestApp1.sln


echo ::
echo :: Run
echo ::

TestApp1\bin\Debug\netcoreapp3.1\TestApp1.exe

cmd /k
