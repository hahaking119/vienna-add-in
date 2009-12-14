// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Windows;
using System.Windows.Controls;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class BdtEditor
    {
        public BdtEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent();
            Model = new TemporaryBdtModel(cctsRepository);
            DataContext = this;
        }

        public TemporaryBdtModel Model { get; set; }

        public static void ShowForm(AddInContext context)
        {
            new BdtEditor(context.CctsRepository).Show();
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            buttonCreate.IsEnabled = false;

            try
            {
                Model.CreateBdt();
                ShowInformativeMessage(String.Format("A new Bdt named \"{0}\" was created successfully.", Model.Name));
            }
            catch (TemporaryBdtModelException tame)
            {
                ShowWarningMessage(tame.Message);
            }  
        }
        #region Methods for supporting the User Interaction

        private static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static void ShowInformativeMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboboxCdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.setSelectedCandidateCdtLibrary(comboboxCdtLibraries.SelectedItem.ToString());
        }

        private void comboboxCdts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.setSelectedCandidateCdt(comboboxCdts.SelectedItem.ToString());
        }

        private void comboboxBdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.setSelectedCandidateBdtLibrary(comboboxBdtLibraries.SelectedItem.ToString());
        }

        private void textboxBdtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.Name = textboxBdtName.Text;
        }

        private void textboxBdtPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.Prefix = textboxBdtPrefix.Text;
        }

        private void checkboxSups_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox) sender; 
            Model.setCheckedAllPotentialSups((bool) checkBox.IsChecked);
        }

        private void listboxSups_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox) sender;
            var stackPanel = (StackPanel) checkbox.Parent;
            var textbox = (TextBox) stackPanel.Children[1];
            Model.setCheckedPotentialSup((bool) checkbox.IsChecked, textbox.Text);
        }
    }
}