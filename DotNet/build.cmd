@echo off

rem Build the library
msbuild WritePadSDK.sln /p:Configuration=Release

rem Create the NuGet
nuget pack PhatWare.WritePad.nuspec

rem Create the Xamarin Component
del WritePadSDK\WritePadSDK.WindowsRuntime\bin\Release\PhatWare.WritePad.WindowsRuntime.xml
xamarin-component package ./