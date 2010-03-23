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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SubSettingWizard
    {
        private ICctsRepository repo;
        public TemporarySubSettingModel Model { get; set;}

        public SubSettingWizard(ICctsRepository cctsRepository)
        {
            InitializeComponent();
            repo = cctsRepository;
            Model = new TemporarySubSettingModel(repo);
            DataContext = this;
        }


        public static void ShowCreateDialog(AddInContext context)
        {
            new SubSettingWizard(context.CctsRepository).ShowDialog();
        }

        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox Doc Libraries
        private void comboboxDocLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            innerCanvas.Visibility = Visibility.Visible;
            //this forces refresh!
            innerCanvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate { }));

            Mouse.OverrideCursor = Cursors.Wait;

            Model.SetSelectedCandidateDocLibrary(comboboxDocLibraries.SelectedItem.ToString());
            
            Mouse.OverrideCursor = Cursors.Arrow;

            innerCanvas.Visibility = Visibility.Collapsed;
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
            var checkableTreeViewItem = GetSelectedCheckableTreeViewItemforTreeView(treeviewAbies,(CheckBox) sender);
            Model.SetSelectedAndCheckedCandidateAbie(checkableTreeViewItem.Text, checkableTreeViewItem.Checked);
        }

        private void treeviewAbies_ItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeviewAbies.SelectedItem != null)
            {
                var checkableTreeViewItem = (CheckableTreeViewItem)treeviewAbies.SelectedItem;
                Model.SetSelectedAndCheckedCandidateAbie(checkableTreeViewItem.Text, checkableTreeViewItem.Checked);
            }
        }

        
        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        

        private void listboxBbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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

        private void listboxAsbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listboxAsbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemforTreeView(TreeView treeView, CheckBox checkBox)
        {
            var parent = (StackPanel)checkBox.Parent;
            string selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(TextBlock))
                {
                    selectedItemText = ((TextBlock)child).Text;
                }
            }
            return GetSelectedCheckableTreeViewItemForTreeView(treeView, selectedItemText);
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeView(TreeView treeView, string selectedItemText)
        {
            foreach (CheckableTreeViewItem checkableTreeViewItem in treeView.Items)
            {
                if (checkableTreeViewItem.Text.Equals(selectedItemText))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children != null)
                {
                    CheckableTreeViewItem tempItem = GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                            selectedItemText);
                    if(tempItem!=null)
                    {
                        return tempItem;
                    }
                }
            }
            return null;
        }
        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeViewItems(Collection<CheckableTreeViewItem> listToSearch, string selectedItemText)
        {
            foreach (var checkableTreeViewItem in listToSearch)
            {
                if (checkableTreeViewItem.Text.Equals(selectedItemText))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children != null)
                {
                    CheckableTreeViewItem tempItem = GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                              selectedItemText);
                    if(tempItem!=null)
                    {
                        return tempItem;
                    }
                }
            }
            return null;
        }
      
        // ------------------------------------------------------------------------------------
        // Event handler: Button Create / Button Update
        private void buttonCreateSubSet_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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