
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

namespace VIENNAAddIn.validator.umm.biv
{
    class bInformationVValidator : AbstractValidator
    {


        public bInformationVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }



        /// <summary>
        /// Validate the BusinessInformationView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        {

            

            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Package biv = repository.GetPackageByID(Int32.Parse(scope));

            //Check the Tagged Values of the Business Information View
            messages.AddRange(checkTV_BusinessInformationView(biv));

            //Check constraint C92
            List<ValidationMessage> vm100 = checkC100(biv);
            if (vm100 != null && vm100.Count != 0) messages.AddRange(vm100);



            return messages;
        }




        /// <summary>
        /// 
        /// Check constraint C100
        /// A BusinessInformationView MUST contain one to many 
        /// InformationEnvelopes or subtypes thereof defined in any other 
        /// extension/specialization modile. Furthermore, it MAY contains 
        /// any other document modeling artifacts.
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ValidationMessage> checkC100(EA.Package biv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            IList<EA.Element> informationEnvelopes = Utility.getAllClasses(biv, new List<EA.Element>());

            bool informationEnvelopeFound = false;

            foreach (EA.Element e in informationEnvelopes)
            {

                //Is the classifier of type InformationEnvelope?
                if (e.Stereotype == UMM.InfEnvelope.ToString())
                {
                    informationEnvelopeFound = true;

                }
                //Is the classifier a subtype of an InformationEnvelope?
                else
                {
                    foreach (EA.Element el in e.BaseClasses)
                    {
                        if (el.Stereotype == UMM.InfEnvelope.ToString())
                        {
                            informationEnvelopeFound = true;
                            break;
                        }
                    }
                }
            }

            if (!informationEnvelopeFound)
            {
                messages.Add(new ValidationMessage("Violation of constraint C100.", "A BusinessInformationView MUST contain one to many InformationEnvelopes (InfEnvelope) or subtypes thereof defined in any other extension/specialization modile. Furthermore, it MAY contains any other document modeling artifacts. \n\nNo InformationEnvelopes or subtypes thereof could be found.", "BIV", ValidationMessage.errorLevelTypes.ERROR, biv.PackageID));
            }


            return messages;

        }



        /// <summary>
        /// Check the tagged values of the BusinessInformationView
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessInformationView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the BusinessInformationView package
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));


            return messages;
        }





    }
}
