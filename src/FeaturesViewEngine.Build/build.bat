@echo off
cls

"..\..\packages\FAKE.4.64.6\tools\Fake.exe" build.fsx "CreatePackages"
pause
