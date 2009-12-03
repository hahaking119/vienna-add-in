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

            SetDefaultSelectedItems();
        }

        private void SetDefaultSelectedItems()
        {
            checkedlistboxBCCs.SelectedIndex = 0;
            checkedlistboxBBIEs.SelectedIndex = 0;
            checkedlistboxBDTs.SelectedIndex = 0;

            checkedlistboxABIEs.SelectedIndex = 0;
            checkedlistboxASCCs.SelectedIndex = 0;
        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                                    
            if (checkedlistboxBCCs.SelectedItem != null)
            {
                Model.SetSelectedCandidateBcc(((CheckableItem)checkedlistboxBCCs.SelectedItem).Text);                
            }            
        }


        // TODO: optimize code - this is just to proof that it works
        // === Begin ===

        private void SelectTextBoxBccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            checkedlistboxBCCs.SelectedIndex = GetItemByText(checkedlistboxBCCs.Items, label.Content.ToString());
        }

        private void checkedlistboxBCCs_ItemChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = (CheckBox) sender;            
            StackPanel parent = (StackPanel) checkbox.Parent;
            bool checkedValue = false;

            if (checkbox.IsChecked == true)
            {
                checkedValue = true;
            }
            
            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(Label))
                {
                    string selectedItem = ((Label) child).Content.ToString();
                    checkedlistboxBCCs.SelectedIndex = GetItemByText(checkedlistboxBCCs.Items, selectedItem);
                    Model.SetCheckedForCandidateBcc(selectedItem, checkedValue);
                }
            }
        }

        private static int GetItemByText(ItemCollection itemCollection, string text)
        {
            foreach (CheckableItem item in itemCollection)
            {
                if (item.Text.Equals(text))
                {
                    return itemCollection.IndexOf(item);
                }
            }
            return -1;
        }

        // === End ===

        private void checkedlistboxBBIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBBIEs.SelectedItem != null)
            {
                Model.SetSelectedPotentialBbie(((CheckableItem)checkedlistboxBBIEs.SelectedItem).Text);    
            }            
        }

        private void checkedlistboxBDTs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkedlistboxABIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (checkedlistboxABIEs.SelectedItem != null)
            {
                Model.SetSelectedCandidateAbie(((CheckableItem)checkedlistboxABIEs.SelectedItem).Text);        
            }            
        }

        private void checkedlistboxASCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void comboBDTLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboBDTLs.SelectedItem.ToString());
        }

        private void comboBIELs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboBIELs.SelectedItem.ToString());
        }
        
        private void ForceTextboxToLoseFocus(object sender, KeyEventArgs e)
        {
            var textbox = (TextBox)sender;
            CheckBox parentCheckBox = (CheckBox)textbox.Parent;

            switch (e.Key)
            {                
                case Key.Return:
                    textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    parentCheckBox.Focus();
                    break;

                case Key.Escape:
                    textbox.Undo();
                    textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    parentCheckBox.Focus();
                    break;
            }
        }

        private void SelectTextBoxBbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox)sender;
            checkedlistboxBBIEs.SelectedIndex = GetItemByText(checkedlistboxBBIEs.Items, textbox.Text);
        }

        private void SelectTextBoxBdtItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            checkedlistboxBDTs.SelectedIndex = GetItemByText(checkedlistboxBDTs.Items, label.Content.ToString());
        }

        private void SelectTextBoxAbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            checkedlistboxABIEs.SelectedIndex = GetItemByText(checkedlistboxABIEs.Items, label.Content.ToString());
        }

        private void SelectTextBoxAsccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label)sender;
            checkedlistboxASCCs.SelectedIndex = GetItemByText(checkedlistboxASCCs.Items, label.Content.ToString());
        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
            Model.AddPotentialBbie();
        }

        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}