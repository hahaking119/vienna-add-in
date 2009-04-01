using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class PRIMLibraryValidator : AbstractValidator
    {





        /// <summary>
        /// Validate the given PRIMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));


            checkC514h(context, package);


            checkC514n(context, package);

        }



        /// <summary>
        /// A PRIMLibrary shall only contain PRIMs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514h(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.PRIM.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in PRIMLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A PRIMLibrary shall only contain PRIMs.", "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A PRIMLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514n(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in PRIMLibrary.", "A PRIMLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }



    }
}
