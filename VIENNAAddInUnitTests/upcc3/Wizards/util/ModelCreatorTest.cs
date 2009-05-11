using System;
using System.Collections.Generic;
using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ModelCreatorTest
    {
        [Test]
        public void TestCreatingDefaultUpccModel()
        {
            Repository repository = new EARepositoryModelCreator();
            ModelCreator creator = new ModelCreator(repository);

            creator.CreateUpccModel("Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest",
                                    "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);
                       

            AssertDefaultModel(repository);
        }

        //[Test]
        //public void TestCreatingUppModelWithStandardCCLibraries()
        //{
        //    Repository repository = new EARepositoryModelCreator();
        //    string[] resources = new[] { "simplified_enumlibrary.xmi", "simplified_primlibrary.xmi", "simplified_cdtlibrary.xmi", "simplified_cclibrary.xmi" };
        //    string downloadUri = "http://www.umm-dev.org/xmi/testresources/";
        //    string storageDirectory = Directory.GetCurrentDirectory() +
        //                        "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\xmi\\download\\";

        //    ModelCreator creator = new ModelCreator(repository, storageDirectory, downloadUri, resources);

        //    creator.CreateUpccModel("Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest",
        //                            "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);

        //    AssertDefaultModel(repository);
        //}

        private static void AssertDefaultModel(Repository eaRepository)
        {
            CCRepository repository = new CCRepository(eaRepository);
            List<IBLibrary> bLibs = new List<IBLibrary>(repository.Libraries<IBLibrary>());

            Assert.AreEqual(1, bLibs.Count,
                            "The number of Business libraries (having stereotype \"bLibrary\") contained in the repository didn't match the expected number of Business libraries.");

            IBLibrary bLib = bLibs[0];

            //AssertLibraryCount<IENUMLibrary>(bLib, 1);
            //AssertLibraryName<IENUMLibrary>(bLib, "ENUMLibraryTest");
            //AssertLibraryCount<IPRIMLibrary>(bLib, 1);
            //AssertLibraryName<IPRIMLibrary>(bLib, "PRIMLibraryTest");
            //AssertLibraryCount<ICDTLibrary>(bLib, 1);
            //AssertLibraryName<ICDTLibrary>(bLib, "CDTLibraryTest");
            //AssertLibraryCount<ICCLibrary>(bLib, 1);
            //AssertLibraryName<ICCLibrary>(bLib, "CCLibraryTest");
            //AssertLibraryCount<IBDTLibrary>(bLib, 1);
            //AssertLibraryName<IBDTLibrary>(bLib, "BDTLibraryTest");
            //AssertLibraryCount<IBIELibrary>(bLib, 1);
            //AssertLibraryName<IBIELibrary>(bLib, "BIELibraryTest");
            //AssertLibraryCount<IDOCLibrary>(bLib, 1);
            //AssertLibraryName<IDOCLibrary>(bLib, "DOCLibraryTest");

        }

        //private static void AssertFileContent(string comparisonFile, string newFile)
        //{
        //    string comparisonContent = System.IO.File.ReadAllText(comparisonFile);
        //    string newContent = System.IO.File.ReadAllText(newFile);

        //    if (!comparisonContent.Equals(newContent))
        //    {
        //        Assert.Fail("Caching XMI file to local file system failed at: {0}", newFile);
        //    }
        //}

        //private static void AssertLibraryCount<T>(IBLibrary bLib, short expectedCount)
        //{
        //    short libCount = 0;

        //    foreach (IBusinessLibrary lib in bLib.Children)
        //    {
        //        if (typeof(T).IsAssignableFrom(lib.GetType()))
        //        {
        //            libCount++;
        //        }
        //    }

        //    Assert.AreEqual(expectedCount, libCount, "The number of libraries of type " + libCount.GetType() + " contained in the business library didn't match the number of exptected libraries.");
        //}

        //private static void AssertLibraryName<T>(IBLibrary bLib, string expectedName)
        //{
        //    bool found = false;

        //    foreach (IBusinessLibrary lib in bLib.Children)
        //    {
        //        if (typeof(T).IsAssignableFrom(lib.GetType()) && lib.Name.Equals(expectedName))
        //        {
        //            found = true;
        //        }
        //    }

        //    if (!found)
        //    {
        //        Assert.Fail("The Business library didn't contain a library of type " + typeof(T) + " with the expected name.\n    expected:<" + expectedName + ">");
        //    }
        //}
    }
}