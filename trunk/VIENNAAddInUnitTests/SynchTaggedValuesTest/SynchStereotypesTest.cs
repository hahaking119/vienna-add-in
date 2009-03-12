using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VIENNAAddIn.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository;

namespace VIENNAAddInUnitTests
{
    
    
    /// <summary>
    ///This is a test class for SynchStereotypesTest and is intended
    ///to contain all SynchStereotypesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SynchStereotypesTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for FixRepository
        ///</summary>
        [TestMethod()]
        public void FixRepositoryTest()
        {
            Repository repo = new EARepository1();
            //to test with hand drawn model use this code:
            //var path = "C:\\Temp\\UPCC3-SampleModel.eap";
            //Debug.WriteLine(path);
            //var success = repo.OpenFile(path);
            //Assert.AreEqual(success,true);
            //initialise test model which should only have one taggedvalue set
            var count = 0;
            var countnew = 0;
            Package temp = (Package) repo.Models.GetAt(0);
            Package temp2 = (Package) temp.Packages.GetAt(0);
            Debug.WriteLine("Testing package: "+temp2.Element.TaggedValues.GetTaggedValue(TaggedValues.BaseURN));
            count = temp2.Element.TaggedValues.Count;   //count taggedvalues
            
            new SynchStereotypes().FixRepository(repo); //call fix function
            Debug.WriteLine("TaggedValue altered? " + temp2.Element.TaggedValues.GetTaggedValue(TaggedValues.BaseURN));
            Package tempnew = (Package)repo.Models.GetAt(0);
            Package tempnew2 = (Package)temp.Packages.GetAt(0);
            countnew = tempnew2.Element.TaggedValues.Count; //now count should be 10 (every package must have 9 taggedvalues, and it has Name already set)
            
            Assert.AreEqual(countnew, 10);
        }
    }
}
