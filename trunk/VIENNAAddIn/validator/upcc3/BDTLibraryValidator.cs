using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class BDTLibraryValidator : AbstractValidator
    {













        /// <summary>
        /// Validate the given bLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514b(context, package);

            checkC514i(context, package);



        }



        
        /// <summary>
        /// A BDTLibrary shall only contain BDTs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514b(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.BDT.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in BDTLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A BDTLibrary shall only contain BDTs.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }                
            }
        }


        /// <summary>
        /// A BDTLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514i(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in BDTLibrary.", "A BDTLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }













    }
}
