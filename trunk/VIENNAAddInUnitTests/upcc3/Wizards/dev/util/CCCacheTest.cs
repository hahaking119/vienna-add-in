using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{  
    [TestFixture]
    public class CCCacheTest
    {
        private EARepositoryCCCache eaRepository;
        private CCRepository ccRepository;

        #region Test SetUp/TearDown

        [SetUp]
        public void Setup()
        {
            eaRepository = new EARepositoryCCCache();
            ccRepository = new CCRepository(eaRepository);
        }

        [TearDown]
        public void Teardown()
        {
            eaRepository = null;
            ccRepository = null;
        }

        #endregion

        [Test]
        public void ShouldGetAndCacheCDTLibraries()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<CDTLibrary> cdtLibraries = ccCache.GetCDTLibraries();

            Assert.That(cdtLibraries, Is.Not.Null);
            Assert.That(cdtLibraries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldGetAndCacheCCLibraries()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<CCLibrary> ccLibraries = ccCache.GetCCLibraries();

            Assert.That(ccLibraries, Is.Not.Null);
            Assert.That(ccLibraries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldGetAndCacheBDTLibraries()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<BDTLibrary> bdtLibraries = ccCache.GetBDTLibraries();

            Assert.That(bdtLibraries, Is.Not.Null);
            Assert.That(bdtLibraries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldGetAndCacheBIELibraries()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<BIELibrary> bieLibraries = ccCache.GetBIELibraries();

            Assert.That(bieLibraries, Is.Not.Null);
            Assert.That(bieLibraries.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldGetAllCDTsFromCDTLibrary()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<CDT> cdts = ccCache.GetCDTsFromCDTLibrary("cdtlib1");

            Assert.That(cdts.Count, Is.EqualTo(5));
        }

        [Test]
        public void ShouldGetParticularCDTFromCDTLibrary()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            CDT cdt = ccCache.GetCDTFromCDTLibrary("cdtlib1", "Text");

            Assert.That(cdt, Is.Not.Null);
            Assert.That(cdt.Name, Is.EqualTo("Text"));            
        }



        [Test]
        public void ShouldGetAllCCsFromCCLibrary()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            List<ACC> ccs = ccCache.GetCCsFromCCLibrary("cclib1");

            Assert.That(ccs.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldGetParticularCCFromCCLibrary()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            ACC cc = ccCache.GetCCFromCCLibrary("cclib1", "Address");

            Assert.That(cc, Is.Not.Null);
            Assert.That(cc.Name, Is.EqualTo("Address"));
        }

        [Test]
        public void ShouldGetBDTLibraryByName()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            BIELibrary bdtLibrary = ccCache.GetBDTLibraryByName("bdtlib1");

            Assert.That(bdtLibrary, Is.Not.Null);
            Assert.That(bdtLibrary.Name, Is.EqualTo("bdtlib1"));
        }

        [Test]
        public void ShouldGetBIELibraryByName()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            BIELibrary bieLibrary = ccCache.GetBIELibraryByName("bielib1");

            Assert.That(bieLibrary, Is.Not.Null);
            Assert.That(bieLibrary.Name, Is.EqualTo("bielib1"));
        }
        

        [Test]
        public void ShouldPrepareCCCacheForParticularABIE()
        {
            IABIE abiePerson = (IABIE)ccRepository.FindByPath(EARepositoryCCCache.PathToBIEPerson());

            CCCache ccCache = CCCache.GetInstance(ccRepository);

            ccCache.PrepareForABIE(abiePerson);

            // TODO: wie testen wir hier am besten, dass der cache die richtigen elemente enthaelt?
            // TODO: denn falls wir auf die libraries mittels e.g. "GetCCLibraries" zugreifen, versucht
            // TODO: der cache im aktuellen design natuerlich auf das Repository zuzugreifen!?

            // richtige cc lib
            // richtige cdt lib
            // richtige bdt lib
            // richtige bie lib
            // in jeder lib muessen die entsprechenden elemente vorhanden sein - aber nicht alle!
        }
    }
}