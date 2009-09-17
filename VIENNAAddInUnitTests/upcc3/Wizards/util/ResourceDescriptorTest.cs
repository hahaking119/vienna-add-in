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
using System.Net;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ResourceDescriptorTest
    {
        [Test]
        public void ShouldAppendRelativePathForMajorAndMinorVersion()
        {
            ResourceDescriptor defaultDescriptor = new ResourceDescriptor();
            ResourceDescriptor customDescriptor = new ResourceDescriptor("major", "minor");

            Assert.That(customDescriptor.DownloadUri, Is.EqualTo(defaultDescriptor.DownloadUri + "major_minor/"));
            Assert.That(customDescriptor.StorageDirectory, Is.EqualTo(defaultDescriptor.StorageDirectory+ "major_minor\\"));
        }
    }
}