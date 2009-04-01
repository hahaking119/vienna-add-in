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

            cache.BIVs[selectedBIVName].LoadDOCs(ccR);

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
    }
}