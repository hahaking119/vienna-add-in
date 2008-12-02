/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using System.ComponentModel;

namespace VIENNAAddIn.validator.umm.brv
{
    class bRequirementsVValidator : AbstractValidator
    {


        public bRequirementsVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }



        internal override List<ValidationMessage> validate()
        {
         
            List<ValidationMessage> messages = new List<ValidationMessage>();

                //Get the BRV package
                EA.Package p = repository.GetPackageByID(Int32.Parse(scope));

                //Check the TaggedValues of the bRequirementsV
                messages.AddRange(new TaggedValueValidator(repository).validatePackage(p)); 

                //Check C5
                ValidationMessage vm5 = checkC5(p);
                if (vm5 != null) messages.Add(vm5);

                //Check C6
                ValidationMessage vm6 = checkC6(p);
                if (vm6 != null) messages.Add(vm6);

                //Check C7
                ValidationMessage vm7 = checkC7(p);
                if (vm7 != null) messages.Add(vm7);

                //Check C8
                List<ValidationMessage> vm8 = checkC8(p);
                if (vm8 != null && vm8.Count != 0)
                    messages.AddRange(vm8);

                //Iterate over the subpackages of the BusinessRequirementsView and check the 
                //constraints of the different subviews
                foreach (EA.Package subpackage in p.Packages)
                {
                    String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                    if (stereotype == UMM.bDomainV.ToString())
                    {
                        messages.AddRange(new TaggedValueValidator(repository).validatePackage(subpackage));
                        messages.AddRange(new bDomainVValidator(repository, subpackage.PackageID.ToString()).validate());
                    }
                    else if (stereotype == UMM.bPartnerV.ToString())
                    {
                        messages.AddRange(new TaggedValueValidator(repository).validatePackage(subpackage));
                        messages.AddRange(new bPartnerVValidator(repository, subpackage.PackageID.ToString()).validate());
                    }
                    else if (stereotype == UMM.bEntityV.ToString())
                    {
                        messages.AddRange(new TaggedValueValidator(repository).validatePackage(subpackage));
                        messages.AddRange(new bEntityVValidator(repository, subpackage.PackageID.ToString()).validate());
                    }
                }      

            return messages;

        }






        /// <summary>
        /// Check constraint C5
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC5(EA.Package package)
        {

            int count = 0;
            foreach (EA.Package p in package.Packages)
            {

                if (Utility.getStereoTypeFromPackage(p) == UMM.bDomainV.ToString())
                {
                    count++;
                }
            }

            //More than one BDV - error
            if (count > 1)
            {
                return new ValidationMessage("Violation of constraint C5.", "A BusinessRequirementsView MAY contain zero or one BusinessDomainView. \n\nYour model contains " + count + " BusinessDomainViews.", "BRV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID);
            }
            //Zero or one - info
            else
            {
                return new ValidationMessage("Information to constraint C5.", "A BusinessRequirementsView MAY contain zero or one BusinessDomainView. \n\nYour model contains " + count + " BusinessDomainViews.", "BRV", ValidationMessage.errorLevelTypes.INFO, package.PackageID);
            }

        }






        /// <summary>
        /// Check constraint C6
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC6(EA.Package package)
        {

            int count = 0;
            foreach (EA.Package p in package.Packages)
            {

                if (Utility.getStereoTypeFromPackage(p) == UMM.bPartnerV.ToString())
                {
                    count++;
                }
            }

            //More than one business partner view - error
            if (count > 1)
            {
                return new ValidationMessage("Violation of constraint C6.", "A BusinessRequirementsView MAY contain zero or one BusinessPartnerView. \n\nYour model contains " + count + " BusinessPartnerView.", "BRV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID);
            }
            //Zero or business partner view - info
            else
            {
                return new ValidationMessage("Information to constraint C6.", "A BusinessRequirementsView MAY contain zero or one BusinessPartnerView. \n\nYour model contains " + count + " BusinessPartnerView.", "BRV", ValidationMessage.errorLevelTypes.INFO, package.PackageID);
            }


        }


        /// <summary>
        /// Check constraint C7
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC7(EA.Package package)
        {

            int count = 0;
            foreach (EA.Package p in package.Packages)
            {

                if (Utility.getStereoTypeFromPackage(p) == UMM.bEntityV.ToString())
                {
                    count++;
                }
            }


            return new ValidationMessage("Information to constraint C7.", "A BusinessRequirementsView MAY contain zero to many  BusinessEntityViews. \n\nYour model contains " + count + " BusinessEntityViews.", "BRV", ValidationMessage.errorLevelTypes.INFO, package.PackageID);

        }


        /// <summary>
        /// Check constraint C8
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC8(EA.Package pa)
        {
                        
            List<ValidationMessage> messages = new List<ValidationMessage>();

            EA.Collection packages = pa.Packages;

            if (packages != null)
            {
                //Iterate over the packages under the business requirements view
                foreach (EA.Package p in packages)
                {
                    //No BusinessDomain View must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bdvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bDomainV.ToString());
                    if (bdvs != null && bdvs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C8.", "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " + p.Name + " contains an invalid BusinessDomainView.", "BRV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    //No BusinessPartnerView must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bpvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bPartnerV.ToString());
                    if (bpvs != null && bpvs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C8.", "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " + p.Name + " contains an invalid BusinessPartnerView.", "BRV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    //No BusinessEntityView must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bevs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bEntityV.ToString());
                    if (bevs != null && bevs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C8.", "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " + p.Name + " contains an invalid BusinessEntityView.", "BRV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }

                //Make sure there are'nt any other packages in underneath the BRV
                foreach (EA.Package p in packages)
                {
                    String stereotype = Utility.getStereoTypeFromPackage(p);
                    if (!(stereotype == UMM.bDomainV.ToString() || stereotype == UMM.bPartnerV.ToString() || stereotype == UMM.bEntityV.ToString()))
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C8.", "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " + p.Name + " has an invalid stereotype.", "BRV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                }
                                 }


            return messages;
        }














    }

}
