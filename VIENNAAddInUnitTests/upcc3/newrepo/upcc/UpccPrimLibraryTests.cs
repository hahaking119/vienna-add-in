using System.Collections.Generic;
using System.Linq;
using CctsRepository.PrimLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    [TestFixture]
    public class UpccPrimLibraryTests
    {
        [Test]
        public void ShouldReturnId()
        {
            var umlPackage = new UmlPackageBuilder()
                .WithId(37)
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.Id, Is.EqualTo(umlPackage.Id));
        }

        [Test]
        public void ShouldReturnName()
        {
            var umlPackage = new UmlPackageBuilder()
                .WithName("aPackage")
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.Name, Is.EqualTo(umlPackage.Name));
        }

		[Test]
		public void ShouldResolveBLibrary()
		{
            var parentUmlPackage = new UmlPackageBuilder()
                .WithId(76)
                .Build();
            var umlPackage = new UmlPackageBuilder()
				.WithParent(parentUmlPackage)
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.BLibrary, Is.Not.Null);
            Assert.That(primLibrary.BLibrary.Id, Is.EqualTo(parentUmlPackage.Id));
		}

		[Test]
		public void ShouldResolvePrims()
		{
            var umlPackage = new UmlPackageBuilder()
                .WithDataType(new UmlDataTypeBuilder().WithId(57).Build())
                .WithDataType(new UmlDataTypeBuilder().WithId(61).Build())
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(new List<int>(primLibrary.Prims.Select(prim => prim.Id)),
						Is.EquivalentTo(new List<int>(umlPackage.DataTypes.Select(dataType => dataType.Id))));
		}

		[Test]
		public void ShouldReturnEmptyEnumerationIfNoPrimsExist()
		{
            var umlPackage = new UmlPackageBuilder()
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.Prims, Is.Empty);
		}

		[Test]
		public void ShouldResolvePrimsByName()
		{
            var umlPackage = new UmlPackageBuilder()
                .WithDataType(new UmlDataTypeBuilder().WithId(57).WithName("prim57").Build())
                .WithDataType(new UmlDataTypeBuilder().WithId(61).WithName("prim61").Build())
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.GetPrimByName("prim57").Id, Is.EqualTo(57));
		}

		[Test]
		public void ShouldReturnNullIfPrimsCannotBeFoundByName()
		{
            var umlPackage = new UmlPackageBuilder()
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.GetPrimByName("should not be found"), Is.Null);
		}

		[Test]
		public void ShouldCreatePrimsAccordingToSpecification()
		{
            var equivalentPrimDataType = new UmlDataTypeBuilder()
                .WithId(22)
                .WithName("prim22")
                .Build();
            var primDataTypeMock = new UmlDataTypeBuilder()
                .WithId(23)
                .BuildMock();
            var umlPackageMock = new UmlPackageBuilder()
                .WithDataType(equivalentPrimDataType)
                .BuildMock();
            umlPackageMock.Setup(umlPackage => umlPackage.CreateDataType(It.IsAny<UmlDataTypeSpec>())).Returns(primDataTypeMock.Object);
            umlPackageMock.Setup(umlPackage => umlPackage.GetDataTypeById(22)).Returns(equivalentPrimDataType);

            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackageMock.Object);
            var equivalentPrim = primLibrary.GetPrimByName("prim22");
            var primSpec = new PrimSpec
                           {
                               Name = "prim23",
                               IsEquivalentTo = equivalentPrim,
            				   BusinessTerms = new[] {"businessTerm_Value1", "businessTerm_Value2"},
            				   Definition = "definition_Value",
            				   DictionaryEntryName = "dictionaryEntryName_Value",
            				   FractionDigits = "fractionDigits_Value",
            				   LanguageCode = "languageCode_Value",
            				   Length = "length_Value",
            				   MaximumExclusive = "maximumExclusive_Value",
            				   MaximumInclusive = "maximumInclusive_Value",
            				   MaximumLength = "maximumLength_Value",
            				   MinimumExclusive = "minimumExclusive_Value",
            				   MinimumInclusive = "minimumInclusive_Value",
            				   MinimumLength = "minimumLength_Value",
            				   Pattern = "pattern_Value",
            				   TotalDigits = "totalDigits_Value",
            				   UniqueIdentifier = "uniqueIdentifier_Value",
            				   VersionIdentifier = "versionIdentifier_Value",
            				   WhiteSpace = "whiteSpace_Value",
                           };
            var prim = primLibrary.CreatePrim(primSpec);
            Assert.That(prim.Id, Is.EqualTo(primDataTypeMock.Object.Id));
            umlPackageMock.Verify(umlPackage => umlPackage.CreateDataType(new UmlDataTypeSpec
                                                                          {
                                                                              Name = primSpec.Name,
                                                                              TaggedValues = new[]
                                                                                             {
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.businessTerm.ToString(),
                                                                                                     Value = MultiPartTaggedValue.Merge(new[] {"businessTerm_Value1", "businessTerm_Value2"}),
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.definition.ToString(),
                                                                                                     Value = "definition_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.dictionaryEntryName.ToString(),
                                                                                                     Value = "dictionaryEntryName_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.fractionDigits.ToString(),
                                                                                                     Value = "fractionDigits_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.languageCode.ToString(),
                                                                                                     Value = "languageCode_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.length.ToString(),
                                                                                                     Value = "length_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.maximumExclusive.ToString(),
                                                                                                     Value = "maximumExclusive_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.maximumInclusive.ToString(),
                                                                                                     Value = "maximumInclusive_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.maximumLength.ToString(),
                                                                                                     Value = "maximumLength_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.minimumExclusive.ToString(),
                                                                                                     Value = "minimumExclusive_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.minimumInclusive.ToString(),
                                                                                                     Value = "minimumInclusive_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.minimumLength.ToString(),
                                                                                                     Value = "minimumLength_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.pattern.ToString(),
                                                                                                     Value = "pattern_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.totalDigits.ToString(),
                                                                                                     Value = "totalDigits_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.uniqueIdentifier.ToString(),
                                                                                                     Value = "uniqueIdentifier_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.versionIdentifier.ToString(),
                                                                                                     Value = "versionIdentifier_Value",
                                                                                                 },
                                                                                                 new UmlTaggedValueSpec
                                                                                                 {
                                                                                                     Name = TaggedValues.whiteSpace.ToString(),
                                                                                                     Value = "whiteSpace_Value",
                                                                                                 },
                                                                                             },
                                                                          }), Times.Exactly(1));
            primDataTypeMock.Verify(dataType => dataType
				.CreateDependency(new UmlDependencySpec<IUmlDataType>
                                                                          {
                                                                              Stereotype = Stereotype.isEquivalentTo,
                                                                              Target = equivalentPrimDataType,
                                                                              LowerBound = "0",
                                                                              UpperBound = "1",
                                                                          }));
		}

		[Test]
		public void ShouldAutoGenerateUnspecifiedPrimTaggedValuesOnCreation()
		{
            Assert.Fail("not implemented");
		}

		[Test]
		public void ShouldUpdatePrimsAccordingToSpecification()
		{
            Assert.Fail("not implemented");
		}

		[Test]
		public void ShouldAutoGenerateUnspecifiedPrimTaggedValuesOnUpdate()
		{
            Assert.Fail("not implemented");
		}

		[Test]
		public void ShouldNotAutoGenerateUnspecifiedPrimTaggedValuesOnUpdateIfAlreadySet()
		{
            Assert.Fail("not implemented");
		}

		[Test]
		public void ShouldRemovePrims()
		{
            Assert.Fail("not implemented");
		}


		[Test]
        public void ShouldReturnEmptyValuesForNullTaggedValues()
        {
            var umlPackage = new UmlPackageBuilder().Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.BusinessTerms, Is.Not.Null);
            Assert.That(primLibrary.BusinessTerms, Is.Empty);
            Assert.That(primLibrary.Copyrights, Is.Not.Null);
            Assert.That(primLibrary.Copyrights, Is.Empty);
            Assert.That(primLibrary.Owners, Is.Not.Null);
            Assert.That(primLibrary.Owners, Is.Empty);
            Assert.That(primLibrary.References, Is.Not.Null);
            Assert.That(primLibrary.References, Is.Empty);
            Assert.That(primLibrary.Status, Is.Not.Null);
            Assert.That(primLibrary.Status, Is.Empty);
            Assert.That(primLibrary.UniqueIdentifier, Is.Not.Null);
            Assert.That(primLibrary.UniqueIdentifier, Is.Empty);
            Assert.That(primLibrary.VersionIdentifier, Is.Not.Null);
            Assert.That(primLibrary.VersionIdentifier, Is.Empty);
            Assert.That(primLibrary.BaseURN, Is.Not.Null);
            Assert.That(primLibrary.BaseURN, Is.Empty);
            Assert.That(primLibrary.NamespacePrefix, Is.Not.Null);
            Assert.That(primLibrary.NamespacePrefix, Is.Empty);
        }

        [Test]
        public void ShouldReturnValueOfTaggedValues()
        {
            var umlPackage = new UmlPackageBuilder()
                .WithMultiValuedTaggedValue(TaggedValues.businessTerm, "businessTerm_Value1", "businessTerm_Value2")
                .WithMultiValuedTaggedValue(TaggedValues.copyright, "copyright_Value1", "copyright_Value2")
                .WithMultiValuedTaggedValue(TaggedValues.owner, "owner_Value1", "owner_Value2")
                .WithMultiValuedTaggedValue(TaggedValues.reference, "reference_Value1", "reference_Value2")
                .WithTaggedValue(TaggedValues.status, "status_Value")
                .WithTaggedValue(TaggedValues.uniqueIdentifier, "uniqueIdentifier_Value")
                .WithTaggedValue(TaggedValues.versionIdentifier, "versionIdentifier_Value")
                .WithTaggedValue(TaggedValues.baseURN, "baseURN_Value")
                .WithTaggedValue(TaggedValues.namespacePrefix, "namespacePrefix_Value")
                .Build();
            IPrimLibrary primLibrary = new UpccPrimLibrary(umlPackage);
            Assert.That(primLibrary.BusinessTerms, Is.EquivalentTo(new[] {"businessTerm_Value1", "businessTerm_Value2"}));
            Assert.That(primLibrary.Copyrights, Is.EquivalentTo(new[] {"copyright_Value1", "copyright_Value2"}));
            Assert.That(primLibrary.Owners, Is.EquivalentTo(new[] {"owner_Value1", "owner_Value2"}));
            Assert.That(primLibrary.References, Is.EquivalentTo(new[] {"reference_Value1", "reference_Value2"}));
            Assert.That(primLibrary.Status, Is.EqualTo("status_Value"));
            Assert.That(primLibrary.UniqueIdentifier, Is.EqualTo("uniqueIdentifier_Value"));
            Assert.That(primLibrary.VersionIdentifier, Is.EqualTo("versionIdentifier_Value"));
            Assert.That(primLibrary.BaseURN, Is.EqualTo("baseURN_Value"));
            Assert.That(primLibrary.NamespacePrefix, Is.EqualTo("namespacePrefix_Value"));
        }
	}
}
