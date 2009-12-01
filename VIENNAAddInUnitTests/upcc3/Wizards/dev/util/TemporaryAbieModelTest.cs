using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;
using Assert=Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class TemporaryABIEModelTest
    {
        private EARepositoryCCCache eaRepository;
        private CCRepository ccRepository;
        private TemporaryAbieModel temporaryABIEModel;
        private CcCache ccCache;

        #region Test SetUp/TearDown

        [SetUp]
        public void Setup()
        {
            eaRepository = new EARepositoryCCCache();
            ccRepository = new CCRepository(eaRepository);
            //temporaryABIEModel = new TemporaryAbieModel(ccRepository);
            ccCache = CcCache.GetInstance(ccRepository);
        }

        [TearDown]
        public void Teardown()
        {
            eaRepository = null;
            ccRepository = null;
            temporaryABIEModel = null;
        }

        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldCreateBBIE()
        {
            List<string> BDTs = new List<string>();
            //temporaryABIEModel.createBBIE("underlyingBCCName", "BBIEName", BDTs);
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldCreateCDT()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldGeneratePotentialASBIEs()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldSelectASBIE()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldSelectBBIE()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldSelectBCC()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void ShouldSelectBDT()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void ShouldSetTargetACC()
        {
            IAcc acc = ccCache.GetCCFromCCLibrary("cclib1", "Address");
            
            //temporaryABIEModel.SetTargetACC("Person");

            //Assert.Equals(acc, temporaryABIEModel.GetBasedOnACC());
        }
    }
}
