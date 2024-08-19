using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace AESEncryptionTestUtils
{
    public class AESTestSettings
    {
        private string? _file;

        /// <summary>
        /// Random text size (characters).
        /// </summary>
        public int RandomTextSize {  get; set; }

        /// <summary>
        /// Number of test phases.
        /// </summary>
        public int NoPhases { get; set; }

        /// <summary>
        /// Number of messages (encryption and decryption) simulated per phase.
        /// </summary>
        public int NoMessages { get; set; } = 100;

        /// <summary>
        /// Test mode.
        /// </summary>
        public TestMode Mode { get; private set; } = TestMode.Default;

        /// <summary>
        /// Test file (for encryption and decryption testing). Note that if file is specified then the test
        /// mode will be set to file, or default if it does not exist.
        /// </summary>
        public string? TestFile 
        { 
            get => _file; 
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && File.Exists(value))
                {
                    _file = value;
                    Mode = TestMode.File;
                }
                else
                {
                    Mode = TestMode.Default;
                }
            }
        }

        /// <summary>
        /// Output file for test results.
        /// </summary>
        public string? OutputFile { get; set; }

        public AESTestSettings(string testFile, int randomTextSize = -1, int noPhases = -1) : this()
        {
            if (randomTextSize > 0)
                RandomTextSize = randomTextSize;

            if (noPhases > 0)
                NoPhases = noPhases;

            TestFile = testFile;
        }

        public AESTestSettings(int randomTextSize, int noPhases)
        {
            RandomTextSize = randomTextSize;
            NoPhases = noPhases;
        }

        public AESTestSettings() 
        {
            RandomTextSize = 1000;
            NoPhases = 10;
        }

        /// <summary>
        /// Loads an XML settings file.
        /// </summary>
        /// <param name="settingsFile">Filename and path.</param>
        /// <returns>AESTestSettings (or default settings if file not found or malformed).</returns>
        public static AESTestSettings LoadSettings(string? settingsFile, out bool defaultUsed)
        {
            defaultUsed = true;
            var settings = new AESTestSettings();

            if (File.Exists(settingsFile) && Path.GetExtension(settingsFile) == ".xml")
            {
                try
                {
                    var xDoc = XDocument.Load(settingsFile);
                    var root = xDoc.Root;

                    if (root != null)
                    {
                        if (int.TryParse(root.Element("RandomTextSize")?.Value, out var ranTxtSize))
                            settings.RandomTextSize = ranTxtSize;

                        if (int.TryParse(root.Element("Phases")?.Value, out var phases))
                            settings.NoPhases = phases;

                        if (int.TryParse(root.Element("Messages")?.Value, out var messages))
                            settings.NoMessages = messages;

                        if (root.Element("TestFile")?.Value is string testFile)
                        {
                            settings.TestFile = testFile;
                        }
                        else
                        {
                            if (int.TryParse(root.Element("Mode")?.Value, out var mode))
                            {
                                var testMode = (TestMode)mode;

                                if (testMode != TestMode.File)
                                    settings.Mode = testMode;
                            }
                        }

                        if (root.Element("OutputFile")?.Value is string outFile)
                        {
                            settings.OutputFile = outFile;
                        }

                        defaultUsed = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return new AESTestSettings();
                }
            }

            return settings;
        }
    }
}
