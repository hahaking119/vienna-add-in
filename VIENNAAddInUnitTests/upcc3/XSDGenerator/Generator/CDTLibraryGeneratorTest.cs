using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class SelectorTest
    {
        [Test]
        public void testSelector()
        {
            var numbersList = new List<int>(new[] {1, 2, 3, 4});
            var sel = new Selector<int>(i => Console.WriteLine("no match for {0}", (object) i), true);
            sel.If(i => i == 1, i => Console.WriteLine("one"));
            sel.If(i => i == 2, i => Console.WriteLine("two"));
            sel.If(i => i == 3, i => Console.WriteLine("three"));
            numbersList.ForEach(sel.Execute);
        }
    }

    [TestFixture]
    public class CDTLibraryGeneratorTest
    {
        public void Playground()
        {
            ICCRepository repository = new DummyCCTSRepository();
            IBusinessLibrary cdtLibrary = repository.GetLibrary(0);
            var context = new GenerationContext(repository, true);
            var generator = new CDTLibraryGenerator(context);
            generator.GenerateXSD((ICDTLibrary) cdtLibrary);
            context.Schemas[0].Write(Console.Out);
        }

        [Test]
        public void Playground3()
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
            }
        }

        [Test]
        public void Playground2()
        {
            var eaRepository = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            eaRepository.OpenFile(repositoryFile);
            var cctsRepository = new EACCRepository(eaRepository);
            var context = new GenerationContext(cctsRepository, true);
            var generator = new CDTLibraryGenerator(context);
            cctsRepository.EachLibrary(lib =>
                                           {
                                               Console.WriteLine("<<{0}>> {1}", lib.Type, lib.Name);
                                               if (lib.Type == BusinessLibraryType.CDTLibrary)
                                               {
                                                   generator.GenerateXSD((ICDTLibrary) lib);
                                               }
                                           });
            foreach (var schema in context.Schemas)
            {
                schema.Write(Console.Out);
            }
        }

        [Test]
        public void testGenerate()
        {
            var repo = new Repository();
            string repositoryFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDGeneratorTest.eap";
            Console.WriteLine("Repository file: \"{0}\"", repositoryFile);
            repo.OpenFile(repositoryFile);
            var ccRepository = new EACCRepository(repo);

            var generator = new VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator(ccRepository, true);
            foreach (XmlSchema schema in generator.GenerateSchemas())
            {
                schema.Write(Console.Out);
            }
        }

        [Test]
        public void testStringDefault()
        {
            Assert.AreEqual("abc" + null, "abc");
            Assert.AreEqual("abc" + "", "abc");
            Assert.AreEqual("abc" + String.Empty, "abc");

            Assert.AreEqual(String.Empty.DefaultTo("abc"), "abc");
            Assert.AreEqual("".DefaultTo("abc"), "abc");

            const string nullString = null;
            Assert.AreEqual(nullString.DefaultTo("abc"), "abc");

            Assert.AreEqual("foo".DefaultTo("bar"), "foo");
        }
    }

    internal class DummyCCTSRepository : ICCRepository
    {
        #region ICCRepository Members

        public IBusinessLibrary GetRootLibrary()
        {
            throw new System.NotImplementedException();
        }

        public IBusinessLibrary GetLibrary(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICDT GetCDT(int id)
        {
            throw new System.NotImplementedException();
        }

        public void EachCDT(int cdtLibraryId, Action<ICDT> visit)
        {
            throw new NotImplementedException();
        }

        public void TraverseLibrariesDepthFirst(Action<IBusinessLibrary> visit)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public IBusinessLibrary GetCDTLibrary(int id)
        {
            throw new NotImplementedException();
        }
    }
}