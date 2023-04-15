@echo off

REM Set character set to UTF-8
chcp 65001

setlocal

REM Run core patcher
 dotnet run -c Release ^
 --xmlPath "../CT-NetworkCore/PacketDefinition/" ^
 --packetTypePath "../CT-Common/Packets/" ^
 --outputServer "../CT-NetworkCore/Packets/" ^
 -baseNamespace "CT.Packets" ^
 -packetTypeName "PacketType" ^
 --patchCount "3" ^
 --source_0 "../CT-NetworkCore" ^
 --target_0 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-NetworkCore" ^
 --source_1 "../CT-Tools" ^
 --target_1 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Tools" ^
 --source_2 "../CT-Common" ^
 --target_2 "../../CruelTales-Client/CruelTales-Client/Assets/Plugins/CT-Common" ^
 --factoryPath "../CT-NetworkCore/Packets/" ^
 --sdispatcher "../CTS-GameplayCore/Packets/" ^
 --cdispatcher "../CTC-DummyClient/Packets/" "../../CruelTales-Client/CruelTales-Client/Assets/Scripts/Networks/Packets"

pause