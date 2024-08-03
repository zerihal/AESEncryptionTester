using System.Diagnostics;
using System.Security.Cryptography;

namespace AESEncryptionTester
{
    /// <summary>
    /// This simple test program is taken from the Microsoft example of AES encryption using the .NET AES class, and 
    /// tests performance (encryption and decryption time) with 128, 192, and 256 bit encyption against over a number
    /// of rounds/phases using a different randomly generated block of text for each one.
    /// See https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0 for details.
    /// </summary>
    class Program
    {
        private static Stopwatch _sw = new Stopwatch();
        private static int _operations = 150;
        private static Dictionary<int, double> _keys = new Dictionary<int, double> { { 128, 0 }, { 192, 0 }, { 256, 0 } };
        private static int _randomTextSize = 1000;
        private static int _noPhases = 10;

        public static void Main(string[] args)
        {
            Console.WriteLine($"Beginning encryption and decryption phases - {_operations} ops");

            for (var x = 0; x < _noPhases; x++)
            {
                // The first phase does not give accurate results due to initial object generation - this needs to be
                // done to prep for further test phases, but output and results should be ignored.
                if (x == 0)
                    Console.WriteLine("Prep phase (output ignored)");
                else
                    Console.WriteLine($"Phase {x} ...");

                // Generate some random text (same text to be used for each key size test)
                var ranStr = EncryptionHelper.GenerateRandomPlaintext(_randomTextSize);

                foreach (var key in _keys)
                {
                    if (x != 0)
                        Console.WriteLine($"Using key length of {key.Key}-bit");

                    _sw.Restart();

                    // Create new instance of AES
                    var tempAes = Aes.Create();

                    // Create a new instance of the Aes class. This generates a new key and initialization vector (IV).
                    tempAes.KeySize = key.Key;
                    tempAes.GenerateKey();

                    for (var i = 0; i < 50; i++)
                    {
                        // Encrypt the string to an array of bytes.
                        byte[] encrypted = EncryptionHelper.EncryptStringToBytes_Aes(ranStr, tempAes.Key, tempAes.IV);

                        // Decrypt the bytes to a string.
                        string roundtrip = EncryptionHelper.DecryptStringFromBytes_Aes(encrypted, tempAes.Key, tempAes.IV);

                        //Display the original data and the decrypted data.
                        //Console.WriteLine("Original:   {0}", original);
                        //Console.WriteLine("Round Trip: {0}", roundtrip);
                    }

                    _sw.Stop();

                    // Dispose AES resources
                    tempAes.Clear();
                    tempAes?.Dispose();

                    if (x != 0)
                    {
                        _keys[key.Key] += _sw.Elapsed.TotalMilliseconds;
                        Console.WriteLine($"Elapsed time: {_sw.Elapsed.TotalMilliseconds} ms");
                    }
                }
            }

            foreach (var key in _keys)
            {
                Console.WriteLine($"Total operation time for {key.Key} bit key = {_keys[key.Key]}");
            }
        }
    }
}
