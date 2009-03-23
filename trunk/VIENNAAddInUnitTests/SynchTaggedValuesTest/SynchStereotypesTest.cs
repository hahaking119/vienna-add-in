using System.Diagnostics;
using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn.Settings;
using Assert=Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using File=System.IO.File;
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
        #region Setup/Teardown

        [SetUp]
        // Create new Repository, copy the original testsetting to temporary file and load it into repository.
        public void Init()
        {
            repo = new Repository();
            File.Copy(
                Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) +
                "\\SynchTaggedValuesTest\\test.eap"
                ,
                Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) +
                "\\SynchTaggedValuesTest\\test_temp.eap", true);
            repo.OpenFile("C:\\VIENNAAddIn\\VIENNAAddInUnitTests\\SynchTaggedValuesTest\\test_temp.eap");
        }

        [TearDown]
        // Close the repository and delete the temporary file.
        public void Clean()
        {
            repo.CloseFile();
            File.Delete(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) +
                        "\\SynchTaggedValuesTest\\test_temp.eap");
        }

        #endregion

        public Repository repo;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// A test for the CheckConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckConnectorTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            var connector = (Connector) element.Connectors.GetAt(0);
            int count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            Assert.AreEqual(7, count1);
        }

        /// <summary>
        /// A test for the CheckElement Method
        /// test.eap contains a Element element1a missing 7 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckElementTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            int count1 = new SynchStereotypes().Check(element).Count;
            Debug.WriteLine("Checking element '" + element.Name + "': it is missing " + count1 + " TaggedValues.");
            Assert.AreEqual(7, count1);
        }

        /// <summary>
        /// A test for the CheckPackage Method
        /// test.eap contains a Package bLibrary missing 9 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckPackageTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            int count1 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("Checking package '" + testpackage.Element.Name + "': it is missing " + count1 +
                            " TaggedValues.");
            Assert.AreEqual(9, count1);
        }

        /// <summary>
        /// A test for the FixConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Fix should detect this correctly and fix it.
        ///</summary>
        [Test]
        public void FixConnectorTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            var connector = (Connector) element.Connectors.GetAt(0);
            int count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(connector);
            repo.RefreshOpenDiagrams(true);
            int count2 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("After the fix connector '" + connector.Name + "' is missing " + count2 + " TaggedValues.");
            Assert.AreEqual(0, count2);
        }

        /// <summary>
        /// A test for the FixElement Method
        /// test.eap contains a Element element1a missing 7 TaggedValues. Fix should detect this correctly and add the missing TaggedValues.
        ///</summary>
        [Test]
        public void FixElementTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            int count1 = new SynchStereotypes().Check(element).Count;
            Debug.WriteLine("Checking element '" + element.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(element);
            repo.RefreshOpenDiagrams(true);
            int count2 = new SynchStereotypes().Check(element).Count;
            Debug.WriteLine("After the fix element '" + element.Name + "' is missing " + count2 + " TaggedValues.");
            Assert.AreEqual(count2, 0);
        }

        /// <summary>
        /// A test for FixPackage
        /// test.eap contains a Package bLibrary missing 9 TaggedValues. Fix should detect this correctly and add the missing TaggedValues.
        ///</summary>
        [Test]
        public void FixPackageTest()
        {
            var testpackage = (Package) repo.Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            int count1 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("Checking package '" + testpackage.Element.Name + "': it is missing " + count1 +
                            " TaggedValues.");
            new SynchStereotypes().Fix(testpackage);
            repo.RefreshOpenDiagrams(true);
            int count2 = new SynchStereotypes().Check(testpackage).Count;
            Debug.WriteLine("After the fix package '" + testpackage.Element.Name + "' is missing " + count2 +
                            " TaggedValues.");
            Assert.AreNotEqual(count2, count1);
        }

        /// <summary>
        /// A test for FixRepository
        /// FixRepository should fix every element, connector and package of the whole model, so after fix no TaggedValues should be missing.
        ///</summary>
        [Test]
        public void FixRepositoryTest()
        {
            var mysynch = new SynchStereotypes();
            int missingvalues = 0;
            mysynch.Fix(repo);
            foreach (Package p in repo.Models)
            {
                foreach (Package pp in p.Packages)
                {
                    missingvalues += mysynch.Check(pp).Count;
                }
                foreach (Element e in p.Elements)
                {
                    missingvalues += mysynch.Check(e).Count;                    
                }
            }
            Assert.AreEqual(0,missingvalues);
        }
    }
}