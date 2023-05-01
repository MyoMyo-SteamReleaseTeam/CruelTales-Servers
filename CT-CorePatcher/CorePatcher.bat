﻿@echo off

REM Set character set to UTF-8
chcp 65001

setlocal

REM Run core patcher
 dotnet run -c Release ^
 --programName "programXml" "programFilePatch" ^
 --xmlPath "../CT-Common/Definitions/Packets/" ^
 --packetTypePath "../CT-Common/Packets/" ^
 --outputServer "../CT-NetworkCore/Packets/" ^
 -baseNamespace "CT.Packets" ^
 -packetTypeName "PacketType" ^
 --patchCount "3" ^
 --source_0 "../CT-NetworkCore" ^
 --target_0 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-NetworkCore" ^
 --source_1 "../CT-Common" ^
 --target_1 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Common" ^
 --source_2 "../CTC-Networks" ^
 --target_2 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CTC-Networks" ^
 --factoryServerPath "../CTS-GameplayServer/Packets/" ^
 --factoryClientPath "../CTC-Networks/Packets/" ^
 --sdispatcher "../CTS-GameplayServer/Packets/" ^
 --cdispatcher "../CTC-DummyClient/Packets/" "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/Packets"

pause