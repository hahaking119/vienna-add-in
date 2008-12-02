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
using VIENNAAddIn.common;


namespace VIENNAAddIn.validator.umm.bcv
{
    class bTransactionVValidator : AbstractValidator
    {


        public bTransactionVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }




        /// <summary>
        /// Validate the BusinessTransactionView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        { 


            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Package btv = repository.GetPackageByID(Int32.Parse(scope));

            //Check the Tagged Values of the Business Transaction View
            messages.AddRange(checkTV_BusinessTransactionView(btv));


            //Check constraint C34
            List<ValidationMessage> vm34 = checkC34(btv);
            if (vm34 != null && vm34.Count != 0) messages.AddRange(vm34);

            //Check constraint C35
            List<ValidationMessage> vm35 = checkC35(btv);
            if (vm35 != null && vm35.Count != 0) messages.AddRange(vm35);

            //Check constraint C36
            ValidationMessage vm36 = checkC36(btv);
            if (vm36 != null) messages.Add(vm36);

            //Check constraint C37
            ValidationMessage vm37 = checkC37(btv);
            if (vm37 != null) messages.Add(vm37);

            //Check constraint C38
            ValidationMessage vm38 = checkC38(btv);
            if (vm38 != null) messages.Add(vm38);

            //Check constraint C39
            ValidationMessage vm39 = checkC39(btv);
            if (vm39 != null) messages.Add(vm39);

            //Check constraint C40
            ValidationMessage vm40 = checkC40(btv);
            if (vm40 != null) messages.Add(vm40);

            //Check constraint C41
            ValidationMessage vm41 = checkC41(btv);
            if (vm41 != null) messages.Add(vm41);

            //Check constraint C42
            List<ValidationMessage> vm42 = checkC42(btv);
            if (vm42 != null && vm42.Count != 0)
                messages.AddRange(vm42);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C43
            List<ValidationMessage> vm43 = checkC43(btv);
            if (vm43 != null && vm43.Count != 0) messages.AddRange(vm43);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C44
            ValidationMessage vm44 = checkC44(btv);
            if (vm44 != null) messages.Add(vm44);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C45
            List<ValidationMessage> vm45 = checkC45(btv);
            if (vm45 != null && vm45.Count != 0) messages.AddRange(vm45);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C46
            ValidationMessage vm46 = checkC46(btv);
            if (vm46 != null) messages.Add(vm46);

            //Check constraint C47
            ValidationMessage vm47 = checkC47(btv);
            if (vm47 != null) messages.Add(vm47);

            //Check constraint C48
            List<ValidationMessage> vm48 = checkC48(btv);
            if (vm48 != null) messages.AddRange(vm48);

            //Check constraing C49
            List<ValidationMessage> vm49 = checkC49(btv);
            if (vm49 != null) messages.AddRange(vm49);

            //Check constraint C50
            List<ValidationMessage> vm50 = checkC50(btv);
            if (vm50 != null && vm50.Count != 0) messages.AddRange(vm50);

            //Return at this point if error messages are present because the successing constraints depend on the correctness
            //of the preceding constraints
            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C51
            List<ValidationMessage> vm51 = checkC51(btv);
            if (vm51 != null && vm51.Count != 0) messages.AddRange(vm51);

            //Check constraint C52
            List<ValidationMessage> vm52 = checkC52(btv);
            if (vm52 != null && vm52.Count != 0) messages.AddRange(vm52);

            //Check constraint C53
            List<ValidationMessage> vm53 = checkC53(btv);
            if (vm53 != null && vm53.Count != 0) messages.AddRange(vm53);

            //Check constraint C54
            List<ValidationMessage> vm54 = checkC54(btv);
            if (vm54 != null && vm54.Count != 0) messages.AddRange(vm54);

            //Check constraint C55
            List<ValidationMessage> vm55 = checkC55(btv);
            if (vm55 != null && vm55.Count != 0) messages.AddRange(vm55);

            //Check constraint C56
            List<ValidationMessage> vm56 = checkC56(btv);
            if (vm56 != null && vm56.Count != 0) messages.AddRange(vm56);

            //Check constraint C57
            List<ValidationMessage> vm57 = checkC57(btv);
            if (vm57 != null && vm57.Count != 0) messages.AddRange(vm57);

            return messages;
        }




        /// <summary>
        /// Check constraint C34
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC34(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            int count_AR = 0;
            int count_btuc = 0;

            foreach (EA.Element e in btv.Elements)
            {

                //Count BTUC
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    count_btuc++;
                }
                //Count Authorized Roles
                else if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
                //Invalid stereotype detected
                else
                {
                    //Ignore subelements
                    if (e.ParentID == 0)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C34.", "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nAn element with an invalid stereotype has been detected: " + e.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                }
            }

            //There must be exactly one business transaction use case
            if (count_btuc != 1)
            {
                messages.Add(new ValidationMessage("Violation of constraint C34.", "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nInvalid number of BusinessTransactionUseCases detected.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }

            //There must be exactly two Authorized Roles
            if (count_AR != 2)
            {
                messages.Add(new ValidationMessage("Violation of constraint C34.", "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nInvalid number of AuthorizedRoles detected.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }

            //We do not check the two participates associations here since that is taken care of in constraint 
            //C35


            return messages;
        }




        /// <summary>
        /// Check constraint C35
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC35(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                int countAssociations = 0;
                //Make sure, that the BTUC is associated with exactly two authorized roles via participates assocations
                foreach (EA.Connector con in btuc.Connectors)
                {

                    //Only associations and dependencies are allowed
                    if (!(con.Type == AssocationTypes.Association.ToString() || con.Type == AssocationTypes.Dependency.ToString() || con.Type == AssocationTypes.Realisation.ToString() || con.Type == "UseCase"))
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C35.", "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nOnly assocations and dependencies are allowed. Invalid connection type found: " + con.Type + ".", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                    else if (con.Stereotype == UMM.participates.ToString())
                    {

                        //Correct connection leading from an AuthorizedRole to a business transaction use case
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //Client must be of type Authorized role
                            EA.Element client = repository.GetElementByID(con.ClientID);

                            if (client.Stereotype != UMM.AuthorizedRole.ToString())
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C35.", "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nThe particiaptes relationship must lead from an AuthorizedRole to a BusinessTransactionUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                            }
                            else
                            {
                                countAssociations++;
                            }
                        }
                        else
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C35.", "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nThe particiaptes relationship must lead from an AuthorizedRole to a BusinessTransactionUseCase.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }

                    }
                    else if (con.Stereotype == UMM.include.ToString())
                    {
                        //do nothing here
                    }
                    //Wrong stereotype
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C35.", "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nInvalid or empty connection stereotype found: " + con.Stereotype, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                }

                //There must be exactly two participates assocations
                if (countAssociations != 2)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C35", "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participate associations.\n\nInvalid number of participates assocations found (" + countAssociations + ").", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }


            }


            return messages;

        }


        /// <summary>
        /// Validate constraint C36
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC36(EA.Package btv)
        {


            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                foreach (EA.Connector con in btuc.Connectors)
                {
                    //A BTUC must not be the client of an assocation of any kind
                    if (con.ClientID == btuc.ElementID)
                    {
                        return new ValidationMessage("Violation of constraint C36.", "A BusinessTransactionUseCase MUST NOT include further UseCases.\n\nA BusinessTransactionUseCase cannot be the client of any relationship (no connection must lead FROM a BusinessTransactionUseCase TO another model element). Please check the correct direction of BusinessTransactionUseCase's connections.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Check constraint C37
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC37(EA.Package btv)
        {

            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                bool found = false;

                //A BTUC must be included in at least one BCUC
                foreach (EA.Connector con in btuc.Connectors)
                {
                    if (con.Stereotype == UMM.include.ToString())
                    {
                        //The BTUC must be the supplier of the include relationship
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //The Client of the relationship must be a bcuc
                            EA.Element e = repository.GetElementByID(con.ClientID);
                            if (e.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                found = true;
                            }
                        }
                    }
                }
                //No BCUC relationship found
                if (!found)
                {
                    return new ValidationMessage("Violation of constraint C37.", "A BusinessTransactionUseCase MUST be included in at least one BusinessCollaborationUseCase. \n\nNo include relationship to a BusinessCollaborationView has been found. Go to the BusinessChoreographyView and make sure, that a connection exists.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                }


            }

            return null;
        }



        /// <summary>
        /// Check constraint C38
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC38(EA.Package btv)
        {

            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                //A BTUC must be included in at least one BCUC
                foreach (EA.Connector con in btuc.Connectors)
                {
                    if (con.Stereotype == "extend")
                    {
                        return new ValidationMessage("Violation of constraint C38.", "A BusinessTransactionUseCase MUST NOT be source or target of an extend association. \n\nAn 'extend' connection has been detected.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                    }
                }

            }

            return null;
        }




        /// <summary>
        /// Check constraint C39
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private ValidationMessage checkC39(EA.Package btv)
        {


            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                String name = "";

                //Make sure, that the two Authorized Roles of a BTUC do not have the same name
                foreach (EA.Connector con in btuc.Connectors)
                {

                    //Only associations and dependencies are allowed
                    if (con.Stereotype == UMM.participates.ToString())
                    {

                        //Correct connection leading from an AuthorizedRole to a business transaction use case
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //Client must be of type Authorized role
                            EA.Element client = repository.GetElementByID(con.ClientID);

                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                if (name == "")
                                {
                                    name = client.Name;
                                }
                                else
                                {
                                    //No two Authorized Roles must have the same name
                                    if (name == client.Name)
                                    {
                                        return new ValidationMessage("Violation of constraint C39.", "The two AuthorizedRoles within a BusinessTransactionView MUST NOT be named identically.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return null;
        }



        /// <summary>
        /// Check constraint C40
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC40(EA.Package btv)
        {


            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                int countBT = 0;
                //Is the BTUC described by a Business Transaction?
                foreach (EA.Element e in btv.Elements)
                {
                    if (e.Stereotype == UMM.bTransaction.ToString())
                    {
                        countBT++;
                    }
                }

                if (countBT != 1)
                {
                    return new ValidationMessage("Violation of constraint C40.", "A BusinessTransactionUseCase MUST be described by exactly one BusinessTransaction defined as a child element of this BusinessTransactionUseCase.\n\nFound " + countBT + " BusinessTransactions.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                }

            }

            return null;
        }


        /// <summary>
        /// Check constraint C41
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC41(EA.Package btv)
        {

            //Get the Business Transaction
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());
            if (bta != null)
            {
                int countPartitions = 0;
                //A business transaction must have two partitions
                foreach (EA.Element e in bta.Elements)
                {
                    if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                if (countPartitions != 2)
                {
                    return new ValidationMessage("Violation of constraint C41.", "A BusinessTransaction MUST have exactly two partitions. Each of them MUST be stereotyped as BusinessTransactionPartition. \n\nMake sure that the BusinessTransactionPartition is located underneath the BusinessTransaction in the TreeView on the right hand side.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                }

            }


            return null;
        }


        /// <summary>
        /// Check constraing C42
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC42(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the Business Transaction
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());
            if (bta != null)
            {

                List<EA.Element> partitions = new List<EA.Element>();

                //Find the two partitions
                foreach (EA.Element e in bta.Elements)
                {
                    if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                    {
                        partitions.Add(e);
                    }
                }

                //Invalid number of partitions
                if (partitions.Count != 2)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nCannot validate constraint due to invalid number of partitions (" + partitions.Count + "). There must be exactly two partitions in a BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {

                    int i = 0;
                    bool bothInTheSame = false;
                    int numberReq = 0;
                    int numberRes = 0;


                    foreach (EA.Element partition in partitions)
                    {
                        i++;
                        foreach (EA.Element element in btv.Elements)
                        {
                            if (element.ParentID == partition.ElementID)
                            {
                                if (element.Stereotype == UMM.ReqAction.ToString())
                                {
                                    numberReq++;
                                }
                                if (element.Stereotype == UMM.ResAction.ToString())
                                {
                                    numberRes++;
                                }
                            }
                        }

                        //Both found in the same partition
                        if (numberReq == 1 && numberRes == 1 && !(i == 2))
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nBoth, RequestingBusinessAction and RespondingBusinessAction have been found in the same partition.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                            bothInTheSame = true;
                            break;
                        }
                    }

                    if (!bothInTheSame)
                    {
                        //Neither/nor found                              
                        if (numberReq == 0 && numberRes == 0)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nNeither the RequestingBusinessAction nor the RespondingBusinessAction have been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                        else if (numberReq == 0)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nThe RequestingBusinessAction has not been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                        else if (numberRes == 0)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nThe RespondingBusinessAction has not been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                        else if (numberReq > 1)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nMore than one RequestingBusinessAction has been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                        else if (numberRes > 1)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C42.", "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nMore than one RespondingBusinessAction has been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                }


            }

            return messages;
        }


        /// <summary>
        /// Validate constraint C43
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC43(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            List<EA.Element> partitions = new List<EA.Element>();
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //Find the two partitions
            foreach (EA.Element e in bta.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //Invalid number of partitions
            if (partitions.Count != 2)
            {
                messages.Add(new ValidationMessage("Violation of constraint C44.", "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint due to invalid number of partitions (" + partitions.Count + "). There must be exactly two partitions in a BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the two authorized roles of the business transaction use case
                //Find the Business Transaction Use Case

                EA.Element btuc = Utility.getElementFromPackage(btv, UMM.bTransactionUC.ToString());


                //No BTUC found
                if (btuc == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C44.", "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint because no BusinessTransactionUseCase could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {
                    //Get the two authorized roles
                    List<EA.Element> authorizedRoles = this.getAuthorizedRolesFromBTUC(btv, btuc);
                    if (authorizedRoles.Count != 2)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C44.", "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint because an invalid number of AuthorizedRoles has been found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                    else
                    {
                        foreach (EA.Element partition in partitions)
                        {
                            if (!(partition.ClassifierID == authorizedRoles[0].ElementID ||
                                partition.ClassifierID == authorizedRoles[1].ElementID))
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C44.", "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nMissing or wrong classifier detected. Make sure, that every BusinessTransactionPartition is classified correctly.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                            }

                        }
                    }

                }
            }

            return messages;

        }


        /// <summary>
        /// Validating constraint C44
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC44(EA.Package btv)
        {

            List<EA.Element> partitions = new List<EA.Element>();

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //There must be two partitions
            if (partitions.Count != 2)
            {
                return new ValidationMessage("Violation of constraint C44.", "The two BusinessTransactionPartitions MUST have different classifiers. \n\nCannot validate the constraint because an invalid number of partitions has been found (" + partitions.Count + "). There must be exactly two partitions in a business transaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
            }
            else
            {
                //Check whether the two partitions have the same classifier
                if (partitions[0].ClassifierID == partitions[1].ClassifierID)
                    return new ValidationMessage("Violation of constraint C44.", "The two BusinessTransactionPartitions MUST have different classifiers.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);

            }
            return null;

        }


        /// <summary>
        /// Validate constraint C45
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC45(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the RequestingBusinessTransactionPartition
            List<EA.Element> partitions = new List<EA.Element>();

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //There must be two partitions
            if (partitions.Count != 2)
            {
                messages.Add(new ValidationMessage("Violation of constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState.\n\nInvalid number of partitions found. There must be exactly two partitions.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the partition containing the requesting action
                EA.Element requestingPartition = null;
                foreach (EA.Element partition in partitions)
                {
                    foreach (EA.Element e in btv.Elements)
                    {
                        if (e.ParentID == partition.ElementID && e.Stereotype == UMM.ReqAction.ToString())
                        {
                            requestingPartition = partition;
                            break;
                        }
                    }
                }

                if (requestingPartition == null)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState.\n\nUnable to find a partition containing the RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {
                    //Iterate over the elements of the Requesting Partition
                    int countFinalStates = 0;
                    int countControlFailures = 0;

                    foreach (EA.Element e in btv.Elements)
                    {
                        if (e.ParentID == requestingPartition.ElementID)
                        {

                            String s = e.Name;
                            String type = e.Type;
                            String subtype = e.Subtype.ToString();

                            if (e.Type == "StateNode")
                            {
                                if (e.Subtype == 101)
                                {
                                    countFinalStates++;

                                    //Final state found - does the final state has a BusinessEntityState as predecessor?
                                    foreach (EA.Connector con in e.Connectors)
                                    {
                                        EA.Element supplier = repository.GetElementByID(con.ClientID);

                                        //A ControlFailure should not have a preceeding bESharedState
                                        if (e.Name == "ControlFailure")
                                        {
                                            if (supplier.Stereotype == UMM.bESharedState.ToString())
                                            {
                                                messages.Add(new ValidationMessage("Info for constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nThe FinalState ControlFailure has a predecessing SharedBusinessEntityState.", "BCV", ValidationMessage.errorLevelTypes.WARN, btv.PackageID));
                                            }
                                        }
                                        else
                                        {
                                            if (supplier.Stereotype != UMM.bESharedState.ToString())
                                            {
                                                messages.Add(new ValidationMessage("Info for constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nFinalState " + e.Name + " does not have a preceeding BusinessEntitySharedState.", "BCV", ValidationMessage.errorLevelTypes.INFO, btv.PackageID));
                                            }
                                        }
                                    }

                                    //No Connectors found
                                    if (e.Connectors.Count == 0)
                                    {
                                        messages.Add(new ValidationMessage("Violation of constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nThe FinalState " + e.Name + " does not have any connections.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                    }


                                    if (e.Name == "ControlFailure")
                                        countControlFailures++;

                                }
                            }

                        }
                    }

                    if (countFinalStates < 2)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nInvalid number of Final States found: " + countFinalStates, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Info for constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nFound " + countFinalStates + " FinalStates.", "BCV", ValidationMessage.errorLevelTypes.INFO, btv.PackageID));
                    }

                    if (countControlFailures != 1)
                    {
                        messages.Add(new ValidationMessage("Info for constraint C45.", "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nNo FinalState ControlFailure found.", "BCV", ValidationMessage.errorLevelTypes.WARN, btv.PackageID));
                    }


                }



            }




            return messages;

        }



        /// <summary>
        /// Check constraint C46
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC46(EA.Package btv)
        {

            //Get the Requesting Action
            EA.Element requestingAction = null;
            requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

            if (requestingAction == null)
            {
                return new ValidationMessage("Violation of constraint C46.", "A RequestingBusinessAction MUST embed exactly one RequestingInformationPin.\n\nNo RequestingBusinessAction could be found. ", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
            }
            else
            {
                int countPin = 0;
                foreach (EA.Element e in requestingAction.EmbeddedElements)
                {
                    if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                    {
                        countPin++;
                    }
                }


                if (countPin != 1)
                {
                    return new ValidationMessage("Violation of constraint C46.", "A RequestingBusinessAction MUST embed exactly one RequestingInformationPin.\n\nInvalid number of RequestingInformationPins found: " + countPin, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                }


            }



            return null;
        }


        /// <summary>
        /// Check constraint C47
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private ValidationMessage checkC47(EA.Package btv)
        {
            //Get the Responding Action
            EA.Element respondingAction = null;
            respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

            if (respondingAction == null)
            {
                return new ValidationMessage("Violation of constraint C47.", "A RespondingBusinessAction MUST embed exactly one RequestingInformationPin. \n\nRespondingBusinessAction not found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
            }
            else
            {
                int countPin = 0;
                foreach (EA.Element e in respondingAction.EmbeddedElements)
                {
                    if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                    {
                        countPin++;
                    }
                }


                if (countPin != 1)
                {
                    return new ValidationMessage("Violation of constraint C47.", "A RespondingBusinessAction MUST embed exactly one RequestingInformationPin. \n\nInvalid number of RequestingInformationPins found: " + countPin, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID);
                }

            }


            return null;
        }



        /// <summary>
        /// Check constraint C48
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC48(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransactionfound
            if (bt == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C48.", "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\n", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the TaggedValue businessTransactionType
                bool found = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" || tvalue.Value == "Request/Confirm" || tvalue.Value == "Commercial Transaction")
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    //Get the Requesting Action
                    EA.Element requestingAction = null;
                    requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                    if (requestingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in requestingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins < 1)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C48.", "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nRequestingBusinessAction has invalid number of RespondingInformationPins: " + countPins, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }

                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C48.", "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nUnable to find RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                    //Get the Responding Action
                    EA.Element respondingAction = null;
                    respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                    if (respondingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in respondingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins < 1)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C48.", "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nRespondingBusinessAction has invalid number of RespondingInformationPins: " + countPins, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C48.", "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nUnable to find RespondingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                }


            }



            return messages;
        }





        /// <summary>
        /// Check constraint C49
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC49(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C49.", "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nBusinessTransaction could not be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the TaggedValue businessTransactionType
                bool found = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Notification" || tvalue.Value == "Information Distribution")
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    //Get the Requesting Action
                    EA.Element requestingAction = null;
                    requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                    if (requestingAction != null)
                    {
                        //There must not be any RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in requestingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins != 0)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C49.", "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nFound " + countPins + " RespondingInformationPins on the RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }

                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C49.", "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nRequestingBusinessAction could not be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                    //Get the Responding Action
                    EA.Element respondingAction = null;
                    respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                    if (respondingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in respondingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins != 0)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C49.", "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nFound " + countPins + " RespondingInformationPins on the RespondingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C49.", "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nRespondingBusinessAction could not be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                }


            }



            return messages;
        }


        /// <summary>
        /// Check constraint C50
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC50(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C50.", "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {

                //Get the Requesting Action
                EA.Element requestingAction = null;
                requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                int countReqPins = 0;
                int countResPins = 0;

                if (requestingAction != null)
                {
                    foreach (EA.Element e in requestingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            countReqPins++;
                        }
                    }
                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C50.", "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

                //Get the Responding Action
                EA.Element respondingAction = null;
                respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                if (respondingAction != null)
                {
                    foreach (EA.Element e in respondingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            countResPins++;
                        }
                    }

                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C50.", "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find RespondingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

                if (countReqPins != countResPins)
                    messages.Add(new ValidationMessage("Violation of constraint C50.", "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nFound " + countReqPins + " RespondingInformationPins on the RequestingBusinessAction and " + countReqPins + " RespondingInformationPins on the RespondingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));

            }





            return messages;
        }




        /// <summary>
        /// Check constraint C51
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC51(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C51.", "he RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {

                //Get the Requesting Action
                EA.Element requestingAction = null;
                requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());
                EA.Element requestingInformationPin = null;


                if (requestingAction != null)
                {
                    foreach (EA.Element e in requestingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                        {
                            requestingInformationPin = e;
                            break;
                        }
                    }


                    if (requestingInformationPin != null)
                    {
                        bool found = false;
                        foreach (EA.Connector con in requestingInformationPin.Connectors)
                        {

                            if (con.Type == "ObjectFlow")
                            {
                                EA.Element supplier = repository.GetElementByID(con.SupplierID);
                                if (supplier.Stereotype == UMM.ReqInfPin.ToString())
                                {
                                    //The parent of the supplier must be the RespondingBusinessAction
                                    EA.Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());
                                    if (supplier.ParentID == respondingAction.ElementID)
                                    {
                                        found = true;
                                    }
                                }
                            }
                        }

                        if (!found)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C51.", "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\n", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }


                    }
                    //No Requesting Information Pin on the RequestingBusinessAction
                    else
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C51.", "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find RequestingInformationPin on RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }

                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C51.", "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }

            return messages;
        }




        /// <summary>
        /// Check constraint C52
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC52(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C52.", "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.\n\nUnable to find BusinessTransaction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {

                //Get the Responding Action
                EA.Element respondingAction = null;
                respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());


                if (respondingAction != null)
                {
                    foreach (EA.Element e in respondingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            bool hasConnector = false;

                            foreach (EA.Connector con in e.Connectors)
                            {

                                if (con.Type == "ObjectFlow")
                                {
                                    EA.Element supplier = repository.GetElementByID(con.SupplierID);
                                    if (supplier.Stereotype == UMM.ResInfPin.ToString())
                                    {
                                        //The parent of the supplier must be the RequestingBusinessAction
                                        EA.Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());
                                        if (supplier.ParentID == requestingAction.ElementID)
                                        {
                                            hasConnector = true;
                                        }
                                    }
                                }
                            }

                            if (!hasConnector)
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C52.", "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C52.", "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.\n\nUnable to find RespondingBusinessAction.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }

            return messages;
        }



        /// <summary>
        /// Check constraint C53
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC53(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            List<EA.Element> partitions = new List<EA.Element>();

            //Get the partition containing the requesting action
            EA.Element requestingPartition = null;
            EA.Element requestingAction = null;

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            foreach (EA.Element partition in partitions)
            {
                foreach (EA.Element e in btv.Elements)
                {
                    if (e.ParentID == partition.ElementID && e.Stereotype == UMM.ReqAction.ToString())
                    {
                        requestingPartition = partition;
                        requestingAction = e;
                        break;
                    }
                }
            }

            if (requestingPartition == null)
            {
                messages.Add(new ValidationMessage("Violation of constraint C53.", "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nUnable to find RequestingBusinessTransactionPartition.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the SharedBusinessEntityStates
                IList<EA.Element> sharedStates = Utility.getAllElements(btv, new List<EA.Element>(), UMM.bESharedState.ToString());

                foreach (EA.Element e in sharedStates)
                {
                    //The shared entity state must be located in the requesting partition
                    if (e.ParentID == requestingPartition.ElementID)
                    {

                        int countConnectionsTo = 0;
                        int countConnectionsFrom = 0;

                        //Check if there is exactly one connector leading from a requesting business action
                        //to the besharedstate
                        foreach (EA.Connector con in e.Connectors)
                        {
                            //Get the client which MUST be a requesting business action

                            EA.Element client = repository.GetElementByID(con.ClientID);
                            EA.Element supplier = repository.GetElementByID(con.SupplierID);

                            //Count the connections from requesting business actions
                            if (client.Stereotype == UMM.ReqAction.ToString())
                            {
                                countConnectionsTo++;
                            }

                            //Count the connections to final states
                            if (supplier.Subtype == 101)
                            {
                                countConnectionsFrom++;
                            }


                        }
                        if (countConnectionsTo != 1)
                        {

                            String sharedBusinessEntityStateName = "undefined";
                            if (e.ClassfierID != 0)
                            {
                                sharedBusinessEntityStateName = repository.GetElementByID(e.ClassifierID).Name;
                            }

                            messages.Add(new ValidationMessage("Violation of constraint C53.", "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nFound " + countConnectionsTo + " ControlFlows leading from the RequestingBusinessAction to SharedBusinessEntityState " + sharedBusinessEntityStateName, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }

                        if (countConnectionsFrom != 1)
                        {
                            String sharedBusinessEntityStateName = "undefined";
                            if (e.ClassfierID != 0)
                            {
                                sharedBusinessEntityStateName = repository.GetElementByID(e.ClassifierID).Name;
                            }

                            messages.Add(new ValidationMessage("Violation of constraint C53.", "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nSharedBusinessEntityState " + sharedBusinessEntityStateName + " does not have a connection to a final state.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                }
            }


            return messages;
        }






        /// <summary>
        /// Check constraint C54
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC54(EA.Package btv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            List<EA.Element> finalStates = new List<EA.Element>();

            //Get all final states
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Subtype == 101)
                {
                    finalStates.Add(e);
                }
            }


            foreach (EA.Element finalState in finalStates)
            {
                int countConnections = 0;

                foreach (EA.Connector con in finalState.Connectors)
                {
                    //Get the client of the connection
                    EA.Element client = repository.GetElementByID(con.ClientID);
                    if (client.Stereotype == UMM.ReqAction.ToString() || client.Stereotype == UMM.bESharedState.ToString())
                    {
                        countConnections++;
                    }
                }

                if (countConnections < 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C54.", "Each FinalState MUST be the target of one to many control flow relationships starting from the RequestingBusinessAction or from a SharedBusinessEntityState. \n\nUnable to find a connection.", "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

            }




            return messages;


        }



        /// <summary>
        /// Check constraint C55
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC55(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the RequestingInformationPins
            IList<EA.Element> reqInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ReqInfPin.ToString());
            //Each Pin must have a classifier which MUST be an InformationEnveleope or a subtype thereof

            EA.Element classifier = null;

            foreach (EA.Element e in reqInfPins)
            {
                bool classifierFound = false;
                classifier = null;

                if (e.ClassifierID != 0)
                    classifier = repository.GetElementByID(e.ClassifierID);

                if (classifier != null)
                {
                    String d = classifier.Stereotype;

                    //Is the classifier of type InformationEnvelope?
                    if (classifier.Stereotype == UMM.InfEnvelope.ToString())
                    {
                        classifierFound = true;

                    }
                    //Is the classifier a subtype of an InformationEnvelope?
                    else
                    {
                        foreach (EA.Element el in classifier.BaseClasses)
                        {
                            if (el.Stereotype == UMM.InfEnvelope.ToString())
                            {
                                classifierFound = true;
                                break;
                            }
                        }
                    }
                }

                //Classifier found?
                if (!classifierFound)
                {
                    String errorneousClassifier = "";
                    if (classifier != null)
                        errorneousClassifier = classifier.Name;

                    messages.Add(new ValidationMessage("Violation of constraint C55.", "Each RequestingInformationPin and each RespondingInformationPin MUST have a classifier, this classifier MUST be an InformationEnvelope or a subtype defined in an extension/specialization module. \n\nUnable to find a classifier for a RequestingInformationPin on the element " + repository.GetElementByID(e.ParentID).Name + " or classifier is invalid. Make sure that the InformationPins have the appropriate classifiers and that classifiers have the appropriate stereotype (InfEnvelope or subtypes thereof). Invalid classifier: " + errorneousClassifier, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }




            //Get the RespondingInformationPins
            IList<EA.Element> resInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ResInfPin.ToString());
            //Each Pin must have a classifier which MUST be an InformationEnveleope or a subtype thereof
            foreach (EA.Element e in resInfPins)
            {
                bool classifierFound = false;
                classifier = null;

                if (e.ClassifierID != 0)
                    classifier = repository.GetElementByID(e.ClassifierID);

                if (classifier != null)
                {
                    //Is the classifier of type InformationEnvelope?
                    if (classifier.Stereotype == UMM.InfEnvelope.ToString())
                    {
                        classifierFound = true;

                    }
                    //Is the classifier a subtype of an InformationEnvelope?
                    else
                    {
                        foreach (EA.Element el in classifier.BaseClasses)
                        {
                            if (el.Stereotype == UMM.InfEnvelope.ToString())
                            {
                                classifierFound = true;
                                break;
                            }
                        }
                    }
                }

                //Classifier found?
                if (!classifierFound)
                {
                    String errorneousClassifier = "";
                    if (classifier != null)
                        errorneousClassifier = classifier.Name;

                    messages.Add(new ValidationMessage("Violation of constraint C55.", "Each RequestingInformationPin and each RespondingInformationPin MUST have a classifier, this classifier MUST be an InformationEnvelope or a subtype defined in an extension/specialization module. \n\nUnable to find a classifier for a RespondingInformationPin on the element " + repository.GetElementByID(e.ParentID) + ".Name Make sure that the InformationPins have the appropriate classifiers and that classifiers have the appropriate stereotype (InfEnvelope or subtypes thereof). Invalid classifier: " + errorneousClassifier, "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

            }
            return messages;
        }







        /// <summary>
        /// Check constraint C56
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC56(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the RequestingInformationPins
            IList<EA.Element> reqInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ReqInfPin.ToString());

            List<int> checkedElement = new List<int>();

            //Each reqInfPin must be connected to another RequestingInformationPin using an object flow and
            //the classifiers of both reqinfpins must be the same
            foreach (EA.Element e in reqInfPins)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Type == "ObjectFlow")
                    {
                        if (!checkedElement.Contains(con.ConnectorID))
                        {

                            //This pin is a client of an object flow
                            if (con.ClientID == e.ElementID)
                            {
                                EA.Element supplier = repository.GetElementByID(con.SupplierID);
                                if (supplier.ClassifierID != e.ClassifierID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C56.", "Two RequestingInformationPins which are connected using an object flow MUST have the same classifier.", "BDV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                }
                            }
                            //This pin is the supplier of an object flow
                            else
                            {
                                EA.Element client = repository.GetElementByID(con.ClientID);
                                if (client.ClassifierID != e.ClassifierID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C56.", "Two RequestingInformationPins which are connected using an object flow MUST have the same classifier.", "BDV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                }
                            }
                            checkedElement.Add(con.ConnectorID);
                        }
                    }
                }
            }
            return messages;
        }



        /// <summary>
        /// Check constraint C57
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC57(EA.Package btv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the RespondingInformationPins
            IList<EA.Element> resInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ResInfPin.ToString());

            List<int> checkedElement = new List<int>();

            //Each resInfPin must be connected to another RespondingInformationPin using an object flow and
            //the classifiers of both resinfpins must be the same
            foreach (EA.Element e in resInfPins)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Type == "ObjectFlow")
                    {
                        if (!checkedElement.Contains(con.ConnectorID))
                        {

                            //This pin is a client of an object flow
                            if (con.ClientID == e.ElementID)
                            {
                                EA.Element supplier = repository.GetElementByID(con.SupplierID);
                                if (supplier.ClassifierID != e.ClassifierID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C57.", "Two RespondingInformationPins which are connected using an object flow MUST have the same classifier.", "BDV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                }
                            }
                            //This pin is the supplier of an object flow
                            else
                            {
                                EA.Element client = repository.GetElementByID(con.ClientID);
                                if (client.ClassifierID != e.ClassifierID)
                                {
                                    messages.Add(new ValidationMessage("Violation of constraint C57.", "Two RespondingInformationPins which are connected using an object flow MUST have the same classifier.", "BDV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                                }
                            }
                            checkedElement.Add(con.ConnectorID);
                        }
                    }
                }
            }
            return messages;
        }




        /// <summary>
        /// Get the two Authorized Roles associated with a business transaction use case
        /// </summary>
        /// <param name="btv"></param>
        /// <returns></returns>
        private List<EA.Element> getAuthorizedRolesFromBTUC(EA.Package btv, EA.Element btuc)
        {

            List<EA.Element> authorizedRoles = new List<EA.Element>();

            //Make sure, that the BTUC is associated with exactly two authorized roles via participates assocations
            foreach (EA.Connector con in btuc.Connectors)
            {

                //Only associations and dependencies are allowed
                if (con.Stereotype == UMM.participates.ToString())
                {

                    //Correct connection leading from an AuthorizedRole to a business transaction use case
                    if (con.SupplierID == btuc.ElementID)
                    {
                        //Client must be of type Authorized role
                        EA.Element client = repository.GetElementByID(con.ClientID);

                        if (client.Stereotype == UMM.AuthorizedRole.ToString())
                        {
                            authorizedRoles.Add(client);
                        }
                    }
                }

            }
            return authorizedRoles;

        }






        /// <summary>
        /// Check the tagged values of the BusinessTransactionView
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessTransactionView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the BusinessTransactionView package
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));

            //Check the TaggedValues of the BusinessTransactionUseCases
            IList<EA.Element> btucs = Utility.getAllElements(p, new List<EA.Element>(), UMM.bTransactionUC.ToString());
            foreach (EA.Element btuc in btucs)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(btuc));
            }

            //Check the tagged values ot the bTransactions
            IList<EA.Element> bts = Utility.getAllElements(p, new List<EA.Element>(), UMM.bTransaction.ToString());
            foreach (EA.Element bt in bts)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(bt));
            }

            //Check the tagged value of the RequestingActions and RespondingActions
            IList<EA.Element> reqActions = Utility.getAllElements(p, new List<EA.Element>(), UMM.ReqAction.ToString());
            IList<EA.Element> resActions = Utility.getAllElements(p, new List<EA.Element>(), UMM.ResAction.ToString());
            foreach (EA.Element reqAction in reqActions)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(reqAction));
            }
            foreach (EA.Element resAction in resActions)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(resAction));
            }


            //Get the Requesting/Responding Action
            EA.Element requestingAction = null;
            EA.Element respondingAction = null;
            requestingAction = Utility.getElementFromPackage(p, UMM.ReqAction.ToString());
            respondingAction = Utility.getElementFromPackage(p, UMM.ResAction.ToString());

            if (requestingAction != null)
            {
                foreach (EA.Element el in requestingAction.EmbeddedElements)
                {
                    if (el.Type == "ActionPin" && (el.Stereotype == UMM.ReqInfPin.ToString() || el.Stereotype == UMM.ResInfPin.ToString()))
                    {
                        messages.AddRange(new TaggedValueValidator(repository).validateElement(el));
                    }
                }
            }

            if (respondingAction != null)
            {
                foreach (EA.Element el in respondingAction.EmbeddedElements)
                {
                    if (el.Type == "ActionPin" && (el.Stereotype == UMM.ResInfPin.ToString() || el.Stereotype == UMM.ReqInfPin.ToString()))
                    {
                        messages.AddRange(new TaggedValueValidator(repository).validateElement(el));
                    }
                }
            }


            return messages;
        }








    }
}
