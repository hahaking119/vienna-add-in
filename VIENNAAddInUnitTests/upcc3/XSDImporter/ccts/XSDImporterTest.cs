// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter;
using VIENNAAddIn.upcc3.XSDImporter.ccts;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ccts
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
            CCRepository ccRepository = GetFileBasedTestRepository();
            var schemas = new List<SchemaInfo>();

            string schemaDirectory = Directory.GetCurrentDirectory() +
                                     "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
            var schemaFiles = new[] {"BusinessDataType_1.xsd", "BusinessInformationEntity_1.xsd", "Invoice_1.xsd"};

            foreach (string schemaFile in schemaFiles)
            {
                var settings = new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Document};
                XmlReader reader = XmlReader.Create(schemaDirectory + schemaFile, settings);
                XmlSchema schema = XmlSchema.Read(reader, null);

                schemas.Add(new SchemaInfo(schema, schemaFile));

                reader.Close();
            }

            return new ImporterContext(ccRepository, schemaDirectory, schemas, schemas[2]);
        }

        private static void SerializeSchema(ImporterContext context)
        {
            const string outputDirectory = "C:\\Temp\\michi";

            //XmlSchema schemaContent = context.Schemas[1].Schema;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            Indent = true,
                                            Encoding = Encoding.UTF8,
                                        };


            foreach (SchemaInfo schemainfo in context.Schemas)
            {
                using (
                    XmlWriter xmlWriter = XmlWriter.Create(outputDirectory + "\\" + schemainfo.FileName,
                                                           xmlWriterSettings))
                {
                    if (xmlWriter != null)
                    {
                        schemainfo.Schema.Write(xmlWriter);

                        xmlWriter.Close();
                    }
                }
            }
        }

        [Test]
        public void PrintSchemasInContext()
        {
            foreach (SchemaInfo schemaInfo in testContext.Schemas)
            {
                schemaInfo.Schema.Write(Console.Out);
                Console.Write("\n");
            }
        }

        [Test]
        [Ignore("Implementation not finished yet.")]
        public void TestBDTSchemaImporter()
        {
            BDTSchemaImporter.ImportXSD(testContext);
            int count = 0;
            foreach (XmlSchemaObject currentElement in testContext.Schemas[0].Schema.Items)
            {
                if (currentElement.GetType().Name == "XmlSchemaComplexType")
                {
                    count++;
                }
            }
            ICCRepository repo = testContext.Repository;
            Console.WriteLine(repo.GetLibrary(1).Name);

            Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestBIESchemaImporter()
        {
            BDTSchemaImporter.ImportXSD(testContext);
            BIESchemaImporter.ImportXSD(testContext);

            //Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestRootSchemaImporter()
        {
            BDTSchemaImporter.ImportXSD(testContext);
            BIESchemaImporter.ImportXSD(testContext);
            RootSchemaImporter.ImportXSD(testContext);
        }

        [Test]
        public void TestXSDImporter()
        {
            VIENNAAddIn.upcc3.XSDImporter.ccts.XSDImporter.ImportSchemas(testContext);

            //Assert.Fail("Unit Test needs to be implemented as well ...");
        }
    }
}