using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EA;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using CheckBox=System.Windows.Controls.CheckBox;
using KeyEventArgs=System.Windows.Input.KeyEventArgs;
using ListBox=System.Windows.Controls.ListBox;
using TextBox=System.Windows.Controls.TextBox;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// Interaction logic for ABIEEditor.xaml
    /// </summary>
    public partial class ABIEEditor
    {
        private readonly CCCache cache;
        private TemporaryABIEModel tempModel;
        private bool renameInProgress;

        /// <summary>
        /// Constructor for ABIE creation wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        public ABIEEditor(Repository eaRepository)
        {
            tempModel = new TemporaryABIEModel(new CCRepository(eaRepository));
            renameInProgress = false;

            InitializeComponent();

            ListModel listModel = this.Resources["listModelCCL"] as ListModel;
            CheckedListModel checkedListModel = this.Resources["checkedListModelCCL"] as CheckedListModel;
            this.tempModel.Initialize(listModel, checkedListModel);

            WindowLoaded();
        }

        /// <summary>
        /// Constructor for ABIE modification wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        /// <param name="abie"></param>
        public ABIEEditor(Repository eaRepository, IABIE abie)
        {
            tempModel = new TemporaryABIEModel(new CCRepository(eaRepository));
            //cache.PrepareForABIE(abie);
            renameInProgress = false;

            InitializeComponent();
            WindowLoaded();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void ShowForm(AddInContext context)
        {
            new ABIEEditor(context.EARepository).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        private void WindowLoaded()
        {
            /*foreach (var cclname in tempModel.getCCLs())
            {
                comboCCLs.Items.Add(cclname);
            }*/

            //foreach (BDTLibrary bdtLibrary in cache.GetBDTLibraries())
            //{
            //    var item = new ComboBoxItem {Content = bdtLibrary.Name};
            //    comboBDTLs.Items.Add(item);
            //}
            //foreach (BIELibrary bieLibrary in cache.GetBIELibraries())
            //{
            //    var item = new ComboBoxItem { Content = bieLibrary.Name };
            //    comboBIELs.Items.Add(item);
            //}
            //BDTs
            // start rename listbox-item test
            var testitem1 = new CheckBox {Content = "Test1"};
            testitem1.MouseDoubleClick += checkboxRename_MouseDoubleClick;
            testitem1.PreviewMouseLeftButtonDown += SelectCheckboxItemOnMouseSingleClick;
            
            var testitem2 = new CheckBox {Content = "Test2"};
            testitem2.MouseDoubleClick += checkboxRename_MouseDoubleClick;
            testitem2.PreviewMouseLeftButtonDown += SelectCheckboxItemOnMouseSingleClick;
            var testitem3 = new CheckBox {Content = "Test3"};
            testitem3.MouseDoubleClick += checkboxRename_MouseDoubleClick;
            testitem3.PreviewMouseLeftButtonDown += SelectCheckboxItemOnMouseSingleClick;
            checkedlistboxBDTs.Items.Add(testitem1);
            checkedlistboxBDTs.Items.Add(testitem2);
            checkedlistboxBDTs.Items.Add(testitem3);
            // end rename listbox-item test

            //BCCs
            var testpanel = new StackPanel
                                {
                                    Orientation = Orientation.Horizontal
                                };
            var coolcheckbox = new CheckBox {VerticalAlignment = VerticalAlignment.Center};
            var cooltextbox = new TextBox {Text = "BCC1", BorderThickness = new Thickness(0)};
            cooltextbox.PreviewMouseLeftButtonDown += SelectTextBoxItemOnMouseSingleClick;
            cooltextbox.KeyUp += textboxKeyUp;
            cooltextbox.UndoLimit = 1;
            cooltextbox.Background = System.Windows.Media.Brushes.Transparent;
            testpanel.Children.Add(coolcheckbox);
            testpanel.Children.Add(cooltextbox);
            checkedlistboxBCCs.Items.Add(testpanel);
            checkedlistboxBCCs.MouseDown += checkedlistboxtboxBCCs_MouseDown;
            //
        }

        private static void checkedlistboxtboxBCCs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox) sender;
            listbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void textboxKeyUp(object sender, KeyEventArgs e)
        {
            var textbox = (TextBox) sender;
            switch (e.Key)
            {
                case Key.Tab:
                case Key.Return:
                    textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                case Key.Escape:
                    textbox.Undo();
                    textbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SelectTextBoxItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textbox = (TextBox) sender;
            textbox.Background = System.Windows.Media.Brushes.Transparent;
            var stackpanel = (StackPanel) textbox.Parent;
            var listbox = (ListBox) stackpanel.Parent;
            listbox.SelectedItem = stackpanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SelectCheckboxItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var listbox = (ListBox) checkbox.Parent;
            listbox.SelectedItem = checkbox;
        }
        // start rename listbox-item test
        private void checkboxRename_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!renameInProgress)
            {
                var checkbox = (CheckBox) sender;
                string oldName = checkbox.Content.ToString();
                int index = checkedlistboxBDTs.Items.IndexOf(sender);

                var textbox = new RenameTextBox
                                  {
                                      Text = oldName,
                                      OldText = oldName,
                                      ListIndex = index,
                                      CheckState = (!(bool) checkbox.IsChecked),
                                      Width = 150
                                  };
                //checkstate has to be inverted because of double click action!
                checkedlistboxBDTs.Items.RemoveAt(index);
                checkedlistboxBDTs.Items.Insert(index, textbox);
                textbox.BorderThickness = new Thickness(1.0);

                //textbox.LostFocus += textRename_LostFocus;
                textbox.KeyUp += textRename_KeyUp;
                renameInProgress = true;
            }
        }

        private void textRename_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                textRenameApply(sender);
            }
            else if (e.Key == Key.Escape)
            {
                textRenameRevert(sender);
            }
        }

        //Unfortunal this does not include the outside click event!
        //private void textRename_LostFocus(object sender, RoutedEventArgs e)
        //{
        //   textRenameApply(sender);
         
        //}

        private void textRenameApply(object sender)
        {
            var textbox = (RenameTextBox) sender;
            string newName = textbox.Text;
            int index = textbox.ListIndex;
            var item = new CheckBox {Content = newName, IsChecked = textbox.CheckState};
            item.MouseDoubleClick += checkboxRename_MouseDoubleClick;
            item.PreviewMouseLeftButtonDown += SelectCheckboxItemOnMouseSingleClick;
            checkedlistboxBDTs.Items.RemoveAt(index);
            checkedlistboxBDTs.Items.Insert(index, item);
            renameInProgress = false;
        }

        private void textRenameRevert(object sender)
        {
            var textbox = (RenameTextBox) sender;
            var index = textbox.ListIndex;
            var item = new CheckBox {Content = textbox.OldText, IsChecked = textbox.CheckState};
            item.MouseDoubleClick += checkboxRename_MouseDoubleClick;
            item.PreviewMouseLeftButtonDown += SelectCheckboxItemOnMouseSingleClick;
            checkedlistboxBDTs.Items.RemoveAt(index);
            checkedlistboxBDTs.Items.Insert(index, item);
            renameInProgress = false;
        }

        // end rename listbox-item test

        private void comboCCLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox) sender;
            tempModel.setCCLInUse(comboBox.SelectionBoxItemStringFormat);
            //foreach (string selection in e.AddedItems)
            //{
            //    foreach (ACC acc in cache.GetCCsFromCCLibrary(selection))
            //    {
            //        comboACCs.Items.Add(acc.Name);
            //    }
            //}
        }

        private void comboACCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkboxBCCs_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void checkboxBCCs_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
        }

        private void checkedlistboxBBIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkedlistboxBDTs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkedlistboxABIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void checkedlistboxASCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void textPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void textABIEName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void comboBDTLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void comboBIELs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}