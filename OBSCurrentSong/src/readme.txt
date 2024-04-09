================================================================
= Thank you for downloading my version/fork of OBSCurrentSong! =
================================================================

Unpack the files anywhere you have Read/Write access. It will
create a folder in which you can find the text files that you
want to reference in your OBS scene.

================================================================
= Configuration ================================================
================================================================

Open 'config.json' in your prefered text-editor to change how
the program outputs the current song text.

"waittime": Time in milliseconds
The 'refresh rate' of the program. Lower it to reduce the delay
in switching songs, or raise it to reduce the load that the
program puts on your CPU.

"trimming": true/false
Enable to shorten the song title by removing
additional information like '2017 Remaster' or 'Vinyl Version'

"prefix": Text string
Text, symbols, or characters to add at the start of the output.

"separator": Text string
Text, symbols, or characters to add inbetween the song title and
the name of the artist.

"postfix": Text string
Text, symbols, or characters to add at the end of the output.
