using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{
 
    [TestFixture]
    public class XMLSchemaReaderTest
    {

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

        [Test]
        public void XMLSchemaReaderConstructorTest()
        {
            XMLSchemaReader.Read("C:/VIENNAAddIn/VIENNAAddIn/upcc3/import/ebInterface/xsd/Invoice.xsd");
        }
    }
}
