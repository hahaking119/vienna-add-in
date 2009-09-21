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
using NUnit.Framework;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter;
using VIENNAAddIn.upcc3.XSDImporter.ccts;
using File=EA.File;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ccts
{
    [TestFixture]
    [Category(TestCategories.FileBased)]
    public class XSDImporterTest
    {
        #region Test Preparation

        private static string BackupFileBasedRepository(string directory, string file)
        {
            string originalRepositoryFile = directory + file;
            string backupRepositoryFile = directory + "_" + file;

            Console.WriteLine("Creating backup of file-based repository: \"{0}\"\n", backupRepositoryFile);

            System.IO.File.Copy(originalRepositoryFile, backupRepositoryFile, true);

            return backupRepositoryFile;
        }

        private static CCRepository GetFileBasedTestRepository()
        {
            string repositoryDirectory = Directory.GetCurrentDirectory() +
                                    "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
            string repositoryFile = "import_repository.eap";

            Console.WriteLine("Repository file: \"{0}\"\n", repositoryDirectory + repositoryFile);

            string backupFile = BackupFileBasedRepository(repositoryDirectory, repositoryFile);

            Repository eaRepository = (new Repository());
            eaRepository.OpenFile(backupFile);

            return new CCRepository(eaRepository);
        }

        private static ImporterContext PrepareTestContext()
        {
            CCRepository ccRepository = GetFileBasedTestRepository();
            List<SchemaInfo> schemas = new List<SchemaInfo>();

            string schemaDirectory = Directory.GetCurrentDirectory() +
                                    "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
            string[] schemaFiles = new[] { "BusinessDataType_1.xsd", "BusinessInformationEntity_1.xsd", "Invoice_1.xsd" };

            foreach (string schemaFile in schemaFiles)
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                XmlReader reader = XmlReader.Create(schemaDirectory + schemaFile, settings);
                XmlSchema schema = XmlSchema.Read(reader, null);
                
                schemas.Add(new SchemaInfo(schema, schemaFile));

                reader.Close();
            }
            
            return new ImporterContext(ccRepository, schemaDirectory, schemas, schemas[2]);
        }

        #endregion

        [Test]
        public void TestBDTSchemaImporter()
        {
            BDTSchemaImporter.ImportXSD(PrepareTestContext());

            Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestBIESchemaImporter()
        {
            BIESchemaImporter.ImportXSD(PrepareTestContext());

            //Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        private static void SerializeSchema(ImporterContext context)
        {
            string outputDirectory = "C:\\Temp\\michi";

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
                    var xmlWriter = XmlWriter.Create(outputDirectory + "\\" + schemainfo.FileName,
                                                     xmlWriterSettings))
                {
                    schemainfo.Schema.Write(xmlWriter);

                    xmlWriter.Close();                    
                }

                
            }
        }

        [Test]
        public void TestRootSchemaImporter()
        {
            RootSchemaImporter.ImportXSD(PrepareTestContext());

        }

        [Test]
        public void TestXSDImporter()   
        {
            ImporterContext context = VIENNAAddIn.upcc3.XSDImporter.ccts.XSDImporter.ImportSchemas(PrepareTestContext());

            Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void PrintSchemasInContext()
        {
            ImporterContext context = PrepareTestContext();
            
            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                schemaInfo.Schema.Write(Console.Out);
                Console.Write("\n");
            }
        }
    }
}