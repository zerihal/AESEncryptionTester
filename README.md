# AES Encryption Tester

This simple test program is taken from the Microsoft example of AES encryption using the .NET AES class, and 
tests performance (encryption and decryption time) with 128, 192, and 256 bit encyption against over a number
of rounds/phases using a different randomly generated block of text for each one.

See https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0 for details.

# Operation

The test application will use the following settings to perform encryption tests (taken from settings file, which is generated if not present):

Test mode:
  0: Default - Randomly generated plaintext and generated file
  1: File - Specified file
  2: Plaintext only - Randomly generated plaintext only

Random text size - Size of randomly generated text in characters (default 1000)

Phases - Number of encryption and decryption phases, performed for each key size and averaged (default 10)

Messages - Number of simulated messages for encryption and decryption

Test file - File to test encryption and decryption on (full filename and path if not in app root). Note that if specified, test mode will be set to 1, but can be left empty for other modes

Output file - Full filename and path (if not in app root) for the csv output file (default AESTestResults.csv)

Running the application will perform encryption and decryption tests using the same input (plaintext or file) for each key size, aggregating the timing results (in milliseconds) for each phase of testing and then averaging the timings over the number of phases in the output file (console will report both). If the application is run multiple times, each test run will be appended to the output file and the overall averages for all test runs recalculated each time.
