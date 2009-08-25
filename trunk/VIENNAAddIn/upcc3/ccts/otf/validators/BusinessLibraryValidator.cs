using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public abstract class BusinessLibraryValidator : ConstraintBasedValidator<IRepositoryItem>
    {
        private readonly string stereotype;

        protected BusinessLibraryValidator(string stereotype)
        {
            this.stereotype = stereotype;
            AddConstraint(new NameMustNotBeEmpty());
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.baseURN));
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.uniqueIdentifier));
            AddConstraint(new TaggedValueMustBeDefined(TaggedValues.versionIdentifier));
        }

        protected override bool SafeMatches(IRepositoryItem item)
        {
            return item.Data != null && item.Data.Stereotype == stereotype;
        }
    }
}