# AES Encryption Tester

This simple test program is taken from the Microsoft example of AES encryption using the .NET AES class, and 
tests performance (encryption and decryption time) with 128, 192, and 256 bit encyption against over a number
of rounds/phases using a different randomly generated block of text for each one.

See https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0 for details.

Note: Random text size (in chars) and number of phases are set by the static _randomTextSize and _noPhases 
variables respectively.
