// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using NUnit.Framework;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.ccts
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