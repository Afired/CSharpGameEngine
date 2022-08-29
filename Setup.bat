dotnet build


@echo off

set SCRIPT="%TEMP%\%RANDOM%-%RANDOM%-%RANDOM%-%RANDOM%.vbs"

echo Set oWS = WScript.CreateObject("WScript.Shell") >> %SCRIPT%
echo sLinkFile = "%~dp0\GameEngine.Editor.lnk" >> %SCRIPT%
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> %SCRIPT%
echo oLink.TargetPath = "%~dp0\GameEngine.Editor\bin\Debug\net6.0\GameEngine.Editor.exe" >> %SCRIPT%
echo oLink.WorkingDirectory = "%~dp0\GameEngine.Editor\bin\Debug\net6.0\" >> %SCRIPT%
echo oLink.Save >> %SCRIPT%

cscript /nologo %SCRIPT%
del %SCRIPT%


setx GameEngineCoreDLL %~dp0GameEngine.Core\bin\Debug\net6.0\GameEngine.Core.dll
setx GameEngineSourceGeneratorDLL %~dp0GameEngine.SourceGenerator\bin\Debug\netstandard2.0\GameEngine.SourceGenerator.dll
setx GameEngineEditorDLL %~dp0GameEngine.Editor\bin\Debug\net6.0\GameEngine.Editor.dll


start cmd /C GameEngine.Editor.lnk