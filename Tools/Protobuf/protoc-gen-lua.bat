@echo off

rem %1 proto dir path
rem %2 lua out path

echo gen lua
for /R %1 %%f in (*.proto) do (
	protoc --plugin=protoc-gen-lua="gen-lua.bat" --proto_path=%%~dpf --lua_out=%2 %%f
	echo gen %%~nf.proto
)
