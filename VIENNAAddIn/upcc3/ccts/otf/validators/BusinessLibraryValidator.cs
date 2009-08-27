using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public abstract class BusinessLibraryValidator : ConstraintBasedValidator<RepositoryItem>
    {
        private readonly string stereotype;

        protected BusinessLibraryValidator(string stereotype)
        {
            this.stereotype = stereotype;
            AddConstraint(new NameMustNotBeEmpty<RepositoryItem>());
            AddConstraint(new TaggedValuesMustNotBeEmpty<RepositoryItem>(
                TaggedValues.baseURN, 
                TaggedValues.uniqueIdentifier, 
                TaggedValues.versionIdentifier
                ));
        }

        protected override bool SafeMatches(RepositoryItem item)
        {
            return item.Stereotype == stereotype;
        }
    }
}