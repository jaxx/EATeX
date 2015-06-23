#include <./it_download.iss>

#define MyAppName "EATeX for Enterprise Architect"
#define MyAppVersion "1.0.0"
#define MyAppURL "https://github.com/jaxx/EATeX"

[Setup]
AppId={{14BC27BE-18FF-4BA5-910C-80CF06FBEBAD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
UninstallDisplayName={#MyAppName}
CreateAppDir=yes
DefaultDirName={pf}\EATeX
DisableDirPage=yes
OutputBaseFilename=eatex_installer
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Registry]
Root: HKCU32; Subkey: "Software\Sparx Systems\EAAddins\EATeX"; ValueType: string; ValueName: ""; ValueData: "EATeX.EATeXAddin"; Flags: uninsdeletekey; Check: not IsWin64();
Root: HKCU64; Subkey: "Software\Sparx Systems\EAAddins\EATeX"; ValueType: string; ValueName: ""; ValueData: "EATeX.EATeXAddin"; Flags: uninsdeletekey; Check: IsWin64();

[Files]
Source: "..\src\EATeX\bin\Debug\EATeX.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\src\EATeX\bin\Debug\EATeX.dll.config"; DestDir: "{app}"; Flags: ignoreversion;

[UninstallDelete]
Type: dirifempty; Name: "{app}";

[Code]
const
  MikTex32BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex.exe';
  MikTex64BitDownloadUrl = 'http://ctan.sharelatex.com/tex-archive/systems/win32/miktex/setup/basic-miktex-x64.exe';

var
  MikTexTempName: String;

function IsEAInstalled: Boolean;
begin
  // on 64-bit windows we need to check both 32-bit and 64-bit installation 
  if IsWin64 then
    Result := RegKeyExists(HKCU32, 'Software\Sparx Systems\EA400\EA') or
              RegKeyExists(HKCU64, 'Software\Sparx Systems\EA400\EA')
  else
    Result := RegKeyExists(HKCU32, 'Software\Sparx Systems\EA400\EA');
end;

function IsMikTexInstalled: Boolean;
begin
  // on 64-bit windows we need to check both 32-bit and 64-bit installation 
  if IsWin64 then
    Result := RegKeyExists(HKLM32, 'Software\MiKTeX.org\MiKTeX') or
              RegKeyExists(HKLM64, 'Software\MiKTeX.org\MiKTeX') or
              RegKeyExists(HKCU32, 'Software\MiKTeX.org\MiKTeX') or
              RegKeyExists(HKCU64, 'Software\MiKTeX.org\MiKTeX')
  else
    Result := RegKeyExists(HKLM32, 'Software\MiKTeX.org\MiKTeX') or
              RegKeyExists(HKCU32, 'Software\MiKTeX.org\MiKTeX');
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
      MsgBox('.NET framework 2.0 or 4.0 must be installed on your system.', mbError, MB_OK);
    end;
  end;
end;

procedure RunMikTexInstaller(DownloadPage: TWizardPage);
var
  ResultCode: Integer;
begin
  if not Exec(MikTexTempName, '', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
    MsgBox('Failed to run MikTex installer.' + #13#10 + SysErrorMessage(ResultCode), mbError, MB_OK);
end;

procedure RegisterEATeXLibrary(DllLocation: String);
var
  Params: String;
  ResultCode: Integer;
begin
  Params := '/codebase ' + AddQuotes(DllLocation);

  if not ShellExec('', GetRegAsmLocation, Params, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
    MsgBox('Failed to register dll.' + #13#10 + SysErrorMessage(ResultCode), mbError, MB_OK);
end;

procedure UnRegisterEATeXLibrary(DllLocation: String);
var
  Params: String;
  ResultCode: Integer;
begin
  Params := '/u ' + AddQuotes(DllLocation);

  if not ShellExec('', GetRegAsmLocation, Params, '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode) then
    MsgBox('Failed to unregister dll.' + #13#10 + SysErrorMessage(ResultCode), mbError, MB_OK);
end;

procedure InitializeWizard;
begin
  if not IsEAInstalled then
  begin
    MsgBox('This application requires Sparx Systems Enterprise Architect to be installed on this computer.' + #13#10 + 'Installation aborted.', mbError, MB_OK);
    Abort;
  end;

  if IsMikTexInstalled then
    Exit;

  MikTexTempName := ExpandConstant('{tmp}\miktex_installer.exe');
  ITD_Init;

  if IsWin64 then
    ITD_AddFile(MikTex64BitDownloadUrl, MikTexTempName)
  else
    ITD_AddFile(MikTex32BitDownloadUrl, MikTexTempName);

  ITD_DownloadAfter(wpWelcome);
  ITD_AfterSuccess := @RunMikTexInstaller;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
    RegisterEATeXLibrary(ExpandConstant('{app}\EATeX.dll'));
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
    UnRegisterEATeXLibrary(ExpandConstant('{app}\EATeX.dll'));
end;