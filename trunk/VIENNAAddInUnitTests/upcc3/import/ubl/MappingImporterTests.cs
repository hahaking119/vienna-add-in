using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.import.ebInterface;
using VIENNAAddInUnitTests.upcc3.import.ebInterface;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.import.ubl
{
    [TestFixture]
    public class MappingImporterTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());
            ccLibrary = cctsRepository.GetCcLibraries().FirstOrDefault();
            bLibrary = ccLibrary.BLibrary;
        }

        #endregion

        private const string DocLibraryName = "UBL Invoice";
        private const string BieLibraryName = "UBL";
        private const string BdtLibraryName = "UBL Types";
        private const string Qualifier = "UBL";
        private const string RootElementName = "Invoice";

        private ICctsRepository cctsRepository;
        private ICcLibrary ccLibrary;
        private IBLibrary bLibrary;

        #region Manual Testing

        [Test]
        [Ignore("for manual testing")]
        public void Test_mapping_ubl_to_ccl()
        {
            string repoPath = TestUtils.PathToTestResource(@"XSDImporterTest\ubl\MappingImporterTests\mapping_ubl_to_ccl\Invoice.eap");
            File.Copy(TestUtils.PathToTestResource(@"XSDImporterTest\ubl\MappingImporterTests\mapping_ubl_to_ccl\Repository-with-CDTs-and-CCs.eap"), repoPath, true);
            var repo = new Repository();
            repo.OpenFile(repoPath);

            var mappingFileNames = new List<string> { "ubl2cll_1_1.mfd", "ubl2cll_2_1.mfd", "ubl2cll_3_1.mfd", "ubl2cll_4_1.mfd", "ubl2cll_5_1.mfd", "ubl2cll_6_1.mfd", "ubl2cll_7_1.mfd", "ubl2cll_8_1.mfd", "ubl2cll_9_1.mfd", "ubl2cll_10_1.mfd", "ubl2cll_11_1.mfd", "ubl2cll_12_1.mfd", "ubl2cll_13_1.mfd" };
            var mappingFiles = new List<string>();

            foreach (var mappingFile in mappingFileNames)
            {
                mappingFiles.Add(TestUtils.PathToTestResource(@"XSDImporterTest\ubl\MappingImporterTests\mapping_ubl_to_ccl\" + mappingFile));
            }

            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\ubl\MappingImporterTests\mapping_ubl_to_ccl\invoice\maindoc\UBL-Invoice-2.0.xsd") };

            Console.Out.WriteLine("Starting mapping");
            var repository = CctsRepositoryFactory.CreateCctsRepository(repo);
            var ccLib = repository.GetCcLibraries().FirstOrDefault();

            new MappingImporter(mappingFiles, schemaFiles, ccLib, ccLib.BLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, repository).ImportMapping();
        }

        #endregion

    }
}