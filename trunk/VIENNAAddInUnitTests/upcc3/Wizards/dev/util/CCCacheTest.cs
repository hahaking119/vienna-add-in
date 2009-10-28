using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{  
    [TestFixture]
    public class CCCacheTest
    {
        private EARepository1 eaRepository;
        private CCRepository ccRepository;

        #region Test SetUp/TearDown

        [SetUp]
        public void Setup()
        {
            eaRepository = new EARepository1();
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
        [Ignore]
        public void ShouldRetrieveAndCacheCDTLibraries()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            ICDTLibrary cdtLibrary = ccCache.CDTLibraryByName("ctdLib1");

            // cdtLibrary.ElementByName("abc") --> CRAP! --> sollte nicht funktionieren!

            //ccCache.LibraryByName<ICDTLibrary>("abc");
            //ccRepository.CreateElement(targetLib, elementSpec);

            Assert.That(cdtLibrary, Is.Not.Null);
        }
        
        [Test]
        [Ignore]
        public void ShouldRetrieveAndCacheACCLibraries()
        {
            // ccCache.RetrieveLibraries<ICDTLibrary>();
            // ccCache.LibraryByName<ICDTLibrary>("cdtLib1");
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveAndCacheBDTLibraries()
        {
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveAndCacheBIELibraries()
        {
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveElementsForCDTLibrary()
        {
            CCCache ccCache = CCCache.GetInstance(ccRepository);

            CDTLibraryCache cdtLibCache = ccCache.CDTLibraryCacheByName("cdtLib1");


            ICDT cdt = cdtLibCache.ElementByName("Text");
            Assert.That(cdt, Is.Not.Null);
            Assert.That(cdt.Name, Is.EqualTo("Text"));

            List<ICDT> allCDTs = cdtLibCache.AllElements();
            Assert.That(allCDTs.Count, Is.EqualTo(5));
         }

        [Test]
        [Ignore]
        public void ShouldRetrieveElementsForACCLibrary()
        {
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveElementsForBDTLibrary()
        {
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveElementsForBIELibrary()
        {
        }

        [Test]
        [Ignore]
        public void ShouldAddElementToBDTLibraryCache()
        {                       
            //IBDT bdtText = (IBDT) ccRepository.FindByPath(EARepository1.PathToBDTText());
            //BDTSpec bdtSpec = new BDTSpec(bdtText);
            //bdtSpec.Name = "AnotherText";

            //IBDTLibrary bdtLibrary = ccRepository.LibraryByName<IBDTLibrary>("bdtlib1");
            //IBDT newBdtText = bdtLibrary.CreateElement(bdtSpec);

            //CCCache ccCache = CCCache.GetInstance(ccRepository);

            //BDTLibraryCache bdtLibCache = ccCache.BDTLibraryCacheByName("bdtlib1");

            //bdtLibCache.AddElementToCache(newBdtText);

            //IBDT bdt = bdtLibCache.ElementByName("AnotherText");
            //Assert.That(bdt, Is.Not.Null);
            //Assert.That(bdt.Name, Is.EqualTo("AnotherText"));

            //List<ICDT> allBDTs = bdtLibCache.AllElements();
            //Assert.That(allBDTs.Count, Is.EqualTo(7));         
        }

        [Test]
        [Ignore]
        public void ShouldAddElementToBIELibraryCache()
        {
        }

        [Test]
        [Ignore]
        public void ShouldRetrieveCorrectLibrariesForParticularABIE()
        {
            //IABIE abiePerson = (IABIE)ccRepository.FindByPath(EARepository1.PathToBIEPerson());

            //CCCache ccCache = CCCache.GetInstance(ccRepository);

            //ccCache.RetrieveAllLibrariesForABIE(abiePerson);

            // richtige cc lib
            // richtige cdt lib
            // richtige bdt lib
            // richtige bie lib
            // in jeder lib muessen die entsprechenden elemente vorhanden sein - aber nicht alle!
        }
    }
}