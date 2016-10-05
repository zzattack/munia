set MSBUILD="%PROGRAMFILES(X86)%\MSBuild\14.0\Bin\msbuild.exe"
set MAKENSIS="%PROGRAMFILES(X86)%\nsis\makensis.exe"

cd ..
%MSBUILD% MUNIA.sln /p:Configuration=Release
cd Installer
%MAKENSIS% installer-rls.nsi