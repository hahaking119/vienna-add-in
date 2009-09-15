// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class VersionDescriptorTest
    {
        [Test]
        public void ValidateGeneratedDownloadDirectory()
        {
            VersionDescriptor versionDescriptor = new VersionDescriptor {Version = "ccl08b", Iteration = "1", Comment = "bla"};

            string downloadUri = versionDescriptor.ResourceDirectory;
            Assert.That(downloadUri, Is.EqualTo("ccl08b_1"));            
        }
    }
    
    [TestFixture]
    public class VersionHandlerTest
    {        
        private VersionHandler versionHandler;

        [SetUp]
        public void Setup()
        {            
            Mock<IWebClientMediator> webClientMediator = new Mock<IWebClientMediator>();
            string versionsString = "ccl08b|1|bla\nccl08b|2|blabla\nccl09a|1|bla";
            webClientMediator.Setup(mediator => mediator.DownloadString("testuri")).Returns(versionsString);
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();

            webClientMediator.VerifyAll();
        }

        [Test]
        public void ShouldDownloadOnlineVersionFileAndExtractAvailableVersions()
        {
            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "1", Comment = "bla"},
                                                    new VersionDescriptor {Version = "ccl09a", Iteration = "1", Comment = "bla"},
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "2", Comment = "blabla"},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }

        [Test]
        public void ShouldFilterVersionDescriptionsBasedOnParticularVersion()                        
        {
            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "1", Comment = "bla"},
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "2", Comment = "blabla"},                                                    
                                                };
                        
            Assert.That(versionHandler.FilterDescriptors("ccl08b"), Is.EquivalentTo(expectedVersions));
        }

        [Test]
        public void ShouldFilterVersionDescriptionsBasedOnParticularVersionAndParticularIteration()
        {
            VersionDescriptor expectedVersion = new VersionDescriptor
                                                    {Version = "ccl09a", Iteration = "1", Comment = "bla"};
                        
            Assert.That(versionHandler.FilterDescriptors("ccl09a", "1"), Is.EqualTo(expectedVersion));
        }        
    }

    [TestFixture]
    public class VersionHandlerIntegrationTest
    {
        [Test]
        [Category(TestCategories.FileBased)]
        public void ShouldDownloadOnlineVersionFileIntegrationTest()
        {
            VersionHandler versionHandler = new VersionHandler(new WebClientMediator(), "http://www.umm-dev.org/xmi/ccl_versions.txt");
            versionHandler.RetrieveAvailableVersions();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "1", Comment = "Core Component Library v08B published by UN/CEFACT."},
                                                    new VersionDescriptor {Version = "ccl09a", Iteration = "1", Comment = "Core Component Library v09A published by UN/CEFACT."},
                                                    new VersionDescriptor {Version = "ccl08b", Iteration = "2", Comment = "Core Component Library v08B published by UN/CEFACT which has been adapted by Research Studios Austria to fix modeling issues."},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }        
    }

    public class WebClientMediator : IWebClientMediator
    {
        public string DownloadString(string uri)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }
    }
}