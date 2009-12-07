﻿using System;
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

            SetSelectedIndexForBccAndBbieAndBdtListBoxes();
            SetSelectedIndexForAbieAndAsbieListBoxes();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Checkbox BCCs
        private void checkboxBccs_Checked(object sender, RoutedEventArgs e)
        {
            int index = checkedlistboxBCCs.SelectedIndex;

            Model.SetCheckedForAllCandidateBccs((bool)((CheckBox)sender).IsChecked);

            checkedlistboxBCCs.SelectedIndex = index;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BCCs
        private void listboxBccs_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBCCs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedCandidateBcc(((CheckableItem)checkedlistboxBCCs.SelectedItem).Text, null);

                SetSelectedIndexForBbieandBdtListBoxes();
            }
        }
        
        private void listboxBccs_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            checkedlistboxBCCs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBCCs, ((TextBox) sender).Text);
        }

        private void listboxBccs_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBCCs, (CheckBox) sender);
            
            checkedlistboxBCCs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedCandidateBcc(returnValues.SelectedItem, returnValues.CheckedValue);

            SetSelectedIndexForBbieandBdtListBoxes();

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button BBIE
        private void buttonAddBbie_Click(object sender, RoutedEventArgs e)
        {
            Model.AddPotentialBbie();

            checkedlistboxBBIEs.SelectedIndex = (checkedlistboxBBIEs.Items.Count) - 1;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BBIEs

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBBIEs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialBbie(((CheckableItem)checkedlistboxBBIEs.SelectedItem).Text, null);

                SetSelectedIndexForBdtListBox();                
            }
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            checkedlistboxBBIEs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBBIEs, ((TextBox)sender).Text);
        }
        
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBBIEs, (CheckBox)sender);

            checkedlistboxBBIEs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialBbie(returnValues.SelectedItem, returnValues.CheckedValue);

            SetSelectedIndexForBdtListBox();

            UpdateFormState();
        }

        private void listboxBbies_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBbieName(((CheckableItem)checkedlistboxBBIEs.SelectedItem).Text);
            }
            catch (TemporaryAbieModelException tame)
            {                                
                ShowWarningMessage(tame.Message);
                ((TextBox) sender).Undo();
            }
            
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button BDT
        private void buttonAddBdt_Click(object sender, RoutedEventArgs e)
        {
            Model.AddPotentialBdt();

            checkedlistboxBDTs.SelectedIndex = (checkedlistboxBDTs.Items.Count) - 1;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BDTs
        private void listboxBdts_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxBDTs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialBdt(((CheckableItem)checkedlistboxBDTs.SelectedItem).Text, null);
            }
        }

        private void listboxBdts_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            checkedlistboxBDTs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxBDTs, ((TextBox)sender).Text);
        }

        private void listboxBdts_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxBDTs, (CheckBox)sender);

            checkedlistboxBDTs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialBdt(returnValues.SelectedItem, returnValues.CheckedValue);

            SetSelectedIndexForBdtListBox();

            UpdateFormState();
        }

        private void listboxBdts_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBdtNameAndSetCheckIfApplicable(((CheckableItem)checkedlistboxBDTs.SelectedItem).Text);                  
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);
                ((TextBox)sender).Undo();
            }
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ABIEs
        private void listboxAbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (checkedlistboxABIEs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedCandidateAbie(((CheckableItem)checkedlistboxABIEs.SelectedItem).Text, null);

                SetSelectedIndexForAsbieListBox();                
            }
            else
            {
                Model.SetNoSelectedCandidateAbie();
            }
        }

        private void listboxAbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            checkedlistboxABIEs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxABIEs, ((TextBox)sender).Text);
        }

        private void listboxAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxABIEs, (CheckBox)sender);

            checkedlistboxABIEs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedCandidateAbie(returnValues.SelectedItem, returnValues.CheckedValue);

            SetSelectedIndexForAsbieListBox();
        }
        
        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ASBIEs
        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkedlistboxASCCs.SelectedItem != null)
            {
                Model.SetSelectedAndCheckedPotentialAsbie(((CheckableItem)checkedlistboxASCCs.SelectedItem).Text, null);
            }
        }

        private void listboxAsbies_ItemTextBoxMouseCapture(object sender, MouseEventArgs e)
        {
            checkedlistboxASCCs.SelectedIndex = GetSelectedIndexforListbox(checkedlistboxASCCs, ((TextBox)sender).Text);
        }

        private void listboxAsbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            ReturnValues returnValues = GetValuesForSelectedItemInCheckableLabelListbox(checkedlistboxASCCs, (CheckBox)sender);

            checkedlistboxASCCs.SelectedIndex = returnValues.SelectedIndex;

            Model.SetSelectedAndCheckedPotentialAsbie(returnValues.SelectedItem, returnValues.CheckedValue);
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

                    if ((comboBIELs.SelectedItem != null) && (Model.ContainsValidConfiguration()))
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
                if (child.GetType() == typeof(TextBox))
                {
                    returnValues.SelectedIndex = GetSelectedIndexforListbox(listBox, ((TextBox)child).Text);
                    returnValues.SelectedItem = ((TextBox)child).Text;
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

        private void SetSelectedIndexForBccAndBbieAndBdtListBoxes()
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

            checkedlistboxBCCs.SelectedIndex = index;

            SetSelectedIndexForBbieandBdtListBoxes();
        }

        private void SetSelectedIndexForBbieandBdtListBoxes()
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

            checkedlistboxBBIEs.SelectedIndex = index;
            
            SetSelectedIndexForBdtListBox();
        }

        private void SetSelectedIndexForBdtListBox()
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

            checkedlistboxBDTs.SelectedIndex = index;
        }

        private void SetSelectedIndexForAbieAndAsbieListBoxes()
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

            checkedlistboxABIEs.SelectedIndex = index;

            SetSelectedIndexForAsbieListBox();
        }

        private void SetSelectedIndexForAsbieListBox()
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

            checkedlistboxASCCs.SelectedIndex = index;
        }

        #endregion
    }
}