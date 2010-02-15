using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public class BLibraryValidator : BusinessLibraryValidator
    {
        public BLibraryValidator():base(Stereotype.bLibrary)
        {
            AddConstraint(new ParentMustBeModelOrBInformationVOrBLibrary());
            AddConstraint(new SubPackagesMustBeBusinessLibraries());
            AddConstraint(new MustNotContainAnyElements());
        }
    }
}