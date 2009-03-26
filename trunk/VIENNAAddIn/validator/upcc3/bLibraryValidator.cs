using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class bLibraryValidator : AbstractValidator
    {



        /// <summary>
        /// Validate a given bLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package d = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514a(context, d);  

        }





        /// <summary>
        /// A bLibrary may only containt packages of type
        /// BDTLibrary, BIELibrary, CCLibrary, DOCLibrary, ENUMLibrary and PRIMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514a(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package p in d.Packages)
            {
                bool found = false;
                //Get the stereotype of the package
                String stereotype = p.Element.Stereotype;
                foreach (UPCC_Packages upcc in Enum.GetValues(typeof(UPCC_Packages)))
                {
                    if (upcc.ToString() == stereotype)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Package with invalid stereotype found.", "A bLibrary shall only contain packages of type BDTLibrary, BIELibrary, CCLibrary, CDTLibrary, DOCLibrary, ENUMLibrary and no other elements.", "UPCC", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }









    }
}
