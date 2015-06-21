#include <./it_download.iss>

#define MyAppName "EATeX for Enterprise Architect"
#define MyAppVersion "0.1"
#define MyAppURL "https://github.com/jaxx/EATeX"

[Setup]
AppId={{14BC27BE-18FF-4BA5-910C-80CF06FBEBAD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
CreateAppDir=no
OutputBaseFilename=eatex_installer
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Registry]
Root: HKCU; Subkey: "Software\Sparx Systems\EAAddins\EATeX"; ValueType: string; ValueName: ""; ValueData: "EATeX.EATeXAddin"

[Files]
;Source: "{tmp}\miktex_installer.exe"; DestDir: "{tmp}"; Flags: ignoreversion; BeforeInstall: RunMiktexInstaller;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
const
  MikTex32BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex.exe';
  MikTex64BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex-x64.exe';

var
  MikTexLocalName: String;

function IsMikTexInstalled: Boolean;
begin
  Result := True;
end;

procedure RunMikTexInstaller(downloadPage:TWizardPage);
var
  ResultCode: Integer;
begin
  if not Exec(MikTexLocalName, '', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
    MsgBox('Failed to run MikTex installer.' + #13#10 + SysErrorMessage(ResultCode), mbError, MB_OK);
end;

procedure RegisterAddin;
var
  RegAsmLocation: String;
  Command: String;
  ResultCode: Integer;
begin
  // TODO: check .NET framework version and remove hardcoded eatex.dll location
  RegAsmLocation := ExpandConstant('{win}\Microsoft.NET\Framework\v4.0.30319\regasm');
  Command := 'C:\eatex\src\EATeX\bin\Debug\EATeX.dll /codebase';

  ShellExec('', RegAsmLocation, Command, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
end;

procedure InitializeWizard;
begin
  // TODO: check if miktex is already installed
  if IsMikTexInstalled then
    Exit;

  MikTexLocalName := ExpandConstant('{tmp}\miktex_installer.exe');
  ITD_Init;

  if IsWin64 then
    ITD_AddFile(MikTex64BitDownloadUrl, MikTexLocalName)
  else
    ITD_AddFile(MikTex32BitDownloadUrl, MikTexLocalName);

  ITD_DownloadAfter(wpWelcome);
  ITD_AfterSuccess := @RunMikTexInstaller;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpInstalling then
    RegisterAddin;
end;