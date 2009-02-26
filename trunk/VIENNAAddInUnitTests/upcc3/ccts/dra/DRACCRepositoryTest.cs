using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.ccts.dra
{
    [TestFixture]
    public class DRACCRepositoryTest
    {
        [Test]
        public void TestReadAccess()
        {
            ICCRepository repository = new CCRepository(new TestEARepository1());
            var libraries = new List<IBusinessLibrary>(repository.Libraries);
            Assert.AreEqual(3, libraries.Count);

            var bLib1 = libraries[0];
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual(BusinessLibraryType.bLibrary, bLib1.Type);
            Assert.AreEqual("http://test/blib1", bLib1.BaseURN);

            var primLib1 = (IPRIMLibrary) libraries[1];
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual(BusinessLibraryType.PRIMLibrary, primLib1.Type);
            Assert.AreEqual("primlib1", primLib1.BaseURN);
            var prims = new List<IPRIM>(primLib1.PRIMs);
            Assert.AreEqual(2, prims.Count);
            var stringType = prims[0];
            Assert.AreEqual("String", stringType.Name);
            var decimalType = prims[1];
            Assert.AreEqual("Decimal", decimalType.Name);

            var cdtLib1 = (ICDTLibrary) libraries[2];
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual(BusinessLibraryType.CDTLibrary, cdtLib1.Type);
            Assert.AreEqual("cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICDT>(cdtLib1.CDTs);
            Assert.AreEqual(2, cdts.Count);
            var date = cdts[0];
            Assert.AreEqual(stringType.Name, date.CON.Type);
            var dateSups = new List<IDTComponent>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            var dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Name, dateFormat.Type);

            // TODO check CDT Measure
        }

        [Test]
        public void TestFindCDTsByName()
        {
            ICCRepository repository = new CCRepository(new TestEARepository1());
            foreach (CDTLibrary cdtLibrary in repository.Libraries.Where(IsCDTLibrary))
            {
                foreach (var date in cdtLibrary.CDTs.Where(cdt => cdt.Name == "Date"))
                {
                    Console.WriteLine(date.CON.Type);
                }
            }
        }

        private static bool IsCDTLibrary(IBusinessLibrary l)
        {
            return l.Type == BusinessLibraryType.CDTLibrary;
        }

        [Test]
        public void TestCreateBDT()
        {
            // TODO
        }
    }
}