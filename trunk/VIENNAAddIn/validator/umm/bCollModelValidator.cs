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
using System.Collections;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

using VIENNAAddIn.validator.umm.brv;
using VIENNAAddIn.validator.umm.bcv;
using VIENNAAddIn.validator.umm.biv;
using System.ComponentModel;

namespace VIENNAAddIn.validator.umm

{
    class bCollModelValidator : AbstractValidator
    {

        public bCollModelValidator(EA.Repository repository, String scope) {
            this.repository = repository;
            this.scope = scope;
        }

         

        internal override List<ValidationMessage> validate() 
        {


            List<ValidationMessage> messages = new List<ValidationMessage>();

                //Validate all has been choosen
                if (scope == "ROOT")
                {

                    EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;
                    int rootModelPackageID = ((EA.Package)(repository.Models.GetAt(0))).PackageID;

                    if (rootPackages == null || rootPackages.Count == 0)
                    {
                        messages.Add(new ValidationMessage("No packages found", "The root package of the UMM model does not contain any packages.", "BCM", ValidationMessage.errorLevelTypes.WARN, rootModelPackageID));
                    }
                    else
                    {

                        //Check C1
                        ValidationMessage c1 = checkC1();
                        if (c1 != null) messages.Add(c1);

                        //Check C2
                        ValidationMessage c2 = checkC2();
                        if (c2 != null) messages.Add(c2);

                        //Check C3
                        ValidationMessage c3 = checkC3();
                        if (c3 != null) messages.Add(c3);

                        //Check C4
                        List<ValidationMessage> c4 = checkC4();
                        if (c4 != null && c4.Count != 0) messages.AddRange(c4);


                        //Iterate over the different packages
                        foreach (EA.Package p in rootPackages)
                        {
                            scope = p.PackageID.ToString();
                            String stereotype = Utility.getStereoTypeFromPackage(p);

                            //Business Requirements View
                            if (stereotype == UMM.bRequirementsV.ToString())
                            {
                                messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));
                                messages.AddRange(new bRequirementsVValidator(repository, p.PackageID.ToString()).validate());
                            }
                            //Business Choreography View
                            else if (stereotype == UMM.bChoreographyV.ToString())
                            {
                                messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));
                                messages.AddRange(new bChoreographyVValidator(repository, p.PackageID.ToString()).validate());   
                            }
                            //Business Information View
                            else if (stereotype == UMM.bInformationV.ToString())
                            {
                                messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));
                                messages.AddRange(new bInformationVValidator(repository, p.PackageID.ToString()).validate());
                            }
                            else
                            {
                                messages.Add(new ValidationMessage("Unknown or missing stereotype detected.", "The package " + p.Name + " has an unknown or missing stereotype and will be ignored for validation. Please make sure that all packages are stereotyped correctly in order to allow for a validation of the entire model.", "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
                            }
                        }
                    }
                }
                else
                {
                    //Validation has been invoked by  a click somewhere in the model (bottom-up) validation
                    //Try to determine the stereotype of the selected scope
                    EA.Package p = repository.GetPackageByID(Int32.Parse(scope));
                    String stereotype = Utility.getStereoTypeFromPackage(p);
                    if (!Utility.isValidUMMStereotype(stereotype))
                    {
                        messages.Add(new ValidationMessage("Unknown or missing stereotype detected.", "The package " + p.Name + " has an unknown or missing stereotype. Validation cannot be started.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                    else
                    {
                        //BusinessRequirementsView
                        if (stereotype == UMM.bRequirementsV.ToString())
                        {
                            messages.AddRange(new bRequirementsVValidator(repository, scope).validate());
                        }
                        //Business Domain View
                        else if (stereotype == UMM.bDomainV.ToString())
                        {
                            messages.AddRange(new bDomainVValidator(repository, scope).validate());
                        }
                        //BusinessAreas and Process Areas are part of the business domain view - hence the whole
                        //business domain view is validated instead of a subpackage
                        else if (stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString())
                        {
                            //Get the parent BDV of the given process area or business area
                            scope = getParentByStereotype(scope, UMM.bDomainV.ToString());
                            if (scope == "")
                            {
                                messages.Add(new ValidationMessage("Unable to detect business domain view.", "Business areas and process areas must be located underneath a business domain view. Validation cannot be started.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                            else
                            {
                                messages.AddRange(new bDomainVValidator(repository, scope).validate());
                            }
                        }
                        //Business Partner View
                        else if (stereotype == UMM.bPartnerV.ToString())
                        {
                            messages.AddRange(new bPartnerVValidator(repository, scope).validate());
                        }
                        //Business Data View is a part of the business entity view and has no own validator class
                        //determine the parent business entity view and invoke validation there
                        else if (stereotype == UMM.bDataV.ToString())
                        {
                            //Get the parent business entity view
                            scope = getParentByStereotype(scope, UMM.bEntityV.ToString());
                            if (scope == "")
                            {
                                messages.Add(new ValidationMessage("Unable to detect business entity view.", "A business data view must be located underneath a business entity view.. Validation cannot be started.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                            else
                            {
                                messages.AddRange(new bEntityVValidator(repository, scope).validate());
                            }
                        }
                        //Business Entity View
                        else if (stereotype == UMM.bEntityV.ToString())
                        {
                            messages.AddRange(new bEntityVValidator(repository, scope).validate());
                        }
                        //Business Choreography View
                        else if (stereotype == UMM.bChoreographyV.ToString())
                        {
                            messages.AddRange(new bChoreographyVValidator(repository, scope).validate());
                        }
                        //Business Transaction View
                        else if (stereotype == UMM.bTransactionV.ToString())
                        {
                            messages.AddRange(new bTransactionVValidator(repository, scope).validate());
                        }
                        //Business Collaboration View
                        else if (stereotype == UMM.bCollaborationV.ToString())
                        {
                            messages.AddRange(new bCollaborationVValidator(repository, scope).validate());
                        }
                        //Business Realization view
                        else if (stereotype == UMM.bRealizationV.ToString())
                        {
                            messages.AddRange(new bRealizationVValidator(repository, scope).validate());
                        }
                        //Business Information View
                        else if (stereotype == UMM.bInformationV.ToString())
                        {
                            messages.AddRange(new bInformationVValidator(repository, scope).validate());
                        }

                    }
                }
                     
            return messages;
        }




        /// <summary>
        /// Check constraint C1
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC1()
        {

            bool found = false;
            EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bChoreographyV.ToString())
                    {
                        found = true;
                        break;
                    }

                }
            }

            if (!found)
            {                
                return new ValidationMessage("Violation of constraint C1.", "A BusinessCollaborationModel MUST contain one to many BusinessChoregraphyViews.", "BCM", ValidationMessage.errorLevelTypes.ERROR, 0);
            }

            return null;

        }


        /// <summary>
        /// Check constraint C2
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC2()
        {
            bool found = false;
            EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bInformationV.ToString())
                    {
                        found = true;
                        break;
                    }

                }
            }

            if (!found)
            {                
                return new ValidationMessage("Violation of constraint C2.", "A BusinessCollaborationModel MUST contain one to many BusinessInformationViews.", "BCM", ValidationMessage.errorLevelTypes.ERROR, 0);
            }

            return null;

        

           
        }




        /// <summary>
        /// Check constraint C3
        /// </summary>
        /// <returns></returns>
        private ValidationMessage checkC3()
        {
            //This constraint is not validated but only an info is given back to the user
            
            EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;
            int count = 0;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bRequirementsV.ToString())
                    {                       
                        count++;                        
                    }

                }
            }

            return new ValidationMessage("Information to constraint C3.", "A BusinessCollaborationModel MAY containt zero to many BusinessRequirementsViews. \n\nYour model contains " + count + " BusinessRequirementsViews.", "BCM", ValidationMessage.errorLevelTypes.INFO, 0);
    
        }



        /// <summary>
        /// Check constraint C4
        /// </summary>
        /// <returns></returns>
        private List<ValidationMessage> checkC4()
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the packages under the model root
                foreach (EA.Package p in rootPackages)
                {
                    //No BusinessRequirementsViews must be located under any of the packages on the first level
                    IList<EA.Package> brvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bRequirementsV.ToString());
                    if (brvs != null && brvs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C4.","A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessRequirementsView.","BCM",ValidationMessage.errorLevelTypes.ERROR,p.PackageID));
                    }

                    //No BusinessChoreographyViews must be located under any of the packages on the first level
                    IList<EA.Package> bcvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bChoreographyV.ToString());
                    if (bcvs != null && bcvs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C4.", "A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessChoreographyView.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    //No BusinessInformationViews must be located under any of the packages on the first level
                    IList<EA.Package> bivs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bInformationV.ToString());
                    if (bivs != null && bivs.Count != 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C4.", "A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessInformationView.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }




            }


            return messages;

        }





        /// <summary>
        /// Return the parent with the given stereotype
        /// be found
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        private String getParentByStereotype(String scope, String stereotype_)
        {

            EA.Package childPackage = repository.GetPackageByID(Int32.Parse(scope));

            if (childPackage.ParentID != 0)
            {
                EA.Package parentPackage = repository.GetPackageByID(childPackage.ParentID);
                String stereotype = Utility.getStereoTypeFromPackage(parentPackage);
                if (stereotype == stereotype_)
                {
                    return parentPackage.PackageID.ToString();
                }
                else
                {
                    return getParentByStereotype(parentPackage.PackageID.ToString(), stereotype_);
                }
            }
            
            
            return "";
            

        }




    }
}
