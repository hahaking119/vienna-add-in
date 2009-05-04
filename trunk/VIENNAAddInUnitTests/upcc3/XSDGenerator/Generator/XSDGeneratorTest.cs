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
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;
using VIENNAAddInUnitTests.TestRepository;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class XSDGeneratorTest
    {

        /// <summary>
        /// Load a repository file for testing.
        /// </summary>
        /// <param name="relativePath">Path to repository file, relative to the "testresources" directory, e.g. "XSDGeneratorTest.eap".</param>
        /// <returns></returns>
        private static Repository GetFileBasedEARepository(string relativePath)
        {
            string repositoryFile = PathToTestResource(relativePath);
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            var repo = new Repository();
            repo.OpenFile(repositoryFile);
            return repo;
        }

        private static string PathToTestResource(string relativePath)
        {
            return Directory.GetCurrentDirectory() + @"\..\..\testresources\" + relativePath;
        }

        private static void AssertSchema(string expectedOutputFile, XmlSchema schema)
        {
            string expectedPath = PathToTestResource("XSDGeneratorTest\\" + expectedOutputFile);
            var xmlWriterSettings = new XmlWriterSettings
                                    {
                                        Indent = true,
//                               NewLineOnAttributes = true,
                                        Encoding = Encoding.UTF8,
                                    };
            if (!File.Exists(expectedPath))
            {
                CreateActualOutputFileAndFail(expectedPath, schema, "expected output file does not exist",
                                              xmlWriterSettings);
            }

            var memStream = new MemoryStream();

// ReSharper disable AssignNullToNotNullAttribute
            schema.Write(XmlWriter.Create(memStream, xmlWriterSettings));
// ReSharper restore AssignNullToNotNullAttribute

            memStream.Seek(0, SeekOrigin.Begin);
            var actualStreamReader = new StreamReader(memStream);

            var expectedStreamReader = new StreamReader(expectedPath, Encoding.UTF8);
            int lineCounter = 0;
            while (true)
            {
                string expectedLine = expectedStreamReader.ReadLine();
                string actualLine = actualStreamReader.ReadLine();
                if (expectedLine != actualLine)
                {
                    CreateActualOutputFileAndFail(expectedPath, schema,
                                                  "mismatch at line " + (lineCounter + 1) + "\nexpected:<" +
                                                  expectedLine + ">\nbut was:<" + actualLine + ">", xmlWriterSettings);
                }
                if (expectedLine == null || actualLine == null)
                {
                    return;
                }
                ++lineCounter;
            }
        }

        private static void CreateActualOutputFileAndFail(string expectedPath, XmlSchema schema, string message,
                                                          XmlWriterSettings xmlWriterSettings)
        {
            string actualPath = expectedPath.Substring(0, expectedPath.Length - 4) + "-actual" +
                                expectedPath.Substring(expectedPath.Length - 4, 4);
// ReSharper disable AssignNullToNotNullAttribute
            schema.Write(XmlWriter.Create(actualPath, xmlWriterSettings));
// ReSharper restore AssignNullToNotNullAttribute
            Assert.Fail(
                @"{0}:
Expected output file: {1}
Actual output file: {2}",
                message, expectedPath, actualPath);
        }

        [Test]
        public void TestAddingComments()
        {
            var schema = new XmlSchema();
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts",
                                  "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:3");
            var annotation = new XmlSchemaAnnotation();
            var documentation = new XmlSchemaDocumentation();
            var helperDocument = new XmlDocument();
            documentation.Markup = new XmlNode[]
                                   {
                                       helperDocument.CreateComment("Schema comment line 1"),
                                       helperDocument.CreateComment("Schema comment line 2"),
                                       helperDocument.CreateComment("Schema comment line 3"),
                                       helperDocument.CreateComment("Schema comment line 4"),
                                   };
            annotation.Items.Add(documentation);
            schema.Items.Add(annotation);
            schema.Write(Console.Out);
        }

//        [Test]
//        public void TestEnumLibraryGenerator()
//        {
//            var expectedOutputFiles = new[]
//                                      {
//                                          "foo.xsd"
//                                      };
//            var ccRepository = new CCRepository(new EARepository2());
//            var context = new GenerationContext(ccRepository, true, TODO);
//            var generator = new ENUMLibraryGenerator(context);
//
//            var enumLibrary = ccRepository.LibraryByName<IENUMLibrary>("ENUMLibrary");
//            generator.GenerateXSD(enumLibrary);
//            for (int i = 0; i < expectedOutputFiles.Length; ++i)
//            {
//                AssertSchema(expectedOutputFiles[i], context.Schemas[i]);
//            }
//        }

        [Test]
        public void TestBDTSchemaGenerator()
        {
            var ccRepository = new CCRepository(new EARepository2());
            var context = new GenerationContext(ccRepository, "urn:test:namespace", "test", true, true, "C:\\dump\\", null, null);
            BDTSchemaGenerator.GenerateXSD(context,
                                           VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.CollectBDTs(context));
            Assert.AreEqual(1, context.Schemas.Count);
            XmlSchema schema = context.Schemas[0].Schema;
            schema.Write(Console.Out);
            AssertSchema("bdts.xsd", schema);
        }

        [Test]
        public void TestBIESchemaGenerator()
        {
            var ccRepository = new CCRepository(new EARepository2());
            var context = new GenerationContext(ccRepository, "urn:test:namespace", "test", true, true, "C:\\dump\\", null, null);
            BIESchemaGenerator.GenerateXSD(context,
                                           VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.CollectBIEs(context));
            Assert.AreEqual(1, context.Schemas.Count);
            XmlSchema schema = context.Schemas[0].Schema;
            schema.Write(Console.Out);
            AssertSchema("bies.xsd", schema);
        }

        [Test]
        public void TestGenerateSchemas()
        {
            var ccRepository = new CCRepository(new EARepository2());
            var docLibrary = ccRepository.LibraryByName<IDOCLibrary>("DOCLibrary");
            string outputDirectory = PathToTestResource(
                "\\XSDGeneratorTest\\all");
            AddInSettings.LoadRegistryEntries();
            VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.GenerateSchemas(new GenerationContext(ccRepository, "urn:test:namespace", "test", true, true, outputDirectory, docLibrary, new List<IABIE>(docLibrary.RootElements)));
        }

        [Test]
        public void TestRootSchemaGenerator()
        {
            var ccRepository = new CCRepository(GetFileBasedEARepository("cc-for-ebInterface-0.5.eap"));
            var context = new GenerationContext(ccRepository, "urn:test:namespace", "eb", true, true, "C:\\dump\\", ccRepository.LibraryByName<IDOCLibrary>(EARepository2.DOCLibrary), new List<IABIE>(ccRepository.LibraryByName<IDOCLibrary>(EARepository2.DOCLibrary).RootElements));
            RootSchemaGenerator.GenerateXSD(context);
            Assert.AreEqual(1, context.Schemas.Count);
            XmlSchema schema = context.Schemas[0].Schema;
            schema.Write(Console.Out);
            //AssertSchema("root.xsd", schema);
        }

        [Test]
        public void TestSchemaGenerator()
        {
            AddInSettings.LoadRegistryEntries();
            var ccRepository = new CCRepository(GetFileBasedEARepository("cc-for-ebInterface-0.5.eap"));
            var context = VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.GenerateSchemas(new GenerationContext(ccRepository, "ebInterface", "eb", false, true, "C:\\dump\\", ccRepository.LibraryByName<IDOCLibrary>(EARepository2.DOCLibrary), new List<IABIE>(ccRepository.LibraryByName<IDOCLibrary>(EARepository2.DOCLibrary).RootElements)));
            Assert.AreEqual(5, context.Schemas.Count);
            XmlSchema schema = context.Schemas[1].Schema;
            schema.Write(Console.Out);

        }

        [Test]
        public void TestStreamReaderExtensions()
        {
            var streamReader = new StreamReader(PathToTestResource("StreamReaderExtensionsTest.txt"));
            var chars = new List<int>(streamReader.Characters());
            foreach (int c in chars)
            {
                Console.WriteLine("char: " + c);
            }
            Assert.AreEqual(10, chars.Count);
        }
    }

    internal static class StreamReaderExtensions
    {
        internal static IEnumerable<int> Characters(this StreamReader streamReader)
        {
            while (true)
            {
                int c = streamReader.Read();
                if (c == -1)
                {
                    break;
                }
                yield return c;
            }
        }
    }
}