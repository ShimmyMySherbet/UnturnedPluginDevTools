@echo off
:: If SteamCMD is not in your PC's PATH, set this the path of SteamCMD
set SteamCMD="SteamCMD.exe"

set APPID=1110390
set UpdateRocket=%1\Extras\Install Rocket.bat

if ["%1"]==[""] GOTO :ERROR_NOPATH
echo [STATUS] Updating ^& Verifying Server %1...
%SteamCMD% +login anonymous +force_install_dir "%1" +app_update %APPID% validate +exit
echo [STATUS] Server updated!
if EXIST "%UpdateRocket%" (call :UPDATE_ROCKET) ELSE echo [Status] Failed to find RocketMod update script.

timeout /T 2 >nul
goto :EOF
:UPDATE_ROCKET
echo [Status] Updating RocketMod...
@call "%UpdateRocket%"
goto :EOF
:ERROR_NOPATH
echo ERROR: No Server Selected.
pause
