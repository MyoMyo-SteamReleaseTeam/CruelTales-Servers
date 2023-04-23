@echo off

REM Set character set to UTF-8
chcp 65001

setlocal

REM Run core patcher
 dotnet run -c Release ^
 --programName "programXml" "programFilePatch" ^
 --xmlPath "../CT-NetworkCore/PacketDefinition/" ^
 --packetTypePath "../CT-Common/Packets/" ^
 --outputServer "../CT-NetworkCore/Packets/" ^
 -baseNamespace "CT.Packets" ^
 -packetTypeName "PacketType" ^
 --patchCount "4" ^
 --source_0 "../CT-NetworkCore" ^
 --target_0 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-NetworkCore" ^
 --source_1 "../CT-Tools" ^
 --target_1 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Tools" ^
 --source_2 "../CT-Common" ^
 --target_2 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Common" ^
 --source_3 "../CTC-Networks" ^
 --target_3 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CTC-Networks" ^
 --factoryServerPath "../CTS-GameplayServer/Packets/" ^
 --factoryClientPath "../CTC-Networks/Packets/" ^
 --sdispatcher "../CTS-GameplayServer/Packets/" ^
 --cdispatcher "../CTC-DummyClient/Packets/" "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/Packets"

pause