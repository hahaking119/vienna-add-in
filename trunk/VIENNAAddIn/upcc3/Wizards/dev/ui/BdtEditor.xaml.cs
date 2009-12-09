// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows;
using CctsRepository;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class BdtEditor
    {
        public BdtEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent(); 
        }

        public static void ShowForm(AddInContext context)
        {
            new BdtEditor(context.CctsRepository).Show();
        }

        private void buttonGenerateBDT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}