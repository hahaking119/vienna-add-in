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

namespace VIENNAAddIn.validator.umm.brv
{
    class bEntityVValidator : AbstractValidator
    {


        public bEntityVValidator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
                       

        }


        /// <summary>
        /// Validate the BusinessEntityView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        internal override List<ValidationMessage> validate()
        {
                      
            List<ValidationMessage> messages = new List<ValidationMessage>();
            EA.Package bev = repository.GetPackageByID(Int32.Parse(scope));

            //Check the Tagged Values of the Business Entity View
            messages.AddRange(checkTV_BusinessEntityView(bev));

            //Check Constraint 23 
            ValidationMessage vm23 = checkC23(bev);
            if (vm23 != null) messages.Add(vm23);

            //Check Constraint 24
            List<ValidationMessage> vm24 = checkC24(bev);
            if (vm24 != null && vm24.Count != 0) messages.AddRange(vm24);

            //Check Constraint 25
            List<ValidationMessage> vm25 = checkC25(bev);
            if (vm25 != null && vm25.Count != 0) messages.AddRange(vm25);

            //Check Constraint 26
            List<ValidationMessage> vm26 = checkC26(bev);
            if (vm26 != null && vm26.Count != 0) messages.AddRange(vm26);

            //Check Constraint 27
            List<ValidationMessage> vm27 = checkC27(bev);
            if (vm27 != null && vm27.Count != 0) messages.AddRange(vm27);

            //Check Constraint 28
            List<ValidationMessage> vm28 = checkC28(bev);
            if (vm28 != null && vm28.Count != 0) messages.AddRange(vm28);

            //Check Constraint 29
            List<ValidationMessage> vm29 = checkC29(bev);
            if (vm29 != null && vm29.Count != 0) messages.AddRange(vm29);



            return messages;
        }



        /// <summary>
        /// Check constraint C23
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private ValidationMessage checkC23(EA.Package bev)
        {

            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());
                      

            //A BusinessEntityView must contain one to many business entities
            if (elements.Count < 1)
            {
                return new ValidationMessage("Violation of constraint C23.", "A BusinessEntityView MUST contain one to many BusinessEntities. \n\nThe BusinessEntityView " + bev.Name + " does not contain any BusinessEntities.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID);
            }
            else
            {
                return new ValidationMessage("Info to constraint C23.", "A BusinessEntityView MUST contain one to many BusinessEntities. \n\nThe BusinessEntitiyView " + bev.Name + " contains " + elements.Count + " BusinessEntities.", "BRV", ValidationMessage.errorLevelTypes.INFO, bev.PackageID);
            }

        }


        /// <summary>
        /// Check constraint C24
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC24(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());

            //Generate an info message for every business entity that is described by a UML State Diagram
            foreach (EA.Element businessEntity in elements)
            {

                int countStateDiagrams = 0;
                bool invalidDiagramDetected = false;

                foreach (EA.Diagram diagram in businessEntity.Diagrams)
                {
                    if (diagram.Type == "Statechart")
                    {
                        countStateDiagrams++;
                    }
                    else
                    {
                        //Invalid diagram detected
                        messages.Add(new ValidationMessage("Violation of constraint C24.", "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nInvalid diagram type detected under business entity " + businessEntity.Name + ".", "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                        invalidDiagramDetected = true;
                        break;
                    }
                }

                //In case no invalid diagram was detected check how often a State diagram occurred under a
                //given business entity
                if (!invalidDiagramDetected)
                {
                    //Error - more than one state diagram detected
                    if (countStateDiagrams > 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C24.", "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nInvalid number of UML state diagrams detected under business entity " + businessEntity.Name + ".", "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                    }
                    //No UML State Diagram detected - show a warning message
                    else if (countStateDiagrams == 0)
                    {
                        messages.Add(new ValidationMessage("Warning for constraint C24.", "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nBusiness entity " + businessEntity.Name + " is not described by a UML State Diagram.", "BRV", ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                    }
                    //No UML State Diagram detected - show a warning message
                    else
                    {
                        messages.Add(new ValidationMessage("Info for constraint C24.", "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nBusiness entity " + businessEntity.Name + " is described by a UML State Diagram.", "BRV", ValidationMessage.errorLevelTypes.INFO, bev.PackageID));
                    }

                }
            }
            return messages;

        }



        /// <summary>
        /// Check constraint C25
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC25(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());

            //Generate an info message for every business entity that is described by a UML State Diagram
            foreach (EA.Element businessEntity in elements)
            {

                //Is this BusinessEntity refined by a diagram?
                int countDiagram = 0;
                foreach (EA.Diagram diag in businessEntity.Diagrams)
                {
                    countDiagram++;
                }

                //This constraint only applies if there are UML state diagrams describing the lifecycle of a 
                //business entity
                if (countDiagram > 0)
                {


                    int countEntityStates = 0;
                    //There must be at least one business entity state 
                    foreach (EA.Element subelement in businessEntity.Elements)
                    {
                        if (subelement.Stereotype == UMM.bEState.ToString())
                        {
                            countEntityStates++;
                        }
                    }




                    //Raise an error if no entity state is found
                    if (countEntityStates < 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C25.", "A UML State Diagram describing the lifecycle of a BusinessEntity MUST contain one to many BusinessEntityStates. \n\nThe state diagram underneath the business entity " + businessEntity.Name + " does not contain BusinessEntityStates.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Info for constraint C25.", "A UML State Diagram describing the lifecycle of a BusinessEntity MUST contain one to many BusinessEntityStates. \n\nThe state diagram underneath the business entity " + businessEntity.Name + " has " + countEntityStates + " BusinessEntityStates.", "BRV", ValidationMessage.errorLevelTypes.INFO, bev.PackageID));
                    }
                }
            }


            return messages;
        }



        /// <summary>
        /// Check constraint C26
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC26(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev, new List<EA.Package>(), UMM.bDataV.ToString());

            //Get all subpackage of the the BusinessEntityView
            IList<EA.Package> allSubpackagesofBEV = Utility.getAllSubPackagesRecursively(bev, new List<EA.Package>());

            //Both counts must be the same, otherwise wrong packages have been found
            if (businessDataViews.Count != allSubpackagesofBEV.Count)
            {
                String wrongPackages = "";
                foreach (EA.Package wrongpackage in allSubpackagesofBEV)
                {
                    if (Utility.getStereoTypeFromPackage(wrongpackage) != UMM.bDataV.ToString())
                    {
                        wrongPackages += " : " + wrongpackage.Name;
                    }
                }

                messages.Add(new ValidationMessage("Violation of constraint C26.", "A BusinessEntityView MAY contain zero to many BusinessDataView that describe its conceptual design. \n\nThe following invalid packages have been found: " + wrongPackages, "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
            }

            return messages;
        }




        /// <summary>
        /// Check constraint C27
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC27(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev, new List<EA.Package>(), UMM.bDataV.ToString());


            //The parent package of every business data view must be a business entity view
            foreach (EA.Package businessDataView in businessDataViews)
            {
                EA.Package parent = repository.GetPackageByID(businessDataView.ParentID);
                if (Utility.getStereoTypeFromPackage(parent) != UMM.bEntityV.ToString())
                {
                    messages.Add(new ValidationMessage("Violation of constraint C27.", "The parent of a BusinessDataView MUST be a BusinessEntityView. \n\nBusinessDataView " + businessDataView.Name + " does not have a BusinessEntityView as parent.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                }
            }

            return messages;
        }


        /// <summary>
        /// Check constraint C28
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC28(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev, new List<EA.Package>(), UMM.bDataV.ToString());


            //A BusinessDataView should use a UML Class Diagram
            foreach (EA.Package businessDataView in businessDataViews)
            {
                foreach (EA.Diagram diagram in businessDataView.Diagrams)
                {
                    if (diagram.Type != "Logical")
                    {
                        messages.Add(new ValidationMessage("Warning for constraint C28.", "A BusinessDataView SHOULD use a UML Class Diagram to describe the conceptual design of a BusinessEntity. \n\nThe business data view " + businessDataView.Name + " uses a diagram type which is not recommended (" + diagram.Type + ").", "BRV", ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                    }
                }
            }

            return messages;
        }






        /// <summary>
        /// Check constraint C29
        /// </summary>
        /// <param name="bev"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC29(EA.Package bev)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev, new List<EA.Package>(), UMM.bDataV.ToString());


            //A BusinessDataView should contain one to many classes
            foreach (EA.Package businessDataView in businessDataViews)
            {
                int countClasses = 0;
                bool invalidfound = false;
                foreach (EA.Element subelement in businessDataView.Elements)
                {
                    if (subelement.Type == "Class")
                    {
                        countClasses++;
                    }
                    else
                    {
                        invalidfound = true;
                    }
                }

                //Classes found?
                if (countClasses < 1)
                {
                    messages.Add(new ValidationMessage("Warning for constraint C29.", "A BusinessDataView SHOULD contain one to many classes.\n\nBusinessDataView " + businessDataView.Name + " does not contain any classes.", "BRV", ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                }
                //Invalid elements found?
                if (invalidfound)
                {
                    messages.Add(new ValidationMessage("Warning for constraint C29.", "A BusinessDataView SHOULD contain one to many classes.\n\nBusinessDataView" + businessDataView.Name + " contains other elements than only classes.", "BRV", ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                }



            }

            return messages;
        }


        /// <summary>
        /// Check the tagged values oth e business entity view
        /// </summary>
        /// <param name="p"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessEntityView(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Check the TaggedValues of the bEntityV itself
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));

            //Get the BusinessDataViews (if any)
            IList<EA.Package> bdataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bDataV.ToString());
            foreach (EA.Package package in bdataViews)
            {
                messages.AddRange(new TaggedValueValidator(repository).validatePackage(package));
            }


            return messages;

        }



    }
}
