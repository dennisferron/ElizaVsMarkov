# ElizaVsMarkov

A game where you try to coach a Markov chat bot in its conversation with an ELIZA chat bot. 

## Description

The player must help Simple Markov convince ELIZA it's not crazy.  This won't be easy, as ELIZA is using her DOCTOR template whereas Markov has been reading Lovecraft.

The game is a Windows 10 desktop application, but no installation is required.

## Getting Started

### Building

Use Visual Studio 2022 Community Edition (or newer) to build.  Requires C# and .NET development packs installed with Visual Studio.

### Dependencies

* Relies on the DryWetMIDI .NET library (nuget package) for music.  
* Uses ELIZA.NET chatbot by Kris Craig (is included).

### Installing

This is a .NET application supporting so-called "XCOPY deployment".  After building it, the game requires the files in its local folder, but there is no installer and no installation required.

If you download the compiled version from itch.io you may get a warning screen that starts with "Windows Protected Your PC" - don't worry the executable is safe to run.  Just click "more info" and "Run Anyway."  It's just because I don't have a code-signing certificate to prove to Windows 10 that it's published software.

### Executing program

Simply start the ElizaVsMarkov.exe program from within its bin folder.

## Help

Known bug:  background music playback doesn't loop back to start after completing, and stopping and restarting the music doesn't reset it.

## Authors

Dennis Ferron

## Version History

* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE file for details

## Acknowledgments

Inspiration, code snippets, etc.
* [README-Template](https://gist.github.com/DomPizzie/7a5ff55ffa9081f2de27c315f5018afc)
* [ELIZA.NET by Kris Craig] (https://github.com/sirkris/ELIZA.NET)
* Markov chatbot original work by myself, Dennis Ferron
* [Training corpus At the Mountains of Madness (public domain) by H.P. Lovecraft] (https://github.com/vilmibm/lovecraftcorpus)
* [FloatOn.MID, author of arrangement unknown] (https://freemidi.org/download3-24471-float-on-modest-mouse)
* [Most images generated using Craiyon image generator] (https://www.craiyon.com/)
