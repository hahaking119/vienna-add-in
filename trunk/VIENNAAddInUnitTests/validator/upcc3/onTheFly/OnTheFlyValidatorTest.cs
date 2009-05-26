using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.validator.upcc3.onTheFly;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.validator.upcc3.onTheFly
{
    [TestFixture]
    public class OnTheFlyValidatorTest
    {
        [Test]
        public void An_element_with_stereotype_PRIM_can_be_added_to_a_PRIMLibrary()
        {
            var primLibrary = new ValidatingPRIMLibrary();
            var primMock = MockPRIM("String", "27");
            primLibrary.ProcessElement(primMock.Object);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
        }

        [Test]
        public void An_element_with_a_stereotype_other_than_PRIM_cannot_be_added_to_a_PRIMLibrary()
        {
            var primLibrary = new ValidatingPRIMLibrary();
            var primMock = new Mock<Element>();
            primMock.SetupGet(e => e.Stereotype).Returns("foobar");
            primMock.SetupGet(e => e.Name).Returns("String");
            primMock.SetupGet(e => e.ElementGUID).Returns("27");
            var validationIssues = primLibrary.ProcessElement(primMock.Object);
            Assert.AreEqual(1, validationIssues.Count());
            var invalidStereotype = validationIssues.First();
            Assert.AreEqual("27", invalidStereotype.GUID);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
        }

        [Test]
        public void PRIM_names_must_be_unique_within_a_PRIMLibrary()
        {
            var primLibrary = new ValidatingPRIMLibrary();
            var primMock1 = MockPRIM("String", "27");
            var primMock2 = MockPRIM("String", "28");
            primLibrary.ProcessElement(primMock1.Object);
            primLibrary.ProcessElement(primMock2.Object);
            Assert.AreEqual(0, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
        }

        private static Mock<Element> MockPRIM(string name, string guid)
        {
            var primMock = new Mock<Element>();
            primMock.SetupGet(e => e.Stereotype).Returns(Stereotype.PRIM);
            primMock.SetupGet(e => e.Name).Returns(name);
            primMock.SetupGet(e => e.ElementGUID).Returns(guid);
            return primMock;
        }
    }

    public enum ValidationError
    {
        InvalidStereotype,
        DuplicateName
    }

    public class ValidationException : Exception
    {
        public ValidationException(string itemGUID, ValidationError reason)
        {
            ItemGuid = itemGUID;
            Reason = reason;
        }

        public string ItemGuid { get; private set; }

        public ValidationError Reason { get; private set; }
    }

    public class ValidatingPRIMLibrary
    {
        private List<IPRIM> prims = new List<IPRIM>();

        public IEnumerable<ValidationIssue> ProcessElement(Element element)
        {
            var issues = new List<ValidationIssue>();
            if (element.Stereotype != Stereotype.PRIM){
                issues.Add(new InvalidStereotype(element.ElementGUID, Stereotype.PRIM));
            }
            prims.Add(new PRIM(element));
            return issues;
        }

        public IEnumerable<IPRIM> PRIMs
        {
            get { return prims; }
        }
    }

    public class PRIM : IPRIM
    {
        private readonly Element element;

        public PRIM(Element element)
        {
            this.element = element;
        }

        public int Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return element.Name; }
        }

        public string DictionaryEntryName
        {
            get { throw new NotImplementedException(); }
        }

        public string Definition
        {
            get { throw new NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string LanguageCode
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public IBusinessLibrary Library
        {
            get { throw new NotImplementedException(); }
        }

        public string Pattern
        {
            get { throw new NotImplementedException(); }
        }

        public string FractionDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string Length
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxLength
        {
            get { throw new NotImplementedException(); }
        }

        public string MinExclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinInclusive
        {
            get { throw new NotImplementedException(); }
        }

        public string MinLength
        {
            get { throw new NotImplementedException(); }
        }

        public string TotalDigits
        {
            get { throw new NotImplementedException(); }
        }

        public string WhiteSpace
        {
            get { throw new NotImplementedException(); }
        }

        public IPRIM IsEquivalentTo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
