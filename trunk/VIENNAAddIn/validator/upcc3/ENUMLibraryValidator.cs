using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class ENUMLibraryValidator : AbstractValidator
    {






        /// <summary>
        /// Validate the given ENUMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514g(context, package);

            checkC514o(context, package);
        }




        /// <summary>
        /// A ENUMLibrary shall only contrain ENUMs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514g(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ENUM.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in ENUMLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A ENUMLibrary shall only contain ENUMs.", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A ENUMLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514o(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in ENUMLibrary.", "A ENUMLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }








    }
}
