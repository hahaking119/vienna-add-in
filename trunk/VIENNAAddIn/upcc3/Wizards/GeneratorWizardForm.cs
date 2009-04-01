using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards
{
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
        private const int MARGIN = 15;
        private int mouseDownPosX;
        private string outputDirectory = "";        

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

        public void MirrorBIVsToUI()
        {
            comboBIVs.Items.Clear();

            foreach (cBIV docl in cache.BIVs.Values)
            {
                comboBIVs.Items.Add(docl.Name);
            }
        }

        public void MirrorModelsToUI()
        {
            comboModels.Items.Clear();

            comboModels.Items.Add("CCTS");
        }

        public void MirrorDOCsToUI()
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

        public void MirrorDOCSettingsToUI()
        {
            GatherUserInput();

            textTargetNS.Text = cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespace;
            textPrefixTargetNS.Text = cache.BIVs[selectedBIVName].DOCs[selectedDOCName].TargetNamespacePrefix;
        }

        public void GatherUserInput()
        {            
            selectedBIVName = comboBIVs.SelectedIndex >= 0 ? comboBIVs.SelectedItem.ToString() : "";
            selectedDOCName = checkedlistboxDOCs.SelectedIndex >= 0 ? checkedlistboxDOCs.SelectedItem.ToString() : "";
            outputDirectory = textOutputDirectory.Text;
        }

        #endregion

        private void comboBIVs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MirrorModelsToUI();            
            MirrorDOCsToUI();

            SetSafeIndex(comboModels, 0);
            SetSafeIndex(checkedlistboxDOCs, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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
            richtextStatus.Text = "";

            GatherUserInput();

            cBIV currentBIV = cache.BIVs[selectedBIVName];

            IDOCLibrary docl = (IDOCLibrary)ccR.GetLibrary(currentBIV.Id);

            // TODO: xsd generator needs to be adapted - currently all doc libraries are being generated whereas
            // only the ones that are checked should be generated.. 

            //TODO: currently the wizard just takes the input from the text fields whereas the prefix and the
            // target namespace should be (a) stored in the cache and (b) read from there while generation.. 
            XSDGenerator.Generator.XSDGenerator.GenerateSchemas(ccR, docl, textTargetNS.Text, textPrefixTargetNS.Text, checkboxAnnotations.CheckState == CheckState.Checked ? true : false, outputDirectory);

            //VIENNAAddIn.upcc3.XSDGenerator.Generator.XSDGenerator.GenerateSchemas(ccRepository, docLibrary,
            //                                                                                  "urn:test:namespace", "test", true,
            //                                                                                  PathToTestResource(
            //                                                                                      "\\XSDGeneratorTest\\all"));

            richtextStatus.Text = "Generating XML schemas completed!";
        }
    }
}