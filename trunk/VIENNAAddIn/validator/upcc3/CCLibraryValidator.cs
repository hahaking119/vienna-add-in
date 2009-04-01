using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class CCLibraryValidator : AbstractValidator
    {





        /// <summary>
        /// Validate the given CCLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514d(context, package);

            checkC514k(context, package);



        }



        /// <summary>
        /// A CCLibrary shall only ACCs, BCCs, and ASCCs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514d(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ACC.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in CCLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A CCLibrary shall only contain ACCs, BCCs, and ASCCs.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A CCLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514k(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in CCLibrary.", "A CCLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }







    }
}
