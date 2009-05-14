using System;
using System.Collections.Generic;
using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;
using Path=VIENNAAddIn.upcc3.ccts.Path;
using VIENNAAddIn;
using File=EA.File;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ModelCreatorTest
    {
        #region Test Settings

        readonly string[] resources = new[] { "simplified_enumlibrary.xmi", "simplified_primlibrary.xmi", "simplified_cdtlibrary.xmi", "simplified_cclibrary.xmi" };
        readonly string downloadUri = "http://www.umm-dev.org/xmi/testresources/";
        readonly string storageDirectory = Directory.GetCurrentDirectory() +
                                  "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\download\\";
        readonly string testRepositoryDirectory = Directory.GetCurrentDirectory() +
                                  "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\";
        #endregion

        [Test]
        public void ChristianTest()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryModelCreator());

            ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);

            Package bLibrary = eaRepository.Resolve<Package>((Path)"Test Model 1" / "bLibrary");

            creator.ImportStandardCcLibraries(bLibrary);

            //AssertCcLibrariesInRepository(eaRepository, "Test Model 1");
            //AssertCcLibrariesContentInRepository(eaRepository, "Test Model 1");
        }

        //[Test]
        //public void TestCreatingEmptyLibrariesWithinParticularEmptyModelWhichIsTheOnlyModel()
        //{
        //    Repository eaRepository = new Repository();

        //    eaRepository.OpenFile(BackupFileBasedRepository(testRepositoryDirectory, "testmodel1.eap"));

        //    ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);
        //    creator.CreateUpccModel("New Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest", "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);

        //    AssertBusinessLibraryCount(eaRepository, 2);
        //    AssertEmptyDefaultModel(eaRepository, 1);



        //    AssertContent(eaRepository, 1);
        //}

    //    [Test]
    //    public void TestCreatingEmptyLibrariesWithinParticularEmptyModelAmongSeveralModels()
    //    {
    //        Repository eaRepository = new Repository();

    //        eaRepository.OpenFile(BackupFileBasedRepository(testRepositoryDirectory, "testmodel2.eap"));

    //        ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);
    //        creator.CreateUpccModel("New Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest", "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);

    //        AssertBusinessLibraryCount(eaRepository, 1);
    //        AssertEmptyDefaultModel(eaRepository, 0);
    //    }

    //    [Test]
    //    public void TestCreatingEmptyLibrariesWhileCreatingNewEmptyModel()
    //    {
    //        Repository eaRepository = new Repository();

    //        eaRepository.OpenFile(BackupFileBasedRepository(testRepositoryDirectory, "testmodel3.eap"));

    //        ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);
    //        creator.CreateUpccModel("New Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest", "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);

    //        AssertBusinessLibraryCount(eaRepository, 2);
    //        AssertEmptyDefaultModel(eaRepository, 1);
    //    }

    //    [Test]
    //    public void TestCreatingLibrariesAndImportingStandardLibrariesWhileCreatingNewEmptyModel()
    //    {
    //        Repository eaRepository = new Repository();

    //        eaRepository.OpenFile(BackupFileBasedRepository(testRepositoryDirectory, "testmodel4.eap"));

    //        ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);
    //        creator.CreateUpccModel("New Test Model", "PRIMLibraryTest", "ENUMLibraryTest", "CDTLibraryTest", "CCLibraryTest", "BDTLibraryTest", "BIELibraryTest", "DOCLibraryTest", false);

    //        AssertBusinessLibraryCount(eaRepository, 2);
    //        AssertEmptyDefaultModel(eaRepository, 1);
    //    }
        
        private static string BackupFileBasedRepository(string directory, string file)
        {
            string originalRepositoryFile = directory + file;
            string backupRepositoryFile = directory + "_" + file;
            
            Console.WriteLine("Creating backup of file-based repository: \"{0}\"", backupRepositoryFile);
            
            System.IO.File.Copy(originalRepositoryFile, backupRepositoryFile, true);

            return backupRepositoryFile;
        }

    //    private static void AssertLibraryCount<T>(IBLibrary bLib, short expectedCount)
    //    {
    //        short libCount = 0;

    //        foreach (IBusinessLibrary lib in bLib.Children)
    //        {
    //            if (typeof(T).IsAssignableFrom(lib.GetType()))
    //            {
    //                libCount++;
    //            }
    //        }

    //        Assert.AreEqual(expectedCount, libCount, "The number of libraries of type " + libCount.GetType() + " contained in the business library didn't match the number of exptected libraries.");
    //    }

    //    private static void AssertLibraryName<T>(IBLibrary bLib, string expectedName)
    //    {
    //        bool found = false;

    //        foreach (IBusinessLibrary lib in bLib.Children)
    //        {
    //            if (typeof(T).IsAssignableFrom(lib.GetType()) && lib.Name.Equals(expectedName))
    //            {
    //                found = true;
    //            }
    //        }

    //        if (!found)
    //        {
    //            Assert.Fail("The Business library didn't contain a library of type " + typeof(T) + " with the expected name.\n    expected:<" + expectedName + ">");
    //        }
    //    }

    //    private static void AssertBusinessLibraryCount(Repository eaRepository, int expectedBusinessLibraryCount)
    //    {
    //        CCRepository repository = new CCRepository(eaRepository);
    //        List<IBLibrary> bLibs = new List<IBLibrary>(repository.Libraries<IBLibrary>());

    //        Assert.AreEqual(expectedBusinessLibraryCount, bLibs.Count, "The number of Business libraries (having stereotype \"bLibrary\") contained in the repository didn't match the expected number of Business libraries.");            
    //    }

    //    private static void AssertEmptyDefaultModel(Repository eaRepository, int libraryIndex)
    //    {
    //        CCRepository repository = new CCRepository(eaRepository);
    //        List<IBLibrary> bLibs = new List<IBLibrary>(repository.Libraries<IBLibrary>());

    //        IBLibrary bLib = bLibs[libraryIndex];

    //        AssertLibraryCount<IENUMLibrary>(bLib, 1);
    //        AssertLibraryName<IENUMLibrary>(bLib, "ENUMLibraryTest");
    //        AssertLibraryCount<IPRIMLibrary>(bLib, 1);
    //        AssertLibraryName<IPRIMLibrary>(bLib, "PRIMLibraryTest");
    //        AssertLibraryCount<ICDTLibrary>(bLib, 1);
    //        AssertLibraryName<ICDTLibrary>(bLib, "CDTLibraryTest");
    //        AssertLibraryCount<ICCLibrary>(bLib, 1);
    //        AssertLibraryName<ICCLibrary>(bLib, "CCLibraryTest");
    //        AssertLibraryCount<IBDTLibrary>(bLib, 1);
    //        AssertLibraryName<IBDTLibrary>(bLib, "BDTLibraryTest");
    //        AssertLibraryCount<IBIELibrary>(bLib, 1);
    //        AssertLibraryName<IBIELibrary>(bLib, "BIELibraryTest");
    //        AssertLibraryCount<IDOCLibrary>(bLib, 1);
    //        AssertLibraryName<IDOCLibrary>(bLib, "DOCLibraryTest");
    //    }
    }
}