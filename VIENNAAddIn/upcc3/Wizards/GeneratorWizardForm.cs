using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddIn.upcc3.Wizards
{
    ///<summary>
    ///</summary>
    public partial class GeneratorWizardForm : Form
    {
        //public Repository Repository
        //{
        //    set { ccR = new CCRepository(value); }
        //}

        private CCRepository ccR;
        private Cache cache;
        private string selectedBIVName;
        private string selectedDOCName;
        private string selectedModelName;
        private const int MARGIN = 15;
        private int mouseDownPosX;
        private string outputDirectory = "";        

        ///<summary>
        ///</summary>
        ///<param name="ccRepository"></param>
        public GeneratorWizardForm(CCRepository ccRepository)
        {
            InitializeComponent();

            ccR = ccRepository;

            cache = new Cache();

            cache.LoadBIVs(ccR);

            comboBIVs.DropDownStyle = ComboBoxStyle.DropDownList;
            comboModels.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void GeneratorWizardForm_Load(object sender, EventArgs e)
        {
            MirrorBIVsToUI();

            ResetForm(0);
        }

        #region Covenience Methods

        private static void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private static void SetSafeIndex(CheckedListBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private void MirrorBIVsToUI()
        {
            comboBIVs.Items.Clear();

            foreach (cBIV docl in cache.BIVs.Values)
            {
                comboBIVs.Items.Add(docl.Name);
            }

            // TODO: preselect first item

            //if (comboBIVs.Items.Count > 0)
            //{
            //    comboBIVs.SelectedIndex = 0;    
            //}            
        }

        private void MirrorModelsToUI()
        {
            comboModels.Items.Clear();

            comboModels.Items.Add("CCTS");
        }

        private void MirrorDOCsToUI()
        {
            // todo: extend PathIsValid 
            GatherUserInput();

            cache.BIVs[selectedBIVName].LoadDOCsInBIV(ccR);

            // model
            // todo: set default value for document model and select afterwards
            //comboModels.SelectedItem = cache.DOCLs[selectedBIVName].DocumentModel;

            // documents
            checkedlistboxDOCs.Items.Clear();

            foreach (cDOC doc in cache.BIVs[selectedBIVName].DOCs.Values)
            {
                checkedlistboxDOCs.Items.Add(doc.Name, doc.State);                
            }            
        }

        private void MirrorDOCSettingsToUI()
        {
            GatherUserInput();

            textTargetNS.Text = cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespace;
            textPrefixTargetNS.Text = cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespacePrefix;
        }

        private void GatherUserInput()
        {            
            selectedBIVName = comboBIVs.SelectedIndex >= 0 ? comboBIVs.SelectedItem.ToString() : "";
            selectedDOCName = checkedlistboxDOCs.SelectedIndex >= 0 ? checkedlistboxDOCs.SelectedItem.ToString() : "";
            selectedModelName = comboModels.SelectedIndex >= 0 ? comboModels.SelectedItem.ToString() : "";
            outputDirectory = textOutputDirectory.Text;
        }

        #endregion

        private void ResetForm(int levelOfReset)
        {
            switch (levelOfReset)
            {
                case 0:
                    comboBIVs.Enabled = true;
                    checkedlistboxDOCs.Enabled = false;
                    textTargetNS.Enabled = false;
                    textPrefixTargetNS.Enabled = false;
                    checkboxAnnotations.Enabled = false;
                    checkboxAllschemas.Enabled = false;
                    comboModels.Enabled = false;
                    textOutputDirectory.Enabled = false;
                    buttonBrowseFolders.Enabled = false;
                    buttonGenerate.Enabled = false;
                    progressBar.Maximum = 100;
                    progressBar.Value = 0;
                    break;

                case 1:
                    checkedlistboxDOCs.Enabled = true;
                    textTargetNS.Enabled = true;
                    textPrefixTargetNS.Enabled = true;
                    checkboxAnnotations.Enabled = true;
                    checkboxAllschemas.Enabled = true;
                    comboModels.Enabled = true;
                    textOutputDirectory.Enabled = true;
                    buttonBrowseFolders.Enabled = true;
                    buttonGenerate.Enabled = false;
                    break;

                case 2:
                    buttonGenerate.Enabled = true;
                    break;

            }
        }

        private void VerifyUserInput()
        {
            GatherUserInput();

            if (!(String.IsNullOrEmpty(selectedBIVName)) &&
                 (checkedlistboxDOCs.CheckedItems.Count > 0) &&
                !(String.IsNullOrEmpty(textTargetNS.Text)) &&
                !(String.IsNullOrEmpty(textPrefixTargetNS.Text)) &&
                !(String.IsNullOrEmpty(selectedModelName)) &&
                !(String.IsNullOrEmpty(textOutputDirectory.Text)))
            {
                ResetForm(2);
            }
            else
            {
                ResetForm(1);
            }
        }

        private void comboBIVs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MirrorModelsToUI();            
            MirrorDOCsToUI();

            SetSafeIndex(comboModels, 0);
            SetSafeIndex(checkedlistboxDOCs, 0);

            ResetForm(1);
            VerifyUserInput();
        }

        private void checkedlistboxDOCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // todo: make path check
            MirrorDOCSettingsToUI();
        }

        private void checkedlistboxDOCs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (mouseDownPosX > MARGIN)
            {
                e.NewValue = e.CurrentValue;
            }
            else
            {
                cache.BIVs[selectedBIVName].DOCs[selectedDOCName].State = e.NewValue;                
            }

            VerifyUserInput();
        }

        private void checkedlistboxDOCs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDownPosX = e.X;
            else
                mouseDownPosX = -1;
        }

        private void buttonBrowseFolders_Click(object sender, EventArgs e)
        {
            GatherUserInput();
            
            FolderBrowserDialog browseDialog = new FolderBrowserDialog();

            browseDialog.Description = "Select the directory where the generated XML schema files are stored:";

            if (!(outputDirectory.Equals("")))
            {
                browseDialog.SelectedPath = outputDirectory;
            }
            
            DialogResult dialogResult = browseDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                textOutputDirectory.Text = browseDialog.SelectedPath;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            richtextStatus.Text = "Starting to generate XML schemas ...\n\n";
            
            GatherUserInput();

            cBIV currentBIV = cache.BIVs[selectedBIVName];

            // TODO: check if path is valid
            IList<IABIE> relevantDocuments = new List<IABIE>();
            foreach (cDOC document in currentBIV.DOCs.Values)
            {
                if (document.State == CheckState.Checked)
                {
                    relevantDocuments.Add(ccR.GetABIE(document.Id));
                }
            }
            
            IDOCLibrary docl = (IDOCLibrary)ccR.GetLibrary(currentBIV.Id);

            // TODO: xsd generator needs to be adapted - currently all doc libraries are being generated whereas
            // only the ones that are checked should be generated.. 

            //TODO: currently the wizard just takes the input from the text fields whereas the prefix and the
            // target namespace should be (a) stored in the cache and (b) read from there while generation.. 
            string targetNamespace = textTargetNS.Text;
            string namespacePrefix = textPrefixTargetNS.Text;
            bool annotate = checkboxAnnotations.CheckState == CheckState.Checked ? true : false;
            bool allschemas = checkboxAllschemas.CheckState == CheckState.Checked ? true : false;
            var generationContext = new GenerationContext(ccR, targetNamespace,
                                                namespacePrefix, annotate, allschemas,
                                                outputDirectory, docl, relevantDocuments);
            generationContext.SchemaAdded += HandleSchemaAdded;
            XSDGenerator.Generator.XSDGenerator.GenerateSchemas(generationContext);

            //VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.GenerateSchemas(ccRepository, docLibrary,
            //                                                                                  "urn:test:namespace", "test", true,
            //                                                                                  PathToTestResource(
            //                                                                                      "\\XSDGeneratorTest\\all"));

            richtextStatus.Text += "\nGenerating XML schemas completed!";
            progressBar.Value = 100;
            Cursor.Current = Cursors.Default;
        }

        private void HandleSchemaAdded(object sender, SchemaAddedEventArgs e)
        {
            //richtextStatus.Text += "Schema generated: file:///C:/Temp/output/" + e.FileName + "\n";
            richtextStatus.Text += "Generated Schema file:" + e.FileName + "\n";

            progressBar.Value += e.Progress;
        }

        private void textTargetNS_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();
            
            // todo: make path check
            cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespace = textTargetNS.Text;

            VerifyUserInput();
        }

        private void comboModels_SelectionChangeCommitted(object sender, EventArgs e)
        {
            VerifyUserInput();
        }

        private void textOutputDirectory_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            // todo: make path check
            cache.BIVs[selectedBIVName].DOCs[selectedDOCName].OutputDirectory = outputDirectory;
            
            VerifyUserInput();
        }

        private void textPrefixTargetNS_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            // todo: make path check
            cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespacePrefix = textPrefixTargetNS.Text;

            VerifyUserInput();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void richtextStatus_TextChanged(object sender, EventArgs e)
        {
            richtextStatus.Refresh();
        }

        private void richtextStatus_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            

            // todo: make path check
            Process.Start(cache.BIVs[selectedBIVName].DOCs[selectedDOCName].OutputDirectory + "/" + e.LinkText.Substring(5));
        }

        public static void ShowGeneratorWizard(AddInContext context)
        {
            new GeneratorWizardForm(context.CCRepository).Show();
        }

    }
}