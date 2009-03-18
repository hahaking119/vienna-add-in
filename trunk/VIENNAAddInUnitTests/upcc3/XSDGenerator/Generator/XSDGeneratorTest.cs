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
using System.Xml;
using System.Xml.Schema;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class XSDGeneratorTest
    {
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

        private static Repository GetFileBasedEARepository()
        {
            var repo = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            repo.OpenFile(repositoryFile);
            return repo;
        }

        [Test]
        public void TestGenerateSchemas()
        {
//            var ccRepository = new CCRepository(GetFileBasedEARepository());
            var ccRepository = new CCRepository(new EARepository1());
            var generator = new VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator(ccRepository, true);
            foreach (XmlSchema schema in generator.GenerateSchemas())
            {
                schema.Write(Console.Out);
                Console.Out.WriteLine();
            }
        }

        [Test]
        public void TestTypeBasedSelector()
        {
            var ccRepository = new CCRepository(new EARepository1());

            var selector = new TypeBasedSelector<string>();
            selector.Case<ICDTLibrary>(l => String.Format("<<CDTLibrary>> {0}", (object) l.Name));
            selector.Case<IPRIMLibrary>(l => String.Format("<<PRIMLibrary>> {0}", l.Name));
            selector.Default(o => "unknown type: " + o.GetType());

            foreach (IBusinessLibrary library in ccRepository.AllLibraries())
            {
                Console.WriteLine(selector.Execute(library));
            }
        }
    }
}