# Save File Format (Version 1)

Game saves have the following format (as defined by `PlayerData.GetBytes()`):

```
HEADER (16 bytes): TAG MAGIC VERSION
  DATA (16 bytes): DEATHS COLLISIONS PROGRESS PADDING PLAY_TIME
```

## Header Format

The file header is always 16 bytes long and should be exactly the following:

```
8b8a 5341 5645 4d45 0d0a 1a0a 0100 0000
```

The first two bytes (`8b8a`) are equivalent to the 16-bit unsigned integer `35467` (`0x8a8b`). If this sequence is
incorrect the file may have been written by a big-endian system or written using a 7-bit (i.e., ASCII only) scheme.
Both of these cases are rare.

The following 10 bytes are the exact ASCII string `"SAVEME\r\n\x1a\n"`. Here the text `"SAVEME"` is just an indicator
that this is a game save file (as opposed to a PNG, Java source code file, or anything else). The following text `\r\n`
is a MS-DOS/Microsoft Windows style line-ending. If the file is read inproperly (e.g., using a file API that converts
line-endings) on a POSIX system this will likely be coverted to `"\n"` and considered invalid. Next, the text `"\x1a"`
is the hexadecimal escape for the ASCII "substitute" character. Many older systems treat this character as indicating
the end of a file. This is another rare case. Finally, the trailing `"\n"` is a POSIX style line-ending and serves
the same purpose as the previous Microsoft style line-ending. If the file is read improperly on a Microsoft system then
this character will likely be converted to `"\r\n"`.

The final four bytes are the file version. This should always be the 32-bit unsigned value `1`. Future
versions of this file type may use different values to indicate their structure.

## Data Format

The file data is always a 16 byte sequence consisting of unsigned integers. `DEATHS` stores the number of times the
player  has died. It is a 16-bit (two byte) field. `COLLISIONS` stores the number of times the player has collided
with an object during play. It is a 16-bit field. `PROGRESS` stores the maximum stage the player has reached during
play. It is a 16-bit field. `FLAGS` is a 16-bit wide space that stores up to 16 player flag values. Currently, only
the first bit is used. `FLAGS` bit `0` controls whether or not the game should present a tutorial level to the player.
Finally, `PLAY_TIME` is a 64-bit (eight byte) field that stores the current elapsed play time in seconds.


## Remarks

The `PlayerData.FromBytes()` method will convert any 32 byte buffer with the above format into a valid `PlayerData`
object. The exact origin of this buffer (i.e., whether it's read from a storage device or constructed in RAM) is
irrelevant. Similarly, the `PlayerData.GetBytes()` method will return a properly formated 32 byte buffer appropriate
for writing play data to a storage medium. Therefore, calling `PlayerData.FromBytes()` with a buffer returned by
`PlayerData.GetBytes()` will always create a valid duplicate of the original `PlayerData` object.
