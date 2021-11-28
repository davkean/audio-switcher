Audio Switcher
==============

Audio Switcher is a simple app that runs in the notification area (system tray) and lets you easily switch the default input and output audio devices.

Tired of having to perform a song and dance through the Windows Sound dialogs just to switch from your speakers to your headphones? Then this app is for you.

![ScreenShot](resources/AudioSwitcher.png)

## How do I install this thing?
Currently, there is no installer or binary that you can run or download. You'll need to build the project yourself. Open the solution in Visual Studio 2013, build and then run `AudioSwitcher.exe` from the bin directory.

## FAQ
#### How do I hide microphones or unplugged devices?
Right-click on the __Audio Switcher__ icon in the notification area, and expand __Appearance__.

#### In the screenshot above, I see that you have given the devices custom names and icons. How do you do that?
Right-click on the __Sound__ icon in the notification area, choose __Playback devices__. Double-click any device to customize its name and icon.

## Acknowledgements
Paul Betts ([@paulcbetts](http://github.com/paulcbetts))
* [#22](https://github.com/davkean/audio-switcher/issues/22): Provided Squirrel for Windows setup

Ian van der Linde ([@Ianvdl](http://github.com/ianvdl))
* [#4](https://github.com/davkean/audio-switcher/issues/4): Provided the headphones tray icon

Abdallah Gomah ([@Abdallah-Gomah](http://www.codeproject.com/Members/Abdallah-Gomah))
* Provided the code to [extract large icons from native resources](http://www.codeproject.com/Articles/32617/Extracting-Icons-from-EXE-DLL-and-Icon-Manipulatio)

NAudio (http://naudio.codeplex.com)
* Provided the original code to manipulate and query the Windows Core Audio API
 
svotar (https://code.google.com/p/szotar)
* Provided the original code for the native Toolstrip renderer 
