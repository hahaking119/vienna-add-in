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
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class VersionDescriptorTest
    {
        [Test]
        public void ValidateGeneratedDownloadDirectory()
        {
            VersionDescriptor versionDescriptor = new VersionDescriptor {Major = "ccl08b", Minor = "1", Comment = "bla"};

            string downloadUri = versionDescriptor.ResourceDirectory;
            Assert.That(downloadUri, Is.EqualTo("ccl08b_1"));            
        }
    }
    
    [TestFixture]
    public class VersionHandlerTest
    {        
        private VersionHandler versionHandler;

        private Mock<IWebClientMediator> AWebClientMediator()
        {
            Mock<IWebClientMediator> webClientMediator = new Mock<IWebClientMediator>();
            string versionsString = "ccl08b|1|bla\nccl08b|2|blabla\nccl09a|1|bla";
            webClientMediator.Setup(mediator => mediator.DownloadString("testuri")).Returns(versionsString);
            return webClientMediator;
        }

        private Mock<IWebClientMediator> AWebClientMediatorWhichThrowsAnException()
        {
            Mock<IWebClientMediator> webClientMediator = new Mock<IWebClientMediator>();
            webClientMediator.Setup(mediator => mediator.DownloadString("testuri")).Throws(new WebException("Der Remoteserver hat einen Fehler zurückgegeben: (404) Nicht gefunden."));
            return webClientMediator;
        }

        private Mock<IWebClientMediator> AWebClientMediatorWhichReturnsAnEmptyString()
        {
            Mock<IWebClientMediator> webClientMediator = new Mock<IWebClientMediator>();
            string versionsString = "";
            webClientMediator.Setup(mediator => mediator.DownloadString("testuri")).Returns(versionsString);
            return webClientMediator;
        }

        private Mock<IWebClientMediator> AWebClientMediatorWhichReturnsAnInvalidVersionsString()
        {
            Mock<IWebClientMediator> webClientMediator = new Mock<IWebClientMediator>();
            string versionsString = "ccl08b|1|bla\nccl08b|2\nccl09a|1|bla";
            webClientMediator.Setup(mediator => mediator.DownloadString("testuri")).Returns(versionsString);
            return webClientMediator;
        }


        [Test]
        public void ShouldDownloadOnlineVersionFileAndExtractAvailableVersions()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediator();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();

            webClientMediator.VerifyAll();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Major = "ccl08b", Minor = "1", Comment = "bla"},
                                                    new VersionDescriptor {Major = "ccl09a", Minor = "1", Comment = "bla"},
                                                    new VersionDescriptor {Major = "ccl08b", Minor = "2", Comment = "blabla"},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }


        [Test]
        public void ShouldProvideAvailableMajorVersions()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediator();
            List<string> expectedMajorVersions = new List<string> { "ccl08b", "ccl09a" };

            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");

            versionHandler.RetrieveAvailableVersions();            

            Assert.That(versionHandler.GetMajorVersions(), Is.EqualTo(expectedMajorVersions));
        }

        [Test]
        public void ShouldProvideAvailableMinorVersionsForMajorVersion()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediator();
            List<string> expectedMinorVersions = new List<string> { "1", "2" };            
            
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
                        
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.GetMinorVersions("ccl08b"), Is.EqualTo(expectedMinorVersions));
        }

        [Test]
        public void ShouldProvideCommentForMajorVersionAndMinorVersion()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediator();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.GetComment("ccl09a", "1"), Is.EqualTo("bla"));
        }        

        [Test]
        [ExpectedException(typeof (WebException))]
        public void ShouldThrowExceptionIfVersionsFileIsUnavailable()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediatorWhichThrowsAnException();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();                                  
        }

        [Test]        
        public void AvailableVersionsListShouldBeEmptyIfVersionsFileIsUnavailable()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediatorWhichThrowsAnException();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");

            try
            {
                versionHandler.RetrieveAvailableVersions();
                Assert.Fail("Expected exception \"System.Net.WebException\" not thrown.");
            }
            catch (WebException)
            {
                Assert.That(versionHandler.AvailableVersions, Is.Empty);
            }            
        }

        [Test]
        public void AvailableVersionsListShouldBeEmptyIfVersionsFileIsEmpty()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediatorWhichReturnsAnEmptyString();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.AvailableVersions, Is.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionIfVersionsStringIsInvalid()
        {
            Mock<IWebClientMediator> webClientMediator = AWebClientMediatorWhichReturnsAnInvalidVersionsString();
            versionHandler = new VersionHandler(webClientMediator.Object, "testuri");
            versionHandler.RetrieveAvailableVersions();
        }
    }

    [TestFixture]
    public class VersionHandlerIntegrationTest
    {
        [Test]
        [Category(TestCategories.FileBased)]
        public void ShouldDownloadOnlineVersionFileIntegrationTest()
        {
            VersionHandler versionHandler = new VersionHandler(new WebClientMediator(), "http://www.umm-dev.org/xmi/testresources/ccl_versions.txt");
            versionHandler.RetrieveAvailableVersions();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Major = "simplified", Minor = "1", Comment = "Simplified Core Component Library v08B published by UN/CEFACT."},
                                                    new VersionDescriptor {Major = "simplified", Minor = "2", Comment = "Simplified Core Component Library v08B published by UN/CEFACT which has been adapted by Research Studios Austria to fix mistakes in the Core Component model. "},
                                                    new VersionDescriptor {Major = "complex", Minor = "1", Comment = "Complex Core Component Library v09A published by UN/CEFACT."},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }        
    }

    [TestFixture]
    public class StandardLibraryImporterFormTest
    {
        [Test]
        public void ShouldLaunchAndPopulateStandardLibraryImporterForm()
        {
            StandardLibraryImporterForm.ShowForm(null);    
        }        
    }
}