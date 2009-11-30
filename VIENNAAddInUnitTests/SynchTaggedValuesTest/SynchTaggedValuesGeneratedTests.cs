using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.Settings;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    [TestFixture]
    public class SynchTaggedValuesGeneratedTests : SynchStereotypesTestsBase
    {

		[Test]
		public void ShouldCreateMissingTaggedValuesForBLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForPrimLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"PRIMLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.PrimLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForPrim()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"PRIMLibrary"/"PRIM");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.DataTypes.Prim.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForEnumLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"ENUMLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.EnumLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForEnum()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"ENUMLibrary"/"ENUM");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.DataTypes.Enum.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdtLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"CDTLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.CdtLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdt()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CDTLibrary"/"CDT");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Cdt.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCcLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"CCLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.CcLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAcc()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CCLibrary"/"ACC");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Acc.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBccs()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CCLibrary"/"ACC");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("BCC");
			Assert.That(attribute, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(attribute, HasTaggedValues(UpccModel.UpccModel.Instance.Attributes.GetByName("Bccs").TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdtLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"BDTLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BdtLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdt()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BDTLibrary"/"BDT");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Bdt.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBieLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"BIELibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.BieLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAbie()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BIELibrary"/"ABIE");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Abie.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForDocLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"DOCLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(UpccModel.UpccModel.Instance.Packages.DocLibrary.TaggedValues), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForMa()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchStereotypes();
            synchStereotypes.Fix(repo);

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"DOCLibrary"/"MA");
			Assert.That(element, Is.Not.Null, "Element not found");
            NUnit.Framework.Assert.That(element, HasTaggedValues(UpccModel.UpccModel.Instance.Classes.Ma.TaggedValues), "missing tagged values");
		}
	}
}
