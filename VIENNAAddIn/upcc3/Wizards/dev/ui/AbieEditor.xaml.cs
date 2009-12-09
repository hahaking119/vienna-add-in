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
using System.Windows.Input;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using CheckBox=System.Windows.Controls.CheckBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox=System.Windows.Controls.ListBox;
using MessageBox=System.Windows.MessageBox;
using TextBox=System.Windows.Controls.TextBox;

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

            SelectDefaultLibraries();

            UpdateFormState();
        }

        public static void ShowCreateDialog(AddInContext context)
        {
            new AbieEditor(context.CctsRepository).ShowDialog();
        }


        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox CC Libraries
        private void comboboxCcls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboCCLs.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox ACCs
        private void comboboxAccs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateAcc(comboACCs.SelectedItem.ToString());
            Model.AbieName = comboACCs.SelectedItem.ToString();

            UpdateFormState();

            SetSelectedItemForBccListBox();
            SetSelectedItemForAbieListBox();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Checkbox BCCs
        private void checkboxBccs_Checked(object sender, RoutedEventArgs e)
        {
            string selectedItemText = ((CheckableItem) checkedlistboxBCCs.SelectedItem).Text;                        

            Model.SetCheckedForAllCandidateBccs((bool)((CheckBox)sender).IsChecked);
                        
            checkedlistboxBCCs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxBCCs, selectedItemText);
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BCCs
        private void listboxBccs_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxBCCs.SelectedItem;
            Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            checkedlistboxBCCs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxBCCs, (CheckBox)sender);
            
            SetSelectedItemForBbieListBox();

            UpdateFormState();
        }

        private void listboxBccs_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBCCs.SelectedItem != null)
            {                
                CheckableItem checkableItem = (CheckableItem)checkedlistboxBCCs.SelectedItem;
                Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBbieListBox();
            }
        }

        
        // ------------------------------------------------------------------------------------
        // Event handler: Button BBIE
        private void buttonAddBbie_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = checkedlistboxBBIEs.SelectedIndex;

            Model.AddPotentialBbie();

            checkedlistboxBBIEs.SelectedIndex = selectedIndex;            
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxBBIEs.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            checkedlistboxBBIEs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxBBIEs, (CheckBox)sender);
            
            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBBIEs.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)checkedlistboxBBIEs.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBdtListBox();
            }
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxBBIEs.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);
            
            SetSelectedItemForBdtListBox();
        }
        

        private void listboxBbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBbieName(((CheckableItem)checkedlistboxBBIEs.SelectedItem).Text);
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);
                ((TextBox)sender).Undo();
            }
        }  

        // ------------------------------------------------------------------------------------
        // Event handler: Button BDT
        private void buttonAddBdt_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = checkedlistboxBDTs.SelectedIndex;

            Model.AddPotentialBdt();   

            checkedlistboxBDTs.SelectedIndex = selectedIndex;                 
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BDTs
        private void listboxBdts_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            checkedlistboxBDTs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxBDTs, (CheckBox)sender);

            CheckableItem checkableItem = (CheckableItem)checkedlistboxBDTs.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }
        
        private void listboxBdts_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBDTs.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)checkedlistboxBDTs.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);                
            }
        }

        private void listboxBdts_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxBDTs.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);
           
            checkedlistboxBDTs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxBDTs, (TextBox)sender);
        }

        private void listboxBdts_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBdtName(((CheckableItem)checkedlistboxBDTs.SelectedItem).Text);
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);
                ((TextBox)sender).Undo();
            }
        }

        
        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ABIEs
        private void listboxAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxABIEs.SelectedItem;
            Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);
            
            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            checkedlistboxABIEs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxABIEs, (CheckBox)sender);

            SetSelectedItemForAsbieListBox();        
        }

        private void listboxAbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (checkedlistboxABIEs.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem) checkedlistboxABIEs.SelectedItem;
                Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForAsbieListBox();                
            }
            else
            {
                Model.SetNoSelectedCandidateAbie();
            }
        }

        
        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ASBIEs
        private void listboxAsbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)checkedlistboxASCCs.SelectedItem;            
            Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);

            checkedlistboxASCCs.SelectedItem = GetSelectedCheckableItemforListbox(checkedlistboxASCCs, (CheckBox)sender);
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxASCCs.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)checkedlistboxASCCs.SelectedItem;

                Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);
            }
        }
        
        
        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Prefix
        private void textAbiePrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textPrefix.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Name
        private void textAbieName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textABIEName.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BDT Libraries
        private void comboboxBdtls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboBDTLs.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BIE Libraries
        private void comboboxBiels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboBIELs.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Create
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            // Disable the "Create Button" while the ABIE is created
            buttonSave.IsEnabled = false;

            try
            {
                Model.CreateAbie();
                ShowInformativeMessage(String.Format("A new ABIE named \"{0}\" was created successfully.", Model.AbieName));
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);            
            }                       

            // After the ABIE is created the "Create Button" is enabled again. 
            buttonSave.IsEnabled = true;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Click
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion


        #region Methods for controlling the Form State

        private void UpdateFormState()
        {
            if (comboCCLs.SelectedItem != null)
            {
                SetEnabledForAccComboBox(true);

                if (comboACCs.SelectedItem != null)
                {
                    SetEnabledForAttributeAndAssoicationTabs(true);
                    SetEnabledForAbieProperties(true);

                    if ((comboBIELs.SelectedItem != null) && (comboBDTLs.SelectedItem != null) && (Model.ContainsValidConfiguration()))
                    {
                        SetEnabledForCreateButton(true);
                    }
                    else
                    {
                        SetEnabledForCreateButton(false);
                    }
                }
            }
            else
            {
                SetEnabledForAccComboBox(false);
                SetEnabledForAttributeAndAssoicationTabs(false);
                SetEnabledForAbieProperties(false);
                SetEnabledForCreateButton(false);
            }
        }

        private void SetEnabledForAccComboBox(bool enabledState)
        {
            comboACCs.IsEnabled = enabledState;
        }

        private void SetEnabledForAttributeAndAssoicationTabs(bool enabledState)
        {
            tabAttributes.IsEnabled = enabledState;
            checkboxBCCs.IsEnabled = enabledState;
            checkedlistboxBCCs.IsEnabled = enabledState;
            buttonAddBBIE.IsEnabled = enabledState;
            checkedlistboxBBIEs.IsEnabled = enabledState;
            checkedlistboxBDTs.IsEnabled = enabledState;
            buttonAddBdt.IsEnabled = enabledState;

            tabAssociations.IsEnabled = enabledState;
            checkedlistboxABIEs.IsEnabled = enabledState;
            checkedlistboxASCCs.IsEnabled = enabledState;
        }

        private void SetEnabledForAbieProperties(bool enabledState)
        {
            textPrefix.IsEnabled = enabledState;
            textABIEName.IsEnabled = enabledState;
        }

        private void SetEnabledForCreateButton(bool enabledState)
        {
            buttonSave.IsEnabled = enabledState;
        }

        #endregion


        #region Methods for extending Control Behaviour

        private void ForceTextboxToLoseFocus(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;           
            CheckBox siblingOfTextBox = (CheckBox)((StackPanel)textBox.Parent).Children[0];

            switch (e.Key)
            {
                case Key.Return:
                    siblingOfTextBox.Focus();
                    break;

                case Key.Escape:
                    textBox.Undo();
                    siblingOfTextBox.Focus();
                    break;
            }
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, string selectedItemText)
        {
            foreach (CheckableItem checkableItem in listBox.Items)
            {
                if (checkableItem.Text.Equals(selectedItemText))
                {
                    return checkableItem;
                }
            }

            return null;
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, CheckBox checkBox)
        {
            StackPanel parent = (StackPanel)checkBox.Parent;
            string selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    selectedItemText = ((TextBox)child).Text;
                }
            }

            return GetSelectedCheckableItemforListbox(listBox, selectedItemText);
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, TextBox textBox)
        {            
            string selectedItemText = textBox.Text;

            return GetSelectedCheckableItemforListbox(listBox, selectedItemText);
        }


        #endregion


        #region Methods for supporting the User Interaction

        private static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static void ShowInformativeMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SelectDefaultLibraries()
        {
            comboCCLs.SelectedIndex = 0;
            comboBIELs.SelectedIndex = 0;
            comboBDTLs.SelectedIndex = 0;
        }

        private void SetSelectedItemForBccListBox()
        {                     
            int index = 0;

            foreach (CheckableItem item in checkedlistboxBCCs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (checkedlistboxBCCs.Items.Count == index)
            {
                index = 0;
            }

            if (index < checkedlistboxBCCs.Items.Count)
            {
                checkedlistboxBCCs.SelectedItem = checkedlistboxBCCs.Items[index];              
            }
        }


        private void SetSelectedItemForBbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in checkedlistboxBBIEs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (checkedlistboxBBIEs.Items.Count == index)
            {
                index = 0;
            }

            if (index < checkedlistboxBBIEs.Items.Count)
            {
                checkedlistboxBBIEs.SelectedItem = checkedlistboxBBIEs.Items[index];           
            }
        }


        private void SetSelectedItemForBdtListBox()
        {
            int index = 0;

            foreach (CheckableItem item in checkedlistboxBDTs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (checkedlistboxBDTs.Items.Count == index)
            {
                index = 0;
            }

            if (index < checkedlistboxBDTs.Items.Count)
            {
                checkedlistboxBDTs.SelectedItem = checkedlistboxBDTs.Items[index];
            }
        }

        private void SetSelectedItemForAbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in checkedlistboxABIEs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (checkedlistboxABIEs.Items.Count == index)
            {
                index = 0;
            }

            if (index < checkedlistboxABIEs.Items.Count)
            {
                checkedlistboxABIEs.SelectedItem = checkedlistboxABIEs.Items[index];           
            }
        }

        private void SetSelectedItemForAsbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in checkedlistboxASCCs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (checkedlistboxASCCs.Items.Count == index)
            {
                index = 0;
            }

            if (index < checkedlistboxASCCs.Items.Count)
            {
                checkedlistboxASCCs.SelectedItem = checkedlistboxASCCs.Items[index];   
            }            
        }

        #endregion
    }
}