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

        /// <summary>
        /// Constructor for ABIE creation wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        public ABIEEditor(Repository eaRepository)
        {
            tempModel = new TemporaryABIEModel(new CCRepository(eaRepository));

            InitializeComponent();

            ListModel listModelCCL = this.Resources["listModelCCL"] as ListModel;
            ListModel listModelACC = this.Resources["listModelACC"] as ListModel;
            CheckedListModel checkedListModelBCC = this.Resources["checkedListModelBCC"] as CheckedListModel;
            CheckedListModel checkedListModelBBIE = this.Resources["checkedListModelBBIE"] as CheckedListModel;
            CheckedListModel checkedListModelBDT = this.Resources["checkedListModelBDT"] as CheckedListModel;
            CheckedListModel checkedListModelABIE = this.Resources["checkedListModelABIE"] as CheckedListModel;
            CheckedListModel checkedListModelASCC = this.Resources["checkedListModelASCC"] as CheckedListModel;
            ListModel listModelBIEL = this.Resources["listModelBIEL"] as ListModel;
            ListModel listModelBDTL = this.Resources["listModelBDTL"] as ListModel;
            tempModel.Initialize(listModelCCL, listModelACC, checkedListModelBCC, checkedListModelBBIE, checkedListModelBDT, checkedListModelABIE, checkedListModelASCC, listModelBIEL, listModelBDTL);

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
            /*var testpanel = new StackPanel
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
            checkedlistboxBCCs.MouseDown += checkedlistboxtboxBCCs_MouseDown;*/
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