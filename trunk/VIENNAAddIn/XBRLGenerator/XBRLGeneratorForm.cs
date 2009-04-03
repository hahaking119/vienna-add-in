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
using System.IO;
using System.Text;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.Utils;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using VIENNAAddIn.Exceptions; 
using VIENNAAddIn.CCTS;
using System.Xml.Schema;

namespace VIENNAAddIn.XBRLGenerator
{
    public partial class XBRLGeneratorForm : Form
    {
        private Repository repository;
        private String scope;
        private Boolean annotate;
        private bool includeLinkedSchema;
        private bool withGUI;
        private bool DEBUG;
        private GeneratorCallBackInterface caller;
        private BIEGenerator bieGen;
        private DOCGenerator docGen;

        //The path where the schema(s) should be saved
        private String path = "";
        private bool linkbase = false;

        public XBRLGeneratorForm(EA.Repository repository, String scope, bool annotate)
        {
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            
            //With GUI
            this.withGUI = true;
            InitializeComponent();
            this.setActivePackageLabel();
        }

        public XBRLGeneratorForm(EA.Repository repository, String scope, bool annotate, bool linkbase)
        {
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.linkbase = true;

            //With GUI
            this.withGUI = true;
            InitializeComponent();
            this.setActivePackageLabel();
        }

        /// <summary>
        /// Append an error message to the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendErrorMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("error", msg, this.getPackageName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "ERROR: (Package: " + packageName + ") " + msg + "\n\n";
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
                caller.appendMessage("info", msg, this.getPackageName());
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
                caller.appendMessage("warn", msg, this.getPackageName());
            }
            else
            {
                if (this.withGUI)
                    this.statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
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
        /// Reset the Text of the StatusTextBox
        /// </summary>
        private void resetMessageText()
        {
            this.statusTextBox.Text = "";
        }


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

        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName()
        {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name.ToString();
        }


        /// <summary>
        /// Set Label
        /// </summary>
        private void setActivePackageLabel()
        {
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
            if (p.ParentID == 0)
                this.selectedBIELibrary.Text = p.Name;
            else
                this.selectedBIELibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
        }


        /// <summary>
        /// Reset Schema Generator
        /// </summary>
        /// <param name="scope"></param>
        public void resetGenerator(String scope)
        {
            this.scope = scope;
            //this.alreadyCreatedSchemas = new ArrayList();
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
        }

        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            //path = "C:\\Documents and Settings\\kristina\\Desktop\\";
            //EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));
            //DoRecursivePackageSearch(selectedPackage);

            resetGenerator(this.scope);

            //Annotate or not?
            if (this.chkAnnotateElement.Checked)
                this.annotate = true;
            else
                this.annotate = false;

            if (this.chkIncludeLinkedSchema.Checked)
                CC_Utils.blnLinkedSchema = true;
            else
                CC_Utils.blnLinkedSchema = false;

            //if (DEBUG)
            //{
            //    path = "C:\\Documents and Settings\\kristina\\Desktop";
            //}
            //else
            //{
                //First we need to know the path where to save the schema(s)            
                DialogResult dr = this.folderBrowserDialog1.ShowDialog(this);
                if (dr.Equals(DialogResult.Cancel))
                {
                    folderBrowserDialog1.Dispose();
                    return;
                }

                this.path = this.folderBrowserDialog1.SelectedPath;
            //}

            if (this.path != null && !this.path.Equals(""))
            {
                this.path = this.path + "\\";

                //Get the active Package
                EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));

                this.performProgressStep();

                String error = "";
                System.Collections.ICollection result = null;

                this.appendInfoMessage("Starting schema generation. Please wait.", this.getPackageName());

                try
                {
                    //create new instance fo generator
                    //bieGen = new BIEGenerator(this.repository, this.chkAnnotateElement.Checked);
                    //docGen = new DOCGenerator(this.repository, this.chkAnnotateElement.Checked);

                    //set the path for saving generated schema
                    //bieGen.path = this.path;
                    //docGen.path = this.path;

                    //search for DOCLibrary/BIELibrary in current package
                    DoRecursivePackageSearch(selectedPackage);
                }
                catch (Exception exc)
                {
                    error = exc.Message;
                }

                if (error != "")
                    this.appendErrorMessage(error, this.getPackageName());

                this.appendInfoMessage("Creating schema finished.", this.getPackageName());
                this.progressBar1.Value = this.progressBar1.Maximum;
            }
            else
            {
                this.appendErrorMessage("Plese select a location for the generated schemas first.", this.getPackageName());
            }
           

        }

        /// <summary>
        /// Recursive search on selected package for BIELibrary or DOCLibrary
        /// </summary>
        /// <param name="p">selected package</param>
        private void DoRecursivePackageSearch(EA.Package p)
        {
            if (p.Packages.Count > 0)
            {
                foreach (EA.Package pkg in p.Packages)
                {
                    DoRecursivePackageSearch(pkg);
                }
            }
            else
            {
                if (p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    //generate XSD of BIELibrary
                    generateBIELibrary(this.repository, p);
                    this.performProgressStep();
                    
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    //generate XSD of DOCLibrary
                    generateDOCLibrary(this.repository, p);
                    this.performProgressStep();
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    generateQDTLibrary(this.repository, p);
                    this.performProgressStep();
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
                {
                    generateCDTLibrary(this.repository, p);
                    this.performProgressStep();
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
                {
                    generateENUMLibrary(this.repository, p);
                    this.performProgressStep();
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.CCLibrary.ToString()))
                {
                    generateCCLibrary(this.repository, p);
                    this.performProgressStep();
                }
                else
                {
                    this.appendInfoMessage("Can not generate XBRL from package with stereotye " + p.Element.Stereotype.ToString(),p.Element.Name);
                }
            }
        }

        private void generateCCLibrary(Repository repository, Package p)
        {
            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection bieGenCollection = null;
            CCGeneratorXBRL ccGenerator = new CCGeneratorXBRL(this.repository, p.PackageID.ToString());

            try
            {
                ccGenerator.path = this.path;
                bieGenCollection = ccGenerator.generateXBRLschema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error != "")
            {
                this.appendErrorMessage(error, p.Name);
            }   
        }


        /// <summary>
        /// Generate DOCLibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateDOCLibrary(EA.Repository repository, EA.Package p)
        {
            //search for element root from IsRoot property
            int rootElement = 0;
            int countRootElement = 0;
            
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
                if (countRootElement > 1)
                    break;
            }

            
            if (countRootElement == 0)
            {
                this.appendErrorMessage("There is none root element in the DOCLibrary. Please add or set 'isRoot' tagged value to 'true' first. Skipped.", p.Name);
            }
            else if (countRootElement > 1)
            {
                this.appendErrorMessage("There are more than one root element in the DOCLibrary. Skipped.", p.Name);
            }
            else
            {
                string error = "";
                System.Collections.ICollection docGenCollection = null;
                DOCGeneratorXBRL docGenerator = new DOCGeneratorXBRL(this.repository, this.scope);

                if (countRootElement == 1)
                {
                    try
                    {
                        //docGen.scope = p.PackageID.ToString();
                        //docGenCollection = docGen.generateSchema(rootElement, p);
                        docGenerator.path = this.path;
                        docGenCollection = docGenerator.generateXBRLschema(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    if (error == "")
                    {
                        //string baseURL = XMLTools.getBaseURL(repository, p.PackageID.ToString());

                        //foreach (XmlSchema schema in docGenCollection)
                        //{
                        //    String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(p.PackageID), repository);
                        //    String filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
                        //    //Create the path
                        //    System.IO.Directory.CreateDirectory(path + schemaPath);
                        //    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                        //    schema.Write(outputStream);
                        //    outputStream.Close();

                        //    if (baseURL != "")
                        //    {
                        //        EA.Package pkg = this.repository.GetPackageByID(p.PackageID);
                        //        string schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
                        //        string webSchemaPath = baseURL + "/" + schemaPath.Replace("\\", "/") + schemaName;

                        //        //add the address to property of package
                        //        if (!IsSchemaExist(webSchemaPath, pkg))
                        //        {
                        //            EA.File myfile = (EA.File)pkg.Element.Files.AddNew(webSchemaPath, "Web Address");

                        //            myfile.Update();
                        //            pkg.Element.Files.Refresh();
                        //        }
                        //    }
                        //}
                        //this.appendInfoMessage("The schema was created successfully.", p.Name);
                    }
                    else
                        this.appendErrorMessage(error, p.Name);
                }
            }
        }


        /// <summary>
        /// Generate BIELibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateBIELibrary(EA.Repository repository, EA.Package p)
        {
            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection bieGenCollection = null;
            BIEGeneratorXBRL bieGenerator = new BIEGeneratorXBRL(this.repository, p.PackageID.ToString());

            try
            {
                bieGenerator.path = this.path;
                bieGenCollection = bieGenerator.generateXBRLschema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                //string baseURL = XMLTools.getBaseURL(repository, scp);
                
                //foreach (XmlSchema schema in bieGenCollection)
                //{
                //    String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(scp)), repository);
                //    String filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scp)));
                //    //Create the path
                //    System.IO.Directory.CreateDirectory(path + schemaPath);
                //    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                //    schema.Write(outputStream);
                //    outputStream.Close();

                //    if (baseURL != "")
                //    {
                //        EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
                //        string schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                //        string webSchemaPath = baseURL + "/" + schemaPath.Replace("\\", "/") + schemaName;

                //        //add the address to property of package
                //        if (!IsSchemaExist(webSchemaPath, pkg))
                //        {
                //            EA.File myfile = (EA.File)pkg.Element.Files.AddNew(webSchemaPath, "Web Address");

                //            myfile.Update();
                //            pkg.Element.Files.Refresh();
                //        }
                //    }
                //}
                //this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
            {
                this.appendErrorMessage(error, p.Name);
            }   
        }

        /// <summary>
        /// Generate QDTLibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateQDTLibrary(EA.Repository repository, EA.Package p)
        {
            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection qdtGenCollection = null;
            QDTGeneratorXBRL qdtGenerator = new QDTGeneratorXBRL(this.repository, p.PackageID.ToString(), true);

            try
            {
                qdtGenerator.path = this.path;
                qdtGenCollection = qdtGenerator.generateXBRLschema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                string baseURL = XMLTools.getBaseURL(repository, scp);

                foreach (XmlSchema schema in qdtGenCollection)
                {
                    String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(scp)), repository);
                    String filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scp)));
                    //Create the path
                    System.IO.Directory.CreateDirectory(path + schemaPath);
                    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                    schema.Write(outputStream);
                    outputStream.Close();

                    if (baseURL != "")
                    {
                        EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
                        string schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                        string webSchemaPath = baseURL + "/" + schemaPath.Replace("\\", "/") + schemaName;

                        //add the address to property of package
                        if (!IsSchemaExist(webSchemaPath, pkg))
                        {
                            EA.File myfile = (EA.File)pkg.Element.Files.AddNew(webSchemaPath, "Web Address");

                            myfile.Update();
                            pkg.Element.Files.Refresh();
                        }
                    }
                }
                this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
            {
                this.appendErrorMessage(error, p.Name);
            } 
        }

        /// <summary>
        /// Generate CDTLibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateCDTLibrary(EA.Repository repository, EA.Package p)
        {
            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection cdtGenCollection = null;
            CDTGeneratorXBRL cdtGenerator = new CDTGeneratorXBRL(this.repository, p.PackageID.ToString(), this.chkAnnotateElement.Checked);

            try
            {
                cdtGenerator.path = this.path;
                cdtGenCollection = cdtGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                string baseURL = XMLTools.getBaseURL(repository, scp);

                foreach (XmlSchema schema in cdtGenCollection)
                {
                    String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(scp)), repository);
                    String filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scp)));
                    //Create the path
                    System.IO.Directory.CreateDirectory(path + schemaPath);
                    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                    schema.Write(outputStream);
                    outputStream.Close();

                    if (baseURL != "")
                    {
                        EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
                        string schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                        string webSchemaPath = baseURL + "/" + schemaPath.Replace("\\", "/") + schemaName;

                        //add the address to property of package
                        if (!IsSchemaExist(webSchemaPath, pkg))
                        {
                            EA.File myfile = (EA.File)pkg.Element.Files.AddNew(webSchemaPath, "Web Address");

                            myfile.Update();
                            pkg.Element.Files.Refresh();
                        }
                    }
                }
                this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
            {
                this.appendErrorMessage(error, p.Name);
            } 
        }

        /// <summary>
        /// Generate ENUMLibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateENUMLibrary(EA.Repository repository, EA.Package p)
        {
            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection enumGenCollection = null;
            ENUMGenerator enumGenerator = new ENUMGenerator(this.repository, this.scope, false, false, this.path);

            try
            {
                //enumGenerator.path = this.path;
                enumGenCollection = enumGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                string baseURL = XMLTools.getBaseURL(repository, scp);

                foreach (XmlSchema schema in enumGenCollection)
                {
                    String filename = "";
                    String schemaPath = "";
                    String schemaName = "";
                    schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(this.scope)), repository);
                    schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                    System.IO.Directory.CreateDirectory(path + schemaPath);

                    //this.appendInfoMessage("Saving schema files...", Int32.Parse(this.scope).Name);
                    filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));

                    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                    schema.Write(outputStream);
                    outputStream.Close();
                }

                this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
            {
                this.appendErrorMessage(error, p.Name);
            } 
        }

        /// <summary>
        /// Search for particular webSchemaPath
        /// </summary>
        /// <param name="webSchemaPath">Web address for schema</param>
        /// <param name="pkg">Package</param>
        /// <returns></returns>
        private bool IsSchemaExist(string webSchemaPath, Package pkg)
        {
            foreach (EA.File file in pkg.Element.Files)
            {
                if (file.Name == webSchemaPath)
                    return true;
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}