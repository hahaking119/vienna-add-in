/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EA;
using System.Collections;

namespace VIENNAAddIn.ExportImport
{
    public partial class ImportPackage : Form
    {
        #region Variables
        private EA.Repository repository;
        private string scope = "";
        private string path = "";
        private ArrayList relationCollection = new ArrayList();
        private ArrayList classCollection = new ArrayList();
        private ArrayList tvCollection = new ArrayList();
        private int createdPackageID = 0;
        private DataTable tvTable;
        private string delimiter = "";
        #endregion

        #region Constructor
        public ImportPackage(EA.Repository repository, string scope)
        {
            InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            this.cmbDelimiter.SelectedIndex = 0;
            this.setActivePackageLabel();
        }
        #endregion

        #region Button
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpenCSVFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.path = dlgOpenCSVFile.FileName;
                resetGenerator(this.scope);
                txtPath.Text = dlgOpenCSVFile.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //resetGenerator(this.scope);
            
            //clear text area..
            statusTextBox.Text = "";

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            
            if (cmbDelimiter.SelectedItem.ToString() == "TAB")
                this.delimiter = "\t";
            else
                this.delimiter = cmbDelimiter.SelectedItem.ToString(); //txtDelimiter.Text;

            //string fileTaggedValue = this.path.Substring(0, this.path.Length - System.IO.Path.GetFileName(this.path).Length) +
            //    System.IO.Path.GetFileNameWithoutExtension(this.path) + "-taggedValue.csv";

            //if (!System.IO.File.Exists(fileTaggedValue))
            //{
            //    this.appendErrorMessage("Can't find tagged value file : " + fileTaggedValue + 
            //        "\nPlease ensure that the file exist", pkg.Name);
            //    return;
            //}
            this.appendInfoMessage("Start exporting...", pkg.Name);
            this.Refresh();
            
            //AddTaggedValue(pkg);
            if (doImport())
                this.appendInfoMessage("Finished exporting.", pkg.Name);
        }

        private void AddTaggedValue(EA.Package pkg)
        {
            foreach (EA.Element elContainer in pkg.Elements)
            {
                EA.Attribute newAttribute = (EA.Attribute)elContainer.Attributes.AddNew("MyAttribute", "");
                newAttribute.Update();
                elContainer.Attributes.Refresh();
                elContainer.Refresh();

                EA.AttributeTag newAttr = (EA.AttributeTag)newAttribute.TaggedValues.AddNew("TagName", "");
                newAttr.Value = "TagValue";
                newAttr.Update();

                newAttribute.TaggedValues.Refresh();
                elContainer.Attributes.Refresh();


                //EA.Attribute alias = newAttribute;
                //EA.AttributeTag newAttr = (EA.AttributeTag)alias.TaggedValues.AddNew("TagName", "");
                //newAttr.Value = "TagValue";
                //newAttr.Update();

                //alias.TaggedValues.Refresh();
                //elContainer.Attributes.Refresh();

                
                //pkg.Elements.Refresh();
                //this.repository.RefreshModelView(pkg.PackageID);

                //foreach (EA.AttributeTag attr in newAttribute.TaggedValues)
                //{
                //    //int elContainerID = elContainer.ElementID;
                //    //EA.Element el = this.repository.GetElementByID(elContainerID).g;
                //    //EA.Attribute attr = 
                //    EA.AttributeTag attrTag = (EA.AttributeTag)newAttribute.TaggedValues.AddNew("TagName", "");
                //    attrTag.Value = "TagValue";
                //    attrTag.Update();
                //    newAttribute.TaggedValues.Refresh();
                //    break;
                //}
                //break;
                //newAttribute.TaggedValues.Refresh();

                //elContainer.Update();

                //foreach (EA.Attribute attr in el.Attributes)
                //{
                //    //foreach (EA.AttributeTag attrTag in attr.TaggedValues)
                //    //{
                //    //    MessageBox.Show("ini isi tv = " + attrTag.Name + "=" + attrTag.Value);
                //    //    //EA.Attribute newAttribute = (EA.Attribute)elContainer.Attributes.AddNew("MyAttribute", "");
                //    //    //newAttribute.Update();

                //        EA.AttributeTag newAttr = (EA.AttributeTag)attr.TaggedValues.AddNew("TagName", "");
                //        newAttr.Value = "TagValue";
                //        newAttr.Update();

                //        attr.TaggedValues.Refresh();
                //        break;
                //    //}
                //}
            }

            //foreach (EA.Element elContainer in pkg.Elements)
            //{
            //    foreach (EA.Attribute attr in elContainer.Attributes)
            //    {
            //        //foreach (EA.AttributeTag attrTag in attr.TaggedValues)
            //        //{
            //        //    MessageBox.Show("ini isi tv = " + attrTag.Name + "=" + attrTag.Value);
            //        //    //EA.Attribute newAttribute = (EA.Attribute)elContainer.Attributes.AddNew("MyAttribute", "");
            //        //    //newAttribute.Update();

            //        EA.AttributeTag newAttr = (EA.AttributeTag)attr.TaggedValues.AddNew("TagName", "");
            //        newAttr.Value = "TagValue";
            //        newAttr.Update();

            //        attr.TaggedValues.Refresh();
            //        elContainer.Attributes.Refresh();
            //        break;
            //        //}
            //    }
            //}
        }
        #endregion

        #region Method
        public void resetGenerator(string scope)
        {
            this.scope = scope;
            this.txtPath.Text = "";
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();

            this.relationCollection = new ArrayList();
            this.classCollection = new ArrayList();
            this.tvCollection = new ArrayList();
        }

        private bool doImport()
        {
            FileStream fs = null;
            StreamReader sw = null;

            FileStream fsTaggedValue = null;
            StreamReader swTaggedValue = null;

            try
            {
                //open File
                fs = new FileStream(this.path, FileMode.Open, FileAccess.Read);
                sw = new StreamReader(fs);
                bool firstLine = true;

                string fileTaggedValue = this.path.Substring(0, this.path.Length - System.IO.Path.GetFileName(this.path).Length) +
                    System.IO.Path.GetFileNameWithoutExtension(this.path) + "-taggedValue.csv";

                fsTaggedValue = new FileStream(fileTaggedValue, FileMode.Open, FileAccess.Read);
                swTaggedValue = new StreamReader(fsTaggedValue);
                bool firstLineTv = true;

            
                while (!sw.EndOfStream)
                {
                    string line = sw.ReadLine();
                    string[] collection = line.Split(this.delimiter.ToCharArray(), StringSplitOptions.None);

                    if (firstLine)//first line is a table header
                    {
                        firstLine = false;
                        //if (collection.Length != 11)
                        //{
                        //    throw new Exception("Not valid CSV file to import. The CSV file must have 11 columns");
                        //}
                        continue;
                    }

                    if (isRelation(collection))
                        this.relationCollection.Add(collection);
                    else
                        this.classCollection.Add(collection);

                }

                while (!swTaggedValue.EndOfStream)
                {
                    string line = swTaggedValue.ReadLine();
                    string[] collection = line.Split(this.delimiter.ToCharArray(), StringSplitOptions.None);

                    if (firstLineTv)//first line is a table header
                    {
                        firstLineTv = false;
                        continue;
                    }
                    this.tvCollection.Add(collection);

                }

                //convert the tagged value array list into data table in order to make it easy to search
                ConvertToDataTable();

                //create class in collection
                CreateNewClass(classCollection);

                //create relation colection
                CreateNewRelation(relationCollection);
            }
            catch (Exception ex)
            {
                this.appendErrorMessage("Failed to export due to error : " + ex.Message, "");
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();

                swTaggedValue.Close();
                fsTaggedValue.Close();
            }

            return true;
        }

        /**
         * Content of collection array :
         * [0] Package Name
         * [1] Element Name
         * [2] Object
         * [3] Property
         * [4] Representation
         * [5] IsRelation
         * [6] min
         * [7] max
         * [8] Related Package
         * [9] CCTS Type
         * [10] Notes
         * [11] Initial Value
         * [12] ElementID
         * */
        //private void CreateNewClass(string[] collection)
        private void CreateNewClass(ArrayList classCollection)
        {
            //string currentPackage = "curr";
            //string prevPackage = "prev";

            //string currentElemet = "";
            //string prevElement = "";
            this.createdPackageID = 0;

            foreach (string[] collection in classCollection)
            {
                //currentPackage = collection[0];

                string pkgID = "";
                //if (prevPackage != currentPackage)
                //{
                    //Create Package
                    if ((pkgID = isPackageAlreadyExist(collection[0])) == "") //&& (this.createdPackageID == 0))
                    {
                        EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));
                        EA.Package newPackage = (EA.Package)selectedPackage.Packages.AddNew(collection[0], "");
                        
                        newPackage.Update();
                        pkgID = newPackage.PackageID.ToString();
                    }
                    else
                        this.createdPackageID = Int32.Parse(pkgID);
                //}

                //if (prevElement != currentElemet)
                //{
                    //Create Element
                    string elementID = "";
                    if ((elementID = isElementAlreadyExist(collection[2], pkgID)) == "")
                    {
                        EA.Package container = this.repository.GetPackageByID(Int32.Parse(pkgID));
                        EA.Element newElement = (EA.Element)container.Elements.AddNew(collection[2], "Class");
                        newElement.Stereotype = collection[9] == "CC" ? "ACC" : "ABIE";
                        
                        //create tagged value
                        //CreateElementTaggedValue(newElement, collection[12]);
                        
                        newElement.Update();
                        newElement.Refresh();
                        elementID = newElement.ElementID.ToString();
                    }
                //}
                //Create Attribute
                if (!isAttributeAlreadyExist(collection[3], elementID))
                {
                    EA.Element elContainer = this.repository.GetElementByID(Int32.Parse(elementID));
                    EA.Attribute newAttribute = (EA.Attribute)elContainer.Attributes.AddNew(collection[3], "");
                    newAttribute.Type = collection[4];
                    newAttribute.LowerBound = collection[6];
                    newAttribute.UpperBound = collection[7];
                    newAttribute.Stereotype = collection[9] == "CC" ? "BCC" : "BBIE";
                    newAttribute.Notes = collection[10];
                    newAttribute.Default = collection[11];
                    newAttribute.Update();
                    ////CreateAttributeTaggedValue(elContainer, newAttribute, collection[12]);

                    DataRow[] foundRows;
                    foundRows = this.tvTable.Select("ElementID = '" + collection[12] + "'");

                    EA.Attribute alias = newAttribute;
                    for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
                    {
                        EA.AttributeTag newAttr = (EA.AttributeTag)alias.TaggedValues.AddNew(foundRows[i][1].ToString(), "");
                        newAttr.Value = foundRows[i][2].ToString();
                        newAttr.Update();

                        alias.TaggedValues.Refresh();
                        elContainer.Attributes.Refresh();
                    }
                    
                    elContainer.Attributes.Refresh();
                    elContainer.Refresh();
                }
            }
        }

        private void CreateAttributeTaggedValue(EA.Element element, EA.Attribute attribute, string id)
        {
            //find the id in the tagged value file.
            DataRow[] foundRows;
            foundRows = this.tvTable.Select("ElementID = '" + id + "'");

            for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
            {
                EA.AttributeTag attrTag = (EA.AttributeTag)attribute.TaggedValues.AddNew(foundRows[i][1].ToString(), "");
                attrTag.Value = foundRows[i][2].ToString();
                attrTag.Update();
            }
            attribute.Update();
        }

        private void ConvertToDataTable()
        {
            //create DataTable
            this.tvTable = new DataTable();
            tvTable.Columns.Add("ElementID");
            tvTable.Columns.Add("TagName");
            tvTable.Columns.Add("TagValue");

            foreach (string[] tvData in this.tvCollection)
            {
                //string[] tvData = line.Split(new char[] { ',' });

                DataRow row = tvTable.NewRow();
                row[0] = tvData[0]; //ElementID column
                row[1] = tvData[1]; //TagName column
                row[2] = tvData[2]; //TagValue column
                tvTable.Rows.Add(row);
            }
        }

        private void CreateElementTaggedValue(EA.Element element, string id)
        {
            DataRow[] foundRows;
            foundRows = this.tvTable.Select("ElementID = '" + id + "'");

            for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
            {
                EA.TaggedValue elementTv = (EA.TaggedValue)element.TaggedValues.AddNew(foundRows[i][1].ToString(), "");
                elementTv.Value = foundRows[i][2].ToString();
                elementTv.Update();
            }
            element.Refresh();
        }

        private void CreateConnectorTaggedValue(EA.Connector connector,string id)
        {
            DataRow[] foundRows;
            foundRows = this.tvTable.Select("ElementID = '" + id + "'");

            for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
            {
                EA.ConnectorTag connectorTv = (EA.ConnectorTag)connector.TaggedValues.AddNew(foundRows[i][1].ToString(), "");
                connectorTv.Value = foundRows[i][2].ToString();
                connectorTv.Update();
            }
            connector.Update();
        }

        //private void CreatePackageTaggedValue()
        //{
        //    DataRow[] foundRows;
        //    foundRows = this.tvTable.Select("ElementID = '" + id + "'");

        //    for (int i = 0; i <= foundRows.GetUpperBound(0); i++)
        //    {
        //        EA.TaggedValue elementTv = (EA.TaggedValue)element.TaggedValues.AddNew(foundRows[i][1], "");
        //        elementTv.Value = foundRows[i][2];
        //        elementTv.Update();
        //    }
        //    element.Refresh();
        //}

        private void CreateNewRelation(ArrayList relationCollection)
        {
            foreach (string[] collection in relationCollection)
            {
                //target element in the same package or not
                if (collection[8] != "")
                {
                    //target element is in different package
                }
                else
                {
                    string supplierID = "";
                    //Check supplier element existence, collection[2] is supplier
                    if ((supplierID = isElementAlreadyExist(collection[2].ToString(),this.createdPackageID.ToString())) == "")
                    {
                        //create new supplier element
                        EA.Package container = this.repository.GetPackageByID(this.createdPackageID);
                        EA.Element newElement = (EA.Element)container.Elements.AddNew(collection[2], "Class");
                        newElement.Update();
                        supplierID = newElement.ElementID.ToString();
                    }

                    string clientID = "";
                    //Check client element existence, collection[4] is client
                    if ((clientID = isElementAlreadyExist(collection[4].ToString(), this.createdPackageID.ToString())) == "")
                    {
                        //create new client element
                        EA.Package container = this.repository.GetPackageByID(this.createdPackageID);
                        EA.Element newElement = (EA.Element)container.Elements.AddNew(collection[4], "Class");
                        newElement.Update();
                        clientID = newElement.ElementID.ToString();
                    }

                    EA.Element supplier = this.repository.GetElementByID(Int32.Parse(supplierID));
                    EA.Connector newConnector = createConnectorType(supplier, collection[5]);//(EA.Connector)supplier.Connectors.AddNew("", getConnectorType(collection[11]));
                    newConnector.SupplierID = supplier.ElementID;
                    newConnector.ClientID = Int32.Parse(clientID);
                    newConnector.ClientEnd.Role = collection[3];
                    newConnector.Stereotype = collection[9] == "CC" ? "ASCC" : "ASBIE";
                    newConnector.Notes = collection[10];

                    //if upperbound = "" it means no upperbound, so only use lowerbound
                    if (collection[7] != "")
                        newConnector.ClientEnd.Cardinality = collection[6] + ".." + collection[7];
                    else
                        newConnector.ClientEnd.Cardinality = collection[6];
                    
                    CreateConnectorTaggedValue(newConnector, collection[12]);

                    newConnector.Update();
                }
            }
        }

        private EA.Connector createConnectorType(EA.Element elSupplier ,string conType)
        {
            EA.Connector tempCon = null;
            if (conType == "A") //Aggregation
            {
                tempCon = (EA.Connector)elSupplier.Connectors.AddNew("", "Aggregation");
                tempCon.SupplierEnd.Aggregation = 0;
            }
            else
                if (conType == "C") //Composition
                {
                    tempCon = (EA.Connector)elSupplier.Connectors.AddNew("", "Aggregation");
                    tempCon.SupplierEnd.Aggregation = 2;
                }
                else
                    if (conType == "G")
                    {
                        tempCon = (EA.Connector)elSupplier.Connectors.AddNew("", "Generalization");
                    }
                    else
                        if (conType == "S")
                        {
                            tempCon = (EA.Connector)elSupplier.Connectors.AddNew("", "Association");
                        }

            return tempCon;
        }

        private bool isAttributeAlreadyExist(string attributeName, string elementID)
        {
            EA.Element element = this.repository.GetElementByID(Int32.Parse(elementID));
            foreach (EA.Attribute attr in element.Attributes)
            {
                if (attr.Name == attributeName)
                    return true;
            }
            return false;
        }

        private string isElementAlreadyExist(string elementName, string packageID)
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(packageID));
            foreach (EA.Element el in pkg.Elements)
            {
                if (el.Name == elementName)
                {
                    return el.ElementID.ToString();
                }
            }
            return "";
        }

        private string isPackageAlreadyExist(string packageName)
        {
            EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));
            foreach (EA.Package pkg in selectedPackage.Packages)
            {
                if (pkg.Name == packageName)
                {
                    return pkg.PackageID.ToString();
                }
            }
            return "";
        }

        private void setActivePackageLabel()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            string name = pkg.Name;//+ "<<" + pkg.Element.Stereotype.ToString() + ">>";
            if (pkg.Name.Length > 60)
                name = name.Substring(0, 57) + "...";

            this.lblSelectedPackage.Text = name;
        }

        private void appendErrorMessage(String msg, String packageName)
        {
            this.statusTextBox.Text += "ERROR: (Package: " + packageName + ") " + msg + "\n\n";
        }

        /// <summary>
        /// Show a info message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendInfoMessage(String msg, String packageName)
        {
            this.statusTextBox.Text += "INFO: (Package: " + packageName + ") " + msg + "\n\n";
        }

        private bool isRelation(string[] collection)
        {
            //collection[5] contains information about current object, whether it's a relation or not
            if (collection[5] == "N")
                return false;
            else
                return true;
        }
        #endregion
    }

    
}