!ifndef CONFIG
!define CONFIG "Debug"
!endif
!define /date DATE "%d-%m-%Y"

; Retrieve version from exe
!getdllversion "../MUNIA/bin/${CONFIG}/MUNIA.exe" expv_
!define VERSION "${expv_1}.${expv_2}.${expv_3}"
!define VERSIONFULL "${expv_1}.${expv_2}.${expv_3}.${expv_4}"

; Define your application name
!define APPNAME "MUNIA"
!define APPNAMEANDVERSION "MUNIA v${VERSION}"

; Main Install settings
Name "${APPNAMEANDVERSION}"
InstallDir "$PROGRAMFILES\MUNIA"
InstallDirRegKey HKLM "Software\MUNIA" ""
OutFile "MUNIA_v${VERSION}_${CONFIG}_${DATE}.exe"

; Use compression
SetCompressor LZMA

; Multiuser functions determine whether a user or computer based install is performed
!define MULTIUSER_EXECUTIONLEVEL Highest
!define MULTIUSER_MUI
!define MULTIUSER_INSTALLMODE_INSTDIR "MUNIA"
!include MultiUser.nsh

; Modern interface settings
ShowInstDetails show
!include "MUI2.nsh"

!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Start MUNIA"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchLink"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "license.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set languages (first is default language)
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Version Information
  VIProductVersion ${VERSIONFULL}
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "MUNIA Installer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "By Frank Razenberg"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "munia.io"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" "MUNIA is a trademark of zzattack.org"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright © munia.io"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "MUNIA Input Viewer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion " "${VERSION}"
;--------------------------------


Section "MUNIA" SectionMain

	; Set Section properties
	SetOverwrite on

	SetOutPath "$INSTDIR\"
	; release mode executables should have their spaa05-developped dlls merged in them
	File "..\MUNIA\bin/${Config}\*.exe"
	File "..\MUNIA\bin/${Config}\*.dll"
	SetOutPath "$INSTDIR\skins"
	File "..\MUNIA\skins\*.svg"

	CreateDirectory "$SMPROGRAMS\MUNIA"
	SetOutPath "$INSTDIR"
	CreateShortCut "$DESKTOP\MUNIA.lnk" "$INSTDIR\MUNIA.exe"
	CreateShortCut "$SMPROGRAMS\MUNIA\MUNIA.lnk" "$INSTDIR\MUNIA.exe"
	CreateShortCut "$SMPROGRAMS\MUNIA\Uninstall.lnk" "$INSTDIR\uninstall.exe"
SectionEnd

Function LaunchLink
  ExecShell "" "$DESKTOP\MUNIA.lnk"
FunctionEnd


;Section "Web Interface" SectionWebInterface
;	; Set Section properties
;	SetOverwrite on
;
;	; Set Section Files and Shortcuts	; Set Section Files and Shortcuts
;	SetOutPath "$INSTDIR\WebInterface\"
;	File /r /x *.txt "../WebInterface\*"
;SectionEnd


Section -FinishSection
	WriteRegStr HKLM "Software\${APPNAME}" "" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$INSTDIR\uninstall.exe"
	WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd

; Modern install component descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SectionMain} "The main program. Mandatory."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;Uninstall section
Section Uninstall

	;Remove from registry...
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	DeleteRegKey HKLM "SOFTWARE\MUNIA"

	; Delete self
	Delete "$INSTDIR\uninstall.exe"

	; Delete Shortcuts
	Delete "$DESKTOP\MUNIA.lnk"
	RMDir /r "$SMPROGRAMS\MUNIA"
	RMDir /r "$INSTDIR"		
SectionEnd

; On initialization
Function .onInit
	!insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd


BrandingText "munia.io"