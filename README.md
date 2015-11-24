# MegaROM

Checksum validator for Sega Megadrive/Genesis ROM (.md) files. It's a CLI application written in C# around a core library that I was building as the basis of a suite of ROM hacking tools. I'm not overly happy with the core, however, so I'm not sure how much further this project will go ... the language choice was definintely not right for this kind of application. I miss some of the lower level forgiveness that a language like C gives you when working with binary data. Felt like I had to really wrestle with the capabilities of C# to achieve what I wanted and consequently I'm not that happy with the code.

To use the validator, simply execute from the command line, passing the path to the ROM file to be validated. If the checksum is invalid, it will recalculate and write to the original ROM, creating a backup with the extension .bak in the same directory.

Example,

`ChecksumValidator.exe "c:\roms\Road Rash II (USA, Europe).md"`
