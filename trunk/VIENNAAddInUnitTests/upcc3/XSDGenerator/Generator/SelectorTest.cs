// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator
{
    [TestFixture]
    public class SelectorTest
    {
        [Test]
        public void testSelector()
        {
            var numbersList = new List<int>(new[] {1, 2, 3, 4});
            var sel = new Selector<int>(i => Console.WriteLine("no match for {0}", (object) i), true);
            sel.If(i => i == 1, i => Console.WriteLine("one"));
            sel.If(i => i == 2, i => Console.WriteLine("two"));
            sel.If(i => i == 3, i => Console.WriteLine("three"));
            numbersList.ForEach(sel.Execute);
        }
    }
}