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

using VIENNAAddIn.constants;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace VIENNAAddIn.validator.umm
{
    class TaggedValueValidator : AbstractValidator
    {


        public TaggedValueValidator(EA.Repository repository)
        {
            this.repository = repository;
        }




        internal override List<ValidationMessage> validate()
        {
            //Not Used
            return null;
        }

        /// <summary>
        /// Validate the tagged values of the package 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal List<ValidationMessage> validatePackage(EA.Package package)
        {
            this.repository = repository;
                          
            //Get the stereotype of the package            
            String stereotype = package.Element.Stereotype;
            
            //The top views
            if (stereotype == UMM.bRequirementsV.ToString() || stereotype == UMM.bChoreographyV.ToString()
                || stereotype == UMM.bInformationV.ToString() || stereotype == UMM.bCollModel.ToString() ||
                stereotype == UMM.bDomainV.ToString() || stereotype == UMM.bPartnerV.ToString() || 
                stereotype == UMM.bEntityV.ToString() || stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString()
                || stereotype == UMM.bDataV.ToString()|| stereotype == UMM.bChoreographyV.ToString() ||
                stereotype == UMM.bCollaborationV.ToString() || stereotype == UMM.bTransactionV.ToString() ||
                stereotype == UMM.bRealizationV.ToString() || stereotype == UMM.bInformationV.ToString())
            {
                return validatePackageTaggedValues(package);
            }

              
            //Invalid stereotype passed - return an empty list
            return new List<ValidationMessage>();
        }


        /// <summary>
        /// Validate the tagged values of the element 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal List<ValidationMessage> validateElement(EA.Element element)
        {
            this.repository = repository;
            

            //Get the stereotype of the element
            String stereotype = element.Stereotype;
            //Get the containing pckage
            EA.Package package = repository.GetPackageByID(element.PackageID);


            //Business Process Use Case / BTUC / BCUC
            if (stereotype == UMM.bProcessUC.ToString() || stereotype == UMM.bTransactionUC.ToString() || stereotype == UMM.bCollaborationUC.ToString())
            {
                return validateBPUC_BTUC_BCUC_TaggedValues(element, package);
            }
            //Stakeholder and BusinessPartner
            else if (stereotype == UMM.Stakeholder.ToString() || stereotype == UMM.bPartner.ToString())
            {
                return validateBusinessPartnerAndStakeholder(element, package);
            }
            //BusinessTransaction
            else if (stereotype == UMM.bTransaction.ToString())
            {
                return validateBusinessTransaction(element, package);
            }
            //ReqAction/ResAction
            else if (stereotype == UMM.ReqAction.ToString() || stereotype == UMM.ResAction.ToString())
            {
                return validateBusinessActions(element, package);
            }
            //ReqInfPin/ResInfPin
            else if (stereotype == UMM.ResInfPin.ToString() || stereotype == UMM.ReqInfPin.ToString())
            {
                return validateInformationPins(element, package);
            }
            //BusinessTransactionActions
            else if (stereotype == UMM.bTransactionAction.ToString())
            {
                return validateBusinessTransactionAction(element, package);
            }


            //Invalid stereotype passed - return an empty list
            return new List<ValidationMessage>();
        }




        /// <summary>
        /// Validate the tagged values of participates and isOfInterestTo connectors of the passed
        /// element in the containing package
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="element"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal List<ValidationMessage> validateConnectors(List<EA.Connector> connectors)
        {            
            List<ValidationMessage> messages = new List<ValidationMessage>();

            

            foreach (EA.Connector con in connectors)
            {
                bool foundInternest = false;
                foreach (EA.ConnectorTag ctag in con.TaggedValues)
                {
                    if (ctag.Name == TaggedValues.interest.ToString())
                    {
                        foundInternest = true;
                    }
                }

                if (!foundInternest)
                {
                    //Get source
                    EA.Element source = repository.GetElementByID(con.ClientID);
                    EA.Element target = repository.GetElementByID(con.SupplierID);
                    EA.Package package = repository.GetPackageByID(source.PackageID); 

                    messages.Add(new ValidationMessage("Missing tagged value.", getConnectorError("interest", con, source, target, package), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
                }
            }





            return messages;
        }









        /// <summary>
        /// Validate BusinessTransactionAction
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateBusinessTransactionAction(EA.Element e, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundTimeToPerform = false;
            bool foundIsConcurrent = false;

            String valueTimeToPerform = "";
            String valueIsConcurrent = "";


            foreach (EA.TaggedValue tv in e.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == TaggedValues.timeToPerform.ToString())
                {
                    foundTimeToPerform = true;
                    valueTimeToPerform = v;
                }
                else if (n == TaggedValues.isConcurrent.ToString())
                {
                    foundIsConcurrent = true;
                    valueIsConcurrent = v;
                }
            }


            if (!foundTimeToPerform)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("timeToPerform", package, e), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToPerform))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value timeToPerform of element " + e.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }


            if (!foundIsConcurrent)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isConcurrent", package, e), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsConcurrent))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isConcurrent of element " + e.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                } 
            }






            return messages;

        }




        /// <summary>
        /// Validate BusinessActions
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateInformationPins(EA.Element e, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundIsConfidential = false;
            bool foundIsTamperProof = false;
            bool foundIsAuthenticated = false;

            String valueIsConfidential = "";
            String valueIsTamperProof = "";
            String valueIsAuthenticated = "";

            EA.Element element = repository.GetElementByID(e.ParentID);


            foreach (EA.TaggedValue tv in e.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == TaggedValues.isConfidential.ToString())
                {
                    foundIsConfidential = true;
                    valueIsConfidential = v;
                }
                else if (n == TaggedValues.isTamperProof.ToString())
                {
                    foundIsTamperProof = true;
                    valueIsTamperProof = v;
                }
                else if (n == TaggedValues.isAuthenticated.ToString())
                {
                    foundIsAuthenticated = true;
                    valueIsAuthenticated = v;
                }
            }

            if (!foundIsConfidential)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isConfidential", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsConfidential))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isConfidential of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                } 
            }


            if (!foundIsTamperProof)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isTamperProof", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsTamperProof))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isTamperProof of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                } 
            }

            if (!foundIsAuthenticated)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isAuthenticated", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsAuthenticated))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isAuthenticated of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                } 
            }



            return messages;
        }



        /// <summary>
        /// Validate BusinessActions
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateBusinessActions(EA.Element element, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundIsAuthorizationRequired = false;
            bool foundIsNonRepudationRequired = false;
            bool foundIsNonRepudationReceiptRequired = false;
            bool foundTimeToAcknowledgeReceipt = false;
            bool foundTimeToAcknowledgeProcessing = false;
            bool foundIsIntelligibleCheckRequired = false;

            bool foundTimeToRespond = false;
            bool foundRetryCount = false;


            String valueIsAuthorizationRequired = "";
            String valueIsNonRepudationRequired = "";
            String valueIsNonRepudationReceiptRequired = "";
            String valueTimeToAcknowledgeReceipt = "";
            String valueTimeToAcknowledgeProcessing = "";
            String valueIsIntelligibleCheckRequired = "";
            String valueTimeToRespond = "";
            String valueRetrycount = "";


            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == TaggedValues.isAuthorizationRequired.ToString())
                {
                    foundIsAuthorizationRequired = true;
                    valueIsAuthorizationRequired = v;
                }
                else if (n == TaggedValues.isNonRepudiationRequired.ToString())
                {
                    foundIsNonRepudationRequired = true;
                    valueIsNonRepudationRequired = v;
                }
                else if (n == TaggedValues.isNonRepudiationReceiptRequired.ToString())
                {
                    foundIsNonRepudationReceiptRequired = true;
                    valueIsNonRepudationReceiptRequired = v;

                }
                else if (n == TaggedValues.timeToAcknowledgeReceipt.ToString())
                {
                    foundTimeToAcknowledgeReceipt = true;
                    valueTimeToAcknowledgeReceipt = v;
                }
                else if (n == TaggedValues.timeToAcknowledgeProcessing.ToString())
                {
                    foundTimeToAcknowledgeProcessing = true;
                    valueTimeToAcknowledgeProcessing = v;
                }
                else if (n == TaggedValues.isIntelligibleCheckRequired.ToString())
                {
                    foundIsIntelligibleCheckRequired = true;
                    valueIsIntelligibleCheckRequired = v;
                }
                else if (n == TaggedValues.timeToRespond.ToString())
                {
                    foundTimeToRespond = true;
                    valueTimeToRespond = v;
                }
                else if (n == TaggedValues.retryCount.ToString())
                {
                    foundRetryCount = true;
                    valueRetrycount = v;
                }
            }


            if (!foundIsAuthorizationRequired) {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isAuthorizationRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else {
                if (!isBoolean(valueIsAuthorizationRequired))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isAuthorizationRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }


            if (!foundIsNonRepudationRequired)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isNonRepudiationRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsNonRepudationRequired))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isNonRepudiationRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }               
            }

            if (!foundIsNonRepudationReceiptRequired)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isNonRepudiationReceiptRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsNonRepudationReceiptRequired))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isNonRepudiationReceiptRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }    
            }

            if (!foundTimeToAcknowledgeReceipt)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("timeToAcknowledgeReceipt", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToAcknowledgeReceipt)) {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value timeToAcknowledgeReceipt of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundTimeToAcknowledgeProcessing)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("timeToAcknowledgeProcessing", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToAcknowledgeProcessing)) {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value timeToAcknowledgeProcessing of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }

            }

            if (!foundIsIntelligibleCheckRequired)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isIntelligibleCheckRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsIntelligibleCheckRequired))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isIntelligibleCheckRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }            
            
            }

            //A RequestingAction has two additional taggedvalues in comparison to a RespondingAction
            if (element.Stereotype == UMM.ReqAction.ToString())
            {

                if (!foundTimeToRespond)
                {
                    messages.Add(new ValidationMessage("Missing tagged value.", getElementError("timeToRespond", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
                else
                {
                    if (!isDuration(valueTimeToRespond))
                    {
                        messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value timeToRespond of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                    }
                }

                if (!foundRetryCount)
                {
                    messages.Add(new ValidationMessage("Missing tagged value.", getElementError("retryCount", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
                else
                {
                    if (!isPositiveInteger(valueRetrycount))
                    {
                        messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value retryCount of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nRetryCount must be a figure greater or equal zero.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                    }
                }

            }

            






            return messages;


        }




        /// <summary>
        /// Validate a abusiness transaction
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateBusinessTransaction(EA.Element element, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            bool foundBusinessTransactionType = false;
            bool foundIsSecureTransportRequired = false;
            String valueBusinessTransactionType = "";
            String valueIsSecureTransportRequired = "";

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name == TaggedValues.businessTransactionType.ToString())
                {
                    foundBusinessTransactionType = true;
                    valueBusinessTransactionType = tv.Value;
                }
                else if (tv.Name == TaggedValues.isSecureTransportRequired.ToString()) {
                    foundIsSecureTransportRequired = true;
                    valueIsSecureTransportRequired = tv.Value;
                }
            }

            if (!foundBusinessTransactionType)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("businessTransactionType", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                //Found the tagged value - check if the values are ok
                if (!(valueBusinessTransactionType == "CommercialTransaction" || 
                    valueBusinessTransactionType == "Request/Confirm" || valueBusinessTransactionType == "Query/Response" 
                    || valueBusinessTransactionType == "Request/Response" 
                    || valueBusinessTransactionType == "Notification" 
                    || valueBusinessTransactionType == "InformationDistribution")) {
                        messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value businessTransactionType of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are: Commercial Transaction, Request/Confirm, Query/Response, Request/Response, Notification, Information Distribution.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }

            }

            if (!foundIsSecureTransportRequired)
            {
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("isSecureTransportRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsSecureTransportRequired))
                {
                    messages.Add(new ValidationMessage("Wrong tagged value.", "Tagged value isSecureTransportRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }



            return messages;

        }










        /// <summary>
        /// Validate the Stakeholder
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateBusinessPartnerAndStakeholder(EA.Element element, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundInterest = false;

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name == TaggedValues.interest.ToString())
                {
                    foundInterest = true;
                }
            }

            if (!foundInterest)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("interest", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            return messages;
        }






        /// <summary>
        /// Validate tagged values of an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private List<ValidationMessage> validateBPUC_BTUC_BCUC_TaggedValues(EA.Element element, EA.Package package)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundDefinition = false;
            bool foundBeginsWhen = false;
            bool foundPreCondition = false;
            bool foundEndsWhen = false;
            bool foundPostCondition = false;
            bool foundExceptions = false;
            bool foundAction = false;
            

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                String n = tv.Name;

                if (n == TaggedValues.definition.ToString())
                    foundDefinition = true;
                if (n == TaggedValues.beginsWhen.ToString())
                    foundBeginsWhen = true;
                if (n == TaggedValues.preCondition.ToString())
                    foundPreCondition = true;
                if (n == TaggedValues.endsWhen.ToString())
                    foundEndsWhen = true;
                if (n == TaggedValues.postCondition.ToString())
                    foundPostCondition = true;
                if (n == TaggedValues.exceptions.ToString())
                    foundExceptions = true;
                if (n == TaggedValues.actions.ToString())
                    foundAction = true;                

            }

            if (!foundDefinition)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("definition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            if (!foundBeginsWhen)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("beginsWhen", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
                        
            if (!foundPreCondition)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("precondition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            if (!foundEndsWhen)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("endsWhen", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            if (!foundPostCondition)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("postCondition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            if (!foundExceptions)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("exceptions", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));

            if (!foundAction)
                messages.Add(new ValidationMessage("Missing tagged value.", getElementError("actions", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));




            
            return messages;

        }










        /// <summary>
        /// Validate the tagged values of a package
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal List<ValidationMessage> validatePackageTaggedValues(EA.Package p)
        {            
            List<ValidationMessage> messages = new List<ValidationMessage>();

            bool foundJustification = false;
            bool foundBusinessTerm = false;
            bool foundCopyright = false;
            bool foundOwner = false;
            bool foundReference = false;
            bool foundStatus = false;
            bool foundURI = false;
            bool foundVersion = false;

            bool foundObjective = false;
            bool foundScope = false;
            bool foundBusinessOpportunity = false;


            foreach (EA.TaggedValue t in p.Element.TaggedValues)
            {
                String n = t.Name;

                if (n == TaggedValues.justification.ToString())
                    foundJustification = true;
                else if (n == TaggedValues.businessTerm.ToString())
                    foundBusinessTerm = true;
                else if (n == TaggedValues.copyright.ToString())
                    foundCopyright = true;
                else if (n == TaggedValues.owner.ToString())
                    foundOwner = true;
                else if (n == TaggedValues.reference.ToString())
                    foundReference = true;
                else if (n == TaggedValues.status.ToString())
                    foundStatus = true;
                else if (n == TaggedValues.URI.ToString())
                    foundURI = true;
                else if (n == TaggedValues.version.ToString())
                    foundVersion = true;
                else if (n == TaggedValues.objective.ToString())
                    foundObjective = true;
                else if (n == TaggedValues.scope.ToString())
                    foundScope = true;
                else if (n == TaggedValues.businessOpportunity.ToString())
                    foundBusinessOpportunity = true;
            }

            //Raise an error if a taggedvalue is missing

            //Justification is a tagged value only occurring in  the bCollModel
            if (p.Element.Stereotype == UMM.bCollModel.ToString() && !foundJustification)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("justification", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            //Objective is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundObjective)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("objective", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            
            //Scope is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundScope)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("scope", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            //businessOpportunity is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundBusinessOpportunity)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("businessOpportunity", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundBusinessTerm)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("businessTerm", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
                 
            if (!foundCopyright)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("copyright", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundOwner)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("owner", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundReference)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("reference", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundStatus)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("status", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundURI)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("URI", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));

            if (!foundVersion)
                messages.Add(new ValidationMessage("Missing tagged value.", getPackageError("version", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            

            return messages;
        }




        /// <summary>
        /// Returns the package error
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private String getPackageError(String taggedValue, EA.Package package)
        {
            return "Tagged value " + taggedValue + " of package " + package.Name + " <<" +package .Element.Stereotype+ ">> is missing.";
        }



        /// <summary>
        /// Returns the package error
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private String getElementError(String taggedValue, EA.Package package, EA.Element element)
        {
            return "Tagged value " + taggedValue + " of element " + element.Name + " in package "+ package.Name +" <<" + package.Element.Stereotype + ">> is missing.";
        }


        /// <summary>
        /// Return the error message for a connector
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="con"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private String getConnectorError(String taggedValue, EA.Connector con, EA.Element source, EA.Element target, EA.Package package)
        {
            
            return "Tagged value " + taggedValue + " of connector with stereotype " + con.Stereotype + " between " + source.Name + " and " + target.Name + " in package " + package.Name + " <<" + package.Element.Stereotype + ">> is missing.";

        }



        /// <summary>
        /// Returns true if the passed string is either true or false
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isBoolean(String s)
        {
            if (s != null)
            {
                if (s.ToLower().Trim() == "true" || s.ToLower().Trim() == "false")
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Returns true if s is an Integer >= 0 or NULL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isPositiveInteger(String s)
        {
            if (s == null || s.ToLower() == "null")
                return true;


            try
            {
                Int32 i = Int32.Parse(s);
                if (i < 0)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                
            }

            return false;

        }



        /// <summary>
        /// Check if the passed TaggedValue is of type duration
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        private bool isDuration(String s)
        {
            
            if (s == null || s.ToLower() == "null")
            {
                return true;
            }
            else
            {
                try
                {
                    TimeSpan t = SoapDuration.Parse(s);
                    /* if a xsd:duration value is not parsable a zero TimeSpan is returned
                     * which means that the value isnt a correct xsd:duration */
                    if (!t.Equals(TimeSpan.Zero))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }
            }
        }


       

    }
}
