using CctsRepository;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public class PRIMValidator : ConstraintBasedValidator<RepositoryItem>
    {
        public PRIMValidator()
        {
            AddConstraint(new ParentPackageMustHaveStereotype(Stereotype.PRIMLibrary));
            AddConstraint(new NameMustNotBeEmpty());
            AddConstraint(new TaggedValuesMustBeDefined(
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
            AddConstraint(new TaggedValuesMustNotBeEmpty(
                TaggedValues.definition
                ));
            // TODO Implement constraint 5.4.4b.
        }

        protected override bool SafeMatches(RepositoryItem item)
        {
            return item.Id.Type == ItemId.ItemType.Element && item.Stereotype == Stereotype.PRIM;
        }
    }
}