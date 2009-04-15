/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using EA;
using VIENNAAddIn.constants;
using VIENNAAddIn.Settings;
using VIENNAAddIn.validator;

namespace VIENNAAddIn.common

{
    internal class Utility 
    {

        internal static bool DEBUG;


        
        /// <summary>
        /// Returns true if the list of validationmessage does not contain any ERRORS
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        internal static bool canProceedWithValidation(List<ValidationMessage> messages)
        {
            if (messages != null)
            {
                foreach (ValidationMessage vm in messages)
                {
                    if (vm.ErrorLevel == ValidationMessage.errorLevelTypes.ERROR)
                        return false;
                }
            }
            return true;

        }







        /// <summary>
        /// Returns true if the given stereotype is a valid UMM stereotype
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static bool isValidUMMStereotype(String s)
        {

            foreach (UMM u in Enum.GetValues(typeof(UMM)))
            {
                if (u.ToString() == s)
                {
                    return true;
                }
            }
            
            return false;
        }



        /// <summary>
        /// Returns true if the given stereotype is a valid UPCC stereotype
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static bool isValidUPCCStereotype(String s)
        {

            foreach (UPCC u in Enum.GetValues(typeof(UPCC)))
            {
                if (u.ToString() == s)
                {
                    return true;
                }
            }

            return false;
        }



        

        /// <summary>
        /// Returns the stereotype associated with a package
        /// if there is no stereotype associated with the given 
        /// package, "" is returned
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        internal static String getStereoTypeFromPackage(EA.Package package)
        {
            if (package.Element != null)
                return package.Element.Stereotype;
            else
                return "";
        }


        /// <summary>
        /// retrieves recursivly all packages of a given stereotype from the startingpoint package
        /// 
        internal static IList<EA.Package> getAllSubPackagesWithGivenStereotypeRecursively(EA.Package package, IList<EA.Package> resultList, String stereotype)
        {
            foreach (EA.Package p in package.Packages)
            {
                if (getStereoTypeFromPackage(p) == stereotype)
                {
                    resultList.Add(p);
                }
                getAllSubPackagesWithGivenStereotypeRecursively(p, resultList, stereotype);
            }
            return resultList;
        }


        /// <summary>
        /// retrieves recursivly all packages of a given stereotype from the startingpoint package
        /// 
        internal static IList<EA.Package> getAllSubPackagesRecursively(EA.Package package, IList<EA.Package> resultList)
        {
            foreach (EA.Package p in package.Packages)
            {
                resultList.Add(p);
                getAllSubPackagesRecursively(p, resultList);
            }
            return resultList;
        }


        /// <summary>
        /// Retrieves the element with the given stereotype from the package
        /// </summary>
        /// <param name="?"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static EA.Element getElementFromPackage(EA.Package p, String stereotype) {

            foreach (EA.Element e in p.Elements)
            {
                String d = e.Stereotype;
                if (e.Stereotype.ToString().Equals(stereotype))
                {
                    return e;
                }
            }
            return null;
        }


       





        /// <summary>
        /// retrieves recursivly elements with a given stereotype
        /// </summary>
        internal static Dictionary<Int32, EA.Element> getAllElements(EA.Package package, Dictionary<Int32, EA.Element> resultList, String stereotype)
        {
            foreach (EA.Element e in package.Elements)
            {
                if (e.Stereotype.Equals(stereotype))
                {
                    resultList.Add(e.ElementID, e);
                }
            }
            foreach (EA.Package p in package.Packages)
            {
                getAllElements(p, resultList, stereotype);
            }
            return resultList;
        }


        /// <summary>
        /// Determines whether a BDT or an ABIE is qualified or not
        /// If the name of the element contains an underscore character, the element is qualified
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        internal static bool isAQualifiedElement(String elementName)
        {
            if (elementName.Contains("_"))
                return true;


            return false;
        }



        /// <summary>
        /// Returns all classes of all subpackages of the given package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        internal static IList<EA.Element> getAllClasses(EA.Package package, IList<EA.Element> resultList) {

            foreach (EA.Element e in package.Elements)
            {
                if (e.Type == "Class")
                {
                    resultList.Add(e);
                }
            }
            foreach (EA.Package p in package.Packages)
            {
                getAllClasses(p, resultList);
            }
            return resultList;




        }



        /// <sUMM2ary>
        /// This methods counts everything (packages, elements, diagrams) within a given package
        /// Excluded are elements/packages/diagrams with parentid = fatherid
        /// </sUMM2ary>
        /// <returns></returns>
        internal static int countAllWithinAPackage(EA.Package p, int fatherid)
        {
            int i = 0;

            foreach (EA.Element e in p.Elements)
            {
                if (e.ParentID != fatherid)
                    i++;
            }

            foreach (EA.Package pa in p.Packages)
            {
                if (pa.ParentID != fatherid)
                    i++;
            }

            foreach (EA.Diagram d in p.Diagrams)
            {
                if (d.ParentID != fatherid)
                    i++;
            }

            return i;
        }


        /// <sUMM2ary>
        /// This methods counts the occurence of packages stereotyped as "stereotype" within the
        /// package container
        /// </sUMM2ary>
        /// <param name="container"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static int countPackagesWithinAPackage(EA.Package container, String stereotype)
        {
            int i = 0;

            foreach (EA.Package p in container.Packages)
            {
                if (p.Element.Stereotype != null && p.Element.Stereotype == stereotype)
                    i++;
            }

            return i;
        }




        /// <sUMM2ary>
        /// Get the diagrams which are stereotyped "stereotype" from the package
        /// which ID = scope
        /// </sUMM2ary>
        /// <param name="scope">The PackageID, whose diagrams should be retrieved</param>
        /// <param name="repo">the model</param>
        /// <param name="stereotype">the stereotype a diagram must have to be in the 
        /// resultlist</param>
        /// <returns>List with all diagrams from a given package with a given stereotype</returns>
        internal static IList getDiagramsFromPackage(EA.Repository repo, String scope, String stereotype)
        {
            IList diagrams = new ArrayList();


            //Get the package
            try
            {
                int packageID = Int32.Parse(scope);
                EA.Package p = repo.GetPackageByID(packageID);
                foreach (EA.Diagram diag in p.Diagrams)
                {
                    if (diag.Stereotype.ToString() == stereotype)
                    {
                        diagrams.Add(diag);
                    }

                }
            }
            catch (Exception e) { }

            return diagrams;
        }



         
        /// <sUMM2ary>
        /// retrieves all packages of the first level of a model which correspond to a 
        /// given stereotype.
        /// </sUMM2ary>
        /// <param name="packageStereotype">the stereotype a package must have, to be in the
        /// result list</param>
        /// <param name="repository">the model</param>
        /// <returns>all packages on the top level</returns>
        internal static ArrayList getRootViewPackages(EA.Repository repository, UMM packageStereotype)
        {
            ArrayList packageList = new ArrayList();
            EA.Package model = (EA.Package)repository.Models.GetAt(0);
            EA.Collection rootPackages = model.Packages;
            foreach (EA.Package p in rootPackages)
            {
                String s = p.Element.Stereotype;
                if (s.Equals(packageStereotype.ToString()))
                {
                    packageList.Add(p);
                }
            }
            return packageList;
        }

        /// <sUMM2ary>
        /// retrieves all packages of the first level of a model which correspond to a 
        /// given stereotype.
        /// </sUMM2ary>
        /// <param name="packageStereotype">the stereotype a package must have, to be in the
        /// result list</param>
        /// <param name="repository">the model</param>
        /// <returns>all packages on the top level</returns>
        internal static ArrayList getRootViewPackages(EA.Repository repository, String packageStereotype)
        {
            ArrayList packageList = new ArrayList();
            EA.Package model = (EA.Package)repository.Models.GetAt(0);
            EA.Collection rootPackages = model.Packages;
            foreach (EA.Package p in rootPackages)
            {
                String s = p.Element.Stereotype;
                if (s.Equals(packageStereotype))
                {
                    packageList.Add(p);
                }
            }
            return packageList;
        }


        /// <sUMM2ary>
        /// Retrieves all diagrams contained in a package or in the 
        /// subpackages of this package.
        /// </sUMM2ary>
        /// <param name="package">The package where to start searching
        /// for diagrams recursively.
        /// </param>
        /// <param name="resultList">The resulting list. this resultlist must be given
        /// as an argument, because the method is used iteratively
        /// </param>
        /// <param name="stereotype">either a given stereotype if only specific diagrams
        /// should be retrieved, or null if all packages should be retrieved</param>
        /// its subpackages</returns>
        private static IList getAllDiagrams(EA.Package package, IList resultList, String stereotype)
        {
            if (package.Diagrams.Count > 0)
            {
                foreach (EA.Diagram d in package.Diagrams)
                {
                    if (stereotype == null)
                    {
                        resultList.Add(d);
                    }
                    else if (d.Stereotype.Equals(stereotype))
                    {
                        resultList.Add(d);
                    }
                }
            }
            foreach (EA.Package subPackage in package.Packages)
            {
                getAllDiagrams(subPackage, resultList, stereotype);
            }
            return resultList;

        }
        /// <sUMM2ary>
        /// retrieves recursivly all packages
        /// </sUMM2ary>
        /// <param name="package">the package to start from</param>
        /// <param name="resultList">list of package ID</param>
        /// <param name="stereotype">either a given stereotype if only specific packages
        /// should be retrieved, or null if all packages should be retrieved</param>
        /// <returns>a List containing all package id that correspond to the given
        /// criteria (stereotype)</returns>
        internal static IList getAllSubPackagesRecursively2(EA.Package package, IList resultList, String stereotype)
        {
            foreach (EA.Package p in package.Packages)
            {
                if (p.Element.Stereotype.Equals(stereotype) || stereotype == null)
                {
                    resultList.Add(p.PackageID.ToString());
                }
                getAllSubPackagesRecursively2(p, resultList, stereotype);
            }
            return resultList;
        }

        /// <sUMM2ary>
        /// retrieves recursivly all packages
        /// </summary>
        /// <param name="package">the package to start from</param>
        /// <param name="resultList">list to which the found packages should be added</param>
        /// <param name="stereotype">either a given stereotype if only specific packages
        /// should be retrieved, or null if all packages should be retrieved</param>
        /// <returns>a List containing all packages that correspond to the given
        /// criteria (stereotype)</returns>
        internal static IList getAllSubPackagesRecursively(EA.Package package, IList resultList, String stereotype)
        {
            foreach (EA.Package p in package.Packages)
            {
                if (p.Element.Stereotype.Equals(stereotype) || stereotype == null)
                {
                    resultList.Add(p);
                }
                getAllSubPackagesRecursively(p, resultList, stereotype);
            }
            return resultList;
        }


        /// <summary>
        /// retrieves recursivly all packages with a given name and a given stereotype
        /// </sUMM2ary>
        /// <param name="package">the package to start from</param>
        /// <param name="resultList">list to which the found packages should be added</param>
        /// <param name="name">then name of the subpackage to be returned</param>
        /// <returns>a List containing all packages that correspond to the given
        /// criteria (name)</returns>
        internal static IList getAllSubPackagesWithSpecificNameRecursively(EA.Package package, IList resultList, String name, String stereotype)
        {
            String k = package.Name;
            foreach (EA.Package p in package.Packages)
            {
                String s = p.Name;
                if (p.Name.Equals(name) && p.Element.Stereotype != null && p.Element.Stereotype == stereotype)
                {
                    resultList.Add(p);
                }
                getAllSubPackagesWithSpecificNameRecursively(p, resultList, name, stereotype);
            }
            return resultList;
        }




        /// <sUMM2ary>
        /// for getting the package which is stereotyped as BusinessTransactionView
        /// </sUMM2ary>
        /// <returns>the BTV package</returns>
        internal static EA.Package getBusinessTransactionView(EA.Repository repository)
        {
            EA.Package root = (EA.Package)repository.Models.GetAt(0);
            foreach (EA.Package p in root.Packages)
            {
                if (p.Element.Stereotype.Equals(UMM.bTransactionV.ToString()))
                {
                    return p;
                }
            }
            return null;
        }


        /// <sUMM2ary>
        /// retrieves recursivly elements with a given stereotype or all elements
        /// if no stereotype (null) is given
        /// </sUMM2ary>
        /// <param name="package">the package to start from</param>
        /// <param name="resultList">list to which the found elements should be added</param>
        /// <param name="stereotype">either a given stereotype if only specific elements
        /// should be retrieved, or null if all elements should be retrieved</param>
        /// <returns>a List containing all elements that correspond to the given
        /// criteria (stereotype)</returns>
        internal static IList<EA.Element> getAllElements(EA.Package package, IList<EA.Element> resultList, String stereotype)
        {
            foreach (EA.Element e in package.Elements)
            {
                if (e.Stereotype.Equals(stereotype) || stereotype == null)
                {
                    resultList.Add(e);
                }
            }
            foreach (EA.Package p in package.Packages)
            {
                getAllElements(p, resultList, stereotype);
            }
            return resultList;
        }


        /// <sUMM2ary>
        /// Retrieves BusinessCollaborationProtocols from a given repository
        /// </sUMM2ary>
        /// <param name="repository">the model</param>
        /// <returns>List containing all BusinessCollaborationProtocols</returns>
        internal static IList getBusinessCollaborationProtocols(EA.Repository repository)
        {
            ArrayList btvList = Utility.getRootViewPackages(repository, UMM.bTransactionV.ToString());
            ArrayList allBTVs = new ArrayList();
            foreach (EA.Package btv in btvList)
            {
                allBTVs.AddRange(Utility.getAllDiagrams(btv, new ArrayList(), UMM.bCollaborationProtocol.ToString()));
            }
            return allBTVs;
        }


        /// <sUMM2ary>
        /// Retrieves all BusinessEntitiyView from a given model
        /// </sUMM2ary>
        /// <param name="repository">the model</param>
        /// <returns>List containing all BusinessEntityViews</returns>
        internal static IList getBusinessEntityViews(EA.Repository repository)
        {
            ArrayList brvList = Utility.getRootViewPackages(repository, UMM.bRequirementsV);
            ArrayList bevList = new ArrayList();
            foreach (EA.Package brv in brvList)
            {
                bevList.AddRange(Utility.getAllSubPackagesRecursively(brv, new ArrayList(), UMM.bEntityV.ToString()));
            }
            return bevList;
        }
        /// <sUMM2ary>
        /// Retrieves all BusinessEntities from a given model
        /// </sUMM2ary>
        /// <param name="repository">the model</param>
        /// <returns>List containing all BusinessEntities</returns>
        internal static IList getBusinessEntities(EA.Repository repository)
        {
            IList businessEntityViews = getBusinessEntityViews(repository);
            ArrayList beList = new ArrayList();
            foreach (EA.Package bev in businessEntityViews)
            {
                foreach (EA.Element possibleBE in bev.Elements)
                {
                    if (possibleBE.Stereotype.Equals(UMM.bEntity.ToString()))
                    {
                        beList.Add(possibleBE);
                    }
                }
            }
            return beList;
        }

        /// <sUMM2ary>
        /// Get all diagrams from a package which ID = scope
        /// </sUMM2ary>
        /// <param name="scope">The PackageID of the scope we want to validate</param>
        /// <returns>a List with all diagrams contained in this package</returns>
        internal static IList getAllDiagramsFromPackage(EA.Repository repo, String scope)
        {
            IList diagrams = new ArrayList();
            //Get the package
            try
            {
                int packageID = Int32.Parse(scope);
                EA.Package p = repo.GetPackageByID(packageID);
                foreach (EA.Diagram diag in p.Diagrams)
                {
                    diagrams.Add(diag);
                }
            }
            catch (Exception e) { }

            return diagrams;
        }


        /// <sUMM2ary>
        /// Returns true, if the EA.Element elem is child of one of the partitions
        /// in the Collection partitions
        /// </sUMM2ary>
        /// <param name="elem"></param>
        /// <param name="partitions"></param>
        /// <returns></returns>
        internal static bool isChildOfPartition(EA.Element elem, IList partitions)
        {
            bool rv = false;

            foreach (EA.Element e in partitions)
            {
                if (elem.ParentID == e.ElementID)
                {
                    rv = true;
                    break;
                }
            }

            return rv;
        }



        /// <sUMM2ary>
        /// retrieves all elements from a given package
        /// </sUMM2ary>
        /// <param name="repo">the repository</param>
        /// <param name="scope">the package referenced by its ID</param>
        /// <returns>a list with all elements from the package with the given ID</returns>
        internal static IList getAllElementsFromPackage(EA.Repository repo, String scope)
        {
            IList elements = new ArrayList();
            int packageID = Int32.Parse(scope);
            EA.Package package = repo.GetPackageByID(packageID);
            foreach (EA.Element element in package.Elements)
            {
                elements.Add(element);
            }
            return elements;
        }

        /// <sUMM2ary>
        /// retrieves all elements from a given package
        /// </sUMM2ary>
        /// <param name="repo">the repository</param>
        /// <param name="scope">the package</param>
        /// <returns>a list with all elements from the package with the given ID</returns>
        internal static IList getAllElementsFromPackage(EA.Repository repo, EA.Package package)
        {
            IList elements = new ArrayList();
            foreach (EA.Element element in package.Elements)
            {
                elements.Add(element);
            }
            return elements;
        }




        /// <sUMM2ary>
        /// returns all packages from a given package
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        internal static IList getAllPackagesFromPackage(EA.Repository repo, String scope)
        {
            IList packages = new ArrayList();
            int packageID = Int32.Parse(scope);
            EA.Package package = repo.GetPackageByID(packageID);
            foreach (EA.Package subPackage in package.Packages)
            {
                packages.Add(subPackage);
            }
            return packages;
        }

        /// <sUMM2ary>
        /// retrieves elements from a given package with have a specific stereotype
        /// </sUMM2ary>
        /// <param name="repo">the repository</param>
        /// <param name="stereotype">the stereotype of the elements which should be retrieved</param>
        /// <param name="scope">the package referenced by its ID</param>
        /// <returns>a list with elements with a specific stereotype
        /// from the package with the given ID</returns>
        internal static IList getElementsFromPackage(EA.Repository repo, String scope, String stereotype)
        {
            IList elements = new ArrayList();
            int packageID = Int32.Parse(scope);
            EA.Package package = repo.GetPackageByID(packageID);
            foreach (EA.Element element in package.Elements)
            {
                if (element.Stereotype.Equals(stereotype))
                {
                    elements.Add(element);
                }
            }
            return elements;
        }

        /// <sUMM2ary>
        /// retrieves elements from the given package which are of the given type
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="scope"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IList getElementsOfGivenTypeFromPackage(EA.Repository repo, String scope, String type)
        {
            IList elements = new ArrayList();
            int packageID = Int32.Parse(scope);
            EA.Package p = repo.GetPackageByID(packageID);
            foreach (EA.Element e in p.Elements)
            {
                if (e.Type == type)
                    elements.Add(e);
            }
            return elements;
        }



        /// <sUMM2ary>
        /// Returns all Partitions from a given package
        /// </sUMM2ary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static IList getPartitionsFromPackage(EA.Package p)
        {
            ArrayList l = new ArrayList();
            foreach (EA.Element e in p.Elements)
            {
                if (e.Type.Equals(EA_Element.ActivityPartition.ToString()))
                    l.Add(e);
            }
            return l;
        }


        /// <sUMM2ary>
        /// Returns true, if the Element e is a Child of a BusinessEntity, contained in the
        /// Collection col
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        internal static bool isChildOfBusinessEntity(EA.Element e, EA.Collection col)
        {
            bool rv = false;
            foreach (EA.Element elem in col)
            {
                if (elem.Stereotype == UMM.bEntity.ToString() && e.ParentID == elem.ElementID)
                {
                    rv = true;
                    break;
                }
            }
            return rv;
        }


        /// <sUMM2ary>
        /// Returns true if the given Element is a PseudoState
        /// (InitialState, Choice, Fork, Join) or a FinalState
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static bool isPseudoStateOrFinalState(EA.Element e)
        {
            bool rv = false;
            String type = e.Type;

            //Synchronisation = Fork/Join
            if (type == "Synchronization" || type == "Decision")
            {
                rv = true;
            }
            //InitialState and FinalState
            else if (type == "StateNode")
            {
                //When using StateNodes from the StateDiagram in an Activity
                //diagram, the subtype functionality does not work
                //Therefore we do not check for subtypes of StateNodes

                //int subtype = e.Subtype;
                //if (subtype == 100 || subtype == 101 || subtype == 4)
                rv = true;
            }


            return rv;
        }



        /// <sUMM2ary>
        /// Returns true if the given Element is a PseudoState
        /// (InitialState, Choice, Fork, Join) or a FinalState
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static bool isPseudoStateOrFinalStateInStateChart(EA.Element e)
        {
            bool rv = false;

            //Synchronisation = Fork/Join
            if (e.Type == "Synchronization")
            {
                rv = true;
            }

                //Although not specified in the EA API,
            //it seems, that an InitalState of a StateDiagram has subtype 3
            //and a FinalState 4, a Choice 11
            else if (e.Type == "StateNode")
            {
                if (e.Subtype == 4 || e.Subtype == 3 || e.Subtype == 11)
                    rv = true;
            }


            return rv;
        }

        /// <sUMM2ary>
        /// Returns true, if the collection contains an element with the same name as the element
        /// e
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <param name="elementList"></param>
        /// <returns></returns>
        internal static bool containsActorWithTheSameName(EA.Element e, IList elementList)
        {
            bool rv = false;

            foreach (EA.Element element in elementList)
            {
                if (element.Name == e.Name)
                {
                    rv = true;
                    break;
                }
            }

            return rv;
        }

        /// <sUMM2ary>
        /// Checks if the roles of the target use case are the supplier of a mapsTo dependency
        /// from a role of the source use case
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        internal static bool doesRoleOfUseCaseMapsToRoleOfSourceUseCase(EA.Element e, IList roles, EA.Repository repo)
        {
            bool rv = true;

            //Get the roles of the target use case
            IList rolesOfTargetUC = new ArrayList();
            foreach (EA.Connector con in e.Connectors)
            {
                EA.Element client = repo.GetElementByID(con.ClientID);
                EA.Element supplier = repo.GetElementByID(con.SupplierID);
                if (con.Type == AssociationTypes.Association.ToString() &&
                    con.Stereotype == UMM.participates.ToString())
                { 
                    if (client.ElementID == e.ElementID)
                        rolesOfTargetUC.Add(supplier);
                    else if (supplier.ElementID == e.ElementID)
                        rolesOfTargetUC.Add(client);
                }
            }


            //Iterate through the roles of the target use case and check if each role has
            //at least one mapsTo dependency to the role of the BCUC
            int dependencyCount = 0;
            //iterate through the elements of the target UC
            foreach (EA.Element element in rolesOfTargetUC)
            {
                //iterate through the roles of the BCUC
                dependencyCount = 0;
                foreach (EA.Element bcrole in roles)
                {
                    foreach (EA.Connector con in bcrole.Connectors)
                    {
                        if (con.Stereotype == UMM.mapsTo.ToString())
                        {
                            EA.Element client = repo.GetElementByID(con.ClientID);
                            EA.Element supplier = repo.GetElementByID(con.SupplierID);
                            if (client.ElementID == bcrole.ElementID &&
                                supplier.ElementID == element.ElementID)
                            {
                                dependencyCount++;
                            }
                        }
                    }
                }
                //There must be at least one connection
                if (dependencyCount < 1)
                {
                    rv = false;
                    break;
                }
            }

            return rv;
        }
        /// <sUMM2ary>
        /// returns the ID of the other object that is connected with a given source object
        /// via an arbitrary relationship 
        /// </sUMM2ary>
        /// <param name="con">the connector</param>
        /// <param name="firstObjectId">the source object</param>
        /// <returns>the EA element ID of the object that is connected with the source object</returns>
        internal static int getOtherEndId(EA.Connector con, int firstObjectId)
        {
            int associatedObjectId;
            /* if the ClientID of the connector is the id of the element itself,
             * the associatedObject must be the supplier, otherwise it must be 
             * the client */
            if (con.ClientID == firstObjectId)
            {
                associatedObjectId = con.SupplierID;
            }
            else
            {
                associatedObjectId = con.ClientID;
            }
            return associatedObjectId;
        }


        /// <sUMM2ary>
        /// Returns a List of all packages with the passed name which are in the 
        /// package package
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static ArrayList getPackagesWithSpecificName(EA.Package package, String name)
        {
            ArrayList l = new ArrayList();
            foreach (EA.Package p in package.Packages)
            {
                if (p.Name == name)
                {
                    l.Add(p);
                }
            }

            return l;
        }


        /// <sUMM2ary>
        /// Searches the package with the ID parentPackageID for subpackages with the
        /// stereotype "stereotype"
        /// If there are multiple occurences of subpackages with the stereotype "stereotype" the
        /// first one is returned
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="parentPackageID"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static EA.Package getSubPackageFromPackage(EA.Repository repo, int parentPackageID, String stereotype)
        {
            EA.Package p = null;

            EA.Package parentPackage = repo.GetPackageByID(parentPackageID);
            foreach (EA.Package subpackage in parentPackage.Packages)
            {
                if (subpackage.Element.Stereotype == stereotype)
                {
                    p = subpackage;
                    break;
                }
            }

            return p;
        }


        /// <sUMM2ary>
        /// Searches the package with the ID parentPackageID for subpackages with the
        /// stereotype "stereotype" and returns a Collection of the packages found
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="parentPackageID"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static ArrayList getSubPackageFromPackage(EA.Repository repo, EA.Package package, String stereotype)
        {
            ArrayList list = new ArrayList();

            foreach (EA.Package subpackage in package.Packages)
            {
                if (subpackage.Element.Stereotype == stereotype)
                {
                    list.Add(subpackage);
                }
            }

            return list;
        }

        /// <sUMM2ary>
        /// returns all elements which are connetcted to a certain element via a specific stereotype
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="root"></param>
        /// <param name="stereotypeOfAllowedConnector"></param>
        /// <returns></returns>
        internal static ArrayList getConnectedElements(EA.Repository repo, EA.Element root, String stereotypeOfAllowedConnector)
        {
            ArrayList list = new ArrayList();

            //search all connectors with a stereotype participates
            foreach (EA.Connector c in root.Connectors)
            {
                if (c.Stereotype.Equals(stereotypeOfAllowedConnector))
                {
                    if (c.ClientID != root.ElementID) list.Add(repo.GetElementByID(c.ClientID));
                    else list.Add(repo.GetElementByID(c.SupplierID));
                }
            }

            return list;
        }

        /// <sUMM2ary>
        /// returns all elements which are connetcted to a certain element via a specific stereotype
        /// AND serve as a client element
        /// </sUMM2ary>
        /// <param name="repo"></param>
        /// <param name="root"></param>
        /// <param name="stereotypeOfAllowedConnector"></param>
        /// <returns></returns>
        internal static ArrayList getConnectedSupplier(EA.Repository repo, EA.Element root, String stereotypeOfAllowedConnector)
        {
            ArrayList list = new ArrayList();

            //search all connectors with a stereotype participates
            foreach (EA.Connector c in root.Connectors)
            {
                if (c.Stereotype.Equals(stereotypeOfAllowedConnector))
                {
                    if (c.ClientID == root.ElementID) list.Add(repo.GetElementByID(c.SupplierID));

                }
            }

            return list;
        }



        /// <sUMM2ary>
        /// Returns the ABIE package from the CCLibrary
        /// If the package does not exist a new one is created
        /// </sUMM2ary>
        /// <param name="ccLibrary"></param>
        /// <returns></returns>
        internal static EA.Package getABIELibrary(EA.Repository repo, EA.Package ccLibraryPackage)
        {
            EA.Package p = null;


            ////try to locate the ABIE package
            //foreach (EA.Package s in ccLibraryPackage.Packages) {
            //    if (s.Element.Stereotype.ToString() == CCTS_Types.ABIELibrary.ToString()) {
            //        p = s;
            //        break;
            //    }
            //}

            ////no ABIE package found - create a new one
            //if (p == null) {
            //    p = (EA.Package)ccLibraryPackage.Packages.AddNew("ABIE Library", "");
            //    p.Update();
            //    p.Element.Stereotype = CCTS_Types.ABIELibrary.ToString();
            //    p.Update();
            //    ccLibraryPackage.Update();
            //    ccLibraryPackage.Packages.Refresh();
            //    repo.Models.Refresh();
            //}


            return p;
        }



        /// <sUMM2ary>
        /// retrieves a tagged value from a package
        /// </sUMM2ary>
        /// <param name="element">the package to which the seeked tagged value belongs</param>
        /// <param name="nameOfTV">the name of the tagged value that is seeked</param>
        /// <returns>the tagged value object if a tagged value with the given name is found, null
        /// if the tagged value cannot be found</returns>
        internal static EA.TaggedValue getTaggedValue(EA.Package package, String nameOfTV)
        {
            foreach (EA.TaggedValue tv in package.Element.TaggedValuesEx)
            {
                String s = tv.Name;
                if (tv.Name.Equals(nameOfTV))
                {
                    return tv;
                }
            }
            return null;
        }



        /// <sUMM2ary>
        /// retrieves a tagged value from a connector
        /// </sUMM2ary>
        /// <param name="element">the connector to which the seeked tagged value belongs</param>
        /// <param name="nameOfTV">the name of the tagged value that is seeked</param>
        /// <returns>the tagged value object if a tagged value with the given name is found, null
        /// if the tagged value cannot be found</returns>
        internal static EA.IConnectorTag getTaggedValue(EA.Connector connector, String nameOfTV)
        {

            foreach (EA.IConnectorTag tv in connector.TaggedValues)
            {
                String s = tv.Name.ToString();
                if (tv.Name.Equals(nameOfTV))
                {
                    
                    return tv;
                }
            }
            return null;
        }

        /// <sUMM2ary>
        /// retrieves a tagged value from an element
        /// </sUMM2ary>
        /// <param name="element">the element to which the seeked tagged value belongs</param>
        /// <param name="nameOfTV">the name of the tagged value that is seeked</param>
        /// <returns>the tagged value object if a tagged value with the given name is found, null
        /// if the tagged value cannot be found</returns>
        internal static EA.TaggedValue getTaggedValue(EA.Element element, String nameOfTV)
        {
            foreach (EA.TaggedValue tv in element.TaggedValuesEx)
            {
                String s = tv.Name;
                if (tv.Name.Equals(nameOfTV))
                {
                    return tv;
                }
            }
            return null;
        }



        /// <sUMM2ary>
        /// retrieves a tagged value from an attribute
        /// </sUMM2ary>
        /// <param name="element">the attribute to which the seeked tagged value belongs</param>
        /// <param name="nameOfTV">the name of the tagged value that is seeked</param>
        /// <returns>the tagged value object if a tagged value with the given name is found, null
        /// if the tagged value cannot be found</returns>
        internal static EA.AttributeTag getTaggedValue(EA.Attribute attribute, String nameOfTV)
        {
            foreach (EA.AttributeTag tv in attribute.TaggedValues)
            {
                String s = tv.Name;
                if (tv.Name.Equals(nameOfTV))
                {
                    return tv;
                }
            }
            return null;
        }


        /// <summary>
        /// Returns the first occurence of an attribute with the given stereotype
        /// </summary>
        /// <param name="e"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static EA.Attribute fetchFirstAttributeFromElement(EA.Element e, String stereotype)
        {

            foreach (EA.Attribute a in e.Attributes)
            {
                if (a.Stereotype == stereotype)
                    return a;

            }
            return null;

        }


        /// <summary>
        /// Fetch all attributes of a given element which have the specified stereotype
        /// </summary>
        /// <param name="e"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static Dictionary<string, EA.Attribute> fetchAllAttributesFromElement(EA.Element e, String stereotype)
        {
            Dictionary<string, EA.Attribute> attributes = new Dictionary<string, EA.Attribute>();

            foreach (EA.Attribute a in e.Attributes)
            {
                if (a.Stereotype == stereotype)
                    attributes.Add(a.AttributeGUID, a);
            }

            return attributes;

        }


        /// <summary>
        /// Checks whether the given element is the source of an isEquivalentTo dependency
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static bool isEquivalentToAnotherElement(EA.Element e)
        {

            foreach (EA.Connector con in e.Connectors)
            {
                if (con.ClientID == e.ElementID && con.Stereotype == UPCC.isEquivalentTo.ToString())
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns the target of an isEquivalentToDependency
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static EA.Element getTargetOfisEquivalentToDependency(EA.Element sourceElement, EA.Repository repo)
        {

            foreach (EA.Connector con in sourceElement.Connectors)
            {
                if (con.ClientID == sourceElement.ElementID && con.Stereotype == UPCC.isEquivalentTo.ToString())
                {
                    return repo.GetElementByID(con.SupplierID);
                }
            }
            return null;
        }


        /// <summary>
        /// Count the attributes of a given element which have the stereotype s
        /// </summary>
        /// <param name="e"></param>
        /// <param name="stereotype"></param>
        /// <returns></returns>
        internal static int countAttributesOfElement(EA.Element e, String s)
        {
            int i = 0;

            if (e != null)
            {
                foreach (EA.Attribute a in e.Attributes)
                {
                    if (a.Stereotype == s)
                        i++;
                }
            }

            return i;
        }


        /// <summary>
        /// Count the number of connectors of the given element which have a stereotype equal to s
        /// </summary>
        /// <param name="e"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static int countConnectorsOfElement(EA.Element e, String s)
        {
            int i = 0;

            if (e != null)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Stereotype == s) i++;
                        
                }
            }

            return i;
        }


        /// <summary>
        /// Return a list with all attribute of a given element which have the stereotype SUP
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static IList<String> getSupplementaryComponentsOfElement(EA.Element e)
        {
            IList<String> list = new List<String>();

            if (e != null)
            {
                foreach (EA.Attribute a in e.Attributes)
                {
                    if (a.Stereotype == UPCC.SUP.ToString())
                        list.Add(a.Name);
                }

            }

            return list;


        }

        
        /// <summary>
        /// Returns a list of Attributes from the passed element which have the stereotype s
        /// </summary>
        /// <param name="e"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static IList<String> getAttributesOfElement(EA.Element e, String s)
        {
            IList<String> list = new List<String>();

            if (e != null)
            {
                foreach (EA.Attribute a in e.Attributes)
                {
                    if (a.Stereotype == s)
                        list.Add(a.Name);
                }

            }

            return list;


        }


        

        /// <summary>
        /// Returns the target of a basedOn dependency
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="repo"></param>
        /// <returns></returns>
        internal static EA.Element getTargetOfisbasedOnDependency(EA.Element sourceElement, EA.Repository repo)
        {

            foreach (EA.Connector con in sourceElement.Connectors)
            {
                if (con.ClientID == sourceElement.ElementID && con.Stereotype == UPCC.basedOn.ToString())
                {
                    return repo.GetElementByID(con.SupplierID);
                }
            }
            return null;
        }



        /// <summary>
        /// Get the unqualified name based on the passed string
        /// 
        /// e.g. Qualified Name = US_Internal_Address
        /// Unqualified Name    = Address
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static String getUnqualifiedName(String s)
        {
            //Are there any _ in the name (= is the name qualified)?
            int pos = s.LastIndexOf("_");
            return s.Substring(pos + 1);

        }





        /// <summary>
        /// Checks whether the given element is based on another element via a basedOn dependency
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static bool isBasedOnAnotherElement(EA.Element e)
        {

            foreach (EA.Connector con in e.Connectors)
            {
                if (con.ClientID == e.ElementID && con.Stereotype == UPCC.basedOn.ToString())
                {
                    return true;
                }
            }
            return false;
        }


        
        

        /// <summary>
        /// Find the key in a given dictionary using the value
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static int findKey(IDictionary<Int32, string> lookup, string value)
        {
            foreach (var pair in lookup)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }                
            }
            return 0;

        }


        /// <summary>
        /// Evaluates whether the passed lowerBound/upperBound values
        ///                     //The only allowed combinations for the cardinality are [0..1] and [1..1]
        //                      [0..1] = lowerBound = 0, upperBound = 1
        //                      [1..1] = lowerBound = 1, upperBound = 1 || lowerBound = "", upperBound = ""
        /// </summary>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        internal static bool isValid_0_1_or_1_1_Cardinality(String lowerBound, String upperBound)
        {
            if (lowerBound.Trim() == "0" && upperBound.Trim() == "1")
            {
                return true;
            }
            else if (lowerBound.Trim() == "1" && upperBound.Trim() == "1")
            {
                return true;
            }
            else if (lowerBound.Trim() == "" && upperBound.Trim() == "")
            {
                return true;
            }

            return false;            
        }

                


        /// <summary>
        /// Find the key in a given dictionary using the value
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string findKey(IDictionary<string, string> lookup, string value)
        {
            foreach (var pair in lookup)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }
            }
            return null;

        }




        internal static IList getAllPackagesFromModel(EA.Repository repo)
        {
            EA.Package root = (EA.Package)repo.Models.GetAt(0);
            return getAllSubPackagesRecursively(root, new ArrayList(), null);
        }
        internal static IList<Element> getAllElementsFromModel(EA.Repository repo)
        {
            EA.Package root = (EA.Package)repo.Models.GetAt(0);
            return getAllElements(root, new List<EA.Element>(), null);
        }
        internal static IList getAllDiagramsFromModel(EA.Repository repo)
        {
            EA.Package root = (EA.Package)repo.Models.GetAt(0);
            return getAllDiagrams(root, new ArrayList(), null);
        }


        /// <sUMM2ary>
        /// Checks whether the passed String is an integer or not
        /// </sUMM2ary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static bool isInteger(String s)
        {
            try
            {
                int i = Int32.Parse(s);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }














    }
}
