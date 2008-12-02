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
    class bCollaborationVValidator : AbstractValidator
    {



        public bCollaborationVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }



        /// <summary>
        /// Validate the Business Collaboration View
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

            //Check the Tagged Values of the Business Collaboration View
            messages.AddRange(checkTV_BusinessCollaborationView(bcv));
             
            //Check constraint C58
            ValidationMessage vm58 = checkC58(bcv);
            if (vm58 != null) messages.Add(vm58);

            //Check constraint C59
            ValidationMessage vm59 = checkC59(bcv);
            if (vm59 != null) messages.Add(vm59);
                        
            //Check constraint C60
            List<ValidationMessage> vm60 = checkC60(bcv);
            if (vm60 != null && vm60.Count != 0) messages.AddRange(vm60);

            //Check constraint C61
            List<ValidationMessage> vm61 = checkC61(bcv);
            if (vm61 != null && vm61.Count != 0) messages.AddRange(vm61);

            //Check constraint C62
            List<ValidationMessage> vm62 = checkC62(bcv);
            if (vm62 != null && vm62.Count != 0) messages.AddRange(vm62);

            //Check constraint C63
            List<ValidationMessage> vm63 = checkC63(bcv);
            if (vm63 != null && vm63.Count != 0) messages.AddRange(vm63);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C64
            List<ValidationMessage> vm64 = checkC64(bcv);
            if (vm64 != null && vm64.Count != 0) messages.AddRange(vm64);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C65
            List<ValidationMessage> vm65 = checkC65(bcv);
            if (vm65 != null && vm65.Count != 0) messages.AddRange(vm65);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C66
            List<ValidationMessage> vm66 = checkC66(bcv);
            if (vm66 != null && vm66.Count != 0) messages.AddRange(vm66);

            //Check constraint C67
            List<ValidationMessage> vm67 = checkC67(bcv);
            if (vm67 != null && vm67.Count != 0) messages.AddRange(vm67);

            //Check constraint C68
            ValidationMessage vm68 = checkC68(bcv);
            if (vm68 != null) messages.Add(vm68);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C69
            ValidationMessage vm69 = checkC69(bcv);
            if (vm69 != null) messages.Add(vm69);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C70
            List<ValidationMessage> vm70 = checkC70(bcv);
            if (vm70 != null && vm70.Count != 0) messages.AddRange(vm70);

            //Check constraint C71
            List<ValidationMessage> vm71 = checkC71(bcv);
            if (vm71 != null && vm71.Count != 0) messages.AddRange(vm71);

            //Check constraint C72
            List<ValidationMessage> vm72 = checkC72(bcv);
            if (vm72 != null && vm72.Count != 0) messages.AddRange(vm72);

            //Check constraint C73
            List<ValidationMessage> vm73 = checkC73(bcv);
            if (vm73 != null && vm73.Count != 0) messages.AddRange(vm73);

            //Check constraint C74
            List<ValidationMessage> vm74 = checkC74(bcv);
            if (vm74 != null && vm74.Count != 0) messages.AddRange(vm74);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C75
            List<ValidationMessage> vm75 = checkC75(bcv);
            if (vm75 != null && vm75.Count != 0) messages.AddRange(vm75);

            //Check constraint C76
            List<ValidationMessage> vm76 = checkC76(bcv);
            if (vm76 != null && vm76.Count != 0) messages.AddRange(vm76);

            //Check constraint C77
            List<ValidationMessage> vm77 = checkC77(bcv);
            if (vm77 != null && vm77.Count != 0) messages.AddRange(vm77);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C78
            List<ValidationMessage> vm78 = checkC78(bcv);
            if (vm78 != null && vm78.Count != 0) messages.AddRange(vm78);

            //Check constraint C79
            List<ValidationMessage> vm79 = checkC79(bcv);
            if (vm79 != null && vm79.Count != 0) messages.AddRange(vm79);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C80
            List<ValidationMessage> vm80 = checkC80(bcv);
            if (vm80 != null && vm80.Count != 0) messages.AddRange(vm80);

            //Check constraint C81
            List<ValidationMessage> vm81 = checkC81(bcv);
            if (vm81 != null && vm81.Count != 0) messages.AddRange(vm81);

            //Check constraint C82
            List<ValidationMessage> vm82 = checkC82(bcv);
            if (vm82 != null && vm82.Count != 0) messages.AddRange(vm82);

            //Check constraint C83
            List<ValidationMessage> vm83 = checkC83(bcv);
            if (vm83 != null && vm83.Count != 0) messages.AddRange(vm83);

            //Check constraint C84
            List<ValidationMessage> vm84 = checkC84(bcv);
            if (vm84 != null && vm84.Count != 0) messages.AddRange(vm84);

            //Check constraint C85
            List<ValidationMessage> vm85 = checkC85(bcv);
            if (vm85 != null && vm85.Count != 0) messages.AddRange(vm85);

            //Check constraint C86
            List<ValidationMessage> vm86 = checkC86(bcv);
            if (vm86 != null && vm86.Count != 0) messages.AddRange(vm86);

            //Check constraint C87
            List<ValidationMessage> vm87 = checkC87(bcv);
            if (vm87 != null && vm87.Count != 0) messages.AddRange(vm87);

            //Check constraint C88
            List<ValidationMessage> vm88 = checkC88(bcv);
            if (vm88 != null && vm88.Count != 0) messages.AddRange(vm88);

            //Check constraint C89
            List<ValidationMessage> vm89 = checkC89(bcv);
            if (vm89 != null && vm89.Count != 0) messages.AddRange(vm89);

            //Check constraint C90
            List<ValidationMessage> vm90 = checkC90(bcv);
            if (vm90 != null && vm90.Count != 0) messages.AddRange(vm90);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C91
            List<ValidationMessage> vm91 = checkC91(bcv);
            if (vm91 != null && vm91.Count != 0) messages.AddRange(vm91);


            return messages;
        }


        /// <summary>
        /// Check constraint c58
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC58(EA.Package bcv)
        {


            int count_BCUC = 0;

            foreach (EA.Element e in bcv.Elements)
            {
                if (e.Stereotype == UMM.bCollaborationUC.ToString())
                {
                    count_BCUC++;
                }
            }

            if (count_BCUC != 1)
            {
                return new ValidationMessage("Violation of constraint C58.", "A BusinessCollaborationView MUST contain exactly one BusinessCollaborationUseCase. \n\nFound " + count_BCUC + " BusinessCollaborationUseCases.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }

            return null;
        }



        /// <summary>
        /// Check constraint c59
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC59(EA.Package bcv)
        {


            int count_AR = 0;

            foreach (EA.Element e in bcv.Elements)
            {
                if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
            }

            if (count_AR < 2)
            {
                return new ValidationMessage("Violation of constraint C59.", "A BusinessCollaborationView MUST contain two to many AuthorizedRoles. \n\nFound " + count_AR + " Authorized Roles.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }

            return null;
        }



        /// <summary>
        /// Check constraint C60
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC60(EA.Package bcv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C60.", "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nUnable to find BusinessCollaborationUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
            else
            {
                int count_ParticipatesAssocations = 0;
                //Iterate of the different assocations of the BCUC
                foreach (EA.Connector con in bcuc.Connectors)
                {

                    if (con.Type == "Association" && con.Stereotype == UMM.participates.ToString())
                    {
                        //The client must be an AuthorizedRole
                        EA.Element client = repository.GetElementByID(con.ClientID);

                        if (client.Stereotype != UMM.AuthorizedRole.ToString())
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C60.", "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nFound a participates assocation which is either wrong-directed (it must lead from an Authorized Role to a Businss Collaboration Use Case) or connected to a wrong element. Wrong source element of the participates assocation: " + client.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                        //Found an Authorized Role - is the Authorized Role from the Business Collaboration View
                        else if (client.PackageID != bcv.PackageID)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C60.", "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nThe Authorized Role " + client.Name + " of the BusinessCollaborationUseCase " + bcuc.Name + "is from a different BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                        count_ParticipatesAssocations++;
                    }
                }

                if (count_ParticipatesAssocations < 2)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C60.", "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nInvalid number of participates assocations found: " + count_ParticipatesAssocations, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }
            return messages;
        }




        /// <summary>
        /// Check constraint C61
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC61(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C61.", "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nUnable to find BusinessCollaborationUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
            else
            {
                //Get the Authorized Roles
                IList<EA.Element> ars = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());



                if (ars != null)
                {
                    foreach (EA.Element ar in ars)
                    {

                        int countAssocations = 0;

                        //Iterate of the connectors of the AR and get the participates associations
                        foreach (EA.Connector con in ar.Connectors)
                        {
                            if (con.Type == "Association" && con.Stereotype == UMM.participates.ToString())
                            {

                                EA.Element client = repository.GetElementByID(con.ClientID);
                                EA.Element supplier = repository.GetElementByID(con.SupplierID);

                                //The client must be of type Authorized Role
                                if (client.Stereotype != UMM.AuthorizedRole.ToString())
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C61.", "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nMissing assocation for AuthorizedRole " + ar.Name + ". Make sure, that the participates assocation has the correct direction (from the AuthorizedRole to the BusinessCollaborationUseCase).", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                                else if (supplier.ElementID != bcuc.ElementID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C61.", "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAssocation of AuthorizedRole " + ar.Name + " is leading to a BusinessCollaborationUseCase from a different BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                                //AR and BCUC must be in the same package
                                else
                                {
                                    if (ar.PackageID != bcuc.PackageID)
                                    {
                                        messages.Add(new ValidationMessage("Violation of constraint C61.", "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAuthorizedRole and BusinessCollaborationUseCase must be in the same package. AuthorizedRole " + ar.Name + " is contained in package " + repository.GetPackageByID(ar.PackageID).Name + " and BusinessCollaborationUseCase" + bcuc.Name + " is contained in package " + bcv.Name + ".", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                    }
                                }
                                countAssocations++;

                            }
                        }

                        //Each Authorized Role muast have exactly one participates assocation
                        if (countAssocations != 1)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C61.", "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAuthorizedRole " + ar.Name + " has an invalid number of participates assocations: " + countAssocations, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                    }
                }
            }

            return messages;
        }




        /// <summary>
        /// Check constraint C62
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC62(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C62.", "A BusinessCollaborationUseCase MUST have one to many include relationships to another BusinessCollaborationUseCase or to a BusinessTransactionUseCase.\n\nUnable to find BusinessCollaborationUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
            else
            {
                int countIncludes = 0;
                foreach (EA.Connector con in bcuc.Connectors)
                {
                    if (con.Stereotype == UMM.include.ToString())
                    {
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);
                        if (supplier.Stereotype == UMM.bTransactionUC.ToString())
                        {
                            countIncludes++;
                        }
                        else if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                        {
                            countIncludes++;
                        }
                    }
                }

                //There must be one to many includes
                if (countIncludes < 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C62.", "A BusinessCollaborationUseCase MUST have one to many include relationships to another BusinessCollaborationUseCase or to a BusinessTransactionUseCase.\n\nFound " + countIncludes + " include relationships.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }



            }




            return messages;
        }





        /// <summary>
        /// Check constraint C63
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC63(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C63.", "Exactly one BusinessCollaborationProtocol MUST be placed beneath each BusinessCollaborationUseCase. \n\nUable to find BusinessCollaborationUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
            else
            {
                int countProtocols = 0;
                //Get the number of BusinessCollaborationProtocols
                foreach (EA.Element e in bcv.Elements)
                {
                    if (e.Stereotype == UMM.bCollaborationProtocol.ToString() && e.ParentID == bcuc.ElementID)
                    {
                        countProtocols++;
                    }

                }

                if (countProtocols != 1)
                    messages.Add(new ValidationMessage("Violation of constraint C63.", "Exactly one BusinessCollaborationProtocol MUST be placed beneath each BusinessCollaborationUseCase. \n\nFound " + countProtocols + " BusinessCollaborationProtocols.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }

            return messages;
        }




        /// <summary>
        /// Check constraint C64
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC64(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessCollaborationProtocol
            EA.Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());
            if (bcpr == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C64.", "A BusinessCollaborationProtocol MUST contain one to many BusinessTransactionActions and/or BusinessCollaborationAction.\n\nUnable to find BusinessCollaborationProtocol.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
            else
            {
                int countAction = 0;
                //Iterate over the elements and get all subelements of the business collaboration protocol
                foreach (EA.Element el in bcv.Elements)
                {
                    //Count the included BusinessCollaborationActions and BusinessTransactionActions
                    if (el.ParentID == bcpr.ElementID)
                    {
                        if (el.Stereotype == UMM.bCollaborationAction.ToString() ||
                            el.Stereotype == UMM.bTransactionAction.ToString())
                        {
                            countAction++;
                        }
                    }
                }
                if (countAction < 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C64.", "A BusinessCollaborationProtocol MUST contain one to many BusinessTransactionActions and/or BusinessCollaborationAction.\n\nNo Actions found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }



            return messages;


        }



        /// <summary>
        /// Check constraint C65
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC65(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessTransactionActions
            IList<EA.Element> btas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());

            if (btas != null && btas.Count != 0)
            {
                foreach (EA.Element bta in btas)
                {
                    EA.Element classifier = null;

                    if (bta.ClassifierID != 0)
                        classifier = repository.GetElementByID(bta.ClassifierID);

                    if (classifier == null || classifier.Stereotype != UMM.bTransaction.ToString())
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C65.", "Each BusinessTransactionAction MUST call exactly one BusinessTransaction\n\nBusinessTransactionAction " + bta.Name + " does not call a BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }

                }
            }
            else
            {
                messages.Add(new ValidationMessage("Info for constraint C65.", "Each BusinessTransactionAction MUST call exactly one BusinessTransaction\n\nNo BusinessTransactionActions found.", "BCV", ValidationMessage.errorLevelTypes.INFO, bcv.PackageID));
            }

            return messages;
        }


        /// <summary>
        /// Check constraint C66
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC66(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessTransactionActions
            IList<EA.Element> btas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());

            if (btas != null && btas.Count != 0)
            {
                foreach (EA.Element bta in btas)
                {
                    EA.Element classifier = null;

                    if (bta.ClassifierID != 0)
                        classifier = repository.GetElementByID(bta.ClassifierID);

                    //No Business Transaction found
                    if (classifier == null || classifier.Stereotype != UMM.bTransaction.ToString())
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C66.", "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessTransaction for BusinessTransactionAction " + bta.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                    else
                    {
                        //Get the business transaction use case under which the business transaction is placed
                        EA.Package btv = repository.GetPackageByID(classifier.PackageID);
                        EA.Element btuc = null;
                        foreach (EA.Element e in btv.Elements)
                        {
                            if (e.Stereotype == UMM.bTransactionUC.ToString())
                            {
                                btuc = e;
                                break;
                            }
                        }

                        //No Business Transaction Use Case found
                        if (btuc == null)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C66.", "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessTransactionUseCase for BusinessTransactionAction " + bta.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                        else
                        {
                            //Get the BusinessCollaborationUseCase of this package
                            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());
                            if (bcuc == null)
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C66.", "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessCollaborationUseCase in which the BusinessTransactionUseCase of the BusinessTransaction " + bta.Name + " should be included.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                            }
                            else
                            {
                                //Does the BusinessCollaborationUseCase include the BTUC
                                bool found = false;
                                foreach (EA.Connector con in bcuc.Connectors)
                                {

                                    if (con.Stereotype == UMM.include.ToString())
                                    {
                                        EA.Element client = repository.GetElementByID(con.ClientID);
                                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                                        if (client.ElementID == bcuc.ElementID &&
                                            supplier.ElementID == btuc.ElementID)
                                        {
                                            found = true;
                                        }
                                    }
                                }

                                if (!found)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C66.", "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nThe BusinessTransactionUseCase " + btuc.Name + " is not included by the BusinessCollaborationUseCase " + bcuc.Name + ". If you want to use a BusinessTransaction because it is called by a BusinessTransactionAction in a BusinessCollaborationView, the following requirement has to be met: \n1. The BusinessTransactionUseCase under which the BusinessTransaction is placed MUST be included by the BusinessCollaborationUseCase of the BusinessCollaborationView where you want to use the BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                            }
                        }

                    }

                }
            }

            return messages;

        }




        /// <summary>
        /// Check constraint C67
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC67(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();



            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());

            if (bcas != null && bcas.Count != 0)
            {
                foreach (EA.Element bca in bcas)
                {
                    EA.Element classifier = null;

                    if (bca.ClassifierID != 0)
                        classifier = repository.GetElementByID(bca.ClassifierID);

                    //No Business CollaborationProtocol found
                    if (classifier == null || classifier.Stereotype != UMM.bCollaborationProtocol.ToString())
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C67.", "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationProtocol for BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                    else
                    {
                        //Get the business collaboration use case under which the business collaboration protocol is placed
                        EA.Package bcv1 = repository.GetPackageByID(classifier.PackageID);
                        EA.Element includedBCUC = null;
                        foreach (EA.Element e in bcv1.Elements)
                        {
                            if (e.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                includedBCUC = e;
                                break;
                            }
                        }

                        //No Business Collaboration Use Case found
                        if (includedBCUC == null)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C67.", "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationUseCase for BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                        else
                        {
                            //Get the BusinessCollaborationUseCase of this package
                            EA.Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());
                            if (bcuc == null)
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C67.", "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationUseCase in which the BusinessCollaborationUseCase of the BusinessCollaborationAction " + bca.Name + " should be included.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                            }
                            else
                            {
                                //Does the BusinessCollaborationUseCase include the BTUC
                                bool found = false;
                                foreach (EA.Connector con in bcuc.Connectors)
                                {

                                    if (con.Stereotype == UMM.include.ToString())
                                    {
                                        EA.Element client = repository.GetElementByID(con.ClientID);
                                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                                        if (client.ElementID == bcuc.ElementID &&
                                            supplier.ElementID == includedBCUC.ElementID)
                                        {
                                            found = true;
                                        }
                                    }
                                }

                                if (!found)
                                {

                                    messages.Add(new ValidationMessage("Violation of constraint C67.", "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nThe BusinessCollaborationUseCase " + includedBCUC.Name + " is not included by the BusinessCollaborationUseCase " + bcuc.Name + ".If you want to use a BusinessCollaborationProtocol because it is called by a BusinessCollaborationAction in a BusinessCollaborationView, the following requirement has to be met:\n1. The BusinessCollaborationUseCase under which the BusinessCollaborationProtocol is placed MUST be included by the BusinessCollaborationUseCase of the BusinessCollaborationView where you want to use the BusinessCollaboration.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));

                                }
                            }
                        }

                    }

                }
            }

            return messages;

        }


        /// <summary>
        /// Check constraint C68
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC68(EA.Package bcv)
        {

            //Get the BusinessCollaborationProtocol
            EA.Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());

            

            if (bcpr == null)
            {
                return new ValidationMessage("Violation of constraint C68.", "A BusinessCollaborationProtocol MUST contain two to many BusinessCollaborationPartions. \n\nUnable to find BusinessCollaborationProtocol", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }
            else 
            {
                int countPartitions = 0;

                foreach (EA.Element e in bcv.Elements)
                {

                    if (e.ParentID == bcpr.ElementID && e.Stereotype == UMM.bCPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                if (countPartitions < 2)
                {
                    return new ValidationMessage("Violation of constraint C68.", "A BusinessCollaborationProtocol MUST contain two to many BusinessCollaborationPartions. \n\nFound invalid number of BusinessCollaborationPartitions: " + countPartitions, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
                }

            }



            return null;
        }



        /// <summary>
        /// Validate constraint C69
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private ValidationMessage checkC69(EA.Package bcv)
        {
            //Get the number of partitions
            IList<EA.Element> partitions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCPartition.ToString());
            //Get the number of Authorized Roles
            IList<EA.Element> authorizedRoles = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            if (partitions.Count != authorizedRoles.Count)
            {
                return new ValidationMessage("Violation of constraint C69.", "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nFound " + partitions.Count + " BusinessCollaborationPartitions and " + authorizedRoles.Count + " AuthorizedRoles.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }

            //Get the BusinessCollaborationProtocol
            EA.Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());

            if (bcpr == null)
            {
                return new ValidationMessage("Violation of constraint C69.", "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nUnable to find BusinessCollaborationProtocol.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
            }
            else
            {
                //Check if all the partitions are located underneath the BusinessCollaboration Protocol
                int countPartitions = 0;
                foreach (EA.Element e in bcv.Elements)
                {
                    if (e.ParentID == bcpr.ElementID && e.Stereotype == UMM.bCPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                //The number of all partitions in the BusinessCollaborationView and the number of partitions underneath
                //the businesscollaborationprotocol must be the same
                if (countPartitions != partitions.Count)
                {
                    return new ValidationMessage("Violation of constraint C69.", "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nAll BusinesscollaborationPartitions must be placed beneath the BusinessCollaborationUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID);
                }
            }



            return null;
        }





        /// <summary>
        /// Check constraint C70
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC70(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all Authorized Roles
            IList<EA.Element> ar = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());
            //Get all Partitions
            IList<EA.Element> partitions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCPartition.ToString());

            //Check for each AR if it is assigned to a partitions
            foreach (EA.Element arole in ar)
            {
                bool found = false;
                foreach (EA.Element partition in partitions)
                {
                    if (partition.ClassifierID == arole.ElementID)
                    {
                        found = true;
                        break;
                    }
                }

                //No partition assigned to this authorized role
                if (!found)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C70.", "Each AuthorizedRole in the BusinessCollaborationView MUST be assigned to a BusinessCollaborationPartition in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView.\n\nNo BusinessCollaborationPartition is assigned to the AuthorizedRole " + arole.Name + ". Drag and drop the BusinessCollaborationPartition onto the diagram canvas, right click on it, choose Advanced > Instance Classifier and select the correct Authorized Role.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }
            return messages;
        }





        /// <summary>
        /// Check constraint C71
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC71(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all Authorized Roles
            IList<EA.Element> ar = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());
            //Get all Partitions
            IList<EA.Element> partitions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCPartition.ToString());

            //Check if each partition is classified by excactly one Authorized role
            foreach (EA.Element partition in partitions)
            {
                bool found = false;
                foreach (EA.Element arole in ar)
                {
                    if (partition.ClassifierID == arole.ElementID)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C71.", "Each BusinessCollaborationPartition MUST be classified by exactly one AuthorizedRole included in the same BusinessCollaborationView as the BusinessCollaborationUseCase covering the BusinessCollaborationProtocol containing this BusinessCollaborationPartition.\n\nFound a partition without a classifier. Drag and drop the BusinessCollaborationPartition onto the diagram canvas, right click on it, choose Advanced > Instance Classifier and select the correct Authorized Role.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }
            return messages;
        }



        /// <summary>
        /// Check constraint C72
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC72(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessCollaborationPartitions            
            IList<EA.Element> partitions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCPartition.ToString());

            foreach (EA.Element e in bcv.Elements)
            {
                foreach (EA.Element partition in partitions)
                {
                    if (e.ParentID == partition.ElementID)
                    {
                        if (e.Stereotype != UMM.bNestedCollaboration.ToString())
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C72.", "A BusinessCollaborationPartition MUST be either empty or contain one to many NestedBusinessCollaborations.\n\nInvalid element detected: " + e.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                    }
                }
            }


            return messages;
        }




        /// <summary>
        /// Check constraint C73
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC73(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            foreach (EA.Element e in btactions)
            {
                int countInitialFlows = 0;

                //Each BusinessTransactionAction MUST be the target of exactly one IntialFlow which source
                //MUST be a BusinessCollaborationPartition
                foreach (EA.Connector con in e.Connectors)
                {

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {

                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);


                        //The business transaction action must be the supplier
                        if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition
                            if (client.Stereotype == UMM.bCPartition.ToString())
                            {

                                //The partition must be in this business collaboration view
                                if (client.PackageID != bcv.PackageID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C73.", "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nBusinessTransactionAction " + e.Name + " is connected to a BusinessCollaborationPartition which is part of another BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                                else
                                {
                                    countInitialFlows++;
                                }
                            }
                            else
                            {
                                //add error
                                messages.Add(new ValidationMessage("Violation of constraint C73.", "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nInvalid connection to BusinessTransactionAction " + e.Name + " detected. Please check the source of the connection and make sure that it is a BusinessCollaborationPartition.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                            }
                        }
                    }
                }

                if (countInitialFlows != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C73.", "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nInvalid number of connections found for BusinessTransactionAction " + e.Name + ": " + countInitialFlows, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }

            }

            return messages;

        }




        /// <summary>
        /// Check constraint C74
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC74(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            foreach (EA.Element e in btactions)
            {
                int countInitialFlows = 0;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (EA.Connector con in e.Connectors)
                {

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {

                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);


                        //The business transaction action must be the supplier
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition
                            if (supplier.Stereotype == UMM.bCPartition.ToString() || supplier.Stereotype == UMM.bNestedCollaboration.ToString())
                            {

                                //The partition must be in this business collaboration view
                                if (supplier.PackageID != bcv.PackageID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C74.", "Each BusinessTransactionAction MUST be the source of exactly one InitialFlow which target MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nBusinessTransactionAction " + e.Name + " is connected to a BusinessCollaborationPartition which is part of another BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                                else
                                {
                                    countInitialFlows++;
                                }
                            }
                        }
                    }
                }

                if (countInitialFlows != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C74.", "Each BusinessTransactionAction MUST be the source of exactly one InitialFlow which target MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid number of connections found for BusinessTransactionAction " + e.Name + ": " + countInitialFlows, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }

            }
            return messages;

        }





        /// <summary>
        /// Check constraint C75
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC75(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            foreach (EA.Element e in btactions)
            {

                EA.Element supplierPartition = null;
                EA.Element clientPartition = null;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                        //The BusinessTransactionAction is the Client                       
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition or a NestedCollaboration
                            if (supplier.Stereotype == UMM.bCPartition.ToString() || client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                supplierPartition = supplier;
                            }
                        }
                        //The business transaction action is the supplier
                        else if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition or a Nested Collaboration
                            if (client.Stereotype == UMM.bCPartition.ToString() || client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                clientPartition = client;
                            }
                        }
                    }
                }

                if (supplierPartition != null && clientPartition != null)
                {
                    if (supplierPartition.ElementID == clientPartition.ElementID)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C75.", "The InitialFlow sourcing from a BusinessTransactionAction and the InitialFlow targeting a BusinessTransactionAction MUST NOT be targeting to / sourcing from the same BusinessCollaborationPartition, nor targeting to a NestedBusinessCollaboration within the same BusinessCollaborationPartition. \n\nPlease check the connections of BusinessTransactionAction " + e.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                }
            }

            return messages;
        }



        /// <summary>
        /// Check constraint C76
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC76(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (EA.Element btaction in btactions)
            {

                //Get the business transaction
                EA.Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C76.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    break;
                }

                bool twoWay = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" || tvalue.Value == "Request/Confirm" || tvalue.Value == "CommercialTransaction")
                        {
                            twoWay = true;
                            break;
                        }
                    }
                }

                //Two Way Transaction?
                if (twoWay)
                {
                    //Go through the connectors and check, if this business transaction action is the source of
                    //exactly one repsonding flow which target must be a businessdollaboration patrtition
                    int countReFlow = 0;
                    foreach (EA.Connector con in btaction.Connectors)
                    {
                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            //connector emanating from this business transaction action
                            if (client.ElementID == btaction.ElementID)
                            {
                                if (supplier.Stereotype == UMM.bCPartition.ToString())
                                {
                                    countReFlow++;
                                }
                                else
                                {
                                    //Invalid supplier of the reflow
                                    messages.Add(new ValidationMessage("Violation of constraint C76.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nInvalid target of reflow connector.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }

                    if (countReFlow != 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C76.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nInvalid number of reflow connections: " + countReFlow, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }


                }
            }
            return messages;

        }




        /// <summary>
        /// Check constraint C77
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC77(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (EA.Element btaction in btactions)
            {

                //Get the business transaction
                EA.Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C77.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    break;
                }

                bool twoWay = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" || tvalue.Value == "Request/Confirm" || tvalue.Value == "CommercialTransaction")
                        {
                            twoWay = true;
                            break;
                        }
                    }
                }

                //Two Way Transaction?
                if (twoWay)
                {
                    //Go through the connectors of the business transaction action and check if it the target of exactly
                    //one responding flow which source must be either a business collaboration partition of a nestedpartition
                    int countReFlow = 0;
                    foreach (EA.Connector con in btaction.Connectors)
                    {
                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            //connector leading to this business transaction action
                            if (supplier.ElementID == btaction.ElementID)
                            {
                                if (client.Stereotype == UMM.bCPartition.ToString() || client.Stereotype == UMM.bNestedCollaboration.ToString())
                                {
                                    countReFlow++;
                                }
                                else
                                {
                                    //Invalid source of the reflow
                                    messages.Add(new ValidationMessage("Violation of constraint C77.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid source of reflow connector.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }

                    if (countReFlow != 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C77.", "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid number of reflow connections: " + countReFlow, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }


                }
            }
            return messages;

        }





        /// <summary>
        /// Check constraint C78
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC78(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            foreach (EA.Element e in btactions)
            {

                EA.Element supplierPartition = null;
                EA.Element clientPartition = null;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        EA.Element client = repository.GetElementByID(con.ClientID);
                        EA.Element supplier = repository.GetElementByID(con.SupplierID);

                        //The BusinessTransactionAction is the Client                       
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition or a NestedCollaboration
                            if (supplier.Stereotype == UMM.bCPartition.ToString() || client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                supplierPartition = supplier;
                            }
                        }
                        //The business transaction action is the supplier
                        else if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition or a Nested Collaboration
                            if (client.Stereotype == UMM.bCPartition.ToString() || client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                clientPartition = client;
                            }
                        }
                    }
                }

                if (supplierPartition != null && clientPartition != null)
                {
                    if (supplierPartition.ElementID == clientPartition.ElementID)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C78.", "The RespondingFlow sourcing from a BusinessTransactionAction and the RespondingFlow targeting a BusinessTransactionAction MUST NOT be targeting to /sourcing from the same BusinessCollaborationPartition, nor targeting to a NestedBusinessCollaboration within the same BusinessCollaborationPartition.\n\nPlease check the connections of BusinessTransactionAction " + e.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                }
            }

            return messages;


        }





        /// <summary>
        /// Check constraint C79
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC79(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (EA.Element btaction in btactions)
            {

                //Get the business transaction
                EA.Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C79.", "If a BusinessTransactionAction calls a one-way BusinessTransaction, this BusinessTransactionAction MUST NOT be the source of a RespondingFlow and MUST NOT be the target of a RespondingFlow. \n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    break;
                }

                bool oneWay = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Notification" || tvalue.Value == "InformationDistribution")
                        {
                            oneWay = true;
                            break;
                        }
                    }
                }

                //One Way Transaction?
                if (oneWay)
                {
                    //Since the Business Transaction is a Onw way one, There must not be any reflows
                    foreach (EA.Connector con in btaction.Connectors)
                    {
                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C79.", "If a BusinessTransactionAction calls a one-way BusinessTransaction, this BusinessTransactionAction MUST NOT be the source of a RespondingFlow and MUST NOT be the target of a RespondingFlow. \n\nInvalid reflow detected on BusinessTransactionAction " + btaction.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                        }
                    }
                }

            }
            return messages;

        }




        /// <summary>
        /// Check constraint C80
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC80(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (EA.Element btaction in btactions)
            {
                int respondingClient = 0;
                int initSupplier = 0;

                //Is there a responding flow targeting this business transaction action?
                foreach (EA.Connector con in btaction.Connectors)
                {

                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        if (con.SupplierID == btaction.ElementID)
                        {
                            respondingClient = con.ClientID;
                        }
                    }

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.ClientID == btaction.ElementID)
                        {
                            initSupplier = con.SupplierID;
                        }
                    }
                }

                if (respondingClient != 0 && initSupplier != 0)
                {

                    if (respondingClient != initSupplier)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C80.", "The RespondingFlow targeting a BusinessTransactionAction must start from the BusinessCollaborationPartition / NestedBusinessCollaboration which is the target of the InitialFlow starting from the same BusinessTransactionAction.\n\nPlease check the connections of the BusinessTransactionAction " + btaction.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                }



            }

            return messages;
        }




        /// <summary>
        /// Check constraint C81
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC81(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btactions = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (EA.Element btaction in btactions)
            {
                int respondingSupplier = 0;
                int initClient = 0;

                //Is there a responding flow starting from the business transaction
                foreach (EA.Connector con in btaction.Connectors)
                {

                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        if (con.ClientID == btaction.ElementID)
                        {
                            respondingSupplier = con.SupplierID;
                        }
                    }

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.SupplierID == btaction.ElementID)
                        {
                            initClient = con.ClientID;
                        }
                    }
                }

                if (respondingSupplier != 0 && initClient != 0)
                {
                    if (respondingSupplier != initClient)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C81.", "The RespondingFlow starting from a BusinessTransactionAction must target the BusinessCollaborationPartition which is the source of the InitialFlow targeting to the same BusinessTransactionAction..\n\nPlease check the connections of the BusinessTransactionAction " + btaction.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }
                }
            }




            return messages;

        }





        /// <summary>
        /// Check constraint C82
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC82(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all NestedBusinesscollaborations (if any)
            IList<EA.Element> nestedC = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bNestedCollaboration.ToString());

            //A NestedBusinessCollaboration MUST be the target of exactly one InitialFlow
            foreach (EA.Element nc in nestedC)
            {
                int countCons = 0;
                foreach (EA.Connector con in nc.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.SupplierID == nc.ElementID)
                        {
                            countCons++;
                        }
                    }
                }

                if (countCons != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C82.", "A NestedBusinessCollaboration MUST be the target of exactly one InitialFlow. \n\nPlease check NestedBusinessCollaboration " + nc.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }
            return messages;
        }


        // <summary>
        /// Check constraint C83
        /// 
        /// A NestedBusinessCollaboration MAY be the source of a RespondingFlow, but MUST NOT be the source of more than one RespondingFlow.
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC83(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();




            //Get all NestedBusinesscollaborations (if any)
            IList<EA.Element> nestedC = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bNestedCollaboration.ToString());

            foreach (EA.Element nbc in nestedC)
            {

                //MUST NOT be the source of more than one RespondingFlow.
                int countCons = 0;
                foreach (EA.Connector con in nbc.Connectors)
                {

                    if (con.Stereotype == UMM.reFlow.ToString() && con.ClientID == nbc.ElementID)
                    {
                        countCons++;
                    }
                }

                if (countCons > 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C83.", "A NestedBusinessCollaboration MAY be the source of a RespondingFlow, but MUST NOT be the source of more than one RespondingFlow.\n\nNestedBusinessCollaboration " + bcv.Name + " has invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }



            }

            return messages;
        }




        // <summary>
        /// Check constraint C84
        /// 
        ///  A BusinessCollaborationAction MUST be the target of two to many InformationFlows (UML standard: <<flow>>). 
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC84(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());


            foreach (EA.Element bca in bcas)
            {
                int countFlows = 0;
                foreach (EA.Connector con in bca.Connectors)
                {



                    if (con.SupplierID == bca.ElementID && con.Type == "InformationFlow" && con.Stereotype == "flow")
                    {
                        countFlows++;
                    }

                }

                if (countFlows < 2)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C84.", "A BusinessCollaborationAction MUST be the target of two to many InformationFlows (UML standard: <<flow>>). \n\nBusinessCollaborationAction " + bca.Name + " has an invalid number of <<flow>> connectors: " + countFlows + ". Please check in the properties menu of the InformationFlow connector, if the stereotype is set correctly.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }


            }





            return messages;
        }



        /// <summary>
        /// Check constraint C85
        /// A BusinessCollaborationAction MUST not be the source of an InformationFlow.
        ///  
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC85(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());


            foreach (EA.Element bca in bcas)
            {
                int countFlows = 0;
                foreach (EA.Connector con in bca.Connectors)
                {
                    if (con.Type == "InformationFlow" && con.ClientID == bca.ElementID)
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C85.", "A BusinessCollaborationAction MUST not be the source of an InformationFlow. \n\nPlease check BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }

            return messages;
        }



        /// <summary>
        /// Check constraint C86
        /// A BusinessCollaborationAction MUST not be the source and MUST not be the target of an InitialFlow.
        ///  
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC86(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());


            foreach (EA.Element bca in bcas)
            {
                int countFlows = 0;
                foreach (EA.Connector con in bca.Connectors)
                {


                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C86.", "A BusinessCollaborationAction MUST not be the source and MUST not be the target of an InitialFlow.\n\nFound " + countFlows + " InitalFlows to/from BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }

            return messages;
        }





        /// <summary>
        /// Check constraint C87
        /// 
        ///  
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC87(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());



            foreach (EA.Element bca in bcas)
            {
                int countFlows = 0;
                foreach (EA.Connector con in bca.Connectors)
                {


                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C87.", "A BusinessCollaborationAction MUST not be the source and MUST not be the target of an RespondingFlow.\n\nFound " + countFlows + " RespondingFlows to/from BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
            }


            return messages;
        }



        /// <summary>
        /// Check constraint C88
        /// 
        ///A BusinessTransactionAction MUST not be the source and MUST not be the target of an InformationFlow (<<flow>>) that is neither stereotyped as InitialFlow nor as RespondingFlow.  
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC88(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all BusinessTransactionActions
            IList<EA.Element> btas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bTransactionAction.ToString());


            foreach (EA.Element bta in btas)
            {
                int count = 0;
                //There must not be any InformationFlow from/to but InitFlow and ReFlow
                foreach (EA.Connector con in bta.Connectors)
                {

                    if (con.Type == "InformationFlow")
                    {

                        //The stereotype flow is no specifically defined in the constraint - however we check it too
                        if (!(con.Stereotype == UMM.initFlow.ToString() || con.Stereotype == UMM.reFlow.ToString() || con.Stereotype == "flow"))
                        {
                            count++;
                        }
                    }

                }




                if (count != 0)
                {
                    messages.Add(new ValidationMessage("Violoation of constraint C88.", "A BusinessTransactionAction MUST not be the source and MUST not be the target of an InformationFlow (<<flow>>) that is neither stereotyped as InitialFlow nor as RespondingFlow.  \n\nBusinessTransactionAction " + bta.Name + " has invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }



            }



            return messages;


        }



        /// <summary>
        /// Check constraint C89
        /// 
        ///A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction.
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC89(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all NestedBusinessCollaboration
            IList<EA.Element> nbcs = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bNestedCollaboration.ToString());


            foreach (EA.Element nbc in nbcs)
            {

                foreach (EA.Connector con in nbc.Connectors)
                {
                    if (con.Type == "InformationFlow")
                    {
                        //NestedBusinessCollaboration is the client
                        if (con.ClientID == nbc.ElementID)
                        {
                            //the supplier must not be a businesscollaborationaction
                            EA.Element supplier = repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationAction.ToString())
                            {
                                //Raise an error
                                messages.Add(new ValidationMessage("Violation of constraint C89.", "A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction. \n\nPlease check NestedBusinessCollaboration " + nbc.Name + " for invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                            }
                        }
                        //NestedBusinessCollaboration is the supplier
                        else
                        {
                            //the client must not be a businesscollaborationaction
                            EA.Element client = repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.bCollaborationAction.ToString())
                            {
                                //Raise an error
                                messages.Add(new ValidationMessage("Violation of constraint C89.", "A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction. \n\nPlease check NestedBusinessCollaboration " + nbc.Name + " for invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                            }
                        }


                    }
                }



            }





            return messages;

        }




        /// <summary>
        /// Check constraint C90
        /// The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction.
        ///
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC90(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessCollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());

            foreach (EA.Element bca in bcas)
            {


                //Get the business collaboration
                EA.Element bc = null;
                if (bca.ClassifierID != 0)
                {
                    bc = repository.GetElementByID(bca.ClassifierID);
                }

                if (bc == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C90.", "The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nUnable to find the BusinessChoreography which is called by the BusinessCollaborationAction " + bca.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                }
                else
                {
                    //Got the business collaboration - get the package and count
                    //the number of BusinessCollaborationPartitions
                    EA.Package bcPackage = repository.GetPackageByID(bc.PackageID);
                    int countPartitions = 0;
                    foreach (EA.Element e in bcPackage.Elements)
                    {

                        if (e.Stereotype == UMM.bCPartition.ToString())
                        {
                            countPartitions++;
                        }

                    }

                    //Get the number of InformationFlows targeting the BusinessCollaborationAction
                    int numberOfFlows = 0;

                    foreach (EA.Connector con in bca.Connectors)
                    {

                        if (con.Type == "InformationFlow" && con.Stereotype == "flow")
                        {
                            if (con.SupplierID == bca.ElementID)
                            {
                                numberOfFlows++;
                            }

                        }

                    }


                    //Compare the two numbers
                    if (countPartitions != numberOfFlows)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C90.", "The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nThe BusinessCollaborationAction " + bca.Name + " is targeted by " + numberOfFlows + " InformationFlows. The number of partitions in the Business Collaboration the BusinessCollaborationAction is calling is " + countPartitions + "\n\nMake sure, that the InformationFlows to the BusinessCollaborationAction are stereotyped correctly (<<flow>>).", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                    }

                }


            }





            return messages;

        }



        /// <summary>
        /// Check constraint C91
        /// 
        ///Either an AuthorizedRole classifying a BusinessCollaborationPartition 
        ///that is the source of an InformationFlow (UML standard: <<flow>>) targeting a 
        ///BusinessCollaborationAction MUST match an AuthorizedRole classifying a 
        ///BusinessCollaborationPartition in the BusinessCollaborationProtocol that is 
        ///called by this BusinessCollaborationAction or the InformationFlow must be 
        ///classified by an AuthorizedRole classifying a BusinessCollaborationPartition 
        ///in the BusinessCollaborationProtocol that is called by this 
        ///BusinessCollaborationAction.
        ///
        /// </summary>
        /// <param name="bcv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC91(EA.Package bcv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check if there are any BusinesscollaborationActions
            IList<EA.Element> bcas = Utility.getAllElements(bcv, new List<EA.Element>(), UMM.bCollaborationAction.ToString());

            
            if (bcas != null && bcas.Count != 0)
            {

                foreach (EA.Element bca in bcas)
                {

                    List<EA.Element> authorizedRolesofCalledCollaboration = new List<EA.Element>();

                    //Get the BusinessCollaboration which the BusinessCollaborationAction is calling
                    EA.Element bc = null;

                    if (bca.ClassfierID != 0)
                    {
                        EA.Element cf = repository.GetElementByID(bca.ClassifierID);
                        EA.Package cfpackage = repository.GetPackageByID(cf.PackageID);
                        foreach (EA.Element el in cfpackage.Elements)
                        {
                            if (el.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                authorizedRolesofCalledCollaboration.Add(el);
                            }
                        }

                    }



                    List<Int32> analyzed = new List<Int32>();

                    foreach (EA.Connector con in bca.Connectors)
                    {
                        if (con.SupplierID == bca.ElementID)
                        {

                            if (con.Type == "InformationFlow" && con.Stereotype == "flow")
                            {
                                                         

                                //Get the partition where the flow is coming from
                                EA.Element partition = repository.GetElementByID(con.ClientID);
                                if (partition.Stereotype == UMM.bCPartition.ToString())
                                {
                                    if (partition.ClassfierID != 0)
                                    {
                                        //Get the Authorized Role, whcih is classifying the partition
                                        EA.Element classifier = repository.GetElementByID(partition.ClassfierID);

                                        if (!analyzed.Contains(classifier.ElementID))
                                        {

                                            bool found = false;
                                            foreach (EA.Element e in authorizedRolesofCalledCollaboration)
                                            {
                                                if (classifier.Name == e.Name)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }

                                            if (!found)
                                            {
                                                messages.Add(new ValidationMessage("Violation of constraint C91.", "Either an AuthorizedRole classifying a BusinessCollaborationPartition that is the source of an InformationFlow (UML standard: <<flow>>) targeting a BusinessCollaborationAction MUST match an AuthorizedRole classifying a BusinessCollaborationPartition in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction or the InformationFlow must be classified by an AuthorizedRole classifying a BusinessCollaborationPartition in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nFor the Authorized Role " + classifier.Name + " no equivalent Authorized Role in the Business Collaboration that is called by the BusinessColaborationAction " + bca.Name + " could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                            }

                                            analyzed.Add(classifier.ElementID);
                                        }



                                    }

                                }
                            }
                        }
                    }
                }
                
            }


            return messages;

        }






        /// <summary>
        /// Check the tagged values of the BusinessCollaborationView
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessCollaborationView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the BusinessCollaborationView package
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));

            //Check the TaggedValues of the BusinessCollaborationUseCase
            IList<EA.Element> bcucs = Utility.getAllElements(p, new List<EA.Element>(), UMM.bCollaborationUC.ToString());
            foreach (EA.Element bcuc in bcucs)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(bcuc));
            }

            //Check the TaggedValues of the BusinessTransactionActions
            IList<EA.Element> btas = Utility.getAllElements(p, new List<EA.Element>(), UMM.bTransactionAction.ToString());
            foreach (EA.Element bta in btas)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(bta));
            }

            
            return messages;
        }







    }
}
