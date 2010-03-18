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

        public void Initialize(ICctsRepository repo)
        {
            var rootItem = new TreeViewItem();
            rootItem.Header = "Current Project";
            foreach (var bLib in repo.GetBLibraries())
            {
                var bLibItem = new TreeViewItem();
                bLibItem.Header = bLib.Name + " <bLibrary>";
                bLibItem.Tag = bLib;
                foreach(var lib in bLib.GetBdtLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <BDTLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("IBdtLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetBieLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <BIELibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("IBieLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetCcLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <CCLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("ICcLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetCdtLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <CDTLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("ICdtLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetDocLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <DOCLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("IDocLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetEnumLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <ENUMLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("IEnumLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                foreach (var lib in bLib.GetPrimLibraries())
                {
                    var temp = new TreeViewItem();
                    temp.Header = lib.Name + " <PRIMLibrary>";
                    temp.Tag = lib;
                    if (AllowOnlyOneType != null)
                        if (!AllowOnlyOneType.Equals("IPrimLibrary"))
                            temp.IsEnabled = false;
                    bLibItem.Items.Add(temp);
                }
                rootItem.Items.Add(bLibItem);
            }
            tree.Items.Add(rootItem);
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
}