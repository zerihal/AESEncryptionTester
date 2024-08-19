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
        private static string _defaults = "defaults.aestester";
        private static string _defaultTestSettings = "AESDefaultTestSettings.xml";

        public static void Main(string[] args)
        {
            Console.WriteLine("AES Encryption Tester");
            Console.WriteLine("**************************");

            var defaultSettingsFile = GetDefaultSettings();

            if (string.IsNullOrWhiteSpace(defaultSettingsFile))
            {
                AESTestSettings.CreateDefaultTestSettings(_defaultTestSettings);
                SetDefaultSettings(_defaultTestSettings);
                defaultSettingsFile = _defaultTestSettings;
            }

            var useExisting = false;

            if (!string.IsNullOrWhiteSpace(defaultSettingsFile))
            {
                ConsoleKeyInfo cki;

                Console.WriteLine($"Default settings file is {defaultSettingsFile}\nDo you wish to continue using this file? [Y/N]");

                while (true)
                {
                    cki = Console.ReadKey();

                    if (cki.Key == ConsoleKey.Y || cki.Key == ConsoleKey.N)
                        break;
                }

                if (cki.Key == ConsoleKey.Y)
                    useExisting = true;

                Console.WriteLine("");
            }
            
            if (!useExisting)
            {
                Console.WriteLine("Specify settings filename and path (this must be an XML file with settings elements):");
                defaultSettingsFile = Console.ReadLine();

                if (defaultSettingsFile != null)
                    SetDefaultSettings(defaultSettingsFile);
            }
            
            var settings = AESTestSettings.LoadSettings(defaultSettingsFile, out var defaultUsed);
            OutputSettings(settings, defaultUsed);

            var testManager = new AESTestManager();
            Console.WriteLine("Starting encryption tests ...");
            var res = testManager.RunTests(settings);

            if (settings.OutputFile != null)
                res.WriteToFile(settings.OutputFile);

            Console.WriteLine("Tests completed\nPress any key to exit");
            Console.ReadKey();
        }

        private static string GetDefaultSettings()
        {
            if (File.Exists(_defaults))
            {
                var lines = File.ReadAllLines(_defaults);

                if (lines.Length > 0)
                    return lines[0];
            }

            return string.Empty;
        }

        private static void SetDefaultSettings(string settingsFile)
        {
            if (File.Exists(_defaults))
            {
                var lines = File.ReadAllLines(_defaults);
                var updateLn = false;

                if (lines.Length > 0)
                {
                    lines[0] = settingsFile;
                    File.WriteAllLines(_defaults, lines);
                    updateLn = true;
                }

                File.WriteAllLines(_defaults, updateLn ? lines : [settingsFile] );
            }
            else
            {
                File.WriteAllText(_defaults, settingsFile);
            }
        }

        private static void OutputSettings(AESTestSettings settings, bool isDefault)
        {
            if (isDefault)
                Console.WriteLine("*** Default settings were used as settings file could not be loaded ***");

            Console.WriteLine("TEST SETTINGS");
            Console.WriteLine($"Test Mode: {settings.Mode}");
            Console.WriteLine($"Test phases: {settings.NoPhases}");
            Console.WriteLine($"Messages: {settings.NoMessages}");

            if (settings.Mode == TestMode.File)
                Console.WriteLine($"Test file: {settings.TestFile}");
            else
                Console.WriteLine($"Random text size: {settings.RandomTextSize} chars");

            Console.WriteLine($"Output file: {settings.OutputFile}");

            Console.WriteLine("");
        }
    }
}
