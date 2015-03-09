:: Remove quotes
SET _projectDir=###%1###
SET _projectDir=%_projectDir:"###=%
SET _projectDir=%_projectDir:###"=%
SET _projectDir=%_projectDir:###=%



:: DictionaryDashboard
MKDIR "%_projectDir%..\Release"
MKDIR "%_projectDir%..\Release\Packages"

RD "%_projectDir%..\Release\Packages\DictionaryDashboard" /S /Q
MKDIR "%_projectDir%..\Release\Packages\DictionaryDashboard"

XCOPY "%_projectDir%bin\Futuristic.*.dll" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R
XCOPY "%_projectDir%Images\*" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /S /E /R
XCOPY "%_projectDir%Usercontrols\*.ascx" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /S /E /R
XCOPY "%_projectDir%Package\package.xml" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R
XCOPY "%_projectDir%Package\readme.txt" "%_projectDir%..\Release\Packages\DictionaryDashboard" /Y /R

"C:\Program Files\WinRAR\winrar.exe" a -afzip -r -ep1 -m1 "%_projectDir%..\Release\Packages\DictionaryDashboard\Futuristic.DictionaryDashboard-0.9.3.zip" "%_projectDir%..\Release\Packages\DictionaryDashboard\*"

