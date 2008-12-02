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
    class bPartnerVValidator : AbstractValidator
    {


        public bPartnerVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }



        /// <summary>
        /// Validate the BusinessDomainView
        /// </summary> 
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        {
             
     
            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Package bpv = repository.GetPackageByID(Int32.Parse(scope));

            //Check the Tagged Values of the Business Partner View
            messages.AddRange(checkTV_BusinessPartnerView(bpv));
            

            //Check constraint C21
            ValidationMessage vm21 = checkC21(bpv);
            if (vm21 != null) messages.Add(vm21);

            //Check constraint C22
            ValidationMessage vm22 = checkC22(bpv);
            if (vm22 != null) messages.Add(vm22);


            
            return messages;

        }




        /// <summary>
        /// Check constraint C21
        /// </summary>
        /// <param name="bpv"></param>
        /// <returns></returns>
        private ValidationMessage checkC21(EA.Package bpv)
        {
            //Get all BusinessPartners from the BusinessPartnerView
            IList<EA.Element> bpartners = Utility.getAllElements(bpv, new List<EA.Element>(), UMM.bPartner.ToString());

            //There must be at least two business partners
            if (bpartners.Count < 2) {
                return new ValidationMessage("Violation of constraint C21.","A BusinessPartnerView MUST contain at least two to many BusinessPartners. If the BusinessPartnerView is hierarchically decomposed into subpackages these BusinessPartners MAY be contained in any of these subpackages. \n\nThe BusinessPartnerView " + bpv.Name + " contains " + bpartners.Count + " BusinessPartners.","BRV",ValidationMessage.errorLevelTypes.ERROR,bpv.PackageID);
            }
            else {
                return new ValidationMessage("Info for constraint C21.","A BusinessPartnerView MUST contain at least two to many BusinessPartners. If the BusinessPartnerView is hierarchically decomposed into subpackages these BusinessPartners MAY be contained in any of these subpackages. \n\nThe BusinessPartnerView " + bpv.Name + " contains " + bpartners.Count + " Stakeholders.","BRV",ValidationMessage.errorLevelTypes.INFO,bpv.PackageID);
            }
            
        }


        /// <summary>
        /// Check constraint C22
        /// </summary>
        /// <param name="bpv"></param>
        /// <returns></returns>
        private ValidationMessage checkC22(EA.Package bpv)
        {
            //Get all Stakeholders from the BusinessPartnerView
            IList<EA.Element> stakeholders = Utility.getAllElements(bpv, new List<EA.Element>(), UMM.Stakeholder.ToString());
            
            return new ValidationMessage("Info for constraint C22.", "A BusinessPartnerView MAY contain zero to many Stakeholders.\n\nThe BusinessPartnerView " + bpv.Name + " contains " + stakeholders.Count, "BRV", ValidationMessage.errorLevelTypes.INFO, bpv.PackageID);
            
        }



        /// <summary>
        /// Check the tagged values of the business partner view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessPartnerView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the bPartnerV
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));

            //Check the TaggedValues of the different Stakeholders
            IList<EA.Element> stakeholders = Utility.getAllElements(p, new List<EA.Element>(), UMM.Stakeholder.ToString());
            foreach (EA.Element sth in stakeholders)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(sth));
            }

            //Check the TaggedValues of the different BusinessParnters
            IList<EA.Element> businessPartners = Utility.getAllElements(p, new List<EA.Element>(), UMM.bPartner.ToString());
            foreach (EA.Element bpartner in businessPartners)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(bpartner));
            }


            return messages;

        }





    }
}
