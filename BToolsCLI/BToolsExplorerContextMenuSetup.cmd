@echo off
title BTools Explorer Context Menu >nul


:: Copy registry files to the TEMP folder.

xcopy /s /y . "%ProgramData%\BTools\"


:: Make sure this script runs from local disk to workaround permission issues.

cd /D "%ProgramData%\BTools\"


:: Install

@REM setx PATH "%PATH%;%ProgramData%\BTools"

regedit /S "%ProgramData%\BTools\BToolsExplorerContextMenu.reg"

echo BTools Explorer Context Menu is installed.


:: Done

pause >nul

exit
