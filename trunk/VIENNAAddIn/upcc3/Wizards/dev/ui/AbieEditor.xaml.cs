// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CctsRepository;
using CctsRepository.BieLibrary;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class AbieEditor
    {
        private readonly IAbie abieToBeUpdated;
        private readonly BackgroundWorker backgroundworker;
        private string bbieNameBeforeRename;
        private string bdtNameBeforeRename;
        private DoWorkEventHandler doWorkEventHandler;
        private string selectedBcc;
        private string selectedBbie;

        public AbieEditor(ICctsRepository cctsRepository)
        {
            backgroundworker = new BackgroundWorker
                                   {
                                       WorkerReportsProgress = false,
                                       WorkerSupportsCancellation = false
                                   };

            InitializeComponent();
            AbieEditorMode = EditorModes.AbieEditorModes.Create;
            abieToBeUpdated = null;

            Model = new TemporaryAbieModel(cctsRepository);

            DataContext = this;

            SelectDefaultLibraries();

            UpdateFormState();
        }

        public AbieEditor(ICctsRepository cctsRepository, Element elementToBeUpdated)
        {
            InitializeComponent();

            AbieEditorMode = EditorModes.AbieEditorModes.Update;
            abieToBeUpdated = cctsRepository.GetAbieById(elementToBeUpdated.ElementID);
            buttonCreateOrUpdate.Content = "_Update ABIE";

            Model = new TemporaryAbieModel(cctsRepository, abieToBeUpdated);

            DataContext = this;
        }

        public TemporaryAbieModel Model { get; set; }
        private EditorModes.AbieEditorModes AbieEditorMode { get; set; }

        public static void ShowCreateDialog(AddInContext context)
        {
            new AbieEditor(context.CctsRepository).ShowDialog();
        }

        public static void ShowUpdateDialog(AddInContext context)
        {
            new AbieEditor(context.CctsRepository, (Element) context.SelectedItem).ShowDialog();
        }

        #region BackgroundWorkers
        private void ShowShield(bool shown)
        {
            if (shown)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                shield.Visibility = Visibility.Visible;
                popupGenerating.Visibility = Visibility.Visible;
                var sbdRotation = (Storyboard)FindResource("sbdRotation");
                sbdRotation.Begin(this);
            }
            else
            {
                shield.Visibility = Visibility.Collapsed;
                var sbdRotation = (Storyboard)FindResource("sbdRotation");
                sbdRotation.Stop();
                popupGenerating.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void backgroundworkerAcc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model = (TemporaryAbieModel) e.Result;
            backgroundworker.DoWork -= doWorkEventHandler;

            UpdateFormState();

            SetSelectedItemForBccListBox();
            SetSelectedItemForAbieListBox();
            if (listboxBccs.SelectedItem != null)
            {
                selectedBcc = ((CheckableItem)listboxBccs.SelectedItem).Text;
            }

            ShowShield(false);
        }

        private static void backgroundworkerAcc_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempModel = (TemporaryAbieModel) e.Argument;
            tempModel.SetSelectedCandidateAcc(tempModel.AbieName);
            e.Result = tempModel;
        }

        private void backgroundworkerBccs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model = (TemporaryAbieModel) e.Result;
            backgroundworker.DoWork -= doWorkEventHandler;

            string selectedItemText = ((CheckableItem) listboxBccs.SelectedItem).Text;
            Model.SetCheckedForAllCandidateBccs(Model.temporaryCheckstate);

            SetSelectedItemForBccListBox();
            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, selectedItemText);

            UpdateFormState();

            ShowShield(false);
        }

        private static void backgroundworkerBccs_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempModel = (TemporaryAbieModel) e.Argument;
            tempModel.SetCheckedForAllCandidateBccs(tempModel.temporaryCheckstate);
            e.Result = tempModel;
        }

        #endregion

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
            Model.AbieName = comboboxAccs.SelectedItem.ToString();
            doWorkEventHandler = backgroundworkerAcc_DoWork;
            backgroundworker.DoWork += doWorkEventHandler;
            backgroundworker.RunWorkerCompleted += backgroundworkerAcc_RunWorkerCompleted;
            ShowShield(true);

            backgroundworker.RunWorkerAsync(Model);
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Checkbox BCCs
        private void checkboxBccs_Checked(object sender, RoutedEventArgs e)
        {
            Model.temporaryCheckstate = (bool) ((CheckBox) sender).IsChecked;
            doWorkEventHandler = backgroundworkerBccs_DoWork;
            backgroundworker.DoWork += doWorkEventHandler;
            backgroundworker.RunWorkerCompleted += backgroundworkerBccs_RunWorkerCompleted;
            ShowShield(true);

            backgroundworker.RunWorkerAsync(Model);

            //    string selectedItemText = ((CheckableItem) listboxBccs.SelectedItem).Text;
            //    Model.SetCheckedForAllCandidateBccs(Model.temporaryCheckstate);
            //    listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, selectedItemText);
            //}
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BCCs
        private void listboxBccs_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableItem checkableItem = (CheckableItem) listboxBccs.SelectedItem ??
                                          GetSelectedCheckableItemforListbox(listboxBccs, (CheckBox) sender);
            Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, (CheckBox) sender);

            SetSelectedItemForBbieListBox();
            if (listboxBbies.SelectedItem != null)
            {
                selectedBbie = ((CheckableItem)listboxBbies.SelectedItem).Text;
            }

            UpdateFormState();
        }

        private void listboxBccs_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBccs.SelectedItem != null)
            {
                CheckableItem checkableItem = (CheckableItem) listboxBccs.SelectedItem ??
                                              GetSelectedCheckableItemforListbox(listboxBccs, selectedBcc);
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
            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxBbies, (CheckBox) sender);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBbies.SelectedItem!=null)
            {
            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;
            
                Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBdtListBox();
            }
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            // The listbox of the BBIEs contains a set of CheckableItems. Each CheckableItem
            // consists of a CheckBox as well as a TextBox. However, in case the user clicks
            // on the text in the TextBox the GotMouseCapture event is triggered but the item
            // in the listbox is not selected causing the rename of a BBIE to fail. Therefore,
            // we need to use the following workaround which ensures that the item in the 
            // ListBox having the focus is also selected. 
            listboxBbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxBbies, (TextBox) sender);

            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;

            // We need to store the original name of the BBIE before it is changed as part of a
            // rename process. The reason for doing so is that in case the BBIE is renamed to 
            // match the name of an existing BBIE it is necessary to display the original name
            // of the BBIE. 
            bbieNameBeforeRename = checkableItem.Text;

            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();
        }


        private void listboxBbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;

            try
            {
                if (checkableItem.Text != bbieNameBeforeRename)
                {
                    Model.UpdateBbieName(checkableItem.Text);
                }
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);

                checkableItem.Text = bbieNameBeforeRename;
                ((TextBox) sender).Text = bbieNameBeforeRename;
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
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (CheckBox) sender);

            var checkableItem = (CheckableItem) listboxBdts.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBdts_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBdts.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxBdts.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);
            }
        }

        private void listboxBdts_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            // The listbox of the BDT contains a set of CheckableItems. Each CheckableItem
            // consists of a CheckBox as well as a TextBox. However, in case the user clicks
            // on the text in the TextBox the GotMouseCapture event is triggered but the item
            // in the listbox is not selected causing the rename of a BBIE to fail. Therefore,
            // we need to use the following workaround which ensures that the item in the 
            // ListBox having the focus is also selected. 
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (TextBox) sender);

            var checkableItem = (CheckableItem) listboxBdts.SelectedItem;

            // We need to store the original name of the BDT before it is changed as part of a
            // rename process. The reason for doing so is that in case the BDT is renamed to 
            // match the name of an existing BDT it is necessary to display the original name
            // of the BDT. 
            bdtNameBeforeRename = checkableItem.Text;

            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);

            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (TextBox) sender);
        }

        private void listboxBdts_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var checkableItem = ((CheckableItem) listboxBdts.SelectedItem);

            try
            {
                if (checkableItem.Text != bdtNameBeforeRename)
                {
                    Model.UpdateBdtName(checkableItem.Text);
                }
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);

                checkableItem.Text = bdtNameBeforeRename;
                ((TextBox) sender).Text = bdtNameBeforeRename;
            }
        }


        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ABIEs
        private void listboxAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxAbies.SelectedItem;
            Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxAbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAbies, (CheckBox) sender);

            SetSelectedItemForAsbieListBox();
        }

        private void listboxAbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxAbies.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxAbies.SelectedItem;
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
            var checkableItem = (CheckableItem) listboxAsbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);

            listboxAsbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAsbies, (CheckBox) sender);
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxAsbies.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxAsbies.SelectedItem;

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

            if (AbieEditorMode == EditorModes.AbieEditorModes.Create)
            {
                try
                {
                    Model.CreateAbie();
                    ShowInformativeMessage(String.Format("A new ABIE named \"{0}\" was created successfully.",
                                                         Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }
            }
            else if (AbieEditorMode == EditorModes.AbieEditorModes.Update)
            {
                try
                {
                    Model.UpdateAbie(abieToBeUpdated);
                    ShowInformativeMessage(String.Format("The ABIE named \"{0}\" was updated successfully.",
                                                         Model.AbieName));
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
            if (AbieEditorMode == EditorModes.AbieEditorModes.Create)
            {
                if (comboboxCcLibraries.SelectedItem != null)
                {
                    SetEnabledForAccComboBox(true);

                    if (comboboxAccs.SelectedItem != null)
                    {
                        SetEnabledForAttributeAndAssoicationTabs(true);
                        SetEnabledForAbieProperties(true);

                        if ((comboboxBieLibraries.SelectedItem != null) && (comboboxBdtLibraries.SelectedItem != null) &&
                            (Model.ContainsValidConfiguration()))
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
            else if (AbieEditorMode == EditorModes.AbieEditorModes.Update)
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
            var textBox = (TextBox) sender;
            var siblingOfTextBox = (CheckBox) ((StackPanel) textBox.Parent).Children[0];

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
            var parent = (StackPanel) checkBox.Parent;
            string selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof (TextBox))
                {
                    selectedItemText = ((TextBox) child).Text;
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