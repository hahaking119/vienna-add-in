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

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboboxCdtLibraries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void comboboxCdts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void comboboxBdtLibraries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void textboxBdtName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void textboxBdtPrefix_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void listboxSups_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void listboxCon_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void checkboxSups_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkboxSups_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void listboxSups_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {

        }

        private void listboxCon_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {

        }
    }
}