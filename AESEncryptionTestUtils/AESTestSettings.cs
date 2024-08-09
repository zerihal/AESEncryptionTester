namespace AESEncryptionTestUtils
{
    public class AESTestSettings
    {
        private string _file;

        public int RandomTextSize {  get; set; }

        public int NoPhases { get; set; }

        public TestMode Mode { get; private set; } = TestMode.Default;

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
    }
}
