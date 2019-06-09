set defaultResourcesDir="Assets\Mods\Default"
set gameModsDir="build\Mods\Default"

if exist %gameModsDir% del /q %gameModsDir%

xcopy /s /e /y %defaultResourcesDir% %gameModsDir%
del /s /q /f %gameModsDir%\*.meta
