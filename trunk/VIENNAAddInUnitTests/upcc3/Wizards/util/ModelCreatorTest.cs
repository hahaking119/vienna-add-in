// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;
using Path=VIENNAAddIn.upcc3.ccts.Path;
using VIENNAAddIn;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    [Category(TestCategories.FileBased)]
    public class ModelCreatorTest
    {
        #region Test Settings

        private readonly string[] resources = new[]
                                                  {
                                                      "simplified_enumlibrary.xmi", "simplified_primlibrary.xmi",
                                                      "simplified_cdtlibrary.xmi", "simplified_cclibrary.xmi"
                                                  };

        private readonly string downloadUri = "http://www.umm-dev.org/xmi/testresources/";

        private readonly string storageDirectory = Directory.GetCurrentDirectory() +
                                                   "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\download\\";

        #endregion

        // Purpose: Test the import capability of the model creator to import the standard CC libraries
        // into a particular model which contains an empty business library named bLibrary. 
        [Test]
        public void TestImportStandardCcLibrariesIntoEmptyBusinessLibrary()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryModelCreator());

            ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);

            Package bLibrary = eaRepository.Resolve<Package>((Path) "Test Model 1"/"bLibrary");

            creator.ImportStandardCcLibraries(bLibrary);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
            AssertStandardCcLibrariesContentInBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
            AssertNonExistenceOfBieLibrariesinBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
        }

        // Purpose: Test the import capability of the model creator to import the standard CC libraries
        // into a particular model which already contains CC libraries. The test therefore checks if the 
        // existing CC libraries are deleted/replaced by the standard CC libraries. 
        [Test]
        public void TestImportStandardCcLibrariesIntoFullBusinessLibrary()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryModelCreator());

            ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);

            Package bLibrary = eaRepository.Resolve<Package>((Path) "Test Model 2"/"bLibrary");

            creator.ImportStandardCcLibraries(bLibrary);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
            AssertStandardCcLibrariesContentInBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
            AssertEmptyBieLibrariesinBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
        }

        // Purpose: The test evaluates the default model creator capabilites in regards of creating
        // an empty UPCC model containg all necessary (empty) libraries. 
        [Test]
        public void TestCreatingEmptyLibrariesWithinParticularEmptyModelAmongSeveralModels()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryModelCreator());

            ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);

            creator.CreateUpccModel("New Test Model", "PRIMLibrary", "ENUMLibrary", "CDTLibrary", "CCLibrary",
                                    "BDTLibrary", "BIELibrary", "DOCLibrary", false);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 3", "New Test Model");
            AssertEmptyBieLibrariesinBusinessLibrary(eaRepository, "Test Model 3", "New Test Model");
        }

        // Purpose: The test evaluates the default model creator capabilities in regards of creating
        // an initial UPCC model containing all necessary (empty) libraries as well as importing the
        // standard CC libraries into the model created. 
        [Test]
        public void TestCreatingLibrariesAndImportingStandardLibrariesWithinParticularEmptyModelAmongSeveralModels()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryModelCreator());

            ModelCreator creator = new ModelCreator(eaRepository, resources, downloadUri, storageDirectory);

            creator.CreateUpccModel("New Test Model", "PRIMLibrary", "ENUMLibrary", "CDTLibrary", "CCLibrary",
                                    "BDTLibrary", "BIELibrary", "DOCLibrary", true);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 3", "New Test Model");
            AssertStandardCcLibrariesContentInBusinessLibrary(eaRepository, "Test Model 3", "New Test Model");
            AssertEmptyBieLibrariesinBusinessLibrary(eaRepository, "Test Model 3", "New Test Model");
        }

        # region Private Class Methods

        private static void AssertEmptyStandardCcLibrariesInBusinessLibrary(Repository repository, string modelName,
                                                                            string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"ENUMLibrary"),
                             "The business library \"{0}\" is missing an ENUM library named \"ENUMLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"PRIMLibrary"),
                             "The business library \"{0}\" is missing a PRIM library named \"PRIMLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"CDTLibrary"),
                             "The business library \"{0}\" is missing a CDT library named \"CDTLibrary\".", bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"CCLibrary"),
                             "The business library \"{0}\" is missing an CC library named \"CCLibrary\".", bLibraryName);
        }

        private static void AssertStandardCcLibrariesContentInBusinessLibrary(Repository repository, string modelName,
                                                                              string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Boolean"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Boolean\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Date"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Date\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Decimal"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Decimal\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"String"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"String\".");

            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"DateTime"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"DateTime\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Name"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Name\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Text"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Text\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Time"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Time\".");

            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CCLibrary"/"Address"),
                             "The CC library \"CCLibrary\" is missing a primitive type named \"Address\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CCLibrary"/"Person"),
                             "The CC library \"CCLibrary\" is missing a primitive type named \"Person\".");
        }

        private static void AssertNonExistenceOfBieLibrariesinBusinessLibrary(Repository repository, string modelName,
                                                                              string bLibraryName)
        {
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BDTLibrary"),
                          "The business library \"{0}\" contains a BDT library named \"BDTLibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BIELibrary"),
                          "The business library \"{0}\" contains a BIE library named \"BIELibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"DOCLibrary"),
                          "The business library \"{0}\" contains a DOC library named \"DOCLibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
        }

        private static void AssertEmptyBieLibrariesinBusinessLibrary(Repository repository, string modelName,
                                                                     string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BDTLibrary"),
                             "The business library \"{0}\" is missing an BDT library named \"BDTLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BIELibrary"),
                             "The business library \"{0}\" is missing a BIE library named \"BIELibrary\".", bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"DOCLibrary"),
                             "The business library \"{0}\" is missing a DOC library named \"DOCLibrary\".", bLibraryName);
        }
        
        #endregion
    }
}