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
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;
using VIENNAAddIn.upcc3.XSDImporter;
using VIENNAAddIn.upcc3.XSDImporter.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.ccts
{
    [TestFixture]
    public class XSDImporterTest
    {
        #region Test Preparation

        private static string BackupFileBasedRepository(string directory, string file)
        {
            string originalRepositoryFile = directory + file;
            string backupRepositoryFile = directory + "_" + file;

            Console.WriteLine("Creating backup of file-based repository: \"{0}\"", backupRepositoryFile);

            System.IO.File.Copy(originalRepositoryFile, backupRepositoryFile, true);

            return backupRepositoryFile;
        }

        private static CCRepository GetFileBasedTestRepository()
        {
            string repositoryDirectory = Directory.GetCurrentDirectory() +
                                    "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";
            string repositoryFile = "import_repository.eap";

            Console.WriteLine("Repository file: \"{0}\"", repositoryDirectory + repositoryFile);

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
                XmlTextReader reader = new XmlTextReader(schemaDirectory + schemaFile);
                XmlSchema schema = XmlSchema.Read(reader, null);
                
                schemas.Add(new SchemaInfo(schema, schemaFile));

                reader.Close();
            }
            
            return new ImporterContext(ccRepository, schemas);
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

            Assert.Fail("Unit Test needs to be implemented as well ...");
        }

        [Test]
        public void TestRootSchemaImporter()
        {           
            RootSchemaImporter.ImportXSD(PrepareTestContext());

            Assert.Fail("Unit Test needs to be implemented as well ...");
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