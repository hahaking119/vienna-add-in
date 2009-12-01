using System.Collections.Generic;
using System.Diagnostics;
using EA;
using NUnit.Framework;
using UpccModel;
using VIENNAAddIn;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Assert=Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Attribute=EA.Attribute;
using Constraint=NUnit.Framework.Constraints.Constraint;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;
using System.Linq;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    [TestFixture]
    public class SynchStereotypesTest : SynchStereotypesTestsBase
    {
        /// <summary>
        /// A test for the CheckConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckConnectorTest()
        {
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            var connector = (Connector) element.Connectors.GetAt(0);
            int count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            Assert.AreEqual(8, count1);
        }

        /// <summary>
        /// A test for the CheckElement Method
        /// test.eap contains a Element element1a missing 7 TaggedValues. Check should detect this correctly.
        ///</summary>
        [Test]
        public void CheckElementTest()
        {
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
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
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            int count1 = new SynchStereotypes().Check(testpackage)[0].Count;
            Debug.WriteLine("Checking package '" + testpackage.Element.Name + "': it is missing " + count1 +
                            " TaggedValues.");
            Assert.AreEqual(10, count1); // 1 title and 9 missing values = 10 items
        }

        /// <summary>
        /// A test for the FixConnector Method
        /// test.eap contains a Connector connector1 missing 8 TaggedValues. Fix should detect this correctly and fix it.
        ///</summary>
        [Test]
        public void FixConnectorTest()
        {
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            var connector = (Connector) element.Connectors.GetAt(0);
            int count1 = new SynchStereotypes().Check(connector).Count;
            Debug.WriteLine("Checking connector '" + connector.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(connector);
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
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            var element = (Element) testpackage.Elements.GetAt(0);
            int count1 = new SynchStereotypes().Check(element).Count;
            Debug.WriteLine("Checking element '" + element.Name + "': it is missing " + count1 + " TaggedValues.");
            new SynchStereotypes().Fix(element);
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
            var testpackage = (Package) CreateRepositoryWithoutTaggedValues().Models.GetAt(0);
            testpackage = (Package) testpackage.Packages.GetAt(0);
            Debug.WriteLine(testpackage.Element.Stereotype);
            List<List<string>> checkResult = new SynchStereotypes().Check(testpackage);
            int count1 = checkResult.Count;
            Debug.WriteLine("Checking package '" + testpackage.Element.Name + "': it is missing " + count1 +
                            " TaggedValues.");
            new SynchStereotypes().Fix(testpackage);
            List<List<string>> checkResult2 = new SynchStereotypes().Check(testpackage);
            int count2 = checkResult2.Count;
            Debug.WriteLine("After the fix package '" + testpackage.Element.Name + "' is missing " + count2 +
                            " TaggedValues.");
            Assert.AreEqual(0, count2);
        }

        /// <summary>
        /// A test for FixRepository
        /// FixRepository should fix every element, connector and package of the whole model, so after fix no TaggedValues should be missing.
        ///</summary>
        [Test]
        public void FixRepositoryTest()
        {
            Repository repo = CreateRepositoryWithoutTaggedValues();
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
            Assert.AreEqual(0, missingvalues);
        }

        [Test]
        public void ShouldCreateAllMissingTaggedValues()
        {
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var bLibrary = repo.Resolve<Package>((Path)"Model" / "bLibrary1");
            NUnit.Framework.Assert.That(bLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package primLibrary = bLibrary.PackageByName("primLibrary");
            NUnit.Framework.Assert.That(primLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.PrimLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package enumLibrary = bLibrary.PackageByName("enumLibrary");
            NUnit.Framework.Assert.That(enumLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.EnumLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package cdtLibrary = bLibrary.PackageByName("cdtLibrary");
            NUnit.Framework.Assert.That(cdtLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.CdtLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package ccLibrary = bLibrary.PackageByName("ccLibrary");
            NUnit.Framework.Assert.That(ccLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.CcLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package bdtLibrary = bLibrary.PackageByName("bdtLibrary");
            NUnit.Framework.Assert.That(bdtLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BdtLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package bieLibrary = bLibrary.PackageByName("bieLibrary");
            NUnit.Framework.Assert.That(bieLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BieLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Package docLibrary = bLibrary.PackageByName("docLibrary");
            NUnit.Framework.Assert.That(docLibrary, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.DocLibrary.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Element prim = primLibrary.ElementByName("prim");
            NUnit.Framework.Assert.That(prim, HasTaggedValues(UpccModel.UpccModel.Instance.DataTypes.Prim.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Element @enum = enumLibrary.ElementByName("enum");
            NUnit.Framework.Assert.That(@enum, HasTaggedValues(UpccModel.UpccModel.Instance.Enumerations.Enum.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            // TODO code list entries
            //Attribute codeListEntry = @enum.Attributes.GetByName("codeListEntry") as Attribute;
            //            NUnit.Framework.Assert.That(codeListEntry, HasTaggedValues(UpccModel.UpccModel.Instance.DataTypes.Enum.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            // TODO IDScheme

            Element cdt = cdtLibrary.ElementByName("cdt");
            NUnit.Framework.Assert.That(cdt, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Cdt.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Element acc = ccLibrary.ElementByName("acc");
            NUnit.Framework.Assert.That(acc, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Acc.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Attribute bcc = acc.Attributes.GetByName("bcc") as Attribute;
            NUnit.Framework.Assert.That(bcc, HasTaggedValues(UpccModel.UpccModel.Instance.Attributes.Bcc.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");

            Connector ascc = (Connector) acc.Connectors.GetByName("ascc");
            NUnit.Framework.Assert.That(ascc, HasTaggedValues(UpccModel.UpccModel.Instance.Associations.Ascc.TaggedValues.Convert(tv => tv.Name)), "missing tagged values");
        }
    }

    public class SynchStereotypesTestsBase
    {
        protected Repository CreateRepositoryWithoutTaggedValues()
        {
            return new SynchStereotypesTestRepository();
        }

        protected static Constraint HasTaggedValues(IEnumerable<string> taggedValues)
        {
            return new HasTaggedValuesConstraint(taggedValues);
        }
    }

    internal class HasTaggedValuesConstraint : Constraint
    {
        private readonly List<string> expectedTaggedValues;
        private string actualName;
        private List<string> missingTaggedValues;

        public HasTaggedValuesConstraint(IEnumerable<string> expectedTaggedValues)
        {
            this.expectedTaggedValues = new List<string>(expectedTaggedValues);
        }

        public override bool Matches(object actual)
        {
            List<string> actualTaggedValues;
            if (actual is Package)
            {
                var package = (Package) actual;
                actualName = package.Name;
                actualTaggedValues = new List<string>(GetTaggedValues(package.Element));
            }
            else if (actual is Element)
            {
                var element = (Element) actual;
                actualName = element.Name;
                actualTaggedValues = new List<string>(GetTaggedValues(element));
            }
            else if (actual is Attribute)
            {
                var attribute = (Attribute) actual;
                actualName = attribute.Name;
                actualTaggedValues = new List<string>(GetTaggedValues(attribute));
            }
            else if (actual is Connector)
            {
                var connector = (Connector) actual;
                actualName = connector.Name;
                actualTaggedValues = new List<string>(GetTaggedValues(connector));
            }
            else
            {
                actualTaggedValues = new List<string>();
            }

            this.actual = actualTaggedValues;

            missingTaggedValues = new List<string>();
            foreach (var expectedTaggedValue in expectedTaggedValues)
            {
                if (!actualTaggedValues.Contains(expectedTaggedValue))
                {
                    missingTaggedValues.Add(expectedTaggedValue);
                }
            }
            return missingTaggedValues.Count == 0;
        }

        private IEnumerable<string> GetTaggedValues(Element element)
        {
            foreach (TaggedValue actualTaggedValue in element.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        private IEnumerable<string> GetTaggedValues(Attribute attribute)
        {
            foreach (TaggedValue actualTaggedValue in attribute.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        private IEnumerable<string> GetTaggedValues(Connector connector)
        {
            foreach (TaggedValue actualTaggedValue in connector.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
//            writer.WriteMessageLine(actualName + " misses tagged values:");
            writer.WriteExpectedValue(expectedTaggedValues);
        }
    }

    public class SynchStereotypesTestRepository : EARepository
    {
        public SynchStereotypesTestRepository()
        {
            Element element1a = null;
            Element element1b = null;
            var model = this.AddModel(
                "Model", m => m.AddPackage(
                                  "bLibrary", bLib =>
                                              {
                                                  bLib.Element.Stereotype = Stereotype.bLibrary;
                                                  bLib.AddPackage(
                                                      "BIELibrary", bieLib =>
                                                                    {
                                                                        bieLib.Element.Stereotype = Stereotype.BIELibrary;
                                                                        element1a = bieLib.AddClass("element1a").With(c => c.Stereotype = Stereotype.ABIE);
                                                                        element1b = bieLib.AddClass("element1b").With(c => c.Stereotype = Stereotype.BBIE);
                                                                    });
                                                  bLib.AddPackage(
                                                      "CCLibrary", bieLib =>
                                                                   {
                                                                       bieLib.Element.Stereotype = Stereotype.CCLibrary;
                                                                       bieLib.AddClass("element2a").With(c => c.Stereotype = Stereotype.ACC);
                                                                       bieLib.AddClass("element2b").With(c => c.Stereotype = Stereotype.BCC);
                                                                       bieLib.AddClass("element2c").With(c => c.Stereotype = "CC");
                                                                   });
                                                  bLib.AddPackage(
                                                      "CDTLibrary", bieLib =>
                                                                    {
                                                                        bieLib.Element.Stereotype = Stereotype.CDTLibrary;
                                                                        bieLib.AddClass("element3a").With(c => c.Stereotype = Stereotype.CDT);
                                                                    });
                                              }
                                  ));
            element1a.AddASBIE(element1b, "connector1", EaAggregationKind.Shared);
            var bLibrary = model.AddPackage(
                "bLibrary", bLib => { bLib.Element.Stereotype = Stereotype.bLibrary; }
                );
            Element prim = null;
            bLibrary.AddPackage(
                "PRIMLibrary", primLib =>
                               {
                                   primLib.Element.Stereotype = Stereotype.PRIMLibrary;
                                   prim = primLib.AddPRIM("PRIM");
                               });
            bLibrary.AddPackage(
                "ENUMLibrary", enumLib =>
                               {
                                   enumLib.Element.Stereotype = Stereotype.ENUMLibrary;
                                   enumLib.AddENUM("ENUM", prim);
                               });
            bLibrary.AddPackage(
                "CDTLibrary", cdtLib =>
                              {
                                  cdtLib.Element.Stereotype = Stereotype.CDTLibrary;
                                  var cdt = cdtLib.AddCDT("CDT");
                                  cdt.AddAttribute("CON", prim).With((a => { a.Stereotype = Stereotype.CON; }));
                                  cdt.AddSUP(prim, "SUP");
                              });
            bLibrary.AddPackage(
                "CCLibrary", ccLib =>
                             {
                                 ccLib.Element.Stereotype = Stereotype.CCLibrary;
                                 var acc = ccLib.AddACC("ACC");
                                 acc.AddBCC(prim, "BCC");
                             });
            bLibrary.AddPackage(
                "BDTLibrary", bdtLib =>
                              {
                                  bdtLib.Element.Stereotype = Stereotype.BDTLibrary;
                                  var bdt = bdtLib.AddBDT("BDT");
                                  bdt.AddAttribute("CON", prim).With((a => { a.Stereotype = Stereotype.CON; }));
                                  bdt.AddSUP(prim, "SUP");
                              });
            bLibrary.AddPackage(
                "BIELibrary", bieLib =>
                              {
                                  bieLib.Element.Stereotype = Stereotype.BIELibrary;
                                  var abie = bieLib.AddABIE("ABIE");
                                  abie.AddBBIE(prim, "BBIE");
                              });
            bLibrary.AddPackage(
                "DOCLibrary", bieLib =>
                              {
                                  bieLib.Element.Stereotype = Stereotype.DOCLibrary;
                              });
        }
    }
}