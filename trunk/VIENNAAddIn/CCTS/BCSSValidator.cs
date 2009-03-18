/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using EA;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.Utils;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.CCTS {
    /// <summary>
    /// This class validates a given BIE diagram 
    /// </summary>
    class BCSSValidator {

        EA.Repository repository;
        EA.Package rootPackage;

        /// <summary>
        /// Creates a new BCSS Validator
        /// </summary>
        /// <param name="r"></param>
        /// <param name="p"></param>
        public BCSSValidator(EA.Repository repository, String scope) {
            this.repository = repository;
            this.rootPackage = this.repository.GetPackageByID(Int32.Parse(scope));
        }

        /// <summary>
        /// Validates a DOCLibrary
        /// </summary>
        /// <returns></returns>
        public String validateDOCLibrary(EA.Diagram diagram) {

            try {

                EA.Collection el = rootPackage.Elements;
                foreach (EA.DiagramObject dobj in diagram.DiagramObjects) {

                    //Inspect every ABIE
                    //It can happen, that dobj is not an element - ignore this fact
                    Element e  = null;
                    try {
                       e = this.repository.GetElementByID(dobj.ElementID);
                    }
                    catch (Exception exc) {
                        continue;
                    }
                    

                    String temp = e.Name;
                        if (e.Type.Equals(EA_Element.Class.ToString()) && e.Stereotype.Equals(CCTS_Types.ABIE.ToString())) {

                            ArrayList aggrEndRoles = new ArrayList();

                            foreach (EA.Connector con in e.Connectors) {
                                //Take only Aggregations and Compositions
                                if (con.Type.ToString() == "Aggregation" &&
                                    con.Stereotype.Equals(CCTS_Types.ASBIE.ToString())) {

                                    EA.Element supplier = this.repository.GetElementByID(con.SupplierID);
                                    EA.Element client   = this.repository.GetElementByID(con.ClientID);

                                    BIEConnector bieCon = new BIEConnector();
                                    bieCon.ConnectorSupplier = supplier.Name;
                                    bieCon.ConnectorSupplierID = supplier.ElementID;
                                    bieCon.RoleName = con.ClientEnd.Role;
                                    bieCon.ConnectorClient = client.Name;
                                    bieCon.ConnectorClientID = client.ElementID;

                                    aggrEndRoles.Add(bieCon);
                                }
                            }

                            //Check if there are two connectors emanating from this element
                            //which have the same role name and lead to the same element
                            String[] doubled = checkForDouble(aggrEndRoles);
                            if (doubled != null) {
                                //No Role name
                                if (doubled[0] == "") {
                                    throw (new UnexpectedElementException("The ABIE '" + doubled[1] + "' has more than one aggregation specified by a Role with no name which leads to the same element (" + doubled[2] + "). Two associations emanating from an ABIE which lead to the same ABIE must not have the same role name whereas no role name also counts as a name. Otherwise no schema can be created."));
                                }
                                //with role names
                                else {
                                    throw (new UnexpectedElementException("The ABIE '" + doubled[1] + "' has more than one aggregation specified by the Role called '" + doubled[0] + "' which leads to the same element (" + doubled[2] + "). Two associations emanating from an ABIE which lead to the same ABIE must not have the same role name. Otherwise no schema can be created."));
                                }
                            }
                            
                        }                    
                }
            }
            catch (Exception exe) {
                return exe.Message;
            }


            return "";
        }


        /// <summary>
        /// Checks whether the given ID is a valid connectorID or not
        /// </summary>
        /// <param name="connectorID"></param>
        /// <returns></returns>
        private bool isValidConnector(int connectorID) {
            try {
                EA.Connector dUMM2y = this.repository.GetConnectorByID(connectorID);
                return true;
            }
            catch (Exception e) {
                return false;
            }

        }









        /// <summary>
        /// Validates a BIE Library
        /// </summary>
        /// <returns></returns>
        public String validateBIELibrary()
        {
            String errormessage = "";
            try
            {

                EA.Collection el = rootPackage.Elements;
                foreach (EA.Element abie in el) {
                    if (!abie.Type.ToString().Equals("Class"))
                    {
                        continue;
                    }

                    //check for the stereotype ABIE
                    if (!abie.Stereotype.Equals(CCTS_Types.ABIE.ToString())) {
                        throw (new UnexpectedElementException("The element '" + abie.Name + "' is not stereotyped as '" + CCTS_Types.ABIE.ToString() + "'. Package: " + rootPackage.Name.ToString()));
                    }
                    //check if there are only classes in the diagram
                    //if (!abie.Type.ToString().Equals("Class")) {
                    //    throw (new UnexpectedElementException("The element '" + abie.Name + "' is not a class element."));
                    //}

                    //check if there is at least one attribute
                    if (abie.Attributes.Count > 0) {
                        foreach (EA.Attribute attr in abie.Attributes) {
                            //check for the attributes stereotype BBIE
                            if (!attr.Stereotype.Equals(CCTS_Types.BBIE.ToString())) {
                                throw (new UnexpectedElementException("The attribute '" + attr.Name + "' in the element '" + abie.Name + "' is not stereotyped as '" + CCTS_Types.BBIE.ToString() + "'."));
                            }
                        }
                    }

                    //check for the connectors of the ABIE
                    //int countDep = 0;
                    int countAggr = 0;
                    ArrayList aggrEndRoles = new ArrayList();
                    foreach (EA.Connector con in abie.Connectors) {
                        if (con.Type.Equals("NoteLink", StringComparison.OrdinalIgnoreCase))
                            continue;
                        
                        if (con.Stereotype.Equals(CCTS_Types.basedOn.ToString(), StringComparison.OrdinalIgnoreCase) && con.Type.ToString().Equals(AssociationTypes.Dependency.ToString())) {
                            //Do nothing here so far
                        }
                        else if (con.Stereotype.Equals(CCTS_Types.ASBIE.ToString()) &&
                                con.Type.ToString().Equals("Aggregation")) {
                            //check if there are more than one aggregations with the same role
                            //the validation should only consider outgoing aggregations
                            if (this.isOutgoing(con, abie)) {
                                //fill ArrayList for later validation
                                BIEConnector bieCon = new BIEConnector();
                                bieCon.ConnectorSupplier = abie.Name;
                                bieCon.ConnectorSupplierID = abie.ElementID;
                                bieCon.RoleName = con.ClientEnd.Role;
                                bieCon.ConnectorClient = this.repository.GetElementByID(con.ClientID).Name;
                                bieCon.ConnectorClientID = con.ClientID;
                                aggrEndRoles.Add(bieCon);
                                countAggr++;
                            }                            
                        }
                        //the connector is neither a dependency nor an aggregation
                        else {
                            throw (new UnexpectedElementException("An ABIE must only have 'aggregation' connectors stereotyped as 'ASBIE' or 'dependency' connectors stereotyped as 'basedOn'. Stereotypes are case-sensitive!. The element " + abie.Name + " has invalid connectors."));
                        }
                    }

                    //Check if there are two connectors emanating from this element
                    //which have the same role name and lead to the same element
                    String [] doubled = checkForDouble(aggrEndRoles);
                    if (doubled != null) {
                        //No Role name
                        if (doubled[0] == "") {
                            throw (new UnexpectedElementException("The ABIE '" + doubled[1] + "' has more than one aggregation specified by a Role with no name which leads to the same element (" + doubled[2] + "). Two associations emanating from an ABIE which lead to the same ABIE must not have the same role name whereas no role name also counts as a name. Otherwise no schema can be created."));
                        }
                        //with role names
                        else {
                            throw (new UnexpectedElementException("The ABIE '" + doubled[1] + "' has more than one aggregation specified by the Role called '" + doubled[0] + "' which leads to the same element (" + doubled[2] + "). Two associations emanating from an ABIE which lead to the same ABIE must not have the same role name. Otherwise no schema can be created."));
                        }
                    }

                }
                

            }
            catch (UnexpectedElementException unexEl) {
                errormessage = unexEl.Message;
            }

            return errormessage;

        }

        /// <summary>
        /// returns the name of the doubled aggregation role
        /// </summary>
        /// <param name="aggr"></param>
        /// <returns></returns>
        private String [] checkForDouble(ArrayList aggr) 
        {
            for (int i = 0; i < aggr.Count; i++)
            {
                BIEConnector ccCon = (BIEConnector)aggr[i];
                ccCon.IsChecked = false;
            }

            for (int i = 0; i < aggr.Count; i++) {
                BIEConnector bieCon = (BIEConnector)aggr[i];
                if (!bieCon.IsChecked) {
                    bieCon.IsChecked = true;
                    for (int j = i + 1; j < aggr.Count; j++) {
                        BIEConnector next = (BIEConnector)aggr[j];
                        if (next.ConnectorClientID == bieCon.ConnectorClientID &&
                            next.ConnectorSupplierID == bieCon.ConnectorSupplierID) {
                            next.IsChecked = true;
                            if (next.RoleName == bieCon.RoleName) {
                                String from = bieCon.ConnectorSupplier;
                                String to = bieCon.ConnectorClient;
                                if (next.RoleName == "") {
                                    return new String[] {"", from, to};
                                }
                                else {
                                    return new String[] { next.RoleName, from, to};
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }



        /// <summary>
        /// Get the Targets for the Role Name and return the separated with commas
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private String getTargetsForRoleName(ArrayList aggrEndRoles, String rolename) {
            String s = "";
            int i = 0;
            foreach (BIEConnector con in aggrEndRoles) {
                i++;
                if (con.RoleName == rolename) {
                    if (i != 1)
                        s += ", ";

                    s += con.ConnectorClient;
                }
            }
            return s;

        }


        /// <summary>
        /// returns true if the connector is outgoing of the element
        /// </summary>
        /// <param name="c"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        private bool isOutgoing(EA.Connector c, EA.Element el)
        {

            if (c.SupplierID == el.ElementID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        


    }
}
