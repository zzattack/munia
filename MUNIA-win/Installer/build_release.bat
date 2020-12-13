call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat"
set MAKENSIS="%PROGRAMFILES(X86)%\nsis\makensis.exe"

cd ..
MSBUILD MUNIA.sln /p:Configuration=Release
cd Installer
%MAKENSIS% installer-rls.nsi