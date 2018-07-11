set MSBUILD="C:\Windows\Microsoft.Net\Framework\v4.0.30319\MSBuild.exe"
set MAKENSIS="%PROGRAMFILES(X86)%\nsis\makensis.exe"

cd ..
%MSBUILD% MUNIA.sln /p:Configuration=Release
cd Installer
%MAKENSIS% installer-rls.nsi