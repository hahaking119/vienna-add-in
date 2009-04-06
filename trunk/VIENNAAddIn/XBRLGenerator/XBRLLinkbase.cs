/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EA;
using VIENNAAddIn.Utils;
using VIENNAAddIn.Exceptions;
using System.IO;
using System.Xml.Schema;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.XBRLGenerator
{
    public partial class XBRLLinkbase : Form, GeneratorCallBackInterface
    {
        private static XBRLLinkbase form;
        public static void ShowForm(Repository repository)
        {
            var diagramID = repository.DetermineDiagramID();
            if (form == null || form.IsDisposed)
            {
                form = new XBRLLinkbase(repository, diagramID);
                form.Show();
            }
            else
            {
                form.resetGenerator(diagramID);
                form.Select();
                form.Focus();
                form.Show();
            }
        }


        #region Variables
        private EA.Repository repository;
        private GeneratorCallBackInterface caller;
        private int diagramID;
        private string targetNameSpacePrefix = "xbrl";
        private bool withGUI = true;
        private string path = "";
        private Hashtable hash = new Hashtable();
        private string scope = "";
        private bool foundSearchedElement = false;
        private ArrayList arrSearchName = new ArrayList();
        //private ArrayList alreadyCreatedSchemas = new ArrayList();
        private EA.Element searchedElement;
        //private bool blnPrevIsDifferentPackage = false;
        //private ArrayList oldName = new ArrayList();
        private int diffLevel = 0;
        
        private ArrayList alreadyCreatedElement = new ArrayList();
        private ArrayList alreadyCreatedPresentationArc = new ArrayList();

        private ArrayList alreadyCreatedLinkedSchema = new ArrayList();

        private string roleName = "";

        private XmlTextWriter wLabel;
        #endregion

        #region Implement
        /// <summary>
        /// Append an error message to the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendErrorMessage(String msg, String diagramName)
        {
            if (caller != null)
            {
                caller.appendMessage("error", msg, this.getDiagramName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "ERROR: (Diagram: " + diagramName + ") " + msg + "\n\n";
                }
            }
        }

        /// <summary>
        /// Show a info message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendInfoMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("info", msg, this.getDiagramName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "INFO: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }

        /// <summary>
        /// Show a warn message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendWarnMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("warn", msg, this.getDiagramName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }

        /// <summary>
        /// Is called from extern to append a message to this GUI
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void appendMessage(String type, String message, String packageName)
        {
            if (type == "info")
                this.appendInfoMessage(message, packageName);
            else if (type == "warn")
                this.appendWarnMessage(message, packageName);
            else if (type == "error")
                this.appendErrorMessage(message, packageName);

        }

        /// <summary>
        /// Performs a step with the progress bar
        /// </summary>
        public void performProgressStep()
        {
            if (this.withGUI)
            {
                this.progressBar1.PerformStep();
            }
            else
            {
                //Is there a caller for this class?
                if (caller != null)
                    caller.performProgressStep();
            }
        }
        #endregion

        #region Constructor
        public XBRLLinkbase(EA.Repository repository, int diagramID)
        {
            InitializeComponent();

            this.repository = repository;
            this.diagramID = diagramID;
            this.withGUI = true;
            this.setActivePackageLabel();
        }
        #endregion

        #region private method

        private void generatePresentationFileDOCLibrary()
        {
            EA.Package pkg = this.repository.GetPackageByID(this.repository.GetDiagramByID(diagramID).PackageID);
            EA.Diagram dgr = this.repository.GetDiagramByID(diagramID);

            String schemaPath = "";
            String schemaName = "";
            schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pkg.PackageID), repository);
            schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
            String filename = path + schemaPath + schemaName + "-presentation.xml";
            System.IO.Directory.CreateDirectory(path + schemaPath);

            XmlTextWriter w = new XmlTextWriter(filename, null);
            generateInitialSchema(w);
            
            //generate label file
            filename = path + schemaPath + schemaName + "-label.xml";
            wLabel = new XmlTextWriter(filename, null);
            generateInitialLabelFile(wLabel);

            int elementCount = 0;
            foreach (EA.Element element in pkg.Elements)
            {
                elementCount++;
                if (element.Stereotype.Equals(CCTS_Types.ABIE.ToString()) && isRootElement(element))
                {
                    try
                    {
                        getSchemaElement(element, w);
                    }
                    catch (Exception ex)
                    {
                        wLabel.Close();
                        w.Close();

                        throw new Exception(ex.Message);
                    }
                }

                //After every 20th iteration we  perform one progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }

            //closing the label file
            wLabel.WriteEndElement();
            wLabel.Flush();
            wLabel.Close();

            //closing the presentation file
            w.WriteEndElement();
            w.Flush();
            w.Close();
            
        }

        private void getSchemaElement(EA.Element element, XmlTextWriter w)
        {
            getHiddenAttribute();

            int order = 0;
            ArrayList arrName = new ArrayList();
            arrName.Add(element.Name);

            #region Looping through all element in the diagram

            //create ABIE for current root
            //generatePresentationLocatorForABIE(element, w, arrName,"", null);
            int package = this.repository.GetDiagramByID(diagramID).PackageID;
            string locatorName = XMLTools.getXBRLAsbieName(arrName);
            string href = XMLTools.getSchemaName(this.repository.GetPackageByID(package)) + "#" + locatorName;

            w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
            w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
            w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
            w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
            w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : ABIE to BBIE");
            w.WriteEndElement();

            //Label file
            generateLabelLocatorForABIE(href, locatorName, element);
            
            //create BBIE for current root
            foreach (EA.Attribute bbie in element.Attributes)
            {
                string attrGUID = bbie.AttributeGUID;
                //check whether this attribute is hidden or not
                if (!isHiddenAttribute(bbie))
                {
                    order++;
                    generatePresentationLocatorForBBIE(bbie, w, order, arrName,"");
                }
            }

            #endregion

            if (element.Connectors.Count > 0)
            {
                //generate linkbase for ASBIE and other linked class
                generateLinkbaseRecursively(element, w, order, arrName, "", 0);
            }
        }

        private bool isRootElement(Element rootEl)
        {
            foreach (EA.TaggedValue tv in rootEl.TaggedValues)
            {
                if (tv.Name.Equals("isRoot", StringComparison.OrdinalIgnoreCase) &&
                    (tv.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || (tv.Value.Equals("1", StringComparison.OrdinalIgnoreCase))))
                    return true;
            }
            return false;
        }

        private void generateInitialSchema(XmlTextWriter w)
        {
            w.Namespaces = true;
            w.Formatting = Formatting.Indented;
            w.WriteStartDocument();
            w.WriteStartElement("", "linkbase", "http://www.xbrl.org/2003/linkbase");
            w.WriteAttributeString("xmlns", "", null, "http://www.xbrl.org/2003/linkbase");
            w.WriteAttributeString("xmlns", "xbrli", null, "http://www.xbrl.org/2003/instance");
            w.WriteAttributeString("xmlns", "xlink", null, "http://www.w3.org/1999/xlink");
            w.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            w.WriteAttributeString("xsi", "schemaLocation", null, "http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd");

            EA.Package tempPkg = this.repository.GetPackageByID(this.repository.GetDiagramByID(diagramID).PackageID);
            w.WriteAttributeString("xmlns", this.targetNameSpacePrefix, null, XMLTools.getNameSpace(this.repository, tempPkg));

            w.WriteStartElement("presentationLink", "http://www.xbrl.org/2003/linkbase");
            w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "extended");
            w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/link");
        }

        /// <summary>
        /// Generate XBRL linkbase file 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="w"></param>
        /// <param name="order"></param>
        /// <param name="name"></param>
        /// <param name="importPath"></param>
        /// <param name="oldName"></param>
        private void generateLinkbaseRecursively(EA.Element element,XmlTextWriter w,int order,ArrayList name, string importPath, int level)
        {
            foreach (EA.Connector con in element.Connectors)
            {
                int localLevel = level;
                ArrayList tempName = new ArrayList(name);

                if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
                    && isOutgoing(con, element))
                {
                    //search whether current connector is in the diagram
                    if (isConnectorInDiagram(con))
                    {
                        order++;
                        EA.Element client = this.repository.GetElementByID(con.ClientID);
                        
                        //client dan parent di current package
                        if ((isInSamePackage(client, element)) && (importPath == ""))
                        {
                            if (con.ClientEnd.Role != "")
                                tempName.Add(con.ClientEnd.Role);
                            tempName.Add(client.Name);

                            generatePresentationLocatorForABIE(client, w, tempName, "", name, order);

                            int bbieOrder = 0;
                            #region in the same package client
                            foreach (EA.Attribute attr in client.Attributes)
                            {
                                //Check if this attr is hidden
                                if (!isHiddenAttribute(attr))
                                {
                                    bbieOrder++;
                                    generatePresentationLocatorForBBIE(attr, w, bbieOrder, tempName, importPath);
                                }
                            }

                            //generate recursively
                            if (client.PackageID.ToString() == this.scope)
                            {
                                generateLinkbaseRecursively(client, w, order, tempName, "", 0);
                            }
                            else
                            {
                                generateLinkbaseRecursively(client, w, order, tempName, importPath, 0);
                            }
                            #endregion
                        }
                            //package id element parent != client
                        else if (element.PackageID != client.PackageID)
                        {
                            #region other package client
                            localLevel = 0;

                            if (con.ClientEnd.Role != "")
                                tempName.Add(con.ClientEnd.Role);
                            tempName.Add(client.Name);

                            generatePresentationLocatorForABIE(client, w, tempName, "", name, order);

                            EA.Package p = this.repository.GetPackageByID(client.PackageID);

                            String schemaName = "";
                            schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));

                            //get the path of element on the other package
                            String theImportPath = XMLTools.getImportPathForSchema(this.repository.GetPackageByID(p.PackageID), repository, schemaName, scope);

                            //check if the linked schema already exist in the array 'alreadyCreatedLinkedSchema'
                            if (!isAlreadyCreatedLinkedSchema(p))
                            {
                                //generate linked schema
                                generateInitialLabelFileLinkedSchema(p);
                                //add the linked schema to the array, in order not to generate the linked schema more than one.
                                alreadyCreatedLinkedSchema.Add(p.Name);
                            }
                            //generate the BBIE of another package.
                            int bbieOrderOtherPkg = 0;


                            foreach (EA.Attribute attr in client.Attributes)
                            {
                                //Check if this attr is hidden
                                if (!isHiddenAttribute(attr))
                                {
                                    bbieOrderOtherPkg++;
                                    generatePresentationLocatorForBBIE_ClientOtherPackage(con, attr, w, bbieOrderOtherPkg, tempName, theImportPath);
                                }
                            }

                            generateLinkbaseRecursively(client, w, order, tempName, theImportPath, localLevel);
                            
                            #endregion
                        }
                            //parent id and client id are in the same "other package"
                        else
                        {
                            localLevel++;

                            if (localLevel == 1)
                                //generatePresentationLocatorForABIE_ClientParentInOtherPackage(client, element, w, importPath, order, tempName);
                                generatePresentationLocatorForABIE_ClientParentInOtherPackage(con, w, importPath, order, tempName);
                            else
                                //generatePresentationLocatorForABIE_ClientParentInOtherPackage(client, element, w, importPath, order, null);
                                generatePresentationLocatorForABIE_ClientParentInOtherPackage(con, w, importPath, order, null);


                            //generate the BBIE of another package.
                            int bbieOrderOtherPkg = 0;

                            foreach (EA.Attribute attr in client.Attributes)
                            {
                                //Check if this attr is hidden
                                if (!isHiddenAttribute(attr))
                                {
                                    bbieOrderOtherPkg++;
                                    generatePresentationLocatorForBBIE_ClientParentInOtherPackage(attr, w, con, bbieOrderOtherPkg, importPath);
                                    
                                }
                            }

                            generateLinkbaseRecursively(client, w, order, tempName, importPath, localLevel);
                        }
                    }

                    #region if connector is not visible but the client element of the connector is visible
                    //else if (isElementInDiagram(con.ClientID)) //if connector is not visible but the client element of the connector is visible
                    //{
                    //    #region Create element without any relation to other element
                    //    //create element here

                    //    order++;

                    //    EA.Element client = this.repository.GetElementByID(con.ClientID);
                    //    //ArrayList arrNameHidden = new ArrayList();
                    //    //arrNameHidden.Add(client.Name);
                    //    tempName.Add(con.ClientEnd.Role);
                    //    tempName.Add(client.Name);

                    //    if (client.PackageID.ToString() == this.scope)
                    //    {
                    //        //generate ABIE
                    //        generatePresentationLocatorForABIE(client, w, tempName, "");
                            
                    //        //generate ASBIE to root element
                    //        //generatePresentationLocatorForASBIE(client, w, name, tempName, order);
                            
                    //        //generate BBIE
                    //        int bbieOrder = 0;
                    //        foreach (EA.Attribute attr in client.Attributes)
                    //        {
                    //            //Check if this attr is hidden
                    //            if (!isHiddenAttribute(attr))
                    //            {
                    //                bbieOrder++;
                    //                #region generate BBIE
                    //                generatePresentationLocatorForBBIE(attr, w, bbieOrder, null, tempName, "");
                    //                #endregion
                    //            }
                    //        }

                    //    }
                    //    else
                    //    {
                    //        EA.Package p = this.repository.GetPackageByID(client.PackageID);
                            
                    //        //generate ABIE
                    //        generatePresentationLocatorForABIE(client, w, tempName, "");

                    //        //generate ASBIE to root element
                    //        generatePresentationLocatorForASBIE(client, w, name, tempName, order);

                    //        String schemaName = "";
                    //        schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));

                    //        //get the path of element on the other package
                    //        String theImportPath = XMLTools.getImportPathForSchema(this.repository.GetPackageByID(p.PackageID), repository, schemaName, scope);

                    //        //generate BBIE
                    //        int bbieOrder = 0;
                    //        foreach (EA.Attribute attr in client.Attributes)
                    //        {
                    //            //Check if this attr is hidden
                    //            if (!isHiddenAttribute(attr))
                    //            {
                    //                bbieOrder++;
                    //                generatePresentationLocatorForBBIE(attr, w, bbieOrder, null, tempName, importPath);
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //}
                    #endregion
                }
            }
        }

        
        private bool isAlreadyCreatedElement(string key)
        {
            if (alreadyCreatedElement.Contains(key))
                return true;
            else
                alreadyCreatedElement.Add(key);

            return false;
        }

        private bool isAlreadyCreatedPresentationArc(string from, string to)
        {
            string temp = from + "," + to;
            
            if (alreadyCreatedPresentationArc.Contains(temp))
                return true;
            else
                alreadyCreatedPresentationArc.Add(from + "," + to);

            return false;
        }

        private void generatePresentationLocatorForBBIE_ClientParentInOtherPackage(EA.Attribute attr, XmlTextWriter w, EA.Connector con, int order, string importPath)
        {
            Element parent = this.repository.GetElementByID(con.SupplierID);
            EA.Element client = this.repository.GetElementByID(attr.ParentID);

            this.roleName = con.ClientEnd.Role;
            string locatorName = findBBIENameFromOtherPackage(attr);
            

            //if (isAlreadyCreatedElement(locatorName))
            //    return;

            string href = importPath + "#" + locatorName;
            string fromElement = findABIENameFromOtherPackage(client);

            if (this.foundSearchedElement == false)
            {
                string errorMsg = "";
                errorMsg = "Linked element with name " + parent.Name;
                errorMsg += this.roleName != "" ? " and role name " + this.roleName : "";
                errorMsg += " is not found.";
                throw new Exception(errorMsg);
            } 

            this.roleName = "";

            if (!isAlreadyCreatedElement(locatorName))
            {
                w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
                w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
                w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + fromElement + " to " + locatorName);
                w.WriteEndElement();

                //Label file
                generateLabelLocatorForBBIE(href, locatorName, attr);
            }

            if (!isAlreadyCreatedPresentationArc(fromElement, locatorName))
            {
                //presentationArc
                w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
                w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");

                w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", fromElement);
                w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("use", "optional");
                w.WriteAttributeString("order", order.ToString());
                w.WriteEndElement();
            }

        }

        private void generatePresentationLocatorForBBIE_ClientOtherPackage(EA.Connector con, EA.Attribute bbie, XmlTextWriter w, int order, ArrayList name, string importPath)
        {
            EA.Element parentElement = this.repository.GetElementByID(bbie.ParentID);
            int package = this.repository.GetDiagramByID(diagramID).PackageID;

            //locator
            string locatorName = "";
            string href = "";
            string fromElement = "";


            fromElement = XMLTools.getXBRLAsbieName(name);


            if (importPath == "") //the attribute is from the same package
            {
                locatorName = XMLTools.getXBRLBbieName(bbie, name);
                href = XMLTools.getSchemaName(this.repository.GetPackageByID(package)) + "#" + locatorName;
            }
            else //the attribut is from different package
            {
                this.roleName = con.ClientEnd.Role;
                locatorName = findBBIENameFromOtherPackage(bbie);

                if (this.foundSearchedElement == false)
                {
                    string errorMsg = "";
                    errorMsg = "Linked element with name " + parentElement.Name;
                    errorMsg += this.roleName != "" ? " and role name " + this.roleName : "";
                    errorMsg += " is not found.";
                    throw new Exception(errorMsg);
                }
                this.roleName = "";

                href = importPath + "#" + locatorName;
            }

            if (!isAlreadyCreatedElement(locatorName))
            {
                //return;
                w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
                w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
                w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + fromElement + " to " + locatorName);
                w.WriteEndElement();

                //Label file
                generateLabelLocatorForBBIE(href, locatorName, bbie);
            }

            if (!isAlreadyCreatedPresentationArc(fromElement, locatorName))
            {
                //presentationArc
                w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
                w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");

                w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", fromElement);
                w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("use", "optional");
                w.WriteAttributeString("order", order.ToString());
                w.WriteEndElement();
            }
        }

        /// <summary>
        /// To create bbie of element
        /// </summary>
        /// <param name="bbie"></param>
        /// <param name="w"></param>
        /// <param name="order"></param>
        private void generatePresentationLocatorForBBIE(EA.Attribute bbie, XmlTextWriter w, int order, ArrayList name, string importPath)
        {
            EA.Element parentElement = this.repository.GetElementByID(bbie.ParentID);
            int package = this.repository.GetDiagramByID(diagramID).PackageID;

            //locator
            string locatorName = "";
            string href = "";
            string fromElement = "";


            fromElement = XMLTools.getXBRLAsbieName(name);


            if (importPath == "") //the attribute is from the same package
            {
                locatorName = XMLTools.getXBRLBbieName(bbie, name);
                href = XMLTools.getSchemaName(this.repository.GetPackageByID(package)) + "#" + locatorName;
            }
            else //the attribut is from different package
            {
                locatorName = findBBIENameFromOtherPackage(bbie);
                href = importPath + "#" + locatorName;
            }

            if (!isAlreadyCreatedElement(locatorName))
            {
                w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
                w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
                w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + fromElement + " to " + locatorName);
                w.WriteEndElement();

                //Label File
                generateLabelLocatorForBBIE(href, locatorName, bbie);
            }

            if (!isAlreadyCreatedPresentationArc(fromElement, locatorName))
            {
                //presentationArc
                w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
                w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");

                w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", fromElement);
                w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("use", "optional");
                w.WriteAttributeString("order", order.ToString());
                w.WriteEndElement();
            }
        }

        private void generatePresentationLocatorForABIE(EA.Element element, XmlTextWriter w, ArrayList arrName, string importPath, ArrayList trgName, int order)
        {
            string locatorName = "";
            string href = "";
            string targetName = "";

            if (importPath == "") //ABIE is from the same package
            {
                int package = this.repository.GetDiagramByID(diagramID).PackageID;
                locatorName = XMLTools.getXBRLAsbieName(arrName);

                //href must be composition of : schemafile + "#" + ID-attribute of the element
                href = XMLTools.getSchemaName(this.repository.GetPackageByID(package)) + "#" + locatorName;

                targetName = XMLTools.getXBRLAsbieName(trgName);
            }
            else //ABIE is from different package
            {
                //href must be composition of : schemafile + "#" + ID-attribute of the element
                locatorName = findABIENameFromOtherPackage(element);
                href = importPath + "#" + locatorName;
                targetName = "";
            }

            //if this locator already exist, skip generation for this element
            if (!isAlreadyCreatedElement(locatorName))
            {
                //locator
                w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
                w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
                w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : ABIE to BBIE");
                w.WriteEndElement();

                //generate label file
                generateLabelLocatorForABIE(href, locatorName, element);
            }

            if (!isAlreadyCreatedPresentationArc(targetName, locatorName))
            {
                //presentationArc
                w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
                w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");
                w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", targetName);
                w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("use", "optional");
                w.WriteAttributeString("order", order.ToString());
                w.WriteEndElement();
            }

        }

        private void generatePresentationLocatorForABIE_ClientParentInOtherPackage(EA.Connector con, XmlTextWriter w, string importPath, int order, ArrayList arrFrom)
        {
            EA.Element client = this.repository.GetElementByID(con.ClientID);
            EA.Element parent = this.repository.GetElementByID(con.SupplierID);

            this.roleName = con.ClientEnd.Role;
            string locatorName = findABIENameFromOtherPackage(client);
            this.roleName = "";

            if (!isAlreadyCreatedElement(locatorName))
            {
                //return;

                string href = importPath + "#" + locatorName;

                w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
                w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
                w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : ABIE to BBIE");
                w.WriteEndElement();

                //Label file
                generateLabelLocatorForABIE(href, locatorName, client);
            }

            string fromName = "";

            if (arrFrom != null)
            {
                fromName = XMLTools.getXBRLAsbieName(arrFrom);
            }
            else
            {
                fromName = findABIENameFromOtherPackage(parent);
            }

            if (!isAlreadyCreatedPresentationArc(fromName, locatorName))
            {
                //presentationArc
                w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
                w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
                w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");
                w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", fromName);
                w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
                w.WriteAttributeString("use", "optional");
                w.WriteAttributeString("order", order.ToString());
                w.WriteEndElement();
            }
        }

        

        private bool isElementInDiagram(int elementID)
        {
            EA.Diagram dgr = this.repository.GetDiagramByID(diagramID);
            foreach (EA.DiagramObject dgrObj in dgr.DiagramObjects)
            {
                if (elementID == dgrObj.ElementID)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isConnectorInDiagram(EA.Connector con)
        {
            EA.Diagram dgr = this.repository.GetDiagramByID(diagramID);
            foreach (EA.DiagramLink dgrLink in dgr.DiagramLinks)
            {
                if (con.ConnectorID == dgrLink.ConnectorID) 
                {
                    if (!dgrLink.IsHidden)
                    {
                        return true;
                    }
                    else
                        break;
                }
            }

            return false;
        }

       

        private void setActivePackageLabel()
        {
            this.selectedDiagrmName.Text = this.repository.GetDiagramByID(diagramID).Name;
        }


        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            resetGenerator(this.diagramID);
            DialogResult result = this.dlgSavePath.ShowDialog(this);
            bool error = false;

            if (result == DialogResult.Cancel)
                dlgSavePath.Dispose();
            else
            {
                path = dlgSavePath.SelectedPath + "\\";
                this.appendInfoMessage("Generating schema file...", getDiagramName());

                this.performProgressStep();
                try
                {
                    this.generateXBRLLinkbase();
                }
                catch (Exception exception)
                {
                    this.appendErrorMessage(exception.Message, getDiagramName());
                    error = true;
                }

                if (!error)
                    this.appendInfoMessage("Successfully generate XBRL linkbase .", getDiagramName());

                this.progressBar1.Value = this.progressBar1.Maximum;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

        #region Recursively find a name for BBIE in other package
        private string findBBIENameFromOtherPackage(EA.Attribute attr)
        {
            EA.Element el = this.repository.GetElementByID(attr.ParentID);
            EA.Package pkg = this.repository.GetPackageByID(el.PackageID);

            foundSearchedElement = false;
            searchedElement = el;
            arrSearchName = new ArrayList();

            //search root 
            foreach (EA.Element element in pkg.Elements)
            {
                if (element.Stereotype.Equals(CCTS_Types.ABIE.ToString()))
                {
                    try
                    {
                        checkForRoot(element);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (foundSearchedElement)
                        break;
                }
            }

            return XMLTools.getXBRLBbieName(attr,arrSearchName);
        }

        private void checkForRoot(EA.Element e)
        {
            if (e.Connectors.Count > 0)
            {
                bool isRoot = true;
                foreach (EA.Connector con in e.Connectors)
                {
                    //if current class is a client of another class in the same package --> not a root
                    if ((con.ClientID == e.ElementID) &&
                        (this.repository.GetElementByID(con.SupplierID).PackageID == e.PackageID))
                    {
                        isRoot = false;
                        break;
                    }
                }

                if (isRoot)
                {
                    ArrayList name = new ArrayList();
                    name.Add(e.Name);

                    if (e.Name != searchedElement.Name)
                        generateSchemaRecursively(e,name);
                    else
                    {
                        arrSearchName = name;
                        foundSearchedElement = true;
                    }
                }
            }
        }

        
        private void generateSchemaRecursively(EA.Element element, ArrayList name)
        {
            EA.Element e = element;
            if (!foundSearchedElement)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    ArrayList tempName = new ArrayList(name);
                    if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
                            && isOutgoing(con, e))
                    {
                        EA.Element client = this.repository.GetElementByID(con.ClientID);

                        if (con.ClientEnd.Role != "")
                            tempName.Add(con.ClientEnd.Role);
                        tempName.Add(client.Name);

                        if (this.roleName != "")
                        {
                            if ((client.Name == searchedElement.Name) && (con.ClientEnd.Role == this.roleName))
                            {
                                arrSearchName = tempName;
                                foundSearchedElement = true;
                                break;
                            }
                        }
                        else
                        {
                            if (client.Name == searchedElement.Name) 
                            {
                                arrSearchName = tempName;
                                foundSearchedElement = true;
                                break;
                            }
                        }

                        if (isInSamePackage(client, element))
                        {
                            //follow ASBIE's to any depth until you reach an ABIE with no further ASBIE.
                            #region ASBIE that connect to ABIE in the same package

                            //generate ASBIE of related ABIE.
                            generateSchemaRecursively(client, tempName);
                            #endregion
                        }
                    }
                }
            }
        }
        #endregion

        #region Recursively find a name for ABIE in other package

        private string findABIENameFromOtherPackage(EA.Element element)
        {
            EA.Package pkg = this.repository.GetPackageByID(element.PackageID);

            foundSearchedElement = false;
            searchedElement = element;
            arrSearchName = new ArrayList();

            //search root 
            foreach (EA.Element e in pkg.Elements)
            {
                if (element.Stereotype.Equals(CCTS_Types.ABIE.ToString()))
                {
                    try
                    {
                        checkForRoot(e);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (foundSearchedElement)
                        break;
                }
            }

            return XMLTools.getXBRLAsbieName(arrSearchName);
        }

        #endregion


        /// <summary>
        /// Check the attribute is hidden or not
        /// </summary>
        /// <param name="bbie">the attribute to check</param>
        /// <returns>boolean value</returns>
        private bool isHiddenAttribute(EA.Attribute bbie)
        {
            //get the first six GUID
            string shortAttrGUID = bbie.AttributeGUID.Substring(1, 6);
            string shortElementGUID = this.repository.GetElementByID(bbie.ParentID).ElementGUID.Substring(1,6);

            if (hash.Contains(shortElementGUID))
            {
                foreach (string hiddenAttr in (string[])(hash[shortElementGUID]))
                {
                    if (hiddenAttr == shortAttrGUID)
                        return true;
                }
                
            }

            return false;
        }

        private void getHiddenAttribute()
        {
            EA.Diagram diagram = this.repository.GetDiagramByID(diagramID);
            string styleEx = diagram.StyleEx;

            bool attrHiddenExist = styleEx.Contains("SPL=");
            if (attrHiddenExist)
            {
                //get index of "SPL=" - suppresion list
                int indexSPL = styleEx.IndexOf("SPL=");

                //get the first index of ";", in order to get all the string supression list
                int indexSemicolonFirstOccurance = styleEx.IndexOf(@";", indexSPL + 3, styleEx.Length - (indexSPL + 3));

                //remove the "SPL=" and ":" at the end of string, so we only get substring of hidden attribute
                string subStrInfo = styleEx.Substring(indexSPL + 4, indexSemicolonFirstOccurance - (indexSPL + 5));

                //split the string of hidden attribute, then we get array of something like these "S_38B6F9=47B320,435D5D"
                string[] a = subStrInfo.Split(@":".ToCharArray());

                foreach (string s in a)
                {
                    string[] tempArray = s.Split(@"=".ToCharArray());
                    string[] hashValue = tempArray[1].Split(@",".ToCharArray());
                    string hashKey = tempArray[0].Remove(0, 2);
                    hash.Add(hashKey, hashValue);
                }
            }
        }

        private String getDiagramName()
        {
            return this.repository.GetDiagramByID(diagramID).Name;
        }

        private bool isInSamePackage(EA.Element client, EA.Element parent)
        {
            if (parent.PackageID == client.PackageID)
                return true;
            else
                return false;
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
                return true;
            else
                return false;
        }

        #region Method for Generate Label File

        private void generateInitialLabelFile(XmlTextWriter wLabel)
        {
            wLabel.Namespaces = true;
            wLabel.Formatting = Formatting.Indented;
            wLabel.WriteStartDocument();
            wLabel.WriteStartElement("", "linkbase", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("xmlns", "", null, "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("xmlns", "xbrli", null, "http://www.xbrl.org/2003/instance");
            wLabel.WriteAttributeString("xmlns", "xlink", null, "http://www.w3.org/1999/xlink");
            wLabel.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            wLabel.WriteAttributeString("xsi", "schemaLocation", null, "http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd");

            EA.Package tempPkg = this.repository.GetPackageByID(this.repository.GetDiagramByID(diagramID).PackageID);
            wLabel.WriteAttributeString("xmlns", this.targetNameSpacePrefix, null, XMLTools.getNameSpace(this.repository, tempPkg));

            wLabel.WriteStartElement("labelLink", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "extended");
            wLabel.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/link");
        }

        private void generateLabelLocatorForABIE(string href, string locatorName, EA.Element element)
        {
            //locator
            wLabel.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
            wLabel.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
            wLabel.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
            wLabel.WriteEndElement();

            //label
            string labelName = locatorName + "_lbl";
            wLabel.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
            wLabel.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
            wLabel.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
            wLabel.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
            wLabel.WriteString(XMLTools.getElementTVValue(CCTS_TV.Definition, element) == "" ? element.Name : XMLTools.getElementTVValue(CCTS_TV.Definition, element)); // get label from definition tagged value
            wLabel.WriteEndElement();//closing tag of label

            //labelArc
            wLabel.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
            wLabel.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
            wLabel.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
            wLabel.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
            wLabel.WriteEndElement();
        }


        private void generateLabelLocatorForBBIE(string href, string locatorName, EA.Attribute attr)//EA.Attribute attr, XmlTextWriter w)
        {
            ////locator
            wLabel.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
            wLabel.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
            wLabel.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
            wLabel.WriteEndElement();

            //label
            string labelName = locatorName + "_lbl";
            wLabel.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
            wLabel.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
            wLabel.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
            wLabel.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
            wLabel.WriteString(XMLTools.getAttributeTVValue(CCTS_TV.Definition, attr) == "" ? attr.Name : XMLTools.getAttributeTVValue(CCTS_TV.Definition, attr)); // get the Definition Tagged Value
            wLabel.WriteEndElement();//closing tag of label

            //labelArc
            wLabel.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
            wLabel.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
            wLabel.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
            wLabel.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
            wLabel.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
            wLabel.WriteEndElement();
        }
        #endregion


        /// <summary>
        /// Check whether the paramater package is already generate
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private bool isAlreadyCreatedLinkedSchema(EA.Package package)
        {
            foreach (string pkgName in alreadyCreatedLinkedSchema)
            {
                if (pkgName == package.Name)
                    return true;
            }

            return false;
        }


        private void generateInitialLabelFileLinkedSchema(EA.Package package)
        {
            if (package.Element.Stereotype.ToString() == CCTS_PackageType.BIELibrary.ToString())
            {
                BIEGeneratorXBRL bieGen = new BIEGeneratorXBRL(this.repository, package, this.path, caller);
                System.Collections.ICollection result;
                CC_Utils.blnLinkedSchema = true;
                result = bieGen.generateXBRLschema(package);

                foreach (XmlSchema schema1 in result)
                {
                    WriteSchemaToFile(package, schema1);
                }
            }
        }

        private void WriteSchemaToFile(EA.Package package, XmlSchema schema)
        {
            String schemaPath = "";
            String schemaName = "";
            schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(package.PackageID), repository);
            schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(package.PackageID));
            String filename = this.path + schemaPath + schemaName;

            System.IO.Directory.CreateDirectory(this.path + schemaPath);
            Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
            schema.Write(outputStream);
            outputStream.Close();
        }

         #endregion

        #region public method
        public void generateXBRLLinkbase()
        {
            int rootElement = 0;
            int countRootElement = 0;

            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));

            foreach (EA.Element el in p.Elements)
            {
                foreach (EA.TaggedValue tv in el.TaggedValues)
                {
                    if ((tv.Name.Equals("isRoot", StringComparison.OrdinalIgnoreCase)) &&
                        (tv.Value.Equals("true", StringComparison.OrdinalIgnoreCase)))
                    {
                        countRootElement++;
                        rootElement = el.ElementID;
                        break;
                    }
                }
                if (countRootElement > 0)
                    break;
            }

            if (countRootElement == 0)
            {
                throw new Exception("There is none root element in the DOCLibrary. Please add or set 'isRoot' tagged value to 'true' first. Skipped.");
            }
            else if (countRootElement > 1)
            {
                this.appendErrorMessage("There are more than one root element in the DOCLibrary. Skipped.", p.Name);
            }
            
            EA.Package parentPkg = this.repository.GetPackageByID(this.repository.GetDiagramByID(diagramID).PackageID);

            if (parentPkg.Element.Stereotype.Equals(CCTS_PackageType.DOCLibrary.ToString()))
            {
                generatePresentationFileDOCLibrary();
            }
            
        }

        public void resetGenerator(int diagramID)
        {
            this.diagramID = diagramID;
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.scope = this.repository.GetDiagramByID(diagramID).PackageID.ToString();
            this.hash = new Hashtable();
            this.alreadyCreatedElement = new ArrayList();
            this.alreadyCreatedLinkedSchema = new ArrayList();
            this.alreadyCreatedPresentationArc = new ArrayList();
            this.setActivePackageLabel();
        }

        #endregion
    }
}