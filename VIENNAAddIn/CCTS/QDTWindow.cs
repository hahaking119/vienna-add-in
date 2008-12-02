/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further on the VIENNAAddIn please visit http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VIENNAAddIn.CCTS.CCTSBIE_MetaModel;
using VIENNAAddIn.Utils;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.CCTS {
    public partial class QDTWindow : Form {

        
        private EA.Repository repository;
        private String scope;

        private bool select = true;

        EA.Element selectedElement;
        EA.Package selectedCDTLibrary;
        ArrayList attributes;
        //Collection of CDTs
        private IList cdts = null;

                

        /// <sUMM2ary>
        /// Initialize the window
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="ccLibraryID"></param>
        public QDTWindow(EA.Repository repository, String scope) {

            this.repository = repository;
            this.scope = scope;
                                    
            InitializeComponent();

            loadSourceCDTLibraries();
             
        }


        /// <sUMM2ary>
        /// Close Button
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            this.Visible = false;
            this.resetWindow("");
        }


        /// <sUMM2ary>
        /// Reset the GUI
        /// </sUMM2ary>
        public void resetWindow(String scope) {
            this.scope = scope;

            //Remove the CCs from the Choice Box
            this.coredatatypes.Items.Clear();
            this.coredatatypes.ResetText();
            this.attributeBox.Items.Clear();
            this.sourceCDTLibrary.Items.Clear();
            this.sourceCDTLibrary.ResetText();

            this.tabControl1.Enabled = false;
            this.button2.Enabled = false;
            this.prefix.Text = "";
            this.selectedElement = null;
            this.attributes = null;

            loadSourceCDTLibraries();
        }


        /// <sUMM2ary>
        /// Load the available CDT Libraries which can be found 
        /// </sUMM2ary>
        private bool loadSourceCDTLibraries() {

            this.sourceCDTLibrary.Items.Clear();

            IList sourceCDTLibaries = new ArrayList();
            //Get a list of all CDTLibraries available 
            foreach (EA.Package root in this.repository.Models)
            {
                //Get a list of all CCLibraries available under the BCSS 
                Utility.getAllSubPackagesRecursively2(root, sourceCDTLibaries, CCTS_Types.CDTLibrary.ToString());
            }

            this.cdts = sourceCDTLibaries;

            foreach (string pkgId in sourceCDTLibaries) {
                this.sourceCDTLibrary.Items.Add(this.repository.GetPackageByID(Int32.Parse(pkgId)).Name);
            }

            return true;
        }




        /// <sUMM2ary>
        /// According to the selected ACCs, the content of the window changes
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ccomponents_SelectedIndexChanged(object sender, EventArgs earg) {
            this.checkBox1.Checked = false;
            if (!this.tabControl1.Enabled)
                this.tabControl1.Enabled = true;

            if (!this.button2.Enabled)
                this.button2.Enabled = true;


            String selected = (String)((System.Windows.Forms.ComboBox)sender).SelectedItem;

            //Get the element
            selectedElement = this.getCDTFromLibrary(selected);

            //Get all the attributes from the element and store it in a 
            //temporary List
            attributes = new ArrayList();
            foreach (EA.Attribute a in selectedElement.AttributesEx) {
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

                               
                //Copy the TaggedValues of the Attribute as well
                ArrayList taggedValues = new ArrayList();         
       
                foreach (EA.AttributeTag tv in a.TaggedValues) {
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

            fillAttributeBox();

        }


        /// <sUMM2ary>
        /// Fill the attribute box
        /// </sUMM2ary>
        private void fillAttributeBox() {
            this.attributeBox.Items.Clear();
            foreach (CCTSBIE_MetaModel.Attribute a in attributes) {
                this.attributeBox.Items.Add(a.Name);
            }
        }


        /// <sUMM2ary>
        /// Insert the Business Information Entity / Qualified Data Type
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {

                //Insert Qualified Data Type
                //Get the QDT-Package
                EA.Package qdtLibrary = this.repository.GetPackageByID(Int32.Parse(this.scope));
                
                //Prefix needed?
                String prefix = "";
                if (this.prefix.Text != "")
                    prefix = this.prefix.Text + "_";

                //Before we proceed with inserting a new QDT we have to check
                //whether a QDT with the same name already exists or not
                if (checkIfElementAlreadyExists(prefix + this.selectedElement.Name, qdtLibrary)) {
                    MessageBox.Show("Cannot create QDT. A QDT with this name already exists in the QDT library. \nUse the existing QDT or create a new with a different prefix.", "Warning");
                    return;
                }

                //Delete the attributes not needed
                selectAttributes();

                //Insert the element
                EA.Element element = this.insertElement(qdtLibrary, "QDT", prefix + this.selectedElement.Name, "Class", true, "", this.selectedElement.TaggedValuesEx);

                //In the next step we have to insert a dependency stereotyped
                //as basedOn leading from the QDT to the CDT
                EA.Connector con = (EA.Connector)qdtLibrary.Connectors.AddNew("", "Dependency");
                con.Stereotype = "basedOn";
                //Get the CDT
                EA.Element cdt = this.getCDTFromLibrary(this.selectedElement.Name);
                con.SupplierID = cdt.ElementID;
                con.ClientID = element.ElementID;
                con.Update();
                element.Update();
                element.Refresh();

            
            this.resetWindow("");
            this.Close();

        }

        
        /// <sUMM2ary>
        /// Delete the attributes which the user has marked in the box
        /// </sUMM2ary>
        private void selectAttributes() {
            ArrayList selected = new ArrayList();
            foreach (object itemChecked in this.attributeBox.CheckedItems) {
                for (int i = 0; i < this.attributes.Count; i++) {
                    String s = itemChecked.ToString();
                    if (((CCTSBIE_MetaModel.Attribute)attributes[i]).Name == itemChecked.ToString()) {
                        selected.Add(attributes[i]);
                        break;
                    }
                }
            }
            //The selected elements are now in the ArrayList selected
            //insert them there
            this.attributes = selected;

        }


        /// <sUMM2ary>
        /// Returns the CDT with the given name from the CDT library
        /// </sUMM2ary>
        /// <param name="name"></param>
        /// <returns></returns>
        private EA.Element getCDTFromLibrary(String name) {
            
            EA.Package p = null;
            EA.Element e = null;
            foreach (EA.Element element in this.selectedCDTLibrary.Elements) {
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
                        
            if (addAttributes) { 
                //Add the attributes to the element
                foreach (CCTSBIE_MetaModel.Attribute attribute in this.attributes) {
                    EA.Attribute a = (EA.Attribute)element.Attributes.AddNew(attribute.Name, attribute.Type);

                    //If the mode is QDT, then every attribute will get the stereotype which
                    //has been taken from the CDT which is the base for the QDT
                    a.Stereotype = attribute.Stereotype;

                    a.LowerBound = attribute.LowerBound;
                    a.UpperBound = attribute.UpperBound;
                    a.ClassifierID = attribute.ClassifierID;
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
                if (e.Name == elementName) {
                    return true;
                }
            }
            return false ;
        }

        /// <sUMM2ary>
        /// Every time the chosen element in the source CDTLibrary changes,
        /// the CDTs must be reloaded
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceCDTLibrary_SelectedIndexChanged(object sender, EventArgs e) {
            this.checkBox1.Checked = false;
            this.coredatatypes.Items.Clear();
            this.coredatatypes.ResetText();
            this.attributeBox.Items.Clear();
            //Get the selected CDTLibrary
            String selected = (String)((System.Windows.Forms.ComboBox)sender).SelectedItem;

            string packageNumber = "";
            //Get the selected item Packcage
            foreach (string pkgId in cdts)
            {
                EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(pkgId));
                if (pkg.Name == selected)
                {
                    packageNumber = pkgId;
                    break;
                }
            }

            this.selectedCDTLibrary = this.repository.GetPackageByID(Int32.Parse(packageNumber));
            //Get the Core Components from the CCLibary and add them to the choice box
            foreach (EA.Element cdt in this.selectedCDTLibrary.Elements)
            {
                this.coredatatypes.Items.Add(cdt.Name);
            }

            
            ////Get the root package
            //EA.Package rootPackage = (EA.Package)this.repository.Models.GetAt(0);
            
            ////Search in the BCSS package for the CDT-Library
            //IList cdts = new ArrayList();
            //Utility.getAllSubPackagesWithSpecificNameRecursively(rootPackage, cdts, selected , CCTS_Types.CDTLibrary.ToString());
            
            ////If more than one CDTs is returned that means, that there is more than one CDTLibrary
            ////within the subpackages with the same name
            //if (cdts.Count > 1) {
            //    MessageBox.Show("There is more than one CDTLibrary within the package Core Component Model with the same name. No two CDTLibraries must have the same name. Correct the names and retry creating a QDT.", "Error");
            //    return;
            //}
            //this.selectedCDTLibrary = (EA.Package)cdts[0];

            ////Get the CDTs from the CDTLibary and add them to the choice box
            //foreach (EA.Element cdt in ((EA.Package)cdts[0]).Elements) {
            //    this.coredatatypes.Items.Add(cdt.Name);
            //}


        }

        /// <sUMM2ary>
        /// Is called when the checkbox select/deselect all is checked
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (this.attributeBox.Items.Count > 0) {
                attributeBox.BeginUpdate();
                //Iterate through the list elements
                for (int y = 0; y < this.attributeBox.Items.Count; y++) {
                    this.attributeBox.SetItemChecked(y, select);
                }
                attributeBox.EndUpdate();
            }

            if (select) {
                this.checkBox1.Checked = true;
                select = false;
            }
            else {
                this.checkBox1.Checked = false;
                select = true;
            }
        }

   }
}