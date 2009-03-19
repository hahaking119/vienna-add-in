using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts.dra;
using EA;
using VIENNAAddIn.upcc3.ccts;

namespace Wizards
{
    public partial class BDTWizardForm : Form
    {
        #region Variable Declarations

        /* used to store the CCTS repository operated on */
        private ICCRepository sourceRepository;

        private const int MAXCDTLs = 100;
        private int[] CDTLIDTable = new int[MAXCDTLs];

        private const int MAXBDTLs = 100;
        private int[] BDTLIDTable = new int[MAXBDTLs];

        private const int MAXCDTs = 100;
        private int[] CDTIDTable = new int[MAXCDTs];

        #endregion

        public BDTWizardForm(Repository repository)
        {
            InitializeComponent();

            sourceRepository = new CCRepository(repository);
        }

        #region Event methods

        private void BDTWizardForm_Load(object sender, EventArgs e)
        {
            PopulateCDTLibraryComboBox();
            PopulateBDTLibraryComboBox();
        }

        private void comboCDTLibraries_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Definition of local variables */
            int i = 0;

            /* Initialize form controls */
            comboCDTs.Items.Clear();
            textBDTPrefix.Clear();
            checkedlistboxAttributes.Items.Clear();
            
            IEnumerable<ICDT> allCDTs = from CDT
                                        in GetCDTLibraryByID(CDTLIDTable[comboCDTLibraries.SelectedIndex]).CDTs
                                        select CDT;

            foreach (ICDT CDT in allCDTs)
            {
                comboCDTs.Items.Add(CDT.Name);
                CDTIDTable[i++] = CDT.Id;   
            }
        }

        private void comboCDTs_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedlistboxAttributes.Items.Clear();
            
            /* list attributes */
            var baseCDT = (ICDT)GetCDTByID(CDTIDTable[comboCDTs.SelectedIndex]);
            
            /* add CON attribute */
            checkedlistboxAttributes.Items.Add(baseCDT.CON.Name);

            /* add SUP attributes */
            
            
            IEnumerable<ISUP> allSUPs = from SUP in baseCDT.SUPs select SUP;
            foreach (ISUP SUP in allSUPs)
            {
                checkedlistboxAttributes.Items.Add(SUP.Name);                    
            }

            textBDTPrefix.Text = "_";
        }

        private void buttonGenerateBDT_Click(object sender, EventArgs e)
        {
            var bdtLibrary = (IBDTLibrary)GetBDTLibraryByID(BDTLIDTable[comboBDTLibraries.SelectedIndex]);
            var baseCDT = (ICDT)GetCDTByID(CDTIDTable[comboCDTs.SelectedIndex]);

            BDTSpec bdtSpec = BDTSpec.CloneCDT(baseCDT, textBDTPrefix.Text);
            IBDT newBDT = bdtLibrary.CreateBDT(bdtSpec);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkboxAttributes_CheckedChanged(object sender, EventArgs e)
        {
            // TODO: implement
        }

        #endregion

        #region Convenience Methods

        private void PopulateCDTLibraryComboBox()
        {
            int i = 0;

            foreach (ICDTLibrary cdtLibrary in sourceRepository.Libraries<ICDTLibrary>())
            {
                comboCDTLibraries.Items.Add(cdtLibrary.Name);
                CDTLIDTable[i++] = cdtLibrary.Id;
            }
        }

        private void PopulateBDTLibraryComboBox()
        {
            int i = 0;

            foreach (IBDTLibrary bdtLibrary in sourceRepository.Libraries<IBDTLibrary>())
            {
                comboBDTLibraries.Items.Add(bdtLibrary.Name);
                BDTLIDTable[i++] = bdtLibrary.Id;
            }

            comboBDTLibraries.SelectedIndex = 0;
        }


        private ICDTLibrary GetCDTLibraryByID(int id)
        {
            foreach (ICDTLibrary cdtLibrary in sourceRepository.Libraries<ICDTLibrary>())
            {
                if (cdtLibrary.Id == id)
                {
                    return cdtLibrary;
                }
            }

            return null;
        }

        private IBDTLibrary GetBDTLibraryByID(int id)
        {
            foreach (IBDTLibrary bdtLibrary in sourceRepository.Libraries<IBDTLibrary>())
            {
                if (bdtLibrary.Id == id)
                {
                    return bdtLibrary;
                }
            }

            return null;
        }

        private ICDT GetCDTByID(int cdtId)
        {
            foreach (ICDTLibrary cdtLibrary in sourceRepository.Libraries<ICDTLibrary>())
            {
                foreach (ICDT cdt in cdtLibrary.CDTs)
                {
                    if (cdt.Id == cdtId)
                    {
                        return cdt;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
