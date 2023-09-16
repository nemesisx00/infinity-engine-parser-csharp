# infinity-engine-parser

An open source C# library for reading Infinity Engine game files.

## Unmaintained

As I've shifted to building a Rust library to handle this functionality, I'm not super interested in continuing to implement this in C#. But there's no reason to keep everything I've done so far to myself.

## Current State

All the information about the file types used to create this project comes from: https://gibberlings3.github.io/iesdp/

KEY and BIF parsing works, including both compressed forms of BIF.

BMP *parsing* works though the decoding isn't quite there. The decoding only really works for 24-bit images, since 24 bit encoding doesn't really require decoding. Never got around to getting the other bit depths working properly. They technically result in bytes that form an image but very much not the correct or expected image.

I had only just started BAM parsing so it isn't fully implemented.

Game installation path detection is only implemented for Windows but what is implemented should be fully functional.
