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
    class bRealizationVValidator : AbstractValidator
    {

         
        public bRealizationVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
            
        }




        /// <summary>
        /// Validate the BusinessRealizationView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        {


            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Package brv = repository.GetPackageByID(Int32.Parse(scope));

            //Check the Tagged Values of the Business Realization View
            messages.AddRange(checkTV_BusinessRealizationView(brv));


            //Check constraint C92
            List<ValidationMessage> vm92 = checkC92(brv);
            if (vm92 != null && vm92.Count != 0) messages.AddRange(vm92);

            if (!Utility.canProceedWithValidation(messages))
                return messages;

            //Check constraint C93
            List<ValidationMessage> vm93 = checkC93(brv);
            if (vm93 != null && vm93.Count != 0) messages.AddRange(vm93);

            //Check constraint C94
            List<ValidationMessage> vm94 = checkC94(brv);
            if (vm94 != null && vm94.Count != 0) messages.AddRange(vm94);

            //Check constraint C95
            List<ValidationMessage> vm95 = checkC95(brv);
            if (vm95 != null && vm95.Count != 0) messages.AddRange(vm95);

            //Check constraint C96
            List<ValidationMessage> vm96 = checkC96(brv);
            if (vm96 != null && vm96.Count != 0) messages.AddRange(vm96);

            //Check constraint C97
            List<ValidationMessage> vm97 = checkC97(brv);
            if (vm97 != null && vm97.Count != 0) messages.AddRange(vm97);

            //Check constraint C98
            List<ValidationMessage> vm98 = checkC98(brv);
            if (vm98 != null && vm98.Count != 0) messages.AddRange(vm98);

            //Check constraint C99
            List<ValidationMessage> vm99 = checkC99(brv);
            if (vm99 != null && vm99.Count != 0) messages.AddRange(vm99);

            return messages;
        }


        /// <summary>
        /// Check constraint C92
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC92(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            int count_BR = 0;
            int count_AR = 0;


            foreach (EA.Element e in brv.Elements)
            {

                //Count the BusinessRealizations
                if (e.Stereotype == UMM.bRealization.ToString())
                {
                    count_BR++;
                }
                else if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
            }

            if (count_BR != 1)
            {
                messages.Add(new ValidationMessage("Violation of constraint C92.", "A BusinessRealizationView MUST contain exactly one BusinessRealization, two to many AuthorizedRoles, and two to many participates associations. \n\nFound " + count_BR + " BusinessRealizations.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }

            if (count_AR < 2)
            {
                messages.Add(new ValidationMessage("Violation of constraint C92.", "A BusinessRealizationView MUST contain exactly one BusinessRealization, two to many AuthorizedRoles, and two to many participates associations. \n\nFound " + count_AR + " AuthorizedRoles.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }



            //We do not check the participates associations here but in the next constraint


            return messages;
        }


        /// <summary>
        /// Check constraint C93
        /// 
        /// A BusinessRealization MUST be associated with two to many AuthorizedRoles via stereotyped binary participates associations.
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC93(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {

                int countConnections = 0;
                foreach (EA.Connector con in br.Connectors)
                {

                    if (con.Stereotype == UMM.participates.ToString())
                    {
                        //The realization must be the supplier
                        if (con.SupplierID == br.ElementID)
                        {
                            EA.Element client = repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                countConnections++;

                            }
                        }
                    }
                }

                if (countConnections < 2)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C93.", "A BusinessRealization MUST be associated with two to many AuthorizedRoles via stereotyped binary participates associations.\n\nInvalid number of assocations found: " + countConnections, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }



            }

            return messages;
        }



        /// <summary>
        /// Check constraint C94
        /// A BusinessRealization MUST be the source of exactly one realization 
        /// dependency to a BusinessCollaborationUseCase.
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC94(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();



            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {

                int count = 0;
                //Get the dependencies
                foreach (EA.Connector con in br.Connectors)
                {
                    String d = con.Type;

                    if (con.Type == "Realisation" && con.Stereotype == "realizes")
                    {

                        if (con.ClientID == br.ElementID)
                        {
                            EA.Element supplier = repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                count++;
                            }
                        }
                    }
                }

                if (count != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C94.", "A BusinessRealization MUST be the source of exactly one realization dependency to a BusinessCollaborationUseCase. \n\nFound invalid number of realize dependencies: " + count, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }

            }

            return messages;
        }


        /// <summary>
        /// Check constraint C95
        /// A BusinessRealization MUST NOT be the 
        /// source or target of an include or extends association.
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC95(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {
                foreach (EA.Connector con in br.Connectors)
                {

                    if (con.Stereotype == "extend" || con.Stereotype == "include")
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C95.", "A BusinessRealization MUST NOT be the source or target of an include or extends association. \n\nBusinessRealization " + br.Name + " has invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                    }
                }


            }



            return messages;
        }


        /// <summary>
        /// Check constraint C96
        /// All dependencies from/to an AuthorizedRole must be stereotyped as mapsTo.
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC96(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            foreach (EA.Element ar in aroles)
            {
                //Get the dependencies
                foreach (EA.Connector con in ar.Connectors)
                {

                    if (con.Type == "Dependency")
                    {
                        if (con.Stereotype != UMM.mapsTo.ToString())
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C96.", "All dependencies from/to an AuthorizedRole must be stereotyped as mapsTo.\n\nBusiness Realization " + brv.Name + " has invalid connectors.", "BDV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                        }
                    }
                }



            }



            return messages;
        }



        /// <summary>
        /// Check constraint C97
        /// An AuthorizedRole, which participates in a BusinessRealization, must be 
        /// the target of exactly one mapsTo dependency starting from a BusinessPartner.
        /// Furthermore the AuthorizedRole, which participates in the BusinessRealization
        /// must be the source of exactly one mapsTo dependency targeting an AuthorizedRole 
        /// participating in a BusinessCollaborationUseCase.
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC97(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            foreach (EA.Element ar in aroles)
            {
                int countTo = 0;
                int countFrom = 0;
                bool doesParticipate = doesParticipateInBR(ar);


                foreach (EA.Connector con in ar.Connectors)
                {

                    if (doesParticipate)
                    {

                        if (con.Type == "Dependency" && con.Stereotype == UMM.mapsTo.ToString())
                        {
                            //Count the mapsTo leading to this authorized role
                            if (con.SupplierID == ar.ElementID)
                            {

                                EA.Element client = repository.GetElementByID(con.ClientID);
                                if (client.Stereotype == UMM.bPartner.ToString())
                                {
                                    countTo++;
                                }
                            }
                            //Count the mapsTo emanating from this authorized role
                            if (con.ClientID == ar.ElementID)
                            {
                                EA.Element supplier = repository.GetElementByID(con.SupplierID);
                                if (supplier.Stereotype == UMM.AuthorizedRole.ToString())
                                {
                                    if (doesParticipateinBC(supplier))
                                    {
                                        countFrom++;
                                    }
                                }


                            }
                        }




                    }
                }

                if (doesParticipate && countTo != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C97.", "An AuthorizedRole, which participates in a BusinessRealization, must be the target of exactly one mapsTo dependency starting from a BusinessPartner. Furthermore the AuthorizedRole, which participates in the BusinessRealization must be the source of exactly one mapsTo dependency targeting an AuthorizedRole participating in a BusinessCollaborationUseCase. \n\nAuthorized Role " + ar.Name + " has an invalid number of incoming mapsTo dependencies: " + countTo, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));

                }

                if (doesParticipate && countFrom != 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C97.", "An AuthorizedRole, which participates in a BusinessRealization, must be the target of exactly one mapsTo dependency starting from a BusinessPartner. Furthermore the AuthorizedRole, which participates in the BusinessRealization must be the source of exactly one mapsTo dependency targeting an AuthorizedRole participating in a BusinessCollaborationUseCase. \n\nAuthorized Role " + ar.Name + " has an invalid number of outgoing mapsTo dependencies: " + countFrom, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));

                }

            }
            return messages;
        }



        /// <summary>
        /// Check constraint C98
        /// AuthorizedRoles in a BusinessRealizationView must have a unique 
        /// name within the scope of the package, they are located in
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC98(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());


            foreach (EA.Element ar in aroles)
            {
                String name = ar.Name;
                foreach (EA.Element ar1 in aroles)
                {
                    if (ar1 != ar)
                    {
                        if (ar1.Name == name)
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C98.", "AuthorizedRoles in a BusinessRealizationView must have a unique name within the scope of the package, they are located in. \n\nThe following name is used multiple times: " + ar.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                            return messages;
                        }
                    }
                }


            }


            return messages;
        }



        /// <summary>
        /// Check constraint C99
        /// 
        /// The number of AuthorizedRoles participating in a BusinessCollaborationUseCase 
        /// MUST match the number of AuthorizedRoles participating in the BusinessRealization 
        /// realizing this BusinessCollaborationUseCase
        /// </summary>
        /// <param name="brv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC99(EA.Package brv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            int countBR = 0;
            int countBC = 0;

            //Count the AR of the BusinessRealization
            foreach (EA.Element ar in aroles)
            {

                if (doesParticipateInBR(ar))
                {
                    countBR++;
                }
            }

            //Count the AR of the BCUC
            //Get the BusinessRealization first
            EA.Element bcuc = null;
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());
            if (br != null)
            {
                foreach (EA.Connector con in br.Connectors)
                {
                    if (con.Type == "Realisation" && con.Stereotype == "realizes")
                    {
                        if (con.ClientID == br.ElementID)
                        {
                            EA.Element supplier = repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                bcuc = supplier;
                                break;
                            }
                        }

                    }
                }

                if (bcuc != null)
                {
                    foreach (EA.Connector con in bcuc.Connectors)
                    {
                        if (con.SupplierID == bcuc.ElementID && con.Stereotype == UMM.participates.ToString())
                        {
                            EA.Element client = repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                countBC++;
                            }
                        }

                    }


                }

            }


            if (countBC != countBR)
            {
                messages.Add(new ValidationMessage("Violation of constraint C99.", "The number of AuthorizedRoles participating in a BusinessCollaborationUseCase MUST match the number of AuthorizedRoles participating in the BusinessRealization realizing this BusinessCollaborationUseCase. \n\n" + countBR + " AuthorizedRoles participate in the BusinessRealization and " + countBC + " AuthorizedRoles participate in the BusinessCollaborationUseCase.", "BRV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }




            return messages;
        }



        /// <summary>
        /// Returns true if the given Element e has a participates assocation to a
        /// BusinessCollaborationUseCase
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool doesParticipateinBC(EA.Element e)
        {


            foreach (EA.Connector con in e.Connectors)
            {

                if (con.ClientID == e.ElementID && con.Stereotype == UMM.participates.ToString())
                {
                    EA.Element supplier = repository.GetElementByID(con.SupplierID);
                    if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                        return true;

                }

            }



            return false;
        }



        /// <summary>
        /// Returns true if the given Element e has a participates assocation to
        /// a BusinessREalization
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool doesParticipateInBR(EA.Element e)
        {

            foreach (EA.Connector con in e.Connectors)
            {

                if (con.ClientID == e.ElementID && con.Stereotype == UMM.participates.ToString())
                {
                    EA.Element supplier = repository.GetElementByID(con.SupplierID);
                    if (supplier.Stereotype == UMM.bRealization.ToString())
                        return true;

                }

            }

            return false;
        }






        /// <summary>
        /// Check the tagged values of the BusinessRealizationView
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessRealizationView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the BusinessRealizationView package
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));


            return messages;
        }



    }
    


}
