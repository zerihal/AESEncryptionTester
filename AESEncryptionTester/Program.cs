using AESEncryptionTestUtils;

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
        public static void Main(string[] args)
        {
            var testManager = new AESTestManager();
            testManager.RunTests(new AESTestSettings());
        }
    }
}
