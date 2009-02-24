using System;
using System.IO;
using System.Xml.Schema;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class XSDGeneratorTest
    {
        [Test]
        public void TestGenerateSchemas()
        {
            var repo = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            repo.OpenFile(repositoryFile);
            var ccRepository = new CCRepository(repo);

            var generator = new VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator(ccRepository, true);
            foreach (XmlSchema schema in generator.GenerateSchemas())
            {
                schema.Write(Console.Out);
                Console.Out.WriteLine();
            }
        }
    }
}