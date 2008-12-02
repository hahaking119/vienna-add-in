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
using System.ComponentModel;

using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.umm.bcv
{
    class bChoreographyVValidator : AbstractValidator
    {


        public bChoreographyVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }


        /// <summary>
        /// Validate the BusinessChoreographyView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        {
     
            List<ValidationMessage> messages = new List<ValidationMessage>();

            EA.Package bcv = repository.GetPackageByID(Int32.Parse(scope));

            //Check the TaggedValues of the bChoreographyV
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(bcv)); 
            
            //Check Constraint C30
            ValidationMessage vm30 = checkC30(bcv);
            if (vm30 != null) messages.Add(vm30);

            //Check Constraint C31
            ValidationMessage vm31 = checkC31(bcv);
            if (vm31 != null) messages.Add(vm31);

            //Check Constraint C32
            ValidationMessage vm32 = checkC32(bcv);
            if (vm32 != null) messages.Add(vm32);

            //Check Constraint C33
            List<ValidationMessage> vm33 = checkC33(bcv);
            if (vm33 != null && vm33.Count != 0) messages.AddRange(vm33);




            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                //Invoke BusinessCollaborationView Validator
                if (stereotype == UMM.bCollaborationV.ToString())
                {
                    messages.AddRange(new bCollaborationVValidator(repository, p.PackageID.ToString()).validate());
                }
                //Invoke Business TransactionVValidator
                else if (stereotype == UMM.bTransactionV.ToString())
                {
                    messages.AddRange(new bTransactionVValidator(repository, p.PackageID.ToString()).validate());
                }
                //Invoke BusinessRealizationViewValidator
                else if (stereotype == UMM.bRealizationV.ToString())
                {
                    messages.AddRange(new bRealizationVValidator(repository, p.PackageID.ToString()).validate());
                }
                //Unknown stereotype
                else
                {
                    messages.Add(new ValidationMessage("Unknown stereotype detected.", "An unknown stereotype has been detected within the BusinessChorographyView: " + stereotype, "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }




            return messages;
        }



        /// <summary>
        /// Check constraint C30
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC30(EA.Package bcv)
        {

            int count_bcollv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bCollaborationV.ToString())
                {
                    count_bcollv++;
                }
            }

            //There must be at least one BusinessCollaborationView
            if (count_bcollv < 1)
            {
                return new ValidationMessage("Violation of constraint C30.", "A BusinessChoreographyView MUST contain one to many BusinessCollaborationViews. \n\nNo BusinessCollaborationView could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }

            return null;
        }




        /// <summary>
        /// Check constraint C31
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC31(EA.Package bcv)
        {

            int count_btv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bTransactionV.ToString())
                {
                    count_btv++;
                }
            }

            //There must be at least one BusinessTransactionView
            if (count_btv < 1)
            {
                return new ValidationMessage("Violation of constraint C31.", "A BusinessChoreographyView MUST contain one to many BusinessTransactionViews. \n\nNo BusinessTransactionView could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }

            return null;
        }



        /// <summary>
        /// Check constraint C32
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC32(EA.Package bcv)
        {

            int count_brv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bRealizationV.ToString())
                {
                    count_brv++;
                }
            }

            //There may be zero to many BRVs
            return new ValidationMessage("Info for constraint C32.", "A BusinessChoreographyView MAY contain zero to many BusinessRealizationViews\n\n" + count_brv + " BusinessRealizationVies have been found.", "BCV", ValidationMessage.errorLevelTypes.INFO, bcv.PackageID);

        }




        /// <summary>
        /// Check constraint C33
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC33(EA.Package bcv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Iterate over the packages under the bcv
            foreach (EA.Package p in bcv.Packages)
            {
                //No BusinessTransactionView must be located under any of the packages on the first level
                IList<EA.Package> btvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bTransactionV.ToString());
                if (btvs != null && btvs.Count != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessTransactionView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }


                //No BusinessCollaborationView must be located under any of the packages on the first level
                IList<EA.Package> bcvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bCollaborationV.ToString());
                if (bcvs != null && bcvs.Count != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                //No BusinessRealizationView must be located under any of the packages on the first level
                IList<EA.Package> brvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bRealizationV.ToString());
                if (brvs != null && brvs.Count != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessRealizationview.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }



            return messages;


        }











    }
}
