# Retro
A portable front end for various emulators, which assumes that you actually don't much care what system and region a game is, you just want to see the choices for a particular search (eg - "Road Rash") and just see the best region version in your collection.

Compile, wang the xmls in the bin folders, and the emulators and roms unzipped in the folders implied by the xmls.

Retro scans the rom folders and compiles the list of games.  Double clicking a game launches the emulator.  It works with any emulator that can be passed a game in the title, the xml can support extra parmas to force fullscreen etc if required (Look at epsxe for an example in the default xml).

Retro relies on the names of the files and tries to parse the region out using the standard scene notifications (Eg - (JUE)).  It will only present the version of the game found with the "best" region (see regions.xml).  Broadly in the default config this is "60hz English speaking countries, 50hz english speaking countries, European countries, Japan, China and other non-roman language countries" in that order.

It has a search box to narrow things down, this does not currently support any wildcards and simply matches the string as typed as part of the game.

Because the paths given are always relative to the launch folder, Retro is portable, my copy is on a usb stick.

Limitations : About eleventy thousand but none that affect my use right now.

I'd like to offer a right click choice of region.
I'd like a front end for config.
I'd like to be able to just launch the emulator for a system.
I'd like to be smarter about file extensions
Ideally I'd also like to be able to discover the emulators in the file sytem without the xml and recognise the popular ones.
You could also consider playing with deep scanning games but you wouldn't want to do that every time, right now Retro starts up quick and is always up to date.
