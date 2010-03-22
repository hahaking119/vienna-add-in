using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CctsRepository;
using CctsRepository.BLibrary;
using Microsoft.Win32;

namespace VIENNAAddInWpfUserControls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class ProjectBrowser : UserControl
    {
        private TreeViewItem root;

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(FileSelector));

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public string AllowOnlyOneType { get; set; }

        public ProjectBrowser()
        {
            InitializeComponent();
        }

        public void Initialize(ProjectBrowserContent content)
        {
            root = new TreeViewItem();
            root.Header = content.rootItem.Header;
            root.Tag = content.rootItem.Tag;
            root.IsExpanded = true;
            AddChildrenToTreeViewItem(content.rootItem, root);
            CheckIfItemEnabled(root);
            tree.Items.Add(root);
        }

        private void AddChildrenToTreeViewItem(TreeViewItem source, TreeViewItem target)
        {
            foreach(TreeViewItem child in source.Items)
            {
                var newChild = new TreeViewItem();
                newChild.Header = child.Header;
                newChild.Tag = child.Tag;
                newChild.IsExpanded = true;
                AddChildrenToTreeViewItem(child, newChild);
                target.Items.Add(newChild);
            }
        }

        private void CheckIfItemEnabled(TreeViewItem item)
        {
            if(item.Items.Count > 0)
            {
                foreach(TreeViewItem child in item.Items)
                {
                    CheckIfItemEnabled(child);
                }
            }
            else
            {
                if(item.Tag != null && AllowOnlyOneType != null)
                {
                    item.IsEnabled = (item.Tag.GetType().Name.EndsWith(AllowOnlyOneType));
                }
            }
        }

        private static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSelector));

        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = (TreeViewItem) tree.SelectedItem;
            if(item.Tag != null)
            {
                SelectedItem = tree.SelectedItem;
                RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
            }
        }
    }

    public class ProjectBrowserContent
    {
        public TreeViewItem rootItem { get; set; }

        public ProjectBrowserContent(ICctsRepository repo)
        {
            rootItem = new TreeViewItem();
            rootItem.Header = "Current Project";
            foreach (var bLib in repo.GetBLibraries())
            {
                var bLibItem = new TreeViewItem();
                bLibItem.Header = bLib.Name + " <bLibrary>";
                bLibItem.Tag = bLib;
                foreach (var lib in bLib.GetBdtLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <BDTLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetBieLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <BIELibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetCcLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <CCLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetCdtLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <CDTLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetDocLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <DOCLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetEnumLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <ENUMLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetPrimLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <PRIMLibrary>";
                    temp.Tag = lib;
                    bLibItem.Items.Add(temp);
                }
                rootItem.Items.Add(bLibItem);
            }
        }
    }
}