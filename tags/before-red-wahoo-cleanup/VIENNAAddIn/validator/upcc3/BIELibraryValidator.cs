using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class BIELibraryValidator : AbstractValidator
    {






        /// <summary>
        /// Validate the given BIELibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514c(context, package);

            String n = package.Name;

            checkC514l(context, package);
        }



        /// <summary>
        /// A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514c(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ABIE.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in BIELibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }




        /// <summary>
        /// A BIELibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514l(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in BIELibrary.", "A BIELibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }













    }
}
