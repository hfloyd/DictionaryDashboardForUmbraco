:: Remove quotes
SET _projectDir=###%1###
SET _projectDir=%_projectDir:"###=%
SET _projectDir=%_projectDir:###"=%
SET _projectDir=%_projectDir:###=%

:: Version number & Zip application
SET _versionNum="1.0"
SET _zipAppPath=7Zip

:: DictionaryDashboard
MKDIR "%_projectDir%..\Release"
MKDIR "%_projectDir%..\Release\Packages"

RD "%_projectDir%..\Release\Packages\DictionaryDashboard" /S /Q
MKDIR "%_projectDir%..\Release\Packages\DictionaryDashboard"

XCOPY "%_projectDir%bin\DictionaryDashboard.dll" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R
XCOPY "%_projectDir%Images\*" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /S /E /R
XCOPY "%_projectDir%Usercontrols\*.ascx" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /S /E /R
XCOPY "%_projectDir%Package\package.xml" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R
XCOPY "%_projectDir%Package\readme.txt" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R

::ECHO %_zipAppPath%
IF %_zipAppPath%==WinRar "C:\Program Files\WinRAR\winrar.exe" a -afzip -r -ep1 -m1 "%_projectDir%..\Release\Packages\DictionaryDashboard\DictionaryDashboard-%_versionNum%.zip" "%_projectDir%..\Release\Packages\DictionaryDashboard\*"
IF %_zipAppPath%==7Zip "C:\Program Files\7-Zip\7z.exe" a -tzip -r  "%_projectDir%..\Release\Packages\DictionaryDashboard\DictionaryDashboard-%_versionNum%.zip" "%_projectDir%..\Release\Packages\DictionaryDashboard\*"

PAUSE