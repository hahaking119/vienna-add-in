using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VIENNAAddIn.validator.upcc3
{
    class UPCCValidator : AbstractValidator
    {


        /// <summary>
        /// Validate the given UPCC model
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {
            //Top-down validation of the entire UPCC model
            if (scope == "ROOT_UPCC")
            {

                EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;
                int rootModelPackageID = ((EA.Package)(context.Repository.Models.GetAt(0))).PackageID;

                if (rootPackages == null || rootPackages.Count == 0)
                {
                    context.AddValidationMessage(new ValidationMessage("No packages found", "The root package of the UPCC model does not contain any packages.", "UPCC", ValidationMessage.errorLevelTypes.WARN, rootModelPackageID));
                }
                else
                {


                }


            }
            else
            {



            }
        }










    }
}
