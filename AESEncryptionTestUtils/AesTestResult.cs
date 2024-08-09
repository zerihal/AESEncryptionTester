using AESEncryptionTestUtils.Interfaces;
using System.Text;

namespace AESEncryptionTestUtils
{
    public class AesTestResult : IAesTestResult
    {
        private const string AESFileColumnHeader = "AES-128,AES-192,AES-256";

        public double Aes128Result { get; }

        public double Aes192Result { get; }

        public double Aes256Result { get; }

        public AesTestResult(Dictionary<int, double> results)
        {
            foreach (var key in results.Keys)
            {
                switch (key)
                {
                    case 128:
                        Aes128Result = results[key];
                        break;

                    case 192:
                        Aes192Result = results[key];
                        break;

                    case 256:
                        Aes256Result = results[key];
                        break;
                }
            }
        }

        public void WriteToFile(string fileName)
        {
            if (Path.GetExtension(fileName) == ".csv")
            {
                var sb = new StringBuilder();

                if (!File.ReadLines(fileName).Any())
                    sb.AppendLine(AESFileColumnHeader);

                sb.AppendLine($"{Aes128Result},{Aes192Result},{Aes256Result}");

                File.AppendAllText(fileName, sb.ToString());
            }
        }
    }
}
