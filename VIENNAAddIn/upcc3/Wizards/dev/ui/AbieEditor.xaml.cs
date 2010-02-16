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
using CctsRepository.BieLibrary;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using CheckBox=System.Windows.Controls.CheckBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox=System.Windows.Controls.ListBox;
using MessageBox=System.Windows.MessageBox;
using TextBox=System.Windows.Controls.TextBox;
using AbieEditorModes = VIENNAAddIn.upcc3.Wizards.dev.util.EditorModes.AbieEditorModes;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class AbieEditor
    {
        public TemporaryAbieModel Model { get; set; }
        private AbieEditorModes AbieEditorMode { get; set; }
        private readonly IAbie abieToBeUpdated;


        public AbieEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent(); 

            AbieEditorMode = AbieEditorModes.Create;
            abieToBeUpdated = null;

            Model = new TemporaryAbieModel(cctsRepository);           

            DataContext = this;

            SelectDefaultLibraries();

            UpdateFormState();
        }

        public AbieEditor(ICctsRepository cctsRepository, Element elementToBeUpdated)
        {
            InitializeComponent();

            AbieEditorMode = AbieEditorModes.Update;
            abieToBeUpdated = cctsRepository.GetAbieById(elementToBeUpdated.ElementID);
            buttonCreateOrUpdate.Content = "_Update ABIE";

            Model = new TemporaryAbieModel(cctsRepository, abieToBeUpdated);

            DataContext = this;
        }

        public static void ShowCreateDialog(AddInContext context)
        {
            new AbieEditor(context.CctsRepository).ShowDialog();
        }

        public static void ShowUpdateDialog(AddInContext context)
        {            
            new AbieEditor(context.CctsRepository, (Element) context.SelectedItem).ShowDialog();
        }


        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox CC Libraries
        private void comboboxCcLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboboxCcLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox ACCs
        private void comboboxAccs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            Model.SetSelectedCandidateAcc(comboboxAccs.SelectedItem.ToString());
            Model.AbieName = comboboxAccs.SelectedItem.ToString();

            UpdateFormState();

            SetSelectedItemForBccListBox();
            SetSelectedItemForAbieListBox();
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        // ------------------------------------------------------------------------------------
        // Event handler: Checkbox BCCs
        private void checkboxBccs_Checked(object sender, RoutedEventArgs e)
        {
            string selectedItemText = ((CheckableItem) listboxBccs.SelectedItem).Text;

            innerCanvas.Visibility = Visibility.Visible;
            
            Mouse.OverrideCursor = Cursors.Wait;

            Model.SetCheckedForAllCandidateBccs((bool)((CheckBox)sender).IsChecked);

            //innerCanvas.Visibility = Visibility.Collapsed;

            Mouse.OverrideCursor = Cursors.Arrow;
            
            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, selectedItemText);

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BCCs
        private void listboxBccs_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)listboxBccs.SelectedItem;
            Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, (CheckBox)sender);
            
            SetSelectedItemForBbieListBox();

            UpdateFormState();
        }

        private void listboxBccs_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBccs.SelectedItem != null)
            {                
                CheckableItem checkableItem = (CheckableItem)listboxBccs.SelectedItem;
                Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBbieListBox();
            }
        }

        
        // ------------------------------------------------------------------------------------
        // Event handler: Button BBIE
        private void buttonAddBbie_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = listboxBbies.SelectedIndex;

            Model.AddPotentialBbie();

            listboxBbies.SelectedIndex = selectedIndex;            
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)listboxBbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxBbies, (CheckBox)sender);
            
            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBbies.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)listboxBbies.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBdtListBox();
            }
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)listboxBbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);
            
            SetSelectedItemForBdtListBox();
        }
        

        private void listboxBbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBbieName(((CheckableItem)listboxBbies.SelectedItem).Text);
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
            int selectedIndex = listboxBdts.SelectedIndex;

            Model.AddPotentialBdt();   

            listboxBdts.SelectedIndex = selectedIndex;                 
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BDTs
        private void listboxBdts_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (CheckBox)sender);

            CheckableItem checkableItem = (CheckableItem)listboxBdts.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }
        
        private void listboxBdts_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBdts.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)listboxBdts.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);                
            }
        }

        private void listboxBdts_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem)listboxBdts.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);
           
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (TextBox)sender);
        }

        private void listboxBdts_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.UpdateBdtName(((CheckableItem)listboxBdts.SelectedItem).Text);
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
            CheckableItem checkableItem = (CheckableItem)listboxAbies.SelectedItem;
            Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);
            
            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxAbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAbies, (CheckBox)sender);

            SetSelectedItemForAsbieListBox();        
        }

        private void listboxAbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (listboxAbies.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem) listboxAbies.SelectedItem;
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
            CheckableItem checkableItem = (CheckableItem)listboxAsbies.SelectedItem;            
            Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);

            listboxAsbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAsbies, (CheckBox)sender);
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxAsbies.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem)listboxAsbies.SelectedItem;

                Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);
            }
        }
        
        
        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Prefix
        private void textboxAbiePrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textboxPrefix.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Name
        private void textboxAbieName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textboxAbieName.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BDT Libraries
        private void comboboxBdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboboxBdtLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BIE Libraries
        private void comboboxBieLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboboxBieLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Create / Button Update
        private void buttonCreateOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Disable the "Create Button"/"Update Button" while the ABIE is created
            buttonCreateOrUpdate.IsEnabled = false;
            
            if (AbieEditorMode == AbieEditorModes.Create)
            {
                try
                {
                    Model.CreateAbie();
                    ShowInformativeMessage(String.Format("A new ABIE named \"{0}\" was created successfully.", Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }                                                       
            }
            else if (AbieEditorMode == AbieEditorModes.Update)
            {
                try
                {                                                            
                    Model.UpdateAbie(abieToBeUpdated);
                    ShowInformativeMessage(String.Format("The ABIE named \"{0}\" was updated successfully.", Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }                                                                     
            }

            // After the ABIE is created the "Create Button"/"Update Button" is enabled again. 
            buttonCreateOrUpdate.IsEnabled = true;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Close
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion


        #region Methods for controlling the Form State

        private void UpdateFormState()
        {
            if (AbieEditorMode == AbieEditorModes.Create)
            {                
                if (comboboxCcLibraries.SelectedItem != null)
                {
                    SetEnabledForAccComboBox(true);

                    if (comboboxAccs.SelectedItem != null)
                    {
                        SetEnabledForAttributeAndAssoicationTabs(true);
                        SetEnabledForAbieProperties(true);

                        if ((comboboxBieLibraries.SelectedItem != null) && (comboboxBdtLibraries.SelectedItem != null) && (Model.ContainsValidConfiguration()))
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
            else if (AbieEditorMode == AbieEditorModes.Update)
            {
            }
        }

        private void SetEnabledForAccComboBox(bool enabledState)
        {
            comboboxAccs.IsEnabled = enabledState;
        }

        private void SetEnabledForAttributeAndAssoicationTabs(bool enabledState)
        {
            tabAttributes.IsEnabled = enabledState;
            checkboxBccs.IsEnabled = enabledState;
            listboxBccs.IsEnabled = enabledState;
            buttonAddBbie.IsEnabled = enabledState;
            listboxBbies.IsEnabled = enabledState;
            listboxBdts.IsEnabled = enabledState;
            buttonAddBdt.IsEnabled = enabledState;

            tabAssociations.IsEnabled = enabledState;
            listboxAbies.IsEnabled = enabledState;
            listboxAsbies.IsEnabled = enabledState;
        }

        private void SetEnabledForAbieProperties(bool enabledState)
        {
            textboxPrefix.IsEnabled = enabledState;
            textboxAbieName.IsEnabled = enabledState;
        }

        private void SetEnabledForCreateButton(bool enabledState)
        {
            buttonCreateOrUpdate.IsEnabled = enabledState;
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
            comboboxCcLibraries.SelectedIndex = 0;
            comboboxBdtLibraries.SelectedIndex = 0; 
            comboboxBieLibraries.SelectedIndex = 0;            
        }

        private void SetSelectedItemForBccListBox()
        {                     
            int index = 0;

            foreach (CheckableItem item in listboxBccs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBccs.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBccs.Items.Count)
            {
                listboxBccs.SelectedItem = listboxBccs.Items[index];              
            }
        }


        private void SetSelectedItemForBbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxBbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBbies.Items.Count)
            {
                listboxBbies.SelectedItem = listboxBbies.Items[index];           
            }
        }


        private void SetSelectedItemForBdtListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxBdts.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBdts.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBdts.Items.Count)
            {
                listboxBdts.SelectedItem = listboxBdts.Items[index];
            }
        }

        private void SetSelectedItemForAbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxAbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxAbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxAbies.Items.Count)
            {
                listboxAbies.SelectedItem = listboxAbies.Items[index];           
            }
        }

        private void SetSelectedItemForAsbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxAsbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxAsbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxAsbies.Items.Count)
            {
                listboxAsbies.SelectedItem = listboxAsbies.Items[index];   
            }            
        }

        #endregion
   }
}