/* This class will generate all the XSD schema on under selected package recursively
 * according to its stereotype.
*/ 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;
using EA;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.Utils;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;


namespace VIENNAAddIn.CCTS
{
    public partial class BusinessLibraryGenerator : Form, GeneratorCallBackInterface
    {
        #region Variables
        private Repository repository;
        private String scope;
        private Boolean annotate = false;
        private bool blnIncludeLinkedSchema;
        private bool withGUI;
        private bool DEBUG;
        
        private GeneratorCallBackInterface caller;
        private BIEGenerator bieGen;
        private DOCGenerator docGen;

        //This ArrayList holds a List of all auxilliary schmeas
        //that had to be created for this schema to be valid
        private System.Collections.ArrayList alreadyCreatedSchemasBLGen = new ArrayList();

        //The path where the schema(s) should be saved
        private String path = "";
        private bool blnNillable;
        private bool blnAlias;
        #endregion

        public BusinessLibraryGenerator(EA.Repository repository, String scope, bool annotate)
        {
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            //With GUI
            this.withGUI = true;
            InitializeComponent();
            this.setActivePackageLabel();
            this.chkIncludeLinkedSchema.Checked = true;
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
            //this.alreadyCreatedSchemasBLGen = new ArrayList();
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.alreadyCreatedSchemasBLGen = new ArrayList();
            this.setActivePackageLabel();
        }

        private void getCheckedOption()
        {
            if (this.chkAnnotateElement.Checked)
                this.annotate = true;
            else
                this.annotate = false;

            if (this.chkIncludeLinkedSchema.Checked)
                this.blnIncludeLinkedSchema = true;
            else
                this.blnIncludeLinkedSchema = false;

            if (this.chkUseAlias.Checked)
                this.blnAlias = true;
            else
                this.blnAlias = false;

            if (this.chkNillable.Checked)
                this.blnNillable = true;
            else
                this.blnNillable = false;
        }

        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            resetGenerator(this.scope);

            getCheckedOption();

            //Annotate or not?
            //if (this.chkAnnotateElement.Checked)
            //    this.annotate = true;
            //else
            //    this.annotate = false;

            //if (this.chkIncludeLinkedSchema.Checked)
            //    CC_Utils.blnLinkedSchema = true;
            //else
            //    CC_Utils.blnLinkedSchema = false;

            //if (this.chkUseAlias.Checked)
            //    CC_Utils.blnUseAlias = true;
            //else
            //    CC_Utils.blnUseAlias = false;


            //First we need to know the path where to save the schema(s)            
            DialogResult dr = this.folderBrowserDialog1.ShowDialog(this);
            if (dr.Equals(DialogResult.Cancel))
            {
                folderBrowserDialog1.Dispose();
                return;
            }

            this.path = this.folderBrowserDialog1.SelectedPath;
            

            if (this.path != null && !this.path.Equals(""))
            {
                              
                this.path = this.path + "\\";

                //Get the active Package
                EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));

                this.performProgressStep();

                String error = "";
                //System.Collections.ICollection result = null;

                this.appendInfoMessage("Starting schema generation. Please wait.", this.getPackageName());

                try
                {
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
                //foreach (EA.Package pkg in p.Packages)
                for (short i = 0; i < p.Packages.Count; i++) 
                {
                    EA.Package pkg = (EA.Package)p.Packages.GetAt(i);
                    DoRecursivePackageSearch(pkg);
                }
            }
            else
            {
                if (p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    //generate XSD of BIELibrary
                    generateBIELibrary(this.repository, p, alreadyCreatedSchemasBLGen);
                    this.performProgressStep();
                }
                else 
                    if (p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    //generate XSD of DOCLibrary
                    generateDOCLibrary(this.repository, p, alreadyCreatedSchemasBLGen);
                    this.performProgressStep();
                }
                else if (p.Element.Stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    generateQDTLibrary(this.repository, p, alreadyCreatedSchemasBLGen);
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
                    generateCCLibrary(this.repository, p, alreadyCreatedSchemasBLGen);
                    this.performProgressStep();
                }
                else
                {
                    if (p.Element.Stereotype.Equals(""))
                        this.appendInfoMessage("Can not generate XSD from package with no stereotyped.", p.Element.Name);
                    else
                        this.appendInfoMessage("XSD generation from package with stereotype " + p.Element.Stereotype.ToString() + " is not supported.", p.Element.Name);
                }
            }
        }

        private void generateCCLibrary(EA.Repository repository, EA.Package p, ArrayList alreadyCreatedSchemasBLGen)
        {
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection ccGenCollection = null;
            CCGenerator ccGenerator = new CCGenerator(this.repository, p.PackageID.ToString(), this.annotate,
                this.blnIncludeLinkedSchema, this.blnAlias, this.blnNillable, this.path, this, alreadyCreatedSchemasBLGen, true);//, this);

            try
            {
                //ccGenerator.path = this.path;
                ccGenCollection = ccGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                #region old code
                //string baseURL = XMLTools.getBaseURL(repository, scp);

                //foreach (XmlSchema schema in ccGenCollection)
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
                //    addSchemaToAlreadyCreatedSchema(p);
                //}
                #endregion
                writeGeneratedSchema(p, ccGenCollection);
                this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
            {
                this.appendErrorMessage(error, p.Name);
            }
        }


        /// <summary>
        /// Generate DOCLibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateDOCLibrary(EA.Repository repository, EA.Package p, ArrayList alreadyCreatedSchemasBLGen)
        {
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            //search for element root from IsRoot property
            //int rootElement = 0;
            int countRootElement = 0;
            
            foreach (EA.Element el in p.Elements)
            {
                if (XMLTools.getElementTVValue("isRoot", el).Equals("true", StringComparison.OrdinalIgnoreCase))
                    countRootElement++;
                
                if (countRootElement > 1)
                    break;
            }
            
            if (countRootElement == 0)
            {
                this.appendErrorMessage("There is none root element in the DOCLibrary. Please add or set 'isRoot' tagged value to 'true' first. Skipped.", p.Name);
                return; //stop processing this package since there is no root
            }
            else if (countRootElement > 1)
            {
                this.appendWarnMessage("There are more than one root element in the DOCLibrary.", p.Name);
            }
            
            string error = "";
            System.Collections.ICollection docGenCollection = null;
            DOCGenerator docGenerator = new DOCGenerator(this.repository, p.PackageID.ToString(), this.annotate,
                this.blnIncludeLinkedSchema, this.blnAlias, this.blnNillable, this.path, this, this.alreadyCreatedSchemasBLGen, true);
            
            try
            {
                //docGenerator.path = this.path;
                //this.appendInfoMessage("Generating DOCLibrary ", p.Name);
                docGenCollection = docGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                writeGeneratedSchema(p, docGenCollection);   
                this.appendInfoMessage("The schema was created successfully.", p.Name);
            }
            else
                this.appendErrorMessage(error, p.Name);
        }


        /// <summary>
        /// Write the generated schema to file
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="schemaCol">Collection of generated schema</param>
        private void writeGeneratedSchema(EA.Package p, System.Collections.ICollection schemaCol)
        {
            string baseURL = XMLTools.getBaseURL(repository, p.PackageID.ToString());

            foreach (XmlSchema schema in schemaCol)
            {
                String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(p.PackageID), repository, this.blnAlias);
                String filename = this.path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
                //Create the path
                System.IO.Directory.CreateDirectory(this.path + schemaPath);
                Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                schema.Write(outputStream);
                outputStream.Close();

                if (baseURL != "")
                {
                    EA.Package pkg = this.repository.GetPackageByID(p.PackageID);
                    string schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
                    string webSchemaPath = baseURL + "/" + schemaPath.Replace("\\", "/") + schemaName;

                    //add the address to property of package
                    if (!IsSchemaExist(webSchemaPath, pkg))
                    {
                        EA.File myfile = (EA.File)pkg.Element.Files.AddNew(webSchemaPath, "Web Address");

                        myfile.Update();
                        pkg.Element.Files.Refresh();
                    }
                }

                addSchemaToAlreadyCreatedSchema(p);

            }
        }


        private void addSchemaToAlreadyCreatedSchema(Package p)
        {
            AuxilliarySchema aux = new AuxilliarySchema();
            aux.Namespace = XMLTools.getNameSpace(this.repository, p);
            aux.PackageOfOrigin = p.Name;
            this.alreadyCreatedSchemasBLGen.Add(aux);
        }


        /// <summary>
        /// Generate BIELibrary
        /// </summary>
        /// <param name="repository">Current repository</param>
        /// <param name="p">The package to generate XSD</param>
        private void generateBIELibrary(EA.Repository repository, EA.Package p, ArrayList alreadyCreatedSchemasBLGen)
        {
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection bieGenCollection = null;
            BIEGenerator bieGenerator = new BIEGenerator(this.repository, p.PackageID.ToString(), this.chkAnnotateElement.Checked, 
                this.blnIncludeLinkedSchema, this.blnAlias, this.blnNillable, this.path, this, this.alreadyCreatedSchemasBLGen, true); //, this);

            try
            {
                //bieGenerator.path = this.path;
                bieGenCollection = bieGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                writeGeneratedSchema(p, bieGenCollection);

                #region old code
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
                //    addSchemaToAlreadyCreatedSchema(p);
                //}
                #endregion
                this.appendInfoMessage("The schema was created successfully.", p.Name);
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
        private void generateQDTLibrary(EA.Repository repository, EA.Package p, ArrayList alreadyCreatedSchemasBLGen)
        {
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            string error = "";
            System.Collections.ICollection qdtGenCollection = null;
            QDTGenerator qdtGenerator = new QDTGenerator(this.repository, p.PackageID.ToString(), this.annotate, 
                this.blnAlias, this.blnIncludeLinkedSchema,  this.path, alreadyCreatedSchemasBLGen);

            try
            {
                //qdtGenerator.path = this.path;
                qdtGenCollection = qdtGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                #region old code
                //string baseURL = XMLTools.getBaseURL(repository, scp);

                //foreach (XmlSchema schema in qdtGenCollection)
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
                //    addSchemaToAlreadyCreatedSchema(p);
                //}
                #endregion 
                writeGeneratedSchema(p, qdtGenCollection);
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
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection cdtGenCollection = null;
            CDTGenerator cdtGenerator = new CDTGenerator(this.repository, p.PackageID.ToString(), this.chkAnnotateElement.Checked, this.blnAlias, this.path);

            try
            {
                //cdtGenerator.path = this.path;
                cdtGenCollection = cdtGenerator.generateSchema(p);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (error == "")
            {
                #region oldcode
                //string baseURL = XMLTools.getBaseURL(repository, scp);

                //foreach (XmlSchema schema in cdtGenCollection)
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
                //    addSchemaToAlreadyCreatedSchema(p);
                //}
                #endregion

                writeGeneratedSchema(p, cdtGenCollection);
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
            //check if current package is already generated
            if (isAlreadyInBLGenArray(p.Name) != "")
            {
                this.appendInfoMessage("The schema was created successfully.", p.Name);
                return;
            }

            string scp = p.PackageID.ToString();
            string error = "";
            System.Collections.ICollection enumGenCollection = null;
            ENUMGenerator enumGenerator = new ENUMGenerator(this.repository, p.PackageID.ToString(), this.chkAnnotateElement.Checked, this.blnAlias, this.path);


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
                #region old code
                //string baseURL = XMLTools.getBaseURL(repository, scp);

                //foreach (XmlSchema schema in enumGenCollection)
                //{
                //    String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(scp)), repository, blnUseAlias);
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
                //    addSchemaToAlreadyCreatedSchema(p);
                //}
                #endregion

                writeGeneratedSchema(p, enumGenCollection);
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

        private string isAlreadyInBLGenArray(string packageName)
        {
            foreach (AuxilliarySchema aux in this.alreadyCreatedSchemasBLGen)
            {
                if (aux.PackageOfOrigin == packageName)
                    return aux.Namespace;
            }
            return "";
        }

        //private void chkIncludeLinkedSchema_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.chkIncludeLinkedSchema.Checked == true)
        //        this.blnIncludeLinkedSchema = true;
        //    else
        //        this.blnIncludeLinkedSchema = false;
        //}

        //private void chkUseAlias_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.chkUseAlias.Checked == true)
        //        this.blnAlias = true;
        //    else
        //        this.blnAlias = false;
        //}

        //private void chkNillable_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.chkNillable.Checked == true)
        //        this.blnNillable = true;
        //    else
        //        this.blnNillable = false;
        //}
        private void chkNillable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNillable.Checked == true)
            {
                DialogResult result = MessageBox.Show("CAUTION: The resultant schemas may not be compliant with UN/CEFACT Naming and Design Rules." +
                "\nPlease confirm with your business domain owner before enabling." + "\nAre you sure want to continue?",
                "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    this.chkNillable.Checked = true;
                    this.blnNillable = true;
                }
                else
                {
                    this.chkNillable.Checked = false;
                    this.blnNillable = false;
                }
            }
            else
                this.chkNillable.Checked = false;
        }
    }
}