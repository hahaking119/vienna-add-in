// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    ///<summary>
    ///</summary>
    public partial class ABIEModelerForm : Form
    {
        private ABIEModelerForm(Repository eaRepository)
        {
            InitializeComponent();
        }

        public static void ShowForm(AddInContext context)
        {
            new ABIEModelerForm(context.EARepository).ShowDialog();
        }


        private void ABIEWizardForm_Load(object sender, EventArgs e)
        {
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void groupboxASBIEs_Enter(object sender, EventArgs e)
        {

        }
    }
}