// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.IO;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.XSDImporter.util
{
    [TestFixture]
    public class CustomSchemaReaderTest
    {
        #region Test Preparation 

        private string testSchemaFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDImporterTest\\util\\BusinessInformationEntity_1.xsd";

        #endregion 

        [Test]
        public void UselessTest()
        {
            Console.WriteLine(File.ReadAllText(testSchemaFile));
        }
   }
}