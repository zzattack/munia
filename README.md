MUNIA is a device that acquires input packets from controllers of various videogame consoles and shows them on your PC with the controller simultaneously remaining attached to its console. This allows you to display your inputs on gameplay videos or livestream content.
Devices for several consoles have been developed under this project:
* Nintendo Gamecube
* Sony Playstation 2
* Nintendo 64
* Super Nintendo

# Repository overview
* The hardware folder contains schematics for the various devices developed under this project. These include input adapters for Nintendo Gamecube, Nintendo 64, Super Nintendo, Sony PlayStation 2 and hybrids of the aforementioned.
* The firmware folder contains firmware projects for each of the circuit boards presented in the hardware folder.
* The munia.io folder contains the Django project powering the http://munia.io website (not functional).
* The MUNIA-win folder contains the .NET project 


## FAQ
Q. I don’t like the default skins, can I change them?
Absolutely. The software has a very strong SVG based theming engine. You can create a new one using tools like Adobe Illustrator or Inkscape. Custom XML attributes are used to map buttons to graphical elements. It’s pretty simple, honestly; take a look in SKINS.md!

Q. What does ‘MUNIA’ mean?
The very first iteration of the device supported SNES, N64 and NGC simultaneously. The acronym stands for ‘Multi-use Nintendo Input Adapter’. Both the Multi-use and Nintendo parts no longer apply to most of our devices, so we’ve parted with the acronym but kept the pretty name.

Q. Will you support console ‘X’?
Unless mentioned on the index page, the likely answer is no. Newer generation consoles are usually too complicated. The low expected demand for adapters for older generation consoles doesn’t warrant the required development investments at the moment.

Q. How does this compare to an Arduino input display?
Favorably. Benefits include not having to splice any cables, the device showing up as HID device on your computer, superior software, more stable firmware, ability to use the controller on our PC in e.g. emulators and better 3rd party controller support. In the end, though, both work and the MUNIA software also supports inputs from Arduino-based NintendoSpy devices!
