using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ResourceHandlerTest
    {
        [Test]
        public void TestResourceCachingToFileSystem()
        {
            // the test repository
            Repository repository = new EARepositoryResourceHandler();

            // the filename of the Resource to be retrieved
            string fileName = "simplified_primlibrary.xmi";

            // the resource handler expects the filenames to be retrieved in an array
            string[] resources = new[] {fileName};

            // the uri where the files to be downloaded are located 
            string downloadUri = "http://www.umm-dev.org/xmi/testresources/";

            // the directory on the local file system where the downloaded file is to be stored
            string storageDirectory = Directory.GetCurrentDirectory() +
                                      "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ResourceHandlerTest\\xmi\\download\\";
            
            // method to be tested 1: caching resources locally
            (new ResourceHandler(resources, downloadUri, storageDirectory)).CacheResourcesLocally();

            // evaluate if the file was downloaded at all
            AssertDownloadedFileExists(storageDirectory + fileName);

            string comparisonDirectory = Directory.GetCurrentDirectory() +
                                         "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ResourceHandlerTest\\xmi\\reference\\";
            
            // evalate the content of the file downloaded
            AssertFileContent(comparisonDirectory + fileName, storageDirectory + fileName);
        }

        private static void AssertDownloadedFileExists(string downloadedFile)
        {
            if (!System.IO.File.Exists(downloadedFile))
            {
                Assert.Fail("Retrieving resource file to local file system failed at: {0}", downloadedFile);
            }
        }

        private static void AssertFileContent(string comparisonFile, string newFile)
        {
            string comparisonContent = System.IO.File.ReadAllText(comparisonFile);
            string newContent = System.IO.File.ReadAllText(newFile);

            if (!comparisonContent.Equals(newContent))
            {
                Assert.Fail("Content of retrieved resource does not match expected content.\nexpected file: <{0}>\nretrieved file: <{1}>", comparisonFile, newFile);
            }
        }
    }
}