using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using TextBox = System.Windows.Controls.TextBox;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// Interaction logic for ABIEEditor.xaml
    /// </summary>
    public partial class ABIEEditor
    {
        private readonly CCCache cache;
        private TemporaryABIEModel tempModel;
        private ListModel listModelACC;
        private CheckedListModel checkedListModelBCC;
        private CheckedListModel checkedListModelBBIE;
        private CheckedListModel checkedListModelBDT;
        private CheckedListModel checkedListModelABIE;
        private CheckedListModel checkedListModelASCC;
        private ListModel listModelBIEL;
        private ListModel listModelBDTL;
        private ListModel listModelCCL;
        /// <summary>
        /// Constructor for ABIE creation wizard
        /// </summary>
        /// <param name="ccRepository">The current repository</param>
        public ABIEEditor(ICctsRepository ccRepository)
        {
            cache = CCCache.GetInstance(ccRepository);
            InitializeComponent();

            
            listModelCCL = this.Resources["listModelCCL"] as ListModel;
            listModelACC = this.Resources["listModelACC"] as ListModel;
            checkedListModelBCC = this.Resources["checkedListModelBCC"] as CheckedListModel;
            checkedListModelBBIE = this.Resources["checkedListModelBBIE"] as CheckedListModel;
            
            checkedListModelBDT = this.Resources["checkedListModelBDT"] as CheckedListModel;
            
            checkedListModelABIE = this.Resources["checkedListModelABIE"] as CheckedListModel;
            
            checkedListModelASCC = this.Resources["checkedListModelASCC"] as CheckedListModel;
            
            listModelBIEL = this.Resources["listModelBIEL"] as ListModel;
            
            listModelBDTL = this.Resources["listModelBDTL"] as ListModel;
            //tempModel.Initialize(listModelCCL, listModelACC, checkedListModelBCC, checkedListModelBBIE, checkedListModelBDT, checkedListModelABIE, checkedListModelASCC, listModelBIEL, listModelBDTL);
            listModelCCL.Content = new ObservableCollection<string>(new List<string>(cache.GetCCLibraries().ConvertAll(
                                                                                         delegate(ICcLibrary iccLibrary) { return iccLibrary.Name; })));
            WindowLoaded();
        }

        /// <summary>
        /// Constructor for ABIE modification wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        /// <param name="abie"></param>
        public ABIEEditor(Repository eaRepository, IAbie abie)
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
            new ABIEEditor(context.CctsRepository).Show();
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

        private void checkedlistboxtboxBCCs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var listbox = (ListBox)sender;
            listbox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxKeyUp(object sender, KeyEventArgs e)
        {
            var textbox = (TextBox)sender;
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
            var textbox = (TextBox)sender;
            textbox.Background = System.Windows.Media.Brushes.Transparent;
            var stackpanel = (StackPanel)textbox.Parent;
            var listbox = (ListBox)stackpanel.Parent;
            listbox.SelectedItem = stackpanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBCCCheckboxItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBox)sender;
            int i = 0;
            foreach (CheckedListItem item in checkedlistboxBCCs.Items)
            {
                if (item.Name.Equals(textBox.Text))
                {
                    checkedlistboxBCCs.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }

        private void comboCCLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            listModelACC.Content = new ObservableCollection<string>(new List<string>(cache.GetCCsFromCCLibrary(comboBox.SelectedItem.ToString()).ConvertAll(
                                                                                         delegate(IAcc acc) { return acc.Name; })));
            //tempModel.setCCLInUse(comboBox.SelectedItem.ToString());
        }

        private void comboACCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tempModel = new TemporaryABIEModel(cache.GetCCFromCCLibrary(comboCCLs.SelectedItem.ToString(),comboACCs.SelectedItem.ToString()),textABIEName.Text,textPrefix.Text);
            checkedListModelBCC.Content = new ObservableCollection<CheckedListItem>(tempModel.BCCs);
            //tempModel.SetTargetACC(comboBox.SelectedItem.ToString());
        }

        private void checkboxBCCs_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkboxBCCs_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = (ListBox)sender;
            var checkedListItem = (CheckedListItem)listbox.SelectedItem;
            if (checkedListItem == null)
            {
                listbox.SelectedIndex = 0;
                checkedListItem = (CheckedListItem)listbox.SelectedItem;
            }
            checkedListModelBBIE.Content =
                    new ObservableCollection<CheckedListItem>(tempModel.GetBBIEs(checkedListItem.Name));
                //hack to force BDT Loading on BCC selection!
            checkedlistboxBBIEs.SelectedIndex = 0;
        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
            var checkedItem = (CheckedListItem) checkedlistboxBCCs.SelectedItem;
            tempModel.AddBBIE(checkedItem.Name,"New"+checkedItem.Name + checkedlistboxBBIEs.Items.Count);
        }

        private void checkedlistboxBBIEs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = (ListBox)sender;
            var checkedListItem = (CheckedListItem)listbox.SelectedItem;
            if (checkedListItem != null)
            {
                checkedListModelBDT.Content =
                    new ObservableCollection<CheckedListItem>(tempModel.GetBDTs(checkedListItem.Name));
                //select first BDT as default for BBIE
                checkedlistboxBDTs.SelectedIndex = 0;
            }
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