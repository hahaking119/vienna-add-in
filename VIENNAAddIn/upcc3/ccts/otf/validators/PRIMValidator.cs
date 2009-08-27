using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public class PRIMValidator : ConstraintBasedValidator<RepositoryItem>
    {
        public PRIMValidator()
        {
            AddConstraint(new ParentPackageMustHaveStereotype<RepositoryItem>(Stereotype.PRIMLibrary));
            AddConstraint(new NameMustNotBeEmpty<RepositoryItem>());
            AddConstraint(new TaggedValuesMustBeDefined<RepositoryItem>(
                TaggedValues.businessTerm,
                TaggedValues.dictionaryEntryName, // We allow the DEN to be empty (in this case, we generate it from the element's type and name).
                TaggedValues.pattern,
                TaggedValues.fractionDigits,
                TaggedValues.length,
                TaggedValues.maxExclusive,
                TaggedValues.maxInclusive,
                TaggedValues.maxLength,
                TaggedValues.minExclusive,
                TaggedValues.minInclusive,
                TaggedValues.minLength,
                TaggedValues.totalDigits,
                TaggedValues.whiteSpace,
                TaggedValues.uniqueIdentifier,
                TaggedValues.versionIdentifier,
                TaggedValues.languageCode
                ));
            AddConstraint(new TaggedValuesMustNotBeEmpty<RepositoryItem>(
                TaggedValues.definition
                ));
            // TODO Implement constraint 5.4.4b.
        }

        protected override bool SafeMatches(RepositoryItem item)
        {
            return item.Stereotype == Stereotype.PRIM;
        }
    }
}