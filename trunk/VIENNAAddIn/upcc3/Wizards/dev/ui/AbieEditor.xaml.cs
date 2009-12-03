using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class AbieEditor
    {
        public TemporaryAbieModel Model { get; set; }        

        public AbieEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent(); 

            Model = new TemporaryAbieModel(cctsRepository);           

            DataContext = this;            
        }

        public static void ShowForm(AddInContext context)
        {
            new AbieEditor(context.CctsRepository).Show();
        }

        private void textPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textPrefix.Text;
        }

        private void textABIEName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textABIEName.Text;
        }

        private void comboCCLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboCCLs.SelectedItem.ToString());
        }

        private void comboACCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateAcc(comboACCs.SelectedItem.ToString());
        }

        private void comboBDTLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboBDTLs.SelectedItem.ToString());
        }

        private void comboBIELs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboBIELs.SelectedItem.ToString());
        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBCCs.SelectedItem != null)
            Model.SetSelectedCandidateBcc(checkedlistboxBCCs.SelectedItem.ToString());
        }

        private void checkedlistboxBBIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(checkedlistboxBBIEs.SelectedItem!=null)
            Model.SetSelectedPotentialBbie(checkedlistboxBBIEs.SelectedItem.ToString());
        }

        private void checkedlistboxBDTs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkedlistboxABIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateAbie(checkedlistboxABIEs.SelectedItem.ToString());
        }

        private void checkedlistboxASCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void textboxKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void SelectTextBoxBccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            checkedlistboxBCCs.SelectedIndex = GetItemByText(checkedlistboxBCCs.Items,label.Content.ToString());
        }
        private void SelectTextBoxBbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox)sender;
            checkedlistboxBBIEs.SelectedIndex = GetItemByText(checkedlistboxBBIEs.Items, textbox.Text);
        }
        private void SelectTextBoxBdtItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox)sender;
            checkedlistboxBDTs.SelectedIndex = GetItemByText(checkedlistboxBDTs.Items, textbox.Text);
        }
        private void SelectTextBoxAbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox)sender;
            checkedlistboxABIEs.SelectedIndex = GetItemByText(checkedlistboxABIEs.Items, textbox.Text);
        }
        private void SelectTextBoxAsccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox)sender;
            checkedlistboxASCCs.SelectedIndex = GetItemByText(checkedlistboxASCCs.Items, textbox.Text);
        }
        private static int GetItemByText(ItemCollection itemCollection, string text)
        {
            foreach (CheckableText item in itemCollection)
            {
                if(item.Text.Equals(text))
                {
                    return itemCollection.IndexOf(item);
                }
            }
            return -1;
        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
            Model.AddBbie();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}