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


        #region Event Handler Methods

        // ------------------------------------------------------------------------------------
        // Event handler for ComboBox containing all CC Libraries
        private void comboCCLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboCCLs.SelectedItem.ToString());
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ComboBox containing ACCs 
        private void comboACCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateAcc(comboACCs.SelectedItem.ToString());
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ListBox containing BCCs
        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBCCs.SelectedItem != null)
            {                  
                Model.SetSelectedAndCheckedCandidateBcc(((CheckableItem)checkedlistboxBCCs.SelectedItem).Text, null);
            }
        }

        private void SelectTextBoxBccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            checkedlistboxBCCs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBCCs, ((Label) sender).Content.ToString());
        }

        private void checkedlistboxBCCs_ItemChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBCCs, (CheckBox) sender);

            checkedlistboxBCCs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedCandidateBcc(returnValues.SelectedItem, returnValues.CheckedValue);
        }

        // ------------------------------------------------------------------------------------
        // Event handler for button to add BBIE
        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
            Model.AddPotentialBbie();
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ListBox containing BBIEs
        private void checkedlistboxBBIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBBIEs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialBbie(((CheckableItem)checkedlistboxBBIEs.SelectedItem).Text, null);    
            }
        }

        private void SelectTextBoxBbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            checkedlistboxBBIEs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBBIEs, ((TextBox) sender).Text);
        }

        private void checkedlistboxBBIEs_ItemChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBBIEs, (CheckBox)sender);

            checkedlistboxBBIEs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialBbie(returnValues.SelectedItem, returnValues.CheckedValue);
        }

        private void checkedlistboxBBIEs_LostFocus(object sender, RoutedEventArgs e)
        {
            Model.UpdateBbieName();
        }


        // ------------------------------------------------------------------------------------
        // Event handler for ListBox containing BDTs
        private void checkedlistboxBDTs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBDTs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialBdt(((CheckableItem)checkedlistboxBDTs.SelectedItem).Text, null); 
            }
        }

        private void SelectTextBoxBdtItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            checkedlistboxBDTs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBDTs, ((Label)sender).Content.ToString());
        }

        private void checkedlistboxBDTs_ItemChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBDTs, (CheckBox)sender);

            checkedlistboxBDTs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialBdt(returnValues.SelectedItem, returnValues.CheckedValue);
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ListBox containing ABIEs
        private void checkedlistboxABIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (checkedlistboxABIEs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedCandidateAbie(((CheckableItem)checkedlistboxABIEs.SelectedItem).Text, null);        
            }            
        }

        private void SelectTextBoxAbieItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            checkedlistboxABIEs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxABIEs, ((Label)sender).Content.ToString());
        }

        private void checkedlistboxABIEs_ItemChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxABIEs, (CheckBox)sender);

            checkedlistboxABIEs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedCandidateAbie(returnValues.SelectedItem, returnValues.CheckedValue);
        }
        
        // ------------------------------------------------------------------------------------
        // Event handler for ListBox containing ASCCs (potential ASBIEs)
        private void checkedlistboxASCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxASCCs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialAsbie(((CheckableItem)checkedlistboxASCCs.SelectedItem).Text, null);
            }
        }

        private void SelectTextBoxAsccItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            checkedlistboxASCCs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxASCCs, ((Label)sender).Content.ToString());
        }

        private void checkedlistboxASCCs_ItemChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxASCCs, (CheckBox)sender);

            checkedlistboxASCCs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialAsbie(returnValues.SelectedItem, returnValues.CheckedValue);
        }

        // ------------------------------------------------------------------------------------
        // Event handler for TextBox containing the prefix of the ABIE
        private void textPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textPrefix.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler for TextBox containing the name of the ABIE
        private void textABIEName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textABIEName.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ComboBox containing all BDT Libraries
        private void comboBDTLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboBDTLs.SelectedItem.ToString());
        }

        // ------------------------------------------------------------------------------------
        // Event handler for ComboBox containing all BIE Libraries
        private void comboBIELs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboBIELs.SelectedItem.ToString());
        }       


        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Convenience Methods

        private void ForceTextboxToLoseFocus(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;
            CheckBox siblingOfTextBox = (CheckBox) ((StackPanel) textBox.Parent).Children[0];            

            switch (e.Key)
            {
                case Key.Return:
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    siblingOfTextBox.Focus();
                    break;

                case Key.Escape:
                    textBox.Undo();
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    siblingOfTextBox.Focus();
                    break;
            }
        }

        private static int GetSelectedIndexforListbox(ListBox listBox, string selectedItem)
        {
            foreach (CheckableItem checkableItem in listBox.Items)
            {
                if (checkableItem.Text.Equals(selectedItem))
                {
                    return listBox.Items.IndexOf(checkableItem);
                }
            }

            return 0;
        }

        private static ReturnValues GetValuesForSelectedItemInCheckableLabelListbox(ListBox listBox, CheckBox checkBox)
        {
            StackPanel parent = (StackPanel)checkBox.Parent;

            ReturnValues returnValues = new ReturnValues(0, "", false);

            if (checkBox.IsChecked == true)
            {
                returnValues.CheckedValue = true;
            }

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(Label))
                {
                    returnValues.SelectedIndex = GetSelectedIndexforListbox(listBox, ((Label)child).Content.ToString());
                    returnValues.SelectedItem = ((Label)child).Content.ToString();
                    break;
                }

                if (child.GetType() == typeof(TextBox))
                {
                    returnValues.SelectedIndex = GetSelectedIndexforListbox(listBox, ((TextBox) child).Text);
                    returnValues.SelectedItem = ((TextBox) child).Text;
                }
            }

            return returnValues;
        }

        private class ReturnValues
        {
            public int SelectedIndex { get; set; }
            public string SelectedItem { get; set; }
            public bool CheckedValue { get; set; }

            public ReturnValues(int selectedIndex, string selectedItem, bool checkedValue)
            {
                SelectedIndex = selectedIndex;
                SelectedItem = selectedItem;
                CheckedValue = checkedValue;
            }
        }

        #endregion
    }
}