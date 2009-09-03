using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class TaggedValuesMustNotBeEmptyConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new TaggedValuesMustNotBeEmpty(TaggedValues.owner, TaggedValues.pattern); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage
                    .WithName("a package with the required tagged values defined and set")
                    .WithTaggedValues(new Dictionary<TaggedValues, string>
                                      {
                                          {TaggedValues.owner, "an owner"},
                                          {TaggedValues.pattern, "a pattern"}
                                      });
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                RepositoryItemBuilder aPackageWithoutAnyTaggedValues = APackage.WithStereotype("a package without any tagged values");
                yield return new ExpectedViolation(aPackageWithoutAnyTaggedValues, aPackageWithoutAnyTaggedValues, aPackageWithoutAnyTaggedValues);

                RepositoryItemBuilder aPackageWithoutOnlySomeTaggedValuesDefined = APackage.WithStereotype("a package with only some tagged values defined and set")
                    .WithTaggedValues(new Dictionary<TaggedValues, string>
                                      {
                                          {TaggedValues.owner, "an owner"},
                                      });
                yield return new ExpectedViolation(aPackageWithoutOnlySomeTaggedValuesDefined, aPackageWithoutOnlySomeTaggedValuesDefined);

                RepositoryItemBuilder aPackageWithAllTaggedValuesDefinedButOnlySomeSet = APackage.WithStereotype("a package with all tagged values defined but only some set")
                    .WithTaggedValues(new Dictionary<TaggedValues, string>
                                      {
                                          {TaggedValues.owner, "an owner"},
                                          {TaggedValues.pattern, ""}
                                      });
                yield return new ExpectedViolation(aPackageWithAllTaggedValuesDefinedButOnlySomeSet, aPackageWithAllTaggedValuesDefinedButOnlySomeSet);
            }
        }
    }
}