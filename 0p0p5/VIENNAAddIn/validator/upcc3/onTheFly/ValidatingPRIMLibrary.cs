using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class ValidatingPRIMLibrary : IValidatingBusinessLibrary
    {
        private readonly IValidationContext validationContext;
        private List<IPRIM> prims = new List<IPRIM>();

        public ValidatingPRIMLibrary(IValidationContext validationContext)
        {
            this.validationContext = validationContext;
        }

        public IEnumerable<IPRIM> PRIMs
        {
            get { return prims; }
        }

        public void ProcessElementCreated(Element element)
        {
            ValidateStereotype(element);
            ValidateDuplicateNames(element);
            prims.Add(new PRIM(element));
        }

        private void ValidateDuplicateNames(Element element)
        {
            string name = element.Name;
            bool hasDuplicateName = false;
            foreach (IPRIM prim in PRIMs)
            {
                if (name == prim.Name)
                {
                    hasDuplicateName = true;
                    validationContext.AddValidationIssue(new DuplicateElementName(prim.GUID));
                }
            }
            if (hasDuplicateName)
            {
                validationContext.AddValidationIssue(new DuplicateElementName(element.ElementGUID));
            }
        }

        private void ValidateStereotype(Element element)
        {
            if (element.Stereotype != Stereotype.PRIM)
            {
                validationContext.AddValidationIssue(new InvalidElementStereotype(element.ElementGUID, Stereotype.PRIM));
            }
        }

        public void ProcessCreatePackage(Package package)
        {
            validationContext.AddValidationIssue(new NoSubpackagesAllowed(package.PackageGUID));
        }
    }
}