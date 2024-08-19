using AESEncryptionTestUtils.Interfaces;
using System.Diagnostics;
using System.Text;

namespace AESEncryptionTestUtils
{
    public class AesTestResult : IAesTestResult
    {
        private const string AESFileSummaryHeader = "No Test Runs,Average AES-128 (ms),Average AES-192 (ms),Average AES-256 (ms)";
        private const string AESFileColumnHeader = "Test Run,AES-128,AES-192,AES-256";

        /// <summary>
        /// AES-128 test result.
        /// </summary>
        public double Aes128Result { get; }

        /// <summary>
        /// AES-192 test result.
        /// </summary>
        public double Aes192Result { get; }

        /// <summary>
        /// AES-256 test result.
        /// </summary>
        public double Aes256Result { get; }

        public AesTestResult(Dictionary<int, double> results)
        {
            foreach (var key in results.Keys)
            {
                switch (key)
                {
                    case 128:
                        Aes128Result = Math.Round(results[key], 4);
                        break;

                    case 192:
                        Aes192Result = Math.Round(results[key], 4);
                        break;

                    case 256:
                        Aes256Result = Math.Round(results[key], 4);
                        break;
                }
            }
        }

        /// <summary>
        /// Writes this AES test results to a CSV file, appending and updating averages if file exists.
        /// </summary>
        /// <param name="fileName">Output filename and path.</param>
        public void WriteToFile(string fileName)
        {
            if (Path.GetExtension(fileName) == ".csv")
            {
                var sb = new StringBuilder();
                var testRun = 1;

                if (!File.Exists(fileName) || !File.ReadLines(fileName).Any())
                {
                    sb.AppendLine(AESFileSummaryHeader);
                    sb.AppendLine("0,0,0,0");
                    sb.AppendLine(AESFileColumnHeader);
                }
                else
                {
                    var lastRun = File.ReadAllLines(fileName).Last()?.Split(",").FirstOrDefault();

                    if (int.TryParse(lastRun, out testRun))
                        testRun++;                     
                    else
                        throw new ApplicationException("Results file append error");
                }

                sb.AppendLine($"{testRun},{Aes128Result},{Aes192Result},{Aes256Result}");

                File.AppendAllText(fileName, sb.ToString());
                UpdateResultsSummary(fileName);
            }
        }

        /// <summary>
        /// Updates the file results summary with averaged results and number of test runs.
        /// </summary>
        /// <param name="file">Output filename and path.</param>
        private void UpdateResultsSummary(string file)
        {
            var fileLines = File.ReadAllLines(file);

            if (fileLines.Length > 3 && fileLines[2] == AESFileColumnHeader)
            {
                var aesAvgs = new double[] { 0, 0, 0 };
                var testRuns = fileLines.Length - 3;

                for (var l = 3; l < fileLines.Length; l++)
                {
                    var ln = fileLines[l].Split(",");

                    if (ln.Length == 4)
                    {
                        for (var e = 1; e < ln.Length; e++)
                        {
                            if (double.TryParse(ln[e], out var val))
                                aesAvgs[e - 1] += val;
                            else
                                Debug.WriteLine($"Unable to parse value on line {l}");
                        }
                    }
                }

                for (var i = 0; i < 3; i++)
                    aesAvgs[i] = Math.Round(aesAvgs[i] / testRuns, 4);

                fileLines[1] = $"{testRuns},{aesAvgs[0]},{aesAvgs[1]},{aesAvgs[2]}";
                File.WriteAllLines(file, fileLines);
            }
        }
    }
}
