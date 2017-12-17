@echo off
cls

"..\..\packages\FAKE.4.63.2\tools\Fake.exe" build.fsx "CreatePackages"
pause
