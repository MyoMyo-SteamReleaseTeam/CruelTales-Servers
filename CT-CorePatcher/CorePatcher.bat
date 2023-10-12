@echo off

REM Set character set to UTF-8
chcp 65001

setlocal

REM Run core patcher
 dotnet run -c Release ^
 --programName "programXml" "programFilePatch" ^
 --xmlPath "../CT-Definition/Packets/" ^
 --packetTypePath "../CT-Common/Packets/" ^
 --outputServer "../CT-NetworkCore/Packets/" ^
 -baseNamespace "CT.Packets" ^
 -packetTypeName "PacketType" ^
 --patchCount "6" ^
 --ignoreIndices "3" ^
 --source_0 "../CT-NetworkCore" ^
 --target_0 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-NetworkCore" ^
 --source_1 "../CT-Common" ^
 --target_1 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Common" ^
 --source_2 "../CTC-Networks" ^
 --target_2 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CTC-Networks" ^
 --source_3 "../CTS-GameplayServer/ClientShared" ^
 --target_3 "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/ClientShared" ^
 --source_4 "../KaNetTool" ^
 --target_4 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/KaNetTool" ^
 --source_5 "../KaNetPhysics" ^
 --target_5 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/KaNetPhysics" ^
 --factoryServerPath "../CTS-GameplayServer/Packets/" ^
 --factoryClientPath "../CTC-Networks/Packets/" ^
 --sdispatcher "../CTS-GameplayServer/Packets/" ^
 --cdispatcher "../CTC-DummyClient/Packets/" "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/Packets"

REM pause