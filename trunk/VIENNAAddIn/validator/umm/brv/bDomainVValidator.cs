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
    class bDomainVValidator : AbstractValidator
    {


        public bDomainVValidator(EA.Repository repository, String scope)
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
                       
            //Get the BusinessDomainView package
            EA.Package p = repository.GetPackageByID(Int32.Parse(scope));
                        
            //Check the TaggedValues of the bDomainV
            messages.AddRange(checkTV_BusinessDomainViews(p));
            
            //Check C9
            List<ValidationMessage> vm9 = checkC9(p);
            if (vm9 != null && vm9.Count != 0) messages.AddRange(vm9);

            //Check C10
            List<ValidationMessage> vm10 = checkC10(p);
            if (vm10 != null && vm10.Count != 0) messages.AddRange(vm10);

            //Check C11
            List<ValidationMessage> vm11 = checkC11(p);
            if (vm11 != null && vm11.Count != 0) messages.AddRange(vm11);

            //Check C12
            List<ValidationMessage> vm12 = checkC12(p);
            if (vm12 != null && vm12.Count != 0) messages.AddRange(vm12);


            //Check C13
            List<ValidationMessage> vm13 = checkC13(p);
            if (vm13 != null && vm13.Count != 0) messages.AddRange(vm13);

            //Check C14
            List<ValidationMessage> vm14 = checkC14(p);
            if (vm14 != null && vm14.Count != 0) messages.AddRange(vm14);

            //Check C15
            List<ValidationMessage> vm15 = checkC15(p);
            if (vm15 != null && vm15.Count != 0) messages.AddRange(vm15);

            //Check C16
            List<ValidationMessage> vm16 = checkC16(p);
            if (vm16 != null && vm16.Count != 0) messages.AddRange(vm16);

            //Check C17
            List<ValidationMessage> vm17 = checkC17(p);
            if (vm17 != null && vm17.Count != 0) messages.AddRange(vm17);

            //Check C18
            List<ValidationMessage> vm18 = checkC18(p);
            if (vm18 != null && vm18.Count != 0) messages.AddRange(vm18);

            //Check C19
            List<ValidationMessage> vm19 = checkC19(p);
            if (vm19 != null && vm19.Count != 0) messages.AddRange(vm19);

            //Check C20
            List<ValidationMessage> vm20 = checkC20(p);
            if (vm20 != null && vm20.Count != 0) messages.AddRange(vm20);


            return messages;

        }


        /// <summary>
        /// Check constraint C9
        /// </summary>
        /// <returns></returns>
        private List<ValidationMessage> checkC9(EA.Package bdv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();
            int count = 0;

            foreach (EA.Package p in bdv.Packages)
            {
                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bArea.ToString())
                {
                    count++; 
                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C9.", "A BusinessDomainView MUST include one to many BusinessAreas. \n\nThe package " + p.Name + " has an invalid stereotype.", "BRV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }

            //Found less than 1 BusinessArea
            if (count < 1)
            {
                messages.Add(new ValidationMessage("Violation of constraint C9.", "A BusinessDomainView MUST include one to many BusinessAreas. \n\nThe package " + bdv.Name + " contains 0 BusinessAreas.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bdv.PackageID));
            }



            return messages;

        }




        /// <summary>
        /// Check constraint C10
        /// </summary>
        /// <returns></returns>
        private List<ValidationMessage> checkC10(EA.Package bdv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get a list with all the BusinessAreas in this BusinessDomainView
            IList<EA.Package> businessAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bdv, new List<EA.Package>(), UMM.bArea.ToString());

            foreach (EA.Package barea in businessAreas)
            {

                //Does the business area have subpackages?
                if (barea.Packages != null && barea.Packages.Count != 0)
                {

                    int countProcessArea = 0;
                    int countBusinessArea = 0;

                    //If so - there must only be BusinessAreas or ProcessAreas
                    foreach (EA.Package subpackage in barea.Packages)
                    {
                        String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                        if (!(stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString()))
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C10", "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + subpackage.Name + " has an invalid stereotype.", "BRV", ValidationMessage.errorLevelTypes.ERROR, subpackage.PackageID));
                        }
                        else if (stereotype == UMM.bArea.ToString())
                        {
                            countBusinessArea++;
                        }
                        else if (stereotype == UMM.ProcessArea.ToString())
                        {
                            countProcessArea++;
                        }
                    }

                    //There MUST be one to many bAreas or one to many ProcessAreas
                    if (countBusinessArea < 1 && countProcessArea < 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C10", "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + barea.Name + " contains " + countBusinessArea + " BusinessAreas and " + countProcessArea + " ProcessAreas.", "BRV", ValidationMessage.errorLevelTypes.ERROR, barea.PackageID));
                    }
                }
                //The given BusinessArea does not have any subpackages - it must have one BusinessProcessUseCase
                else
                {

                    int countBpuc = 0;

                    foreach (EA.Element element in barea.Elements)
                    {
                        if (element.Stereotype == UMM.bProcessUC.ToString())
                        {
                            countBpuc++;
                        }
                    }

                    if (countBpuc < 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C10", "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + barea.Name + " contains " + countBpuc + " BusinessProcessUseCases.", "BRV", ValidationMessage.errorLevelTypes.ERROR, barea.PackageID));
                    }

                }

            }

            return messages;
        }


        /// <summary>
        /// Check constraint C11
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC11(EA.Package bdv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();
            //Get a list with all the ProcessAreas in this BusinessDomainView
            IList<EA.Package> processAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bdv, new List<EA.Package>(), UMM.ProcessArea.ToString());

            foreach (EA.Package processArea in processAreas)
            {

                //Given process area has subpackages - check if one of them is a another process area
                if (processArea.Packages != null && processArea.Packages.Count != 0)
                {
                    int count_subProcessAreas = 0;
                    foreach (EA.Package subpackage in processArea.Packages)
                    {

                        String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                        if (stereotype == UMM.ProcessArea.ToString())
                        {
                            count_subProcessAreas++;
                        }
                        //The only subpackages allowed underneath a process area are other process areas
                        else
                        {
                            messages.Add(new ValidationMessage("Violoation of constraint C11.", "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + processArea.Name + " contains a package with an invalid stereotype (" + stereotype + ").", "BRV", ValidationMessage.errorLevelTypes.ERROR, processArea.PackageID));
                        }

                    }

                    //The given processarea has subpackages - there MUST be at least one process area underneath
                    if (count_subProcessAreas < 1)
                    {
                        messages.Add(new ValidationMessage("Violoation of constraint C11.", "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + processArea.Name + " contains subpackages but no ProcessArea could be found.", "BRV", ValidationMessage.errorLevelTypes.ERROR, processArea.PackageID));
                    }
                }
                else
                {
                    //No subpackages in this process area - hence there MUST be one to many BusinessProcessUseCases
                    int count_bpuc = 0;
                    foreach (EA.Element e in processArea.Elements)
                    {
                        if (e.Stereotype == UMM.bProcessUC.ToString())
                        {
                            count_bpuc++;
                        }
                    }

                    if (count_bpuc < 1)
                    {
                        messages.Add(new ValidationMessage("Violoation of constraint C11.", "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " + processArea.Name + " does neither contain ProcessAreas nor BusinessProcessUseCases.", "BRV", ValidationMessage.errorLevelTypes.ERROR, processArea.PackageID));
                    }
                }

            }

            return messages;

        }



        /// <summary>
        /// Check constraint C12
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC12(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Iterate over the business process use cases and check whether every use case is associated with one to many business partners
            foreach (EA.Element bpuc in bpucs)
            {
                int count_participatesAssocationsFound = 0;

                foreach (EA.Connector con in bpuc.Connectors)
                {

                    //Only associations or dependencies are allowed
                    if (!(con.Type == AssocationTypes.Association.ToString() || con.Type == AssocationTypes.Dependency.ToString()))
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C12", "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nInvalid connection (" + con.Type.ToString() + ") to BusinessProcessUseCase " + bpuc.Name + " detected.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                    //Correct participates assocation detected
                    else if (con.Stereotype == UMM.participates.ToString())
                    {
                        count_participatesAssocationsFound++;

                        EA.Element client = repository.GetElementByID(con.ClientID);
                        //Correct connection leading from a business partner to a business process use case
                        if (con.SupplierID == bpuc.ElementID)
                        {
                            //Client must be of type Business Partner
                            if (client.Stereotype != UMM.bPartner.ToString())
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C12", "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nThe particiaptes relationship must lead from a business partner to a business process use case.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                            }
                        }
                        else
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C12", "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nA participates relationship must lead from a business partner to a business process use case.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                        }
                    }
                    else if (con.Stereotype == UMM.isOfInterestTo.ToString())
                    {
                        //do nothing here - will be vaidated in later constraints
                    }
                    else
                    {
                        //Invalid connection stereotype
                        messages.Add(new ValidationMessage("Violation of constraint C12", "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nInvalid connector to BusinessProcessUseCase " + bpuc.Name + " detected.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }

                }

                //Does this BPUC has any particpates connections?
                if (count_participatesAssocationsFound < 1)
                {
                    messages.Add(new ValidationMessage("Violation of constraint C12", "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nNo participates assocations found.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                }


            }

            return messages;
        }





        /// <summary>
        /// Check constraint C13
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC13(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Iterate over the business process use cases and check if the business process use case is connected to stakeholders
            foreach (EA.Element bpuc in bpucs)
            {

                foreach (EA.Connector con in bpuc.Connectors)
                {

                    //Only associations or dependencies are allowed
                    if (!(con.Type == AssocationTypes.Association.ToString() || con.Type == AssocationTypes.Dependency.ToString()))
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C13", "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship. \n\nInvalid connection (" + con.Type.ToString() + ") to BusinessProcessUseCase " + bpuc.Name + " detected.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                    //Correct isOfInterestTo assocation detected
                    else if (con.Stereotype == UMM.isOfInterestTo.ToString())
                    {

                        EA.Element client = repository.GetElementByID(con.ClientID);
                        //Correct connection leading from a stakeholder to a business process use case
                        if (con.SupplierID == bpuc.ElementID)
                        {
                            //Client must be of type Stakeholder
                            if (client.Stereotype != UMM.Stakeholder.ToString())
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C13", "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship. \n\nThe isOfInterestTo relationship must lead from a stakeholder to a business process use case.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                            }
                        }
                        else
                        {
                            messages.Add(new ValidationMessage("Violation of constraint C13", "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship.  \n\nA participates relationship must lead from a stakeholder to a business process use case.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                        }
                    }
                    else if (con.Stereotype == UMM.participates.ToString())
                    {
                        //do nothing here - already validated in the last constraint
                    }
                    else
                    {
                        //Invalid connection stereotype
                        messages.Add(new ValidationMessage("Violation of constraint C13", "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship.  \n\nInvalid connector to BusinessProcessUseCase " + bpuc.Name + " detected.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }

                }


            }

            return messages;
        }


        /// <summary>
        /// Check constraint C14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC14(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            foreach (EA.Element bpuc in bpucs)
            {

                int parentPackageID = repository.GetPackageByID(bpuc.PackageID).PackageID;

                bool found = false;
                //Is the business process use case refined by a business process?
                foreach (EA.Element subelement in bpuc.Elements)
                {

                    if (subelement.Stereotype == UMM.bProcess.ToString())
                    {
                        found = true;
                        break;
                    }
                }

                //No bProcess found - show a warn message
                if (!found)
                {
                    messages.Add(new ValidationMessage("Warning for constraint C 14.", "A BusinessProcessUseCase SHOULD be refined by zero to many BusinessProcesses. \n\nThe BusinessProcessUseCase " + bpuc.Name + " is not refined by a BusinessProcess.", "BRV", ValidationMessage.errorLevelTypes.WARN, parentPackageID));
                }
                else
                {
                    messages.Add(new ValidationMessage("Info to constraint C 14.", "A BusinessProcessUseCase SHOULD be refined by zero to many BusinessProcesses. \n\nThe BusinessProcessUseCase " + bpuc.Name + " is refined by a BusinessProcess.", "BRV", ValidationMessage.errorLevelTypes.INFO, parentPackageID));
                }
            }

            return messages;
        }



        /// <summary>
        /// Check constraint C15
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC15(EA.Package bdv)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());

            foreach (EA.Element bp in bps)
            {

                //Does the business process have a parent?
                if (bp.ParentID != 0)
                {
                    EA.Element el = repository.GetElementByID(bp.ParentID);
                    if (el.Stereotype != UMM.bProcessUC.ToString())
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C15.", "A BusinessProcess MUST be modeled as a child of a BusinessProcessUseCase \n\nThe BusinessProcess " + bp.Name + " is not located underneath a BusinessProcessUseCase.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                    }
                }
                else
                {
                    messages.Add(new ValidationMessage("Violation of constraint C15.", "A BusinessProcess MUST be modeled as a child of a BusinessProcessUseCase \n\nThe BusinessProcess " + bp.Name + " is not located underneath a BusinessProcessUseCase.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                }

            }

            return messages;
        }



        /// <summary>
        /// Validate constraint C16
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC16(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Check which businessprocessusecase is refined by a UML Sequence Diagram
            foreach (EA.Element bpuc in bpucs)
            {
                int countSequenceDiagrams = 0;
                foreach (EA.Diagram diagram in bpuc.Diagrams)
                {
                    if (diagram.Type == "Sequence")
                    {
                        countSequenceDiagrams++;
                    }
                }

                if (countSequenceDiagrams > 0)
                {
                    messages.Add(new ValidationMessage("Info to constraint C16.", "A BusinessProcessUseCase MAY be refined by zero to many UML Sequence Diagrams. \n\nThe BusinessProcessUseCase " + bpuc.Name + " is refined by " + countSequenceDiagrams + " UML Sequence Diagrams.", "BRV", ValidationMessage.errorLevelTypes.INFO, bpuc.PackageID));
                }
            }


            return messages;

        }



        /// <summary>
        /// Check constraint C17
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC17(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contains Activity Partitions
            foreach (EA.Element bp in bps)
            {
                int countPartition = 0;
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        countPartition++;
                    }

                }

                messages.Add(new ValidationMessage("Info to constraint C17.", "A BusinessProcess MAY contain zero to many ActivityPartitions. \n\nThe BusinessProcess " + bp.Name + " contains " + countPartition + " ActivityPartitions.", "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));

            }


            return messages;
        }




        /// <summary>
        /// Check constraint C18
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC18(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contains Activity Partitions
            foreach (EA.Element bp in bps)
            {
                int countPartition = 0;
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        countPartition++;
                    }
                }

                //No partition found
                if (countPartition < 1)
                {
                    int countBusinessProcessActions = 0;
                    int countInternalStates = 0;
                    int countSharedStates = 0;

                    //The business process MUST contain one or more business process actions
                    foreach (EA.Element subelement in bp.Elements)
                    {
                        //Count the business process actions - there  MUST be actions
                        if (subelement.Stereotype == UMM.bProcessAction.ToString())
                        {
                            countBusinessProcessActions++;
                        }
                        //count the internal business entity states - there MAY be states
                        else if (subelement.Stereotype == UMM.bEInternalState.ToString())
                        {
                            countInternalStates++;
                        }
                        //count the shared business entity state - there MAY be states
                        else if (subelement.Stereotype == UMM.bESharedState.ToString())
                        {
                            countSharedStates++;
                        }
                    }

                    //No business process actions
                    if (countBusinessProcessActions < 1)
                    {
                        messages.Add(new ValidationMessage("Violation of constraint C18.", "A BusinessProcess, which has no ActivityPartitions, MUST contain one or more BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates and zero to many SharedBusinessEntityStates. \n\nNo BusinessProcessActions where found for the BusinessProcess " + bp.Name + ".", "BRV", ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                    }
                    else
                    {
                        messages.Add(new ValidationMessage("Info for constraint C18.", "A BusinessProcess, which has no ActivityPartitions, MUST contain one or more BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates and zero to many SharedBusinessEntityStates. \n\nThe BusinessProcess " + bp.Name + " contains " + countInternalStates + " InternalBusinessEntityStates and " + countSharedStates + " SharedBusinessEntityStates.", "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
                    }
                }

                messages.Add(new ValidationMessage("Info to constraint C17.", "A BusinessProcess MAY contain zero to many ActivityPartitions. \n\nThe BusinessProcess " + bp.Name + " contains " + countPartition + " ActivityPartitions.", "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));

            }

            return messages;
        }





        /// <summary>
        /// Check constraint C19
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC19(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();


            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contain Activity Partitions
            foreach (EA.Element bp in bps)
            {


                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        int countBusinessProcessActions = 0;
                        int countInternalStates = 0;

                        //Activity Parition found -                         
                        foreach (EA.Element partitionSubElement in subelement.Elements)
                        {
                            //It MUST contain one to many BusinessProcessActions
                            if (partitionSubElement.Stereotype == UMM.bProcessAction.ToString())
                            {
                                countBusinessProcessActions++;
                            }
                            //It MAY contain SharedEntityStates
                            else if (partitionSubElement.Stereotype == UMM.bEInternalState.ToString())
                            {
                                countInternalStates++;
                            }
                        }

                        //No BusinessProcessActions found - error
                        if (countBusinessProcessActions < 1)
                        {
                            messages.Add(new ValidationMessage("Violoation of constraint C19.", "An ActivityPartition being part of a BusinessProcess MUST contain one to many BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates. \n\nNo BusinessProcessActions where found in the ActivityPartition " + subelement.Name + " of the BusinessProcess " + bp.Name, "BRV", ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                        }
                        //BusinessProcessActions found - report an INFO
                        else
                        {
                            messages.Add(new ValidationMessage("Info for constraint C19.", "An ActivityPartition being part of a BusinessProcess MUST contain one to many BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates. \n\nThe ActivityParition " + subelement.Name + " of the BusinessProcess " + bp.Name + " contains " + countBusinessProcessActions + " BusinessProcessActions and " + countInternalStates + " InternalBusinessEntityStates.", "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
                        }
                    }
                }


            }

            return messages;
        }




        /// <summary>
        /// Check constraint C20
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkC20(EA.Package bdv)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contain Activity Partitions
            foreach (EA.Element bp in bps)
            {


                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        //An ActivityParition MUST not contain SharedBusinessEntityStates
                        foreach (EA.Element partitionElement in subelement.Elements)
                        {
                            if (partitionElement.Stereotype == UMM.bESharedState.ToString())
                            {
                                messages.Add(new ValidationMessage("Violation of constraint C20.", "A SharedBusinessEntityStates MUST NOT be located in an ActivityPartition. (They must be contained within the BusinessProcess even if this BusinessProcess contains ActivityPartitions.) \n\nSharedBusinessEntityState " + partitionElement.Name + " is locatdd in an ActivityPartition.", "BRV", ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                            }
                        }
                    }
                }
            }

            return messages;

        }




        /// <summary>
        /// Checks the TV of the Stereotypes from the Business Domain View
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<ValidationMessage> checkTV_BusinessDomainViews(EA.Package p)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Get all BusinessAreas
            IList<EA.Package> bAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bArea.ToString());
            //Get all ProcessAreas
            IList<EA.Package> pAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.ProcessArea.ToString());

            //Check the TaggedValues of the BusinessDomainView package
            messages.AddRange(new TaggedValueValidator(repository).validatePackage(p));

            //Check all BusinessAreas
            foreach (EA.Package bArea in bAreas)
            {
                messages.AddRange(new TaggedValueValidator(repository).validatePackage(bArea));
            }

            //Check all ProcressAreas
            foreach (EA.Package pArea in pAreas)
            {
                messages.AddRange(new TaggedValueValidator(repository).validatePackage(pArea));
            }

            //Get all BusinessProcessUseCases recursively from the Business Domain View
            IList<EA.Element> bPUCs = Utility.getAllElements(p, new List<EA.Element>(), UMM.bProcessUC.ToString());            
            List<EA.Connector> connectors = new List<EA.Connector>();
            foreach (EA.Element bPUC in bPUCs)
            {
                messages.AddRange(new TaggedValueValidator(repository).validateElement(bPUC));
                foreach (EA.Connector con in bPUC.Connectors)
                {
                    connectors.Add(con);
                }
            }

            
            //Validate the participates and isOfInterestTo connectors
            messages.AddRange(new TaggedValueValidator(repository).validateConnectors(connectors));


                        
            return messages;
        }









    }
}
