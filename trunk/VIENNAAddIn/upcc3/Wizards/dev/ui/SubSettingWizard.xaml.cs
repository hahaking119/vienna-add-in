// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SubSettingWizard
    {
        private readonly BackgroundWorker backgroundworker;
        private readonly ICctsRepository repo;
        private DoWorkEventHandler doWorkEventHandler;

        public SubSettingWizard(ICctsRepository cctsRepository)
        {
            backgroundworker = new BackgroundWorker
                                   {
                                       WorkerReportsProgress = false,
                                       WorkerSupportsCancellation = false
                                   };

            InitializeComponent();
            repo = cctsRepository;
            Model = new TemporarySubSettingModel(repo);
            DataContext = this;
        }

        #region BackgroundWorkers

        private void backgroundworkerDocLibs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model = (TemporarySubSettingModel) e.Result;
            backgroundworker.DoWork -= doWorkEventHandler;


            ShowShield(false);
        }

        private static void backgroundworkerDocLibs_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempModel = (TemporarySubSettingModel) e.Argument;
            tempModel.SetSelectedCandidateDocLibrary(tempModel.currentDocLibrary);
            e.Result = tempModel;
        }

        private void ShowShield(bool shown)
        {
            if (shown)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                shield.Visibility = Visibility.Visible;
                popupGenerating.Visibility = Visibility.Visible;
                var sbdRotation = (Storyboard) FindResource("sbdRotation");
                sbdRotation.Begin(this);
            }
            else
            {
                shield.Visibility = Visibility.Collapsed;
                var sbdRotation = (Storyboard) FindResource("sbdRotation");
                sbdRotation.Stop();
                popupGenerating.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion

        public TemporarySubSettingModel Model { get; set; }

        public static void ShowCreateDialog(AddInContext context)
        {
            new SubSettingWizard(context.CctsRepository).ShowDialog();
        }

        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox Doc Libraries
        private void comboboxDocLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.currentDocLibrary = comboboxDocLibraries.SelectedItem.ToString();
            doWorkEventHandler = backgroundworkerDocLibs_DoWork;
            backgroundworker.DoWork += doWorkEventHandler;
            backgroundworker.RunWorkerCompleted += backgroundworkerDocLibs_RunWorkerCompleted;
            ShowShield(true);

            backgroundworker.RunWorkerAsync(Model);
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox RootElements
        //private void comboboxRootElement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (comboboxRootElement.SelectedItem != null)
        //    {
        //        Model.SetSelectedCandidateRootElement(comboboxRootElement.SelectedItem.ToString());
        //        foreach (CheckableTreeViewItem checkableTreeViewItem in Model.CandidateAbieItems)
        //        {
        //            Console.WriteLine(checkableTreeViewItem.Text);
        //        }
        //    }
        //}

        private void treeviewAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckableTreeViewItem checkableTreeViewItem = GetSelectedCheckableTreeViewItemforTreeView(treeviewAbies,
                                                                                                      (CheckBox) sender);
            Model.SetCheckedCandidateAbie(checkableTreeViewItem.Text, checkableTreeViewItem.Checked);
        }

        private void treeviewAbies_ItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeviewAbies.SelectedItem != null)
            {
                var checkableTreeViewItem = (CheckableTreeViewItem) treeviewAbies.SelectedItem;
                Model.SetSelectedCandidateAbie(checkableTreeViewItem.Text, checkableTreeViewItem.Checked);
            }
        }


        private void treeviewAbies_Expanded(object sender, RoutedEventArgs e)
        {
            var treeviewItem = (TreeViewItem) e.OriginalSource;
            var checkabletreeviewItem = (CheckableTreeViewItem) treeviewItem.Header;
            if (checkabletreeviewItem != null)
            {
                Model.SetExpandedCheckableTreeViewItem(checkabletreeviewItem.Text, treeviewItem.IsExpanded);
            }
        }

        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox) e.OriginalSource;

            var checkableTreeViewItem = GetSelectedCheckableItemforListbox(listboxBbies,checkbox);
            Model.SetCheckedPotentialBbie(checkableTreeViewItem.Text,checkableTreeViewItem.Checked);
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        // Event handler: ListBox BDTs
        private void listboxAsbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }


        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, CheckBox checkBox)
        {
            var parent = (StackPanel)checkBox.Parent;
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

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemforTreeView(TreeView treeView,
                                                                                         CheckBox checkBox)
        {
            var parent = (StackPanel) checkBox.Parent;
            var selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof (TextBlock))
                {
                    selectedItemText = ((TextBlock) child).Text;
                }
            }
            return GetSelectedCheckableTreeViewItemForTreeView(treeView, selectedItemText);
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeView(TreeView treeView,
                                                                                         string selectedItemText)
        {
            foreach (CheckableTreeViewItem checkableTreeViewItem in treeView.Items)
            {
                if (checkableTreeViewItem.Text.Equals(selectedItemText))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children == null) continue;
                var tempItem =
                    GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                     selectedItemText);
                if (tempItem != null)
                {
                    return tempItem;
                }
            }
            return null;
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeViewItems(
            IEnumerable<CheckableTreeViewItem> listToSearch, string selectedItemText)
        {
            foreach (CheckableTreeViewItem checkableTreeViewItem in listToSearch)
            {
                if (checkableTreeViewItem.Text.Equals(selectedItemText))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children == null) continue;
                CheckableTreeViewItem tempItem =
                    GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                     selectedItemText);
                if (tempItem != null)
                {
                    return tempItem;
                }
            }
            return null;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Create / Button Update
        private void buttonCreateSubSet_Click(object sender, RoutedEventArgs e)
        {
            Model.createSubSet();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Close
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}