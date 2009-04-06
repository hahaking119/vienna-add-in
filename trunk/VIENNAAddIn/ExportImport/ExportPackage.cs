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

namespace VIENNAAddIn.ExportImport
{
    public partial class ExportPackage : Form
    {
        private static ExportPackage form;
        public static void ShowForm(Repository repository)
        {
            string scope = repository.DetermineScope();
            if (form == null || form.IsDisposed)
            {
                form = new ExportPackage(repository, scope);
                form.Show();
            }
            else
            {
                form.resetGenerator(scope);
                form.Select();
                form.Focus();
                form.Show();
            }
        }

        #region Variable
        private EA.Repository repository;
        private string scope = "";
        private string path = "";
        private string delimiter = "";
        private bool blnWriteEscapeChar = false;

        #endregion 

        #region Constructor
        public ExportPackage(EA.Repository repository, string scope)
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
            DialogResult result = dlgSavePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.path = dlgSavePath.FileName;
                resetGenerator(this.scope);
                txtPath.Text = dlgSavePath.FileName;

                string fileTaggedValue = this.path.Substring(0, this.path.Length - System.IO.Path.GetFileName(this.path).Length) +
                System.IO.Path.GetFileNameWithoutExtension(this.path) + "-taggedValue.csv";

                txtTaggedValue.Text = fileTaggedValue;
            }

            
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //resetGenerator(this.scope);
            
            //clear text area..
            statusTextBox.Text = "";

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            
            this.appendInfoMessage("Start exporting... ", pkg.Name);
            this.Refresh();

            if (doExport())
                this.appendInfoMessage("Finished exporting.", pkg.Name);
            
        }
        #endregion

        #region Method

        private bool doExport()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            
            if (cmbDelimiter.SelectedItem.ToString() == "TAB")
                this.delimiter = "\t";
            else
                this.delimiter = cmbDelimiter.SelectedItem.ToString(); //txtDelimiter.Text;

            string text, textTaggedValue;
            FileStream fs = null;
            StreamWriter sw = null;

            FileStream fsTaggedValue = null;
            StreamWriter swTaggedValue = null;
            //string fileTaggedValue = this.path.Substring(0, this.path.Length - System.IO.Path.GetFileName(this.path).Length) +
            //    System.IO.Path.GetFileNameWithoutExtension(this.path) + "-taggedValue.csv";
            
            string fileTaggedValue = txtTaggedValue.Text;

            int elementID = 0;
            try
            {
                fs = new FileStream(this.path, FileMode.Create);
                sw = new StreamWriter(fs);

                fsTaggedValue = new FileStream(fileTaggedValue, FileMode.Create);
                swTaggedValue = new StreamWriter(fsTaggedValue);

                text = "Package" + delimiter + "ElementName" + delimiter + "Object" + delimiter +
                    "Property" + delimiter + "Representation" + delimiter + "IsRelation" + delimiter +
                    "min" + delimiter + "max" + delimiter + "Related Package" + delimiter + "CCTS Types" + delimiter + 
                    "Notes" + delimiter + "Initial Value" + delimiter + "ElementID" +
                    "\n"; //title
                sw.Write(text);

                textTaggedValue = "ElementID" + delimiter + "Tagged Name" + delimiter + "Value" + "\n";
                swTaggedValue.Write(textTaggedValue);

                foreach (EA.Element el in pkg.Elements)
                {
                    foreach (EA.Attribute attr in el.Attributes)
                    {
                        elementID++;
                        text = pkg.Name + delimiter;        //Package
                        text += el.Name + "." + attr.Name + "." + attr.Type + delimiter;//ElementName
                        text += el.Name + delimiter;        //Object
                        text += attr.Name + delimiter;      //Property
                        text += attr.Type + delimiter;      //Representation
                        text += "N" + delimiter;            //IsRelation
                        text += attr.LowerBound + delimiter;//min
                        text += attr.UpperBound + delimiter;//max
                        text += delimiter;                  //Related Package - for connector only
                        text += getCCTSType(attr.Stereotype) + delimiter;                  //CCTS Type
                        text += removeEscapeChar(attr.Notes) + delimiter;     //Notes
                        text += attr.Default + delimiter;                  //initial value
                        text += elementID;
                        text += "\n";
                        sw.Write(text);

                        foreach (EA.AttributeTag tv in attr.TaggedValues)
                        {
                            textTaggedValue = elementID + delimiter;
                            textTaggedValue += tv.Name + delimiter;
                            textTaggedValue += tv.Value;
                            textTaggedValue += "\n";
                            swTaggedValue.Write(textTaggedValue);
                        }
                    }

                    //elementID++;
                    foreach (EA.Connector con in el.Connectors)
                    {
                        if ((con.Stereotype.ToString() == "ASBIE" || con.Stereotype.ToString() == "ASCC")
                            && con.SupplierID == el.ElementID)
                        {
                            elementID++;
                            EA.Element client = this.repository.GetElementByID(con.ClientID);
                            EA.Element supplier = this.repository.GetElementByID(con.SupplierID);

                            text = pkg.Name + delimiter;                                //Package
                            text += el.Name + "." + con.ClientEnd.Role + "." + client.Name + delimiter;//ElementName
                            text += supplier.Name + delimiter;                          //Object
                            text += con.ClientEnd.Role + delimiter;                     //Property
                            text += client.Name + delimiter;                            //Representation

                            text += defineConnectorType(con) + delimiter;               //IsRelation
                            //get lower bound
                            string lowerbound = con.ClientEnd.Cardinality == "" ? "" : con.ClientEnd.Cardinality.Substring(0, 1);
                            text += lowerbound + delimiter;                             //min

                            string upperbound = con.ClientEnd.Cardinality == "" ? "" : con.ClientEnd.Cardinality.Substring(con.ClientEnd.Cardinality.Length - 1, 1);
                            text += upperbound + delimiter;                             //max

                            text += getRelatedPackage(client,supplier) + delimiter;     //Related Package
                            text += getCCTSType(con.Stereotype) + delimiter;            //CCTS Type
                            text += removeEscapeChar(con.Notes) + delimiter;                              //Notes
                            text += "" + delimiter;                                          //Initial value - only for attribute
                            text += elementID;
                            text += "\n";
                            sw.Write(text);

                            foreach (EA.ConnectorTag tv in con.TaggedValues)
                            {
                                textTaggedValue = elementID + delimiter;
                                textTaggedValue += tv.Name + delimiter;
                                textTaggedValue += tv.Value;
                                textTaggedValue += "\n";
                                swTaggedValue.Write(textTaggedValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                this.appendErrorMessage("Failed to export due to error : " + ex.Message, pkg.Name);
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

        private string removeEscapeChar(string strNotes)
        {
            string editedString = "";
            if (this.blnWriteEscapeChar)
                editedString = strNotes.Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", "\\r");
            else
                editedString = strNotes.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");

            return editedString;
        }

        private string defineConnectorType(EA.Connector con)
        {
            if ((con.Type == "Aggregation") && (con.SupplierEnd.Aggregation == 0))
                return "A";
            else
            if ((con.Type == "Aggregation") && (con.SupplierEnd.Aggregation == 2))
                return "C";
            else
            if (con.Type == "Generalization")
                return "G";
            else
            if (con.Type == "Association")
                return "S";
            
            return "";
        }

        private string getRelatedPackage(EA.Element client, EA.Element supplier)
        {
            if (client.PackageID != supplier.PackageID)
                return this.repository.GetPackageByID(client.PackageID).Name;
            else
                return "";
        }

        private string getCCTSType(string stereotype)
        {
            switch (stereotype)
            {
                case "BCC"  : return "CC";
                case "ASCC" : return "CC";
                case "BBIE": return "BIE";
                case "ASBIE": return "BIE";
                default: return "";
            }
        }

        private void setActivePackageLabel()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            string name = pkg.Name;// +"<<" + pkg.Element.Stereotype.ToString() + ">>";
            if (pkg.Name.Length > 60)
                name = name.Substring(0, 57) + "...";
            
            this.lblSelectedPackage.Text = name;
        }

        public void resetGenerator(string scope)
        {
            this.scope = scope;
            this.txtPath.Text = "";
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
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
        #endregion

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.blnWriteEscapeChar = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.blnWriteEscapeChar = false;
        }
    }
}