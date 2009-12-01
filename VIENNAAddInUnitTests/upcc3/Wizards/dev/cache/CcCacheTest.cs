using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.cache
{
    [TestFixture]
    public class CcCacheTest
    {
        [Test]
        public void ShouldGetAndCacheCdtLibraries()
        {
            // Setup
            var cdtLibraryMock = new Mock<ICdtLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCdtLibraries()).Returns(new [] {cdtLibraryMock.Object});
            
            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICdtLibrary> cdtLibraries = ccCache.GetCDTLibraries();
            ccCache.GetCDTLibraries();
            
            // Assertion and Verification
            Assert.That(cdtLibraries, Is.Not.Null);
            Assert.That(cdtLibraries.Count, Is.EqualTo(1));            
            Assert.That(cdtLibraries[0], Is.SameAs(cdtLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetCdtLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheCcLibraries()
        {
            // Setup
            var ccLibraryMock = new Mock<ICcLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCcLibraries()).Returns(new[] { ccLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICcLibrary> ccLibraries = ccCache.GetCCLibraries();
            ccCache.GetCCLibraries();

            // Assertion and Verification
            Assert.That(ccLibraries, Is.Not.Null);
            Assert.That(ccLibraries.Count, Is.EqualTo(1));
            Assert.That(ccLibraries[0], Is.SameAs(ccLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetCcLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheBdtLibraries()
        {
            // Setup
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetBdtLibraries()).Returns(new[] { bdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBdtLibrary> bdtLibraries = ccCache.GetBDTLibraries();
            ccCache.GetBDTLibraries();

            // Assertion and Verification
            Assert.That(bdtLibraries, Is.Not.Null);
            Assert.That(bdtLibraries.Count, Is.EqualTo(1));
            Assert.That(bdtLibraries[0], Is.SameAs(bdtLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetBdtLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheBieLibraries()
        {
            // Setup
            var bieLibraryMock = new Mock<IBieLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetBieLibraries()).Returns(new[] { bieLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBieLibrary> bieLibraries = ccCache.GetBIELibraries();
            ccCache.GetBIELibraries();

            // Assertion and Verification
            Assert.That(bieLibraries, Is.Not.Null);
            Assert.That(bieLibraries.Count, Is.EqualTo(1));
            Assert.That(bieLibraries[0], Is.SameAs(bieLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetBieLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheAllCdtsFromCdtLibrary()
        {
            // Setup
            var cdtMock = new Mock<ICdt>();
            var cdtLibraryMock = new Mock<ICdtLibrary>();
            ICdt[] expectedCdts = new[] {cdtMock.Object, cdtMock.Object, cdtMock.Object};
            cdtLibraryMock.SetupGet(l => l.Name).Returns("cdtlib1");
            cdtLibraryMock.SetupGet(l => l.Cdts).Returns(expectedCdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCdtLibraries()).Returns(new[] { cdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICdt> cdts = ccCache.GetCDTsFromCDTLibrary("cdtlib1");
            ccCache.GetCDTsFromCDTLibrary("cdtlib1");

            // Assertion and Verification
            Assert.That(cdts, Is.EquivalentTo(expectedCdts));            
            cdtLibraryMock.VerifyGet(l => l.Cdts, Times.Exactly(1));            
        }

        [Test]
        public void ShouldGetAndCacheAllCcsFromCcLibrary()
        {
            // Setup
            var accMock = new Mock<IAcc>();
            var ccLibraryMock = new Mock<ICcLibrary>();
            IAcc[] expectedAccs = new[] { accMock.Object, accMock.Object};
            ccLibraryMock.SetupGet(l => l.Name).Returns("cclib1");
            ccLibraryMock.SetupGet(l => l.Accs).Returns(expectedAccs);
            
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCcLibraries()).Returns(new[] { ccLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IAcc> accs = ccCache.GetCCsFromCCLibrary("cclib1");
            ccCache.GetCCsFromCCLibrary("cclib1");

            // Assertion and Verification
            Assert.That(accs, Is.EquivalentTo(expectedAccs));
            ccLibraryMock.VerifyGet(l => l.Accs, Times.Exactly(1));   
        }

        [Test]
        public void ShouldGetAndCacheParticularCdtFromCdtLibrary()
        {
            // Setup
            var cdtMockText = new Mock<ICdt>();
            cdtMockText.SetupGet(c => c.Name).Returns("Text");
            var cdtMockDate = new Mock<ICdt>();
            cdtMockDate.SetupGet(c => c.Name).Returns("Date");

            var cdtLibraryMock = new Mock<ICdtLibrary>();
            ICdt[] expectedCdts = new[] { cdtMockDate.Object, cdtMockText.Object };
            cdtLibraryMock.SetupGet(l => l.Name).Returns("cdtlib1");
            cdtLibraryMock.SetupGet(l => l.Cdts).Returns(expectedCdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCdtLibraries()).Returns(new[] { cdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            ICdt cdt = ccCache.GetCDTFromCDTLibrary("cdtlib1", "Text");
            ccCache.GetCDTFromCDTLibrary("cdtlib1", "Text");
                
            // Assertion and Verification
            Assert.That(cdt, Is.SameAs(cdtMockText.Object));
            cdtLibraryMock.VerifyGet(l => l.Cdts, Times.Exactly(1));   
        }

        [Test]
        public void ShouldGetAndCacheParticularCcFromCcLibrary()
        {
            // Setup
            var accMockPerson = new Mock<IAcc>();
            accMockPerson.SetupGet(a => a.Name).Returns("Person");
            var accMockAddress = new Mock<IAcc>();
            accMockAddress.SetupGet(a => a.Name).Returns("Address");

            var ccLibraryMock = new Mock<ICcLibrary>();
            IAcc[] expectedAccs = new[] { accMockAddress.Object, accMockPerson.Object };
            ccLibraryMock.SetupGet(l => l.Name).Returns("cclib1");
            ccLibraryMock.SetupGet(l => l.Accs).Returns(expectedAccs);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetCcLibraries()).Returns(new[] { ccLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IAcc acc = ccCache.GetCCFromCCLibrary("cclib1", "Address");
            ccCache.GetCCFromCCLibrary("cclib1", "Address");

            // Assertion and Verification
            Assert.That(acc, Is.SameAs(accMockAddress.Object));
            ccLibraryMock.VerifyGet(l => l.Accs, Times.Exactly(1));  
        }

        [Test]
        public void ShouldGetAndCacheBdtLibraryByName()
        {
            // Setup
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            bdtLibraryMock.SetupGet(l => l.Name).Returns("bdtlib1");
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetBdtLibraries()).Returns(new[] { bdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IBdtLibrary bdtLibrary = ccCache.GetBDTLibraryByName("bdtlib1");
            ccCache.GetBDTLibraryByName("bdtlib1");

            // Assertion and Verification
            Assert.That(bdtLibrary, Is.SameAs(bdtLibraryMock.Object));            
            cctsRepositoryMock.Verify(r => r.GetBdtLibraries(), Times.Exactly(1));
        }


        [Test]
        public void ShouldGetAndCacheBieLibraryByName()
        {
            // Setup
            var bieLibraryMock = new Mock<IBieLibrary>();
            bieLibraryMock.SetupGet(l => l.Name).Returns("bielib1");
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetBieLibraries()).Returns(new[] { bieLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IBieLibrary bieLibrary = ccCache.GetBIELibraryByName("bielib1");
            ccCache.GetBIELibraryByName("bielib1");

            // Assertion and Verification
            Assert.That(bieLibrary, Is.SameAs(bieLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetBieLibraries(), Times.Exactly(1));
        }       
    }
}