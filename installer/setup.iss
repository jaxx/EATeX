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
Root: HKCU; Subkey: "Software\Sparx Systems\EAAddins\EATeX"; ValueType: string; ValueName: ""; ValueData: "EATeX.EATeXAddin"; Flags: uninsdeletekey;

[Files]
; TODO: add eatex.dll to installer and install it to program files
; Source: "C:\EATeX\src\EATeX\bin\Debug\EATeX.dll"; DestDir: "{tmp}"; Flags: ignoreversion; BeforeInstall: RunMiktexInstaller;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
const
  MikTex32BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex.exe';
  MikTex64BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex-x64.exe';

var
  MikTexLocalName: String;

function IsMikTexInstalled: Boolean;
begin
  Result := RegKeyExists(HKEY_LOCAL_MACHINE, 'Software\MiKTeX.org\MiKTeX');
end;

function IsEAInstalled: Boolean;
begin
  Result := RegKeyExists(HKEY_CURRENT_USER, 'Software\Sparx Systems\EA400\EA');
end;

function GetRegAsmLocation: String;
begin
  Result := '';

  try
    Result := ExpandConstant('{dotnet40}\regasm.exe');
  except 
    try
      Result := ExpandConstant('{dotnet20}\regasm.exe');
    except
      MsgBox('You don''t have .NET framework installed.', mbError, MB_OK);
    end;
  end;
end;

procedure RunMikTexInstaller(DownloadPage: TWizardPage);
var
  ResultCode: Integer;
begin
  if not Exec(MikTexLocalName, '', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
    MsgBox('Failed to run MikTex installer.' + #13#10 + SysErrorMessage(ResultCode), mbError, MB_OK);
end;

procedure RegisterEatexLibrary(DllLocation: String);
var
  ResultCode: Integer;
begin
  ShellExec('', GetRegAsmLocation, '/codebase ' + DllLocation, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
end;

procedure UnRegisterEatexLibrary(DllLocation: String);
var
  ResultCode: Integer;
begin
  ShellExec('', GetRegAsmLocation, '/u ' + DllLocation, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
end;

procedure InitializeWizard;
begin
  if not IsEAInstalled then
  begin
    MsgBox('You must install Enterprise Architect first before continuing this installation.', mbError, MB_OK);
    Abort;
  end;

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
    RegisterEatexLibrary('C:\EATeX\src\EATeX\bin\Debug\EATeX.dll');
end;