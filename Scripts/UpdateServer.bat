@echo off
:: You can change this for a different type of game server update
set APPID=1110390
:: If SteamCMD is not in your PC's PATH, set this the path of SteamCMD
set SteamCMD=SteamCMD.exe

if ["%1"]==[""] GOTO :ERROR_NOPATH
echo Updating ^& Verifying Server %1...
%SteamCMD% +login anonymous +force_install_dir "%1" +app_update %APPID% validate +exit
timeout /T 2 >nul
goto :EOF
:ERROR_NOPATH
echo ERROR: No Server Selected.
pause
