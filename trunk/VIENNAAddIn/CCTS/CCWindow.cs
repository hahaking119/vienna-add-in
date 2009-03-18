/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VIENNAAddIn.CCTS.CCTSBIE_MetaModel;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.CCTS {
    public partial class CCWindow : Form
    {

        #region Variables
        private EA.Repository repository;
        private String scope;
        private EA.Package selectedCCLibrary;

        private bool select = true;
        private bool blnChildForm = false;

        private int parentIndex = -1;
        EA.Element selectedElement;
        ArrayList attributes;
        ArrayList connectors;
        //Collection of Core Components
        private IList cc = null;
        
        private ArrayList associationColl;
        private bool blnError = false;
        #endregion


        #region Constructor
        /// <sUMM2ary>
        /// Initialize the window
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="ccLibraryID"></param>
        public CCWindow(EA.Repository repository, String scope) {

            this.repository = repository;
            this.scope = scope;
            this.blnChildForm = false;
   
            InitializeComponent();

            this.loadInitialCCLibaries();
        }

        public CCWindow(EA.Repository repository, String scope, IList cc, bool blnChildForm)
        {
            this.repository = repository;
            this.scope = scope;
            this.blnChildForm = true;
            this.cc = cc;

            InitializeComponent();
        }
        #endregion


        #region Event Handler 
        /// <sUMM2ary>
        /// Close Button
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e) {
            
            //determine the result of this button when clicked
            DialogResult = DialogResult.Cancel;

            //close current form
            this.Close();

            CCWindow parentFrm = ((CCWindow)this.Owner);
            if (parentFrm != null)
            {
                //set focus on parent form
                parentFrm.Select();
                parentFrm.Focus();
            }
        }

        /// <sUMM2ary>
        /// Insert the Business Information Entity / Qualified Data Type
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertABIE_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.blnError = false;

            //Get the package where we want to insert the BIE            
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));

            //Prefix needed?
            String prefix = "";
            if (this.prefix.Text != "")
                prefix = this.prefix.Text + "_";

            //Before we proceed with inserting a new BIE we have to check
            //whether a BIE with the same name already exists or not
            if (checkIfElementAlreadyExists(prefix + this.selectedElement.Name, p))
            {
                MessageBox.Show("Cannot create ABIE. An ABIE with this name already exists in the ABIE library. \nUse the existing ABIE or create a new with a different prefix.", "Warning");
                this.blnError = true;
            }

            if (!this.blnError)
            {
                try
                {

                    #region Insert Attributes
                    //Delete the attributes not needed
                    selectAttributes();

                    //Insert the element
                    EA.Element element = this.insertElement(p, "ABIE", prefix + this.selectedElement.Name, "Class", true, "BBIE", this.selectedElement.TaggedValuesEx);

                    //In the next step we have to insert a dependency stereotyped
                    //as basedOn leading from the ABIE to the ACC
                    EA.Connector con = (EA.Connector)p.Connectors.AddNew("", AssociationTypes.Dependency.ToString());
                    con.Stereotype = CCTS_Types.basedOn.ToString(); // "basedOn";

                    //Get the ACC
                    EA.Element acc = this.getACCFromLibrary(this.selectedElement.Name);
                    con.SupplierID = acc.ElementID;
                    con.ClientID = element.ElementID;
                    con.Update();
                    element.Update();
                    element.Refresh();
                    #endregion

                    #region Insert Connectors
                    //get selected connector
                    selectConnectors();

                    for (int i = 0; i < this.connectors.Count; i++)
                    {
                        CCTSBIE_MetaModel.ASCC tempAscc = (CCTSBIE_MetaModel.ASCC)this.connectors[i];
                        EA.Connector tempCon = (EA.Connector)p.Connectors.AddNew("", AssociationTypes.Aggregation.ToString());

                        tempCon.SupplierID = element.ElementID; 
                        EA.Element sourceClient = this.repository.GetElementByID(tempAscc.ClientID);
                        tempCon.ClientID = findClientIdInSelectedPackage(sourceClient.Name);//tempAscc.ClientID;
                        tempCon.ClientEnd.Role = tempAscc.RoleName;
                        tempCon.Notes = tempAscc.Notes;
                        tempCon.Stereotype = CCTS_Types.ASBIE.ToString();
                        tempCon.Update();

                        foreach (CCTSBIE_MetaModel.TaggedValue tv in tempAscc.TaggedValues)
                        {
                            EA.ConnectorTag conTag = (EA.ConnectorTag)tempCon.TaggedValues.AddNew(tv.Name, "");
                            conTag.Value = tv.Value;
                            conTag.Update();
                        }
                        element.Update();
                        tempCon.TaggedValues.Refresh();
                    }
                    element.Refresh();

                    #endregion

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed adding new BIE caused by " + ex.Message);
                }
                //Insert the new element into the diagram
                //drawElement(element);

                if (blnChildForm)
                {
                    this.Close();
                    this.Owner.Select();
                    this.Owner.Focus();
                }
                else
                    this.resetWindow(this.scope);
            }
            
        }

        /// <sUMM2ary>
        /// According to the selected ACCs, the content of the window changes
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ccomponents_SelectedIndexChanged(object sender, EventArgs earg)
        {
            this.chkSelectDeselectAttr.Checked = false;

            if (!this.tabControl1.Enabled)
                this.tabControl1.Enabled = true;

            if (!this.btnInsertABIE.Enabled)
                this.btnInsertABIE.Enabled = true;


            String selected = (String)((System.Windows.Forms.ComboBox)sender).SelectedItem;
            selectedElement = this.getACCFromLibrary(selected);

            #region Attribute Tab
            //Get all the attributes from the element and store it in a 
            //temporary List
            attributes = new ArrayList();
            foreach (EA.Attribute a in selectedElement.AttributesEx)
            {
                //Use the internal data structure
                //otherwise the object in the repository will
                //be affected which must not happen (the elements
                //in the CCrepository must not change)
                CCTSBIE_MetaModel.Attribute attribute = new CCTSBIE_MetaModel.Attribute();
                attribute.Name = a.Name;
                attribute.Type = a.Type;
                attribute.Stereotype = a.Stereotype;
                attribute.UpperBound = a.UpperBound;
                attribute.LowerBound = a.LowerBound;
                attribute.ClassifierID = a.ClassifierID;
                attribute.Notes = a.Notes;



                //Copy the TaggedValues of the Attribute as well
                ArrayList taggedValues = new ArrayList();

                foreach (EA.AttributeTag tv in a.TaggedValues)
                {
                    String nss = tv.Name;
                    CCTSBIE_MetaModel.TaggedValue taggedValue = new TaggedValue();
                    taggedValue.Name = tv.Name;
                    taggedValue.Value = tv.Value;
                    taggedValues.Add(taggedValue);
                }

                //Add the Array with the TaggedValues to the Attribute
                attribute.TaggedValues = taggedValues;
                attributes.Add(attribute);
            }

            fillAttributeBox(attributes);
            #endregion

            #region Association
            connectors = new ArrayList();
            foreach (EA.Connector con in selectedElement.Connectors)
            {
                if ((selectedElement.ElementID == con.SupplierID) && (con.Stereotype == CCTS_Types.ASCC.ToString()))//&& (con.ClientEnd.Role != ""))
                {

                    CCTSBIE_MetaModel.ASCC ascc = new CCTSBIE_MetaModel.ASCC();
                    ascc.ClientID = con.ClientID;
                    ascc.SupplierID = con.SupplierID;
                    ascc.RoleName = con.ClientEnd.Role;
                    ascc.Notes = con.Notes;
                    ascc.Name = con.Name;

                    ////Copy the TaggedValues of the Attribute as well
                    ArrayList taggedValues = new ArrayList();

                    foreach (EA.ConnectorTag tv in con.TaggedValues)
                    {
                        CCTSBIE_MetaModel.TaggedValue taggedValue = new TaggedValue();
                        taggedValue.Name = tv.Name;
                        taggedValue.Value = tv.Value;
                        taggedValues.Add(taggedValue);
                    }

                    //Add the Array with the TaggedValues to the Attribute
                    ascc.TaggedValues = taggedValues;
                    connectors.Add(ascc);
                }
            }

            fillConnectorBox(connectors);
            #endregion
        }

        /// <sUMM2ary>
        /// Fire when the choice box with the Source CCLibraries changes
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ccomponents.Items.Clear();
            this.ccomponents.ResetText();
            this.attributeBox.Items.Clear();
            this.clbAssociation.Items.Clear();
            this.chkSelectDeselectAttr.Checked = false;
            //Get the selected CCLibrary
            String selected = (String)((System.Windows.Forms.ComboBox)sender).SelectedItem;
            string packageNumber = "";
            //Get the selected item Packcage
            foreach (string pkgId in cc)
            {
                EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(pkgId));
                if (pkg.Name == selected)
                {
                    packageNumber = pkgId;
                    break;
                }
            }

            this.selectedCCLibrary = this.repository.GetPackageByID(Int32.Parse(packageNumber));
            //Get the Core Components from the CCLibary and add them to the choice box
            foreach (EA.Element cdt in this.selectedCCLibrary.Elements)
            {
                this.ccomponents.Items.Add(cdt.Name);
            }
        }

        /// <sUMM2ary>
        /// Select/Deselect all elements in the attribute list (if there are any)
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectDeselectAttr_CheckedChanged(object sender, EventArgs e)
        {

            if (this.attributeBox.Items.Count > 0)
            {
                attributeBox.BeginUpdate();
                //Iterate through the list elements
                for (int y = 0; y < this.attributeBox.Items.Count; y++)
                {
                    this.attributeBox.SetItemChecked(y, select);
                }
                attributeBox.EndUpdate();
            }

            if (select)
            {
                this.chkSelectDeselectAttr.Checked = true;
                select = false;
            }
            else
            {
                this.chkSelectDeselectAttr.Checked = false;
                select = true;
            }

        }

        private void clbAssociation_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                int index = e.Index;
                this.parentIndex = index;

                //target element != selected CC Element (on dropdownlist) and the associated element stereotyped as 
                //if (this.ccomponents.SelectedItem != ccomponents.SelectedIndex)
                //{
                //}

                //check whether the target ABIE already exists
                if (!checkTargetElementAlreadyExist(index))
                {
                    DialogResult result = MessageBox.Show("Target element of association doesn't exist. Do you want to create it?", "Info", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.OK)
                    {
                        CCTSBIE_MetaModel.ASCC selectedCon = (CCTSBIE_MetaModel.ASCC)connectors[index];
                        EA.Element clientElement = this.repository.GetElementByID(selectedCon.ClientID);
                        //create new instance of CCWindow
                        CCWindow target = new CCWindow(this.repository, this.scope, this.cc, true);
                        target.Owner = this;

                        target.sourceCCLibrary.Enabled = false;
                        foreach (string pkgId in target.cc)
                        {
                            target.sourceCCLibrary.Items.Add(this.repository.GetPackageByID(Int32.Parse(pkgId)).Name);
                        }
                        target.sourceCCLibrary.SelectedItem = this.repository.GetPackageByID(clientElement.PackageID).Name;

                        target.ccomponents.Enabled = false;
                        target.ccomponents.SelectedItem = clientElement.Name;

                        ArrayList colAttr = collectAttributesOfTargetElement(clientElement);
                        target.fillAttributeBox(colAttr);

                        ArrayList colConn = collectConnectorsOfTargetElement(clientElement);
                        target.fillConnectorBox(colConn);
                        target.connectors = colConn;

                        //show child form
                        target.Select();
                        target.Focus();

                        DialogResult targetResult = target.ShowDialog(this);
                        if (targetResult == DialogResult.Cancel)
                        {
                            e.NewValue = e.CurrentValue;
                        }
                    }
                    else
                    {
                        e.NewValue = e.CurrentValue;
                    }
                }
            }
        }

        #endregion


        #region Method
        /// <sUMM2ary>
        /// Reset the GUI
        /// </sUMM2ary>
        public void resetWindow(String scope) {
            this.scope = scope;
            
            //Remove the CCs from the Choice Box
            this.ccomponents.Items.Clear();
            this.ccomponents.ResetText();
            this.attributeBox.Items.Clear();
            this.clbAssociation.Items.Clear();
            this.tabControl1.SelectedTab = tabPage1;
            this.sourceCCLibrary.Items.Clear();
            this.sourceCCLibrary.ResetText();
            this.tabControl1.Enabled = false;
            this.btnInsertABIE.Enabled = false;
            this.prefix.Text = "";
            this.selectedElement = null;
            this.attributes = null;
            this.chkSelectDeselectAttr.Checked = false;
            
            this.loadInitialCCLibaries();
        }


        /// <sUMM2ary>
        /// The BCSS Folder can contain more CC-Libraries
        /// Show the different CCLibraries in the ChoiceBox
        /// </sUMM2ary>
        private void loadInitialCCLibaries() {
           
            //No iterate through all of the subpackages and search for packages
            //stereotyped as CCLibrary
            //Undone due to a request by Sparx (20060404)
            //EA.Package rootPackage = (EA.Package)repository.Models.GetAt(0);
           
            //Get a list of all CCLibraries available
            IList sourceCCLibaries = new ArrayList();
            foreach (EA.Package root in this.repository.Models)
            {
                //Get a list of all CCLibraries available under the BCSS 
                Utility.getAllSubPackagesRecursively2(root, sourceCCLibaries, CCTS_Types.CCLibrary.ToString());
            }

            this.cc = sourceCCLibaries; //variable 'cc' contain all packages id of the CC Library

            //IList sourceCCLibaries = new ArrayList();
            //Utility.getAllSubPackagesRecursively(rootPackage, sourceCCLibaries, CCTS_Types.CCLibrary.ToString());
            this.sourceCCLibrary.Items.Clear();
            this.sourceCCLibrary.ResetText();
            foreach (string pkgId in sourceCCLibaries) {
                this.sourceCCLibrary.Items.Add(this.repository.GetPackageByID(Int32.Parse(pkgId)).Name);
            } 
        }

        private void fillConnectorBox(ArrayList connectors)
        {
            this.clbAssociation.Items.Clear();
            this.clbAssociation.BeginUpdate();
            
            foreach (CCTSBIE_MetaModel.ASCC con in connectors)
            {
                if (con.RoleName != "")
                {
                    string clientName = con.RoleName + "." + this.repository.GetElementByID(con.ClientID).Name;
                    this.clbAssociation.Items.Add(clientName);
                }
                else
                {
                    string clientName = this.repository.GetElementByID(con.ClientID).Name;
                    this.clbAssociation.Items.Add(clientName);
                }
                //this.clbAssociation.Items.Add(con.RoleName);
            }
            this.clbAssociation.EndUpdate();
        }

        /// <sUMM2ary>
        /// Fill the attribute box
        /// </sUMM2ary>
        private void fillAttributeBox(ArrayList attributes) {
            this.attributeBox.Items.Clear();
            this.attributeBox.BeginUpdate();
            foreach (CCTSBIE_MetaModel.Attribute a in attributes) {
                this.attributeBox.Items.Add(a.Name);
            }
            this.attributeBox.EndUpdate();
        }

        private int findClientIdInSelectedPackage(string elementName)
        {
            EA.Package selectedPkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            foreach (EA.Element el in selectedPkg.Elements)
            {
                if ((el.Name == elementName) ||              //for element with no qualifier name
                    (el.Name.EndsWith("_" + elementName)))   //for element with qualifier name, e.g. my_Address
                {
                    return el.ElementID;
                }
            }

            return 0;
        }

        private void selectConnectors()
        {
            ArrayList selectedCon = new ArrayList();

            foreach (object itemChecked in this.clbAssociation.CheckedItems)
            {
                for (int i = 0; i < this.connectors.Count; i++)
                {
                    int index = itemChecked.ToString().IndexOf(".");
                    string substr1 = "";
                    string substr2 = "";

                    if (index >= 0)
                    {
                        substr1 = itemChecked.ToString().Substring(0, index);
                        substr2 = itemChecked.ToString().Substring(index + 1, itemChecked.ToString().Length - index - 1);
                    }
                    else
                        substr2 = itemChecked.ToString();

                    int clientId = ((CCTSBIE_MetaModel.ASCC)connectors[i]).ClientID;
                    string clientName = this.repository.GetElementByID(clientId).Name;
                    string roleName = ((CCTSBIE_MetaModel.ASCC)connectors[i]).RoleName;

                    if (index >= 0)
                    {
                        if ((clientName == substr2) && (roleName == substr1))
                        {
                            selectedCon.Add(this.connectors[i]);
                            break;
                        }
                    }
                    else
                    {
                        if (clientName == substr2)
                        {
                            selectedCon.Add(this.connectors[i]);
                            break;
                        }
                    }
                }
            }
            this.connectors = selectedCon;
        }


        /// <sUMM2ary>
        /// Delete the attributes which the user has marked in the box
        /// </sUMM2ary>
        private void selectAttributes()
        {
            ArrayList selected = new ArrayList();

            foreach (object itemChecked in this.attributeBox.CheckedItems)
            {
                for (int i = 0; i < this.attributes.Count; i++)
                {
                    String s = itemChecked.ToString();
                    if (((CCTSBIE_MetaModel.Attribute)attributes[i]).Name == itemChecked.ToString())
                    {
                        selected.Add(this.attributes[i]);
                        break;
                    }
                }
            }
            this.attributes = selected;
        }


        /// <sUMM2ary>
        /// Draw the element into the diagram
        /// </sUMM2ary>
        private void drawElement(EA.Element e) {
            
            //Get the diagram where the mouse is currently
            EA.Diagram diagram = repository.GetCurrentDiagram();
            this.repository.SaveDiagram(diagram.DiagramID);
                        
            EA.DiagramObject abie = (EA.DiagramObject)diagram.DiagramObjects.AddNew("l=113;r=295;t=-393;b=-463;", "");
            
            abie.InstanceID = e.ElementID;
            abie.ElementID = e.ElementID;
            abie.Update();
            this.repository.SaveDiagram(diagram.DiagramID);                         
            this.repository.RefreshModelView(diagram.PackageID);
            this.repository.Models.Refresh();


        }


        /// <sUMM2ary>
        /// Return the ACC from the library
        /// </sUMM2ary>
        /// <param name="name"></param>
        /// <returns></returns>
        private EA.Element getACCFromLibrary(String name) {
            
            EA.Element e = null;
            foreach (EA.Element element in this.selectedCCLibrary.Elements) {
                if (element.Name == name) {
                    e = element;
                    break;
                }
            }
            return e;
        }

        /// <sUMM2ary>
        /// Inserts an element in the given package with the given stereotype and with the given name
        /// </sUMM2ary>
        /// <param name="package"></param>
        /// <param name="stereotype"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        private EA.Element insertElement(EA.Package package, String stereotype, String name, String type, bool addAttributes, string sterotypeOfAttribute, EA.Collection taggedValues) {

            //Create new Element
            EA.Element element = (EA.Element)package.Elements.AddNew(name, type);
            element.Stereotype = stereotype;
            element.Update();

            //Add the taggedValues to the element
            foreach (EA.TaggedValue tv in taggedValues) {
                EA.TaggedValue tv_new = (EA.TaggedValue)element.TaggedValues.AddNew(tv.Name, "");
                tv_new.Value = tv.Value;
                tv_new.Update();
            }
            //copy Notes property
            element.Notes = this.selectedElement.Notes;
                        
            if (addAttributes) { 
                //Add the attributes to the element
                foreach (CCTSBIE_MetaModel.Attribute attribute in this.attributes) {
                    EA.Attribute a = (EA.Attribute)element.Attributes.AddNew(attribute.Name, attribute.Type);
                
                    a.Stereotype = sterotypeOfAttribute;
                                        
                    a.LowerBound = attribute.LowerBound;
                    a.UpperBound = attribute.UpperBound;
                    a.ClassifierID = attribute.ClassifierID;
                    //copy Notes property
                    a.Notes = attribute.Notes;
                    a.Update();
                    //Add the tagged values of the attribute
                    foreach (CCTSBIE_MetaModel.TaggedValue tv in attribute.TaggedValues) {
                        EA.AttributeTag attributeTag = (EA.AttributeTag)a.TaggedValues.AddNew(tv.Name,tv.Type);
                        attributeTag.Value = tv.Value;
                        attributeTag.Update();                         
                    }                    
                    a.Update();                                        
                    a.TaggedValues.Refresh();
                }                                
            }
                        
            element.Update();
            element.Attributes.Refresh();
            element.Refresh();

                       
            return element;
        }


        /// <sUMM2ary>
        /// Checks whether the ABIE with the given name already exists or not
        /// Returns true if the ABIE exists
        /// </sUMM2ary>
        /// <param name="ABIEname"></param>
        /// <returns></returns>
        private bool checkIfElementAlreadyExists(String elementName, EA.Package p) {
            foreach (EA.Element e in p.Elements) {
                if (e.Name == elementName) 
                {
                    return true;
                }
            }
            return false ;
        }

        private ArrayList collectConnectorsOfTargetElement(EA.Element clientElement)
        {
            ArrayList tempConn = new ArrayList();
            foreach (EA.Connector con in clientElement.Connectors)
            {
                if ((clientElement.ElementID == con.SupplierID) && (con.Stereotype == CCTS_Types.ASCC.ToString()))//&& (con.ClientEnd.Role != ""))
                {

                    CCTSBIE_MetaModel.ASCC ascc = new CCTSBIE_MetaModel.ASCC();
                    ascc.ClientID = con.ClientID;
                    ascc.SupplierID = con.SupplierID;
                    ascc.RoleName = con.ClientEnd.Role;
                    ascc.Notes = con.Notes;
                    ascc.Name = con.Name;

                    ////Copy the TaggedValues of the Attribute as well
                    ArrayList taggedValues = new ArrayList();

                    foreach (EA.ConnectorTag tv in con.TaggedValues)
                    {
                        CCTSBIE_MetaModel.TaggedValue taggedValue = new TaggedValue();
                        taggedValue.Name = tv.Name;
                        taggedValue.Value = tv.Value;
                        taggedValues.Add(taggedValue);
                    }

                    //Add the Array with the TaggedValues to the Attribute
                    ascc.TaggedValues = taggedValues;
                    tempConn.Add(ascc);
                }
            }
            return tempConn;
        }

        private ArrayList collectAttributesOfTargetElement(EA.Element clientElement)
        {
            //Get all the attributes from the element and store it in a 
            //temporary List
            ArrayList attr = new ArrayList();
            foreach (EA.Attribute a in clientElement.AttributesEx)
            {
                //Use the internal data structure
                //otherwise the object in the repository will
                //be affected which must not happen (the elements
                //in the CCrepository must not change)
                CCTSBIE_MetaModel.Attribute attribute = new CCTSBIE_MetaModel.Attribute();
                attribute.Name = a.Name;
                attribute.Type = a.Type;
                attribute.Stereotype = a.Stereotype;
                attribute.UpperBound = a.UpperBound;
                attribute.LowerBound = a.LowerBound;
                attribute.ClassifierID = a.ClassifierID;
                attribute.Notes = a.Notes;

                //Copy the TaggedValues of the Attribute as well
                ArrayList taggedValues = new ArrayList();

                foreach (EA.AttributeTag tv in a.TaggedValues)
                {
                    String nss = tv.Name;
                    CCTSBIE_MetaModel.TaggedValue taggedValue = new TaggedValue();
                    taggedValue.Name = tv.Name;
                    taggedValue.Value = tv.Value;
                    taggedValues.Add(taggedValue);
                }

                //Add the Array with the TaggedValues to the Attribute
                attribute.TaggedValues = taggedValues;
                attr.Add(attribute);
            }
            return attr;
        }

        private bool checkTargetElementAlreadyExist(int index)
        {
            //get the connector from arraylist
            CCTSBIE_MetaModel.ASCC selectedCon = (CCTSBIE_MetaModel.ASCC)connectors[index];
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            string clientName = this.repository.GetElementByID(selectedCon.ClientID).Name;

            //target element exist?
            foreach (EA.Element el in pkg.Elements)
            {
                if ((el.Name == clientName) ||              //for element with no qualifier name
                    (el.Name.EndsWith("_" + clientName)))   //for element with qualifier name, e.g. my_Address
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

        private void CCWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //to prevent child dialog closing when error occurs caused by the new created element already exists
            if (this.blnChildForm && this.blnError)
            {
                e.Cancel = true;
            }
        }
    }
}