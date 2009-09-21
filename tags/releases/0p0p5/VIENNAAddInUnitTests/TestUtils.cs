using System.IO;

namespace VIENNAAddInUnitTests
{
    public static class TestUtils
    {
        public static string PathToTestResource(string relativePath)
        {
            return Directory.GetCurrentDirectory() + @"\..\..\testresources\" + relativePath;
        }
    }
}