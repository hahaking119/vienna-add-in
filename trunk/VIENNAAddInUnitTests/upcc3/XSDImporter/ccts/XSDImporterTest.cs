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
            string[] schemaFiles = new[] { "BusinessDataType_1.xsd", "BusinessInformationEntity_1_simplified.xsd", "Invoice_1.xsd" };

            foreach (string schemaFile in schemaFiles)
            {
                //XmlTextReader reader = new XmlTextReader(schemaDirectory + schemaFile);
                //XmlSchema schema = XmlSchema.Read(reader, null);

                // Create the XmlParserContext.
                
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                XmlReader reader = XmlReader.Create(schemaDirectory + schemaFile, settings);
                XmlSchema schema = XmlSchema.Read(reader, null);
                
                schemas.Add(new SchemaInfo(schema, schemaFile));

                reader.Close();
            }
            
            return new ImporterContext(ccRepository, schemas);
        }

        private static ImporterContext PrepareVerySimpleTestContext()
        {
            CCRepository ccRepository = GetFileBasedTestRepository();
            List<SchemaInfo> schemas = new List<SchemaInfo>();

            string schemaDirectory = Directory.GetCurrentDirectory() +
                                    "\\..\\..\\testresources\\XSDImporterTest\\ccts\\verysimpleXSDs\\";
            string[] schemaFiles = new[] { "bdts.xsd", "bies.xsd", "root.xsd" };

            foreach (string schemaFile in schemaFiles)
            {
                //XmlTextReader reader = new XmlTextReader(schemaDirectory + schemaFile);
                //XmlSchema schema = XmlSchema.Read(reader, null);

                //Stream stream = new Stream();
                FileStream fs = System.IO.File.OpenRead(schemaDirectory + schemaFile);

                XmlSchema schema = XmlSchema.Read(fs, null);

                schemas.Add(new SchemaInfo(schema, schemaFile));

                fs.Close();

                foreach (XmlSchemaObject obj in schema.Items)
                {
                    if (obj is XmlSchemaElement)
                    {
                        int i = 1;
                    }                        
                }

                

                //XmlReaderSettings settings = new XmlReaderSettings();
                //settings.ConformanceLevel = ConformanceLevel.Document;
                ////settings.IgnoreProcessingInstructions = true;
                //XmlReader reader = XmlReader.Create(schemaDirectory + schemaFile, settings);
                //XmlSchema schema = XmlSchema.Read(reader, null);

                //schemas.Add(new SchemaInfo(schema, schemaFile));

                //reader.Close();
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
            
            //ImporterContext context = PrepareTestContext();                       
            //BIESchemaImporter.ImportXSD(context);

            //ImporterContext context = PrepareVerySimpleTestContext();

            //SerializeSchema(context);

            string schemaDirectory = Directory.GetCurrentDirectory() +
                        "\\..\\..\\testresources\\XSDImporterTest\\ccts\\simpleXSDs\\";

            //XmlTextReader reader = new XmlTextReader(schemaDirectory + "BusinessInformationEntity_1.xsd");
            //XmlSchema schema = XmlSchema.Read(reader, null);

            // Create the XmlParserContext.
            
            XmlDocument doc = new XmlDocument();
            doc.Load(schemaDirectory + "BusinessInformationEntity_1.xsd");
            XmlNodeReader reader = new XmlNodeReader(doc);

            while (reader.Read())
            {
                Console.WriteLine(reader.Name);
                Console.WriteLine(reader.NamespaceURI);

                //// we've found a sequence
                //if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "xsd:sequence"))
                //{
                //    //Console.WriteLine("Element: " + reader.Name);

                //    while (reader.Read())
                //    {
                //        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "xsd:element"))
                //        {
                //            Console.WriteLine("found element in xsd:sequence!");
                //            Console.WriteLine("attribute count: " + reader.AttributeCount);

                //            reader.MoveToAttribute("type");
                            
                //            string type = reader.ReadContentAsString();
                //            Console.WriteLine("type: " + type);
                //            Console.WriteLine("");

                //            int endPrefix = type.IndexOf(':');

                //            Console.WriteLine("PREFIX: {0}", type.Substring(0, endPrefix));

                            

                //            break;
                //        }
                //    }
                //    break;
                    
                //}
            }

            


            
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