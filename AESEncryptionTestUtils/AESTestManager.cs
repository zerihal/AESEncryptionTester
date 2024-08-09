using AESEncryptionTestUtils.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AESEncryptionTestUtils
{
    public class AESTestManager
    {
        private readonly Stopwatch _sw = new Stopwatch();
        private readonly Dictionary<int, double> _keys = new Dictionary<int, double> { { 128, 0 }, { 192, 0 }, { 256, 0 } };

        public IAesTestResult RunTests(AESTestSettings settings)
        {
            Console.WriteLine($"Beginning encryption and decryption phases - {settings.NoPhases} rounds\n");

            var initRound = settings.Mode == TestMode.File ? 1 : 0;

            for (var round = initRound; round <= 1; round++)
            {
                if (round == 0)
                    Console.WriteLine($"*** ENCRYPTION TEST WITH RANDOMLY GENERATED STRING ***\n");
                else if (round == 1)
                    Console.WriteLine($"*** ENCRYPTION TEST WITH SIMPLE GENERATED FILE (100MB) ***\n");

                for (var x = 0; x < settings.NoPhases; x++)
                {
                    // The first phase does not seem to give accurate results due to test intialisation (app in memory) - this needs 
                    // to be done to prep in debug at least for further test phases, but output and results should be ignored.
                    if (x == 0)
                        Console.WriteLine("Prep phase (output ignored)");
                    else
                        Console.WriteLine($"Phase {x} ...");

                    // Generate some random text or use file (same to be used for each key size test)
                    object? input = null;
                    var tempfile = string.Empty;

                    if (round == 0)
                    {
                        input = EncryptionHelper.GenerateRandomPlaintext(settings.RandomTextSize);
                    }
                    else if (round == 1)
                    {
                        if (settings.Mode == TestMode.File && settings.TestFile != null)
                        {
                            input = File.Open(settings.TestFile, FileMode.Open);
                        }
                        else
                        {
                            tempfile = EncryptionHelper.GenerateFile(100);
                            input = File.Open(tempfile, FileMode.Open);
                        }
                    }

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

                        // Simulate 100 messages (encyption and descryptions of the data)
                        for (var i2 = 0; i2 < 100; i2++)
                        {
                            // Encrypt the string to an array of bytes.
                            var encrypted = EncryptionHelper.EncryptStringToBytes_Aes(input, tempAes.Key, tempAes.IV);

                            // Decrypt the bytes to a string.
                            var decrypted = EncryptionHelper.DecryptStringFromBytes_Aes(encrypted, tempAes.Key, tempAes.IV);
                        }

                        _sw.Stop();

                        // Dispose AES resources
                        tempAes.Clear();
                        tempAes?.Dispose();

                        // Cleanup file if applicable
                        if (round == 1 && input is FileStream fs)
                        {
                            fs.Close();

                            if (settings.Mode != TestMode.File && File.Exists(tempfile))
                                File.Delete(tempfile);
                        }

                        if (x != 0)
                        {
                            _keys[key.Key] += _sw.Elapsed.TotalMilliseconds;
                            Console.WriteLine($"Elapsed time: {_sw.Elapsed.TotalMilliseconds} ms");
                        }
                    }
                }

                Console.WriteLine($"\n");
            }

            foreach (var key in _keys)
            {
                Console.WriteLine($"Total operation time for {key.Key} bit key = {_keys[key.Key]}");
            }

            return new AesTestResult(_keys);
        }
    }
}
