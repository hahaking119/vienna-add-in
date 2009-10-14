// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.import.cctsndr;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.import.cctsndr
{
    [TestFixture]
    [Category(TestCategories.FileBased)]
    public class XSDImporterTest
    {
        #region Setup/Teardown

        [SetUp]
        public void init()
        {
            testContext = PrepareTestContext();
        }

        [TearDown]
        public void clean()
        {
            eaRepository.Exit();
            //   File.Delete(backupRepositoryFile);
        }

        #endregion

        public ImporterContext testContext;
        public static string backupRepositoryFile;
        public static Repository eaRepository;

        private static string BackupFileBasedRepository(string directory, string file)
        {
            string originalRepositoryFile = directory + file;
            backupRepositoryFile = directory + "_" + file;

            Console.WriteLine("Creating backup of file-based repository: \"{0}\"\n", backupRepositoryFile);

            File.Copy(originalRepositoryFile, backupRepositoryFile, true);

            return backupRepositoryFile;
        }

        private static CCRepository GetFileBasedTestRepository()
        {
            string repositoryDirectory = Directory.GetCurrentDirectory() +
                                         "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
            const string repositoryFile = "import_repository.eap";

            Console.WriteLine("Repository file: \"{0}\"\n", repositoryDirectory + repositoryFile);

            string backupFile = BackupFileBasedRepository(repositoryDirectory, repositoryFile);

            eaRepository = (new Repository());
            eaRepository.OpenFile(backupFile);

            return new CCRepository(eaRepository);
        }

        private static ImporterContext PrepareTestContext()
        {
            return new ImporterContext(GetFileBasedTestRepository(),
                                       Directory.GetCurrentDirectory() + @"\..\..\testresources\XSDImporterTest\ccts\simpleXSDs\Invoice_1.xsd");
        }

        [Test]
        public void PrintSchemasInContext()
        {
//            foreach (SchemaInfo schemaInfo in testContext.Schemas)
//            {
//                schemaInfo.Schema.Write(Console.Out);
//                Console.Write("\n");
//            }
        }

        [Test]
        [Ignore("Implementation not finished yet.")]
        public void TestBDTSchemaImporter()
        {
            new BDTSchemaImporter(testContext).ImportXSD();
//            int count = 0;
//            foreach (XmlSchemaObject currentElement in testContext.Schemas[0].Schema.Items)
//            {
//                if (currentElement.GetType().Name == "XmlSchemaComplexType")
//                {
//                    count++;
//                }
//            }

            Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestBIESchemaImporter()
        {
            new BDTSchemaImporter(testContext).ImportXSD();
            BIESchemaImporter.ImportXSD(testContext);

            //Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestRootSchemaImporter()
        {
            new BDTSchemaImporter(testContext).ImportXSD();
            BIESchemaImporter.ImportXSD(testContext);
            RootSchemaImporter.ImportXSD(testContext);
        }

        [Test]
        public void TestXSDImporter()
        {
            VIENNAAddIn.upcc3.import.cctsndr.XSDImporter.ImportSchemas(testContext);

            //Assert.Fail("Unit Test needs to be implemented as well ...");
        }
    }
}