using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;
using System.Collections;

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

            checkC524a(context, package),


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


        /// <summary>
        /// An ACC shall not contain - directly or at any nested level - a mandatory ASCC whose associated ACC is the same as the top level ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkC524a(IValidationContext context, EA.Package p)
        {

            //Get a list of all ACCs            
            Dictionary<Int32, EA.Element> accs = null;
            Utility.getAllElements(p, accs, UPCC.ACC.ToString());

            if (accs == null) return;

            IDictionaryEnumerator enumerator = accs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                EA.Element e = (EA.Element)enumerator.Value;


            }


        }







    }
}
