# Project Fractal413
[![Supported by the No Man's Sky Community Developers & Designers](https://raw.githubusercontent.com/NMSCD/About/master/badge/green-ftb.svg)](https://github.com/NMSCD)

Project Fractal 413 is an application which installs No Man's Sky 4.13 (Fractal Update) from Steam using DepotDownloader, and converts it to the GOG 4.13 version which includes PDB (Program Database) files and Debug Version of No Man's Sky.

# Background
On February 22nd 2023, Hello Games released the Fractal update 4.10 on GOG.com. This version of the game included an *nms.pdb* file, as well as a debug version of No Man's Sky titled *XGOG Release_x64.exe*. These assets were also included in version 4.12 released March 1st 2023, and version 4.13 released March 8th 2023, publicly available to all users who purchased No Man's Sky through GOG.com. However in subsequent GOG releases, and Steam releases, these files were no longer included.

# Purpose
The *nms.pdb* file has been invaluable to the modding community, as it provides a map to all the types, objects, classes and functions in the NMS.EXE game engine, which has enabled the creation of advanced mods and tools which support NMS 4.13, such as [NMS.py](https://github.com/monkeyman192/NMS.py) by monkeyman192 and [ReNMS](https://github.com/VITALISED/renms) by VITALISED. However these mods target the 4.13 version which is difficult to install and obtain for the average user, which is why this isntaller has been created.

Additionally, as these files are no longer easily available, this project serves as a *software preservation* effort.

# How it Works
For the first step - Download - the installer requires the user to have **a valid, purchased copy of No Man's Sky on Steam and provide their own Steam account details which the installer uses to download the game from Steam.** 

The second step - Restore - is accomplished by applying 4 [xdelta]((https://github.com/jmacd/xdelta)) patches to the Steam version of NMS.exe 4.13 to restore it to the GOG version, *NMS.exe.xdelta3, nms.pdb.xdelta3, XGOG Release_x64.exe.xdelta3, xgog release_x64.pdb.xdelta3*. The Steam executable serves as a key to unlock these files, and so the pdb and debug version cannot be used without a legally purchased copy of No Man's Sky from Steam. Additional small support and library files are bundled with the installer and copied in this step also.

The last step creates shortcuts to run either version of the game.

# Disclaimer
The creators of the application do not condone or support piracy, and users are reminded that it is illegal to distribute or use pirated software.

The use of this application is at the user's own risk, and the creators of the application cannot be held responsible for any damages that may result from its use. This includes, but is not limited to, any damage to the user's computer, loss of data, or legal repercussions.

Users are advised to exercise caution when using this application and to ensure that they are complying with all applicable laws and regulations. The creators of the application are not responsible for any misuse or illegal activity associated with the use of this application.

# Tools used and Included
SteamKit2 and DepotDownloader by OpenSteamworks - https://github.com/SteamRE
LICENSE: GNU General Public License v2.0
- Used to login to steam and download the old version of the game.

XDelta by jmacd - https://github.com/jmacd/xdelta
LICENSE: Apache Public License version 2.0
- Used to apply xdelta3 patches

RunAsDate by nirsoft - https://www.nirsoft.net/utils/run_as_date.html
LICENSE: Freeware
- Used to bypass cutoff date for Debug Version

SmartSaveFolder by qjimbo - https://github.com/qjimbo/smartsavefolder
LICENSE: MIT License
- Helper application to redirect NMS save game folder.

# See Also
No Man's Sky Legacy Version installer - https://github.com/qjimbo/NMSLegacyVersionInstaller/
