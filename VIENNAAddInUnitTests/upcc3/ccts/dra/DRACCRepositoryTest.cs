using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts.dra
{
    [TestFixture]
    public class DRACCRepositoryTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            eaRepository = new EARepository1();
            repository = new CCRepository(eaRepository);
        }

        #endregion

        private ICCRepository repository;
        private EARepository1 eaRepository;

        [Test]
        public void TestCreateBDT()
        {
            var stringType = (IPRIM) repository.FindByPath((Path) "blib1"/"primlib1"/"String");
            Assert.IsNotNull(stringType, "stringType is null");
            var cdtDate = (ICDT) repository.FindByPath((Path) "blib1"/"cdtlib1"/"Date");
            Assert.IsNotNull(cdtDate, "cdtDate is null");
            IBDTLibrary bdtLibrary = repository.Libraries<IBDTLibrary>().ElementAt(0);
            var bdtSpec = CloneCDT(cdtDate, "My");
            IBDT bdtDate = bdtLibrary.CreateBDT(bdtSpec);
            Assert.IsNotNull(bdtDate, "BDT is null");
            Assert.AreEqual("My_Date", bdtDate.Name);
            Assert.IsNotNull(bdtDate.BasedOn, "BasedOn is null");
            Assert.AreEqual(cdtDate.Id, bdtDate.BasedOn.Id);
            Assert.AreEqual(stringType.Id, bdtDate.CON.Type.Id);
            Assert.AreEqual(cdtDate.SUPs.Count(), bdtDate.SUPs.Count());
            IEnumerator<IDTComponent> bdtSups = bdtDate.SUPs.GetEnumerator();
            foreach (IDTComponent cdtSup in cdtDate.SUPs)
            {
                bdtSups.MoveNext();
                IDTComponent bdtSup = bdtSups.Current;
                Assert.AreEqual(cdtSup.Name, bdtSup.Name);
                Assert.AreEqual(cdtSup.Type.Id, bdtSup.Type.Id);
//                Assert.AreEqual(cdtSup.LowerBound, bdtSup.LowerBound);
//                Assert.AreEqual(cdtSup.UpperBound, bdtSup.UpperBound);
            }
        }

        private static BDTSpec CloneCDT(ICDT cdtDate, string qualifier)
        {
            return new BDTSpec
                   {
                       Name = (qualifier != null ? qualifier + "_" + cdtDate.Name : cdtDate.Name),
                       BasedOn = cdtDate,
                       CON = new CONSpec
                             {
                                 Type = cdtDate.CON.Type,
                             },
                       SUPs = cdtDate.SUPs.Convert(sup => new SUPSpec
                                                          {
                                                              Name = sup.Name,
                                                              Type = sup.Type
                                                          }),
                   };
        }

        [Test]
        public void TestFindCDTs()
        {
            foreach (ICDTLibrary library in repository.Libraries<ICDTLibrary>())
            {
                IEnumerable<IGrouping<IType, ICDT>> cdtByType = from cdt in library.CDTs group cdt by cdt.CON.Type;
                foreach (var cdtGroup in cdtByType)
                {
                    Console.WriteLine(cdtGroup.Key);
                    foreach (ICDT cdt in cdtGroup)
                    {
                        Console.WriteLine("  " + cdt.Name);
                    }
                }
            }
        }

        [Test]
        public void TestReadAccess()
        {
            var libraries = new List<IBusinessLibrary>(repository.AllLibraries());
            Assert.AreEqual(5, libraries.Count);

            IBusinessLibrary bLib1 = libraries[0];
            Assert.AreEqual("blib1", bLib1.Name);
            Assert.AreEqual("http://test/blib1", bLib1.BaseURN);

            var primLib1 = (IPRIMLibrary) libraries[1];
            Assert.AreEqual("primlib1", primLib1.Name);
            Assert.AreEqual("primlib1", primLib1.BaseURN);
            var prims = new List<IPRIM>(primLib1.PRIMs);
            Assert.AreEqual(2, prims.Count);
            IPRIM stringType = prims[0];
            Assert.AreEqual("String", stringType.Name);
            IPRIM decimalType = prims[1];
            Assert.AreEqual("Decimal", decimalType.Name);

            var cdtLib1 = (ICDTLibrary) libraries[2];
            Assert.AreEqual("cdtlib1", cdtLib1.Name);
            Assert.AreEqual("cdtlib1", cdtLib1.BaseURN);
            var cdts = new List<ICDT>(cdtLib1.CDTs);
            Assert.AreEqual(2, cdts.Count);
            ICDT date = cdts[0];
            Assert.AreEqual(stringType.Id, date.CON.Type.Id);
            var dateSups = new List<IDTComponent>(date.SUPs);
            Assert.AreEqual(1, dateSups.Count);
            IDTComponent dateFormat = dateSups[0];
            Assert.AreEqual("Format", dateFormat.Name);
            Assert.AreEqual(stringType.Id, dateFormat.Type.Id);

            // TODO check CDT Measure
        }
    }
}