@echo off

rem %1 proto dir path
rem %2 csharp out path
echo gen csharp
for /R %1 %%f in (*.proto) do (
	protoc --proto_path=%%~dpf --csharp_out=%2 %%f
	echo gen %%~nf.proto
)
