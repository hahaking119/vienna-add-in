using System;
using NUnit.Framework;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void testStringDefault()
        {
            Assert.AreEqual("abc" + null, "abc");
            Assert.AreEqual("abc" + "", "abc");
            Assert.AreEqual("abc" + String.Empty, "abc");

            Assert.AreEqual(String.Empty.DefaultTo("abc"), "abc");
            Assert.AreEqual("".DefaultTo("abc"), "abc");

            const string nullString = null;
            Assert.AreEqual(nullString.DefaultTo("abc"), "abc");

            Assert.AreEqual("foo".DefaultTo("bar"), "foo");
        }
    }
}