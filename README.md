# Project Fractal413
![logo](https://github.com/NMSCD/Fractal413/assets/21266513/743ee9bf-a5d1-4f27-a2b5-9539318d0ed4)
[![Supported by the No Man's Sky Community Developers & Designers](https://raw.githubusercontent.com/NMSCD/About/master/badge/green-ftb.svg)](https://github.com/NMSCD)

[![License: MIT](https://img.shields.io/github/license/NMSCD/Fractal413)](https://github.com/NMSCD/Fractal413/blob/main/LICENSE)
[![Version](https://img.shields.io/github/v/release/NMSCD/Fractal413?color=7a39fb)](https://github.com/NMSCD/Fractal413/releases/latest)

Project Fractal 413 is an application which installs No Man's Sky 4.13 (Fractal Update) from Steam using DepotDownloader, and converts it to the GOG 4.13 version which includes PDB (Program Database) files and Debug Version of No Man's Sky.

| ![debug_version](https://github.com/NMSCD/Fractal413/assets/21266513/a8a294dc-6e38-47e1-88b8-3bc1e321288d)|
|:-------------------------------------------:|
| *No Man's Sky 4.13 Debug Version*           |

# Download
Download the latest version of the installer [here](https://github.com/NMSCD/Fractal413/releases/latest).

# Purpose
## Support Projects that target No Man's Sky 4.13
The installer makes it easier for users to install the 4.13 version which is supported by modding projects such as:
* [NMS.py](https://github.com/monkeyman192/NMS.py) by monkeyman192
  - A Python hooking and modding library for No Man's Sky. It provides tools for reading, modifying, and writing game data.

* [ReNMS](https://github.com/VITALISED/renms) by VITALISED
  - A No Man's Sky SDK modding framework and decompilation project. It offers access to the game's runtime memory, supports dynamic plugin loading, and includes a header generator for metadata classes.

## Debugging Tools
The Debug Version of No Man's Sky provides terrain editing, model viewing and other capabilities which are useful for users looking to create mods for the game and preview their changes. Debug features are accessed with the ` key on the keyboard (typically located below the ESC key).

## Debug Files
The *nms.pdb* file has been invaluable to the modding community, as it provides a map to all the types, objects, classes and functions in the NMS.EXE game engine, helpful for creating lower level more advanced mods. Additionally, as these files are no longer easily available, this project serves as a *software preservation* effort.

# Background
On February 22nd 2023, Hello Games released the Fractal update 4.10 on GOG.com. This version of the game included an *nms.pdb* file, as well as a debug version of No Man's Sky titled *XGOG Release_x64.exe*. These assets were also included in version 4.12 released March 1st 2023, and version 4.13 released March 8th 2023, publicly available to all users who purchased No Man's Sky through GOG.com. However in subsequent GOG releases, and Steam releases, these files were no longer included.

The debug tools were first discovered and brought to light by [RayRod](https://www.youtube.com/@rayrodtv) who went on to share information how to use the features of the debug version. The first mod that used the *nms.pdb* file was Lapig for his [Native Gyro on PC](https://www.nexusmods.com/nomanssky/mods/2665). Without RayRod it's likely that this special version would have been missed, so thank you! 🙏

# How it Works
For the first step - **Download** - the installer requires the user to have **a valid, purchased copy of No Man's Sky on Steam and provide their own Steam account details which the installer uses to download the game from Steam.** 

![patch](https://github.com/NMSCD/Fractal413/assets/21266513/716aba41-0af7-45e7-a5b5-c0f4b819a7f2)

The second step - **Restore** - is accomplished by applying [xdelta](https://github.com/jmacd/xdelta) patches in [VCDIFF format](https://en.wikipedia.org/wiki/VCDIFF) to the Steam version of NMS.exe 4.13 to restore it to the GOG version, *NMS.exe.xdelta3, nms.pdb.xdelta3, XGOG Release_x64.exe.xdelta3, xgog release_x64.pdb.xdelta3*.

**The Steam executable serves as a key to unlock these files**. The pdb and debug version cannot be recovered or used without a legally purchased copy of No Man's Sky from Steam.

Additional small support and library files are bundled with the installer and copied in this step also, and finally the installer creates shortcuts to run either version of the game.

# Disclaimer
The creators of the application do not condone or support piracy, and users are reminded that it is illegal to distribute or use pirated software.

The use of this application is at the user's own risk, and the creators of the application cannot be held responsible for any damages that may result from its use. This includes, but is not limited to, any damage to the user's computer, loss of data, or legal repercussions.

Users are advised to exercise caution when using this application and to ensure that they are complying with all applicable laws and regulations. The creators of the application are not responsible for any misuse or illegal activity associated with the use of this application.

# Tools Used and Included
* SteamKit2 and DepotDownloader by OpenSteamworks - https://github.com/SteamRE
  * LICENSE: GNU General Public License v2.0
  * Used to login to steam and download the old version of the game.
* XDelta by jmacd - https://github.com/jmacd/xdelta
  * LICENSE: Apache Public License version 2.0
  * Used to apply xdelta3 patches
* RunAsDate by nirsoft - https://www.nirsoft.net/utils/run_as_date.html
  * LICENSE: Freeware
  * Used to bypass cutoff date for Debug Version
* SmartSaveFolder by qjimbo - https://github.com/qjimbo/smartsavefolder
  * LICENSE: MIT License
  * Helper application to redirect NMS save game folder.

# See Also
* No Man's Sky Legacy Version installer - https://github.com/qjimbo/NMSLegacyVersionInstaller/
* TerrariaDepotDownloader - https://github.com/RussDev7/TerrariaDepotDownloader/
