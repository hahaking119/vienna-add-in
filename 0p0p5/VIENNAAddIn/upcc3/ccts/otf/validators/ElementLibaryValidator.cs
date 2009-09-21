using System;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators
{
    public class ElementLibaryValidator : BusinessLibraryValidator
    {
        public ElementLibaryValidator(string libraryStereotype, string elementStereotype):base(libraryStereotype)
        {
            AddConstraint(new ParentPackageMustHaveStereotype(Stereotype.bLibrary));
            AddConstraint(new ElementsMustHaveStereotype(elementStereotype));
            AddConstraint(new MustNotContainAnyPackages());
            // TODO implement constraint 5.4.4a and equivalent constraints: element names must be unique
        }
    }
}