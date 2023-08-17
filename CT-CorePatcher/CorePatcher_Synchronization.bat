@echo off

REM Set character set to UTF-8
chcp 65001

setlocal

REM --syncRemotePath "../CTC-DummyClient/SyncObjects/" "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/SyncObjects" ^
REM Run core patcher
 dotnet run -c Release ^
 --programName "programSync" ^
 --syncMasterPath "../CTS-GameplayServer/SyncObjects/" ^
 --syncRemotePath "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/SyncObjects" ^
 --masterPoolPath "../CTS-GameplayServer/Gameplay/ObjectManagements/" ^

REM pause