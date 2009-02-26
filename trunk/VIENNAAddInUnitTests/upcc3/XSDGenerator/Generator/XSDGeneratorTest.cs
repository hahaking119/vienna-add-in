using System;
using System.Collections.Generic;
using System.IO;
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
        public void TestGenerateSchemas()
        {
//            var ccRepository = new CCRepository(GetFileBasedEARepository());
            var ccRepository = new CCRepository(new TestEARepository1());
            var generator = new VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator(ccRepository, true);
            foreach (XmlSchema schema in generator.GenerateSchemas())
            {
                schema.Write(Console.Out);
                Console.Out.WriteLine();
            }
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
        public void TestTypeBasedSelector()
        {
            var ccRepository = new CCRepository(new TestEARepository1());

            var selector = new TypeBasedSelector<string>();
            selector.Case<ICDTLibrary>(l => String.Format("<<CDTLibrary>> {0}", l.Name));
            selector.Case<IPRIMLibrary>(l => String.Format("<<PRIMLibrary>> {0}", l.Name));
            selector.Default(o => "unknown type: " + o.GetType());

            foreach (var library in ccRepository.AllLibraries())
            {
                Console.WriteLine(selector.Execute(library));
            }
        }
    }


}