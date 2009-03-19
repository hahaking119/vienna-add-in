using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using EA;
using VIENNAAddIn.Settings;
using Assert=Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using TestContext=Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    /// <summary>
    ///This is a test class for SynchStereotypesTest and is intended
    ///to contain all SynchStereotypesTest Unit Tests
    ///</summary>
    [TestFixture]
    public class SynchStereotypesTest
    {
        public Repository repo;
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            repo = new Repository();
            System.IO.File.Copy(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + "\\SynchTaggedValuesTest\\test.eap"
            , Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + "\\SynchTaggedValuesTest\\test_temp.eap", true);
            repo.OpenFile("C:\\VIENNAAddIn\\VIENNAAddInUnitTests\\SynchTaggedValuesTest\\test_temp.eap");
        }
        [TearDown]
        public void Clean()
        {
            repo.CloseFile();
            System.IO.File.Delete(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + "\\SynchTaggedValuesTest\\test_temp.eap");
        }
        #endregion
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// A test for the CheckPackage Method
        /// test.eap contains a Package bLibrary missing 9 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckPackageTest()
        {         
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var count1 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("Checking package '" +testpackage.Element.Name+ "': it is missing " + count1+" TaggedValues.");
            Assert.AreEqual(9,count1);
        }
        /// <summary>
        /// A test for the CheckElement Method
        /// test.eap contains a Element element1a missing 7 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckElementTest()
        {
            var testpackage = (Package)repo.Models.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            var count1 = new SynchStereotypes().Check(element).Count;
            Debug.WriteLine("Checking element '" + element.Name + "': it is missing " + count1 + " TaggedValues.");
            Assert.AreEqual(7, count1);
        }
        /// <summary>
        /// A test for the CheckConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckConnectorTest()
        {
            var testpackage = (Package)repo.Models.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            var element = (Element)testpackage.Elements.GetAt(0);
            var connector = (Connector) element.Connectors.GetAt(0);
            var count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            Assert.AreEqual(7, count1);
        }


        /// <summary>
        /// A test for the FixConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Fix should detect this correctly and fix it.
        ///</summary>
        [Test]
        public void FixConnectorTest()
        {
            var testpackage = (Package)repo.Models.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            var element = (Element)testpackage.Elements.GetAt(0);
            var connector = (Connector)element.Connectors.GetAt(0);
            var count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(connector);
            repo.RefreshOpenDiagrams(true);
            var count2 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("After the fix connector '" + connector.Name + "' is missing " + count2 + " TaggedValues.");
            Assert.AreNotEqual(count1,count2);
        }
        /// <summary>
        /// A test for FixPackage
        /// test.eap contains a Package bLibrary missing 9 TaggedValues. Fix should detect this correctly and add the missing TaggedValues.
        ///</summary>
        [Test]
        public void FixPackageTest()
        {
            var testpackage = (Package)repo.Models.GetAt(0);
            testpackage = (Package)testpackage.Packages.GetAt(0);
            var count1 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("Checking package '" + testpackage.Element.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(testpackage);
            repo.RefreshOpenDiagrams(true);
            var count2 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("After the fix package '" + testpackage.Element.Name + "' is missing " + count2 + " TaggedValues.");
            Assert.AreNotEqual(count2, count1);
        }
    }
}