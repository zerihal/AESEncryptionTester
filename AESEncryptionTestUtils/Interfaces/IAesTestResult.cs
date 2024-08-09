namespace AESEncryptionTestUtils.Interfaces
{
    public interface IAesTestResult
    {
        public double Aes128Result { get; }
        public double Aes192Result { get; }
        public double Aes256Result { get; }

        public void WriteToFile(string fileName);
    }
}
