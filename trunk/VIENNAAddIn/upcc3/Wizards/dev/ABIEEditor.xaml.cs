using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EA;
using UPCCRepositoryInterface;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using CheckBox=System.Windows.Controls.CheckBox;
using KeyEventArgs=System.Windows.Input.KeyEventArgs;
using KeyEventHandler=System.Windows.Input.KeyEventHandler;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// Interaction logic for ABIEEditor.xaml
    /// </summary>
    public partial class ABIEEditor : Window
    {
        private readonly CCCache cache;
        private bool renameInProgress;

        /// <summary>
        /// Constructor for ABIE creation wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        public ABIEEditor(Repository eaRepository)
        {
            cache = CCCache.GetInstance(new CCRepository(eaRepository));
            renameInProgress = false;

            InitializeComponent();
            WindowLoaded();
        }

        /// <summary>
        /// Constructor for ABIE modification wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        public ABIEEditor(Repository eaRepository, IABIE abie)
        {
            cache = CCCache.GetInstance(new CCRepository(eaRepository));
            cache.PrepareForABIE(abie);
            renameInProgress = false;

            InitializeComponent();
            WindowLoaded();
        }

        public static void ShowForm(AddInContext context)
        {
            new ABIEEditor(context.EARepository).Show();
        }

        private void WindowLoaded()
        {
            foreach (CCLibrary ccl in cache.GetCCLibraries())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = ccl.Name;
                comboCCLs.Items.Add(item);
            }
            // start rename listbox-item test
            CheckBox testitemA = new CheckBox();
            testitemA.Content = "TestA";
            checkedlistboxBBIEs.Items.Add(testitemA);
            CheckBox testitem1 = new CheckBox();
            testitem1.Content = "Test1";
            testitem1.MouseDoubleClick += new MouseButtonEventHandler(checkboxRename_MouseDoubleClick);
            CheckBox testitem2 = new CheckBox();
            testitem2.Content = "Test2";
            testitem2.MouseDoubleClick += new MouseButtonEventHandler(checkboxRename_MouseDoubleClick);
            CheckBox testitem3 = new CheckBox();
            testitem3.Content = "Test3";
            testitem3.MouseDoubleClick += new MouseButtonEventHandler(checkboxRename_MouseDoubleClick);
            checkedlistboxBDTs.Items.Add(testitem1);
            checkedlistboxBDTs.Items.Add(testitem2);
            checkedlistboxBDTs.Items.Add(testitem3);
            // end rename listbox-item test
        }

        // start rename listbox-item test
        private void checkboxRename_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!renameInProgress)
            {
                CheckBox checkbox = (CheckBox) sender;
                string oldName = checkbox.Content.ToString();
                int index = checkedlistboxBDTs.Items.IndexOf(sender);

                RenameTextBox textbox = new RenameTextBox();
                textbox.Text = oldName;
                textbox.OldText = oldName;
                textbox.ListIndex = index;
                textbox.CheckState = (bool) checkbox.IsChecked;
                textbox.Width = 150;
                checkedlistboxBDTs.Items.RemoveAt(index);
                checkedlistboxBDTs.Items.Insert(index, textbox);
                textbox.BorderThickness = new Thickness(1.0);

                textbox.LostFocus += new RoutedEventHandler(textRename_LostFocus);
                textbox.KeyUp += new KeyEventHandler(textRename_KeyUp);
                renameInProgress = true;
            }
        }

        private void textRename_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Return)
            {
                textRenameApply(sender);
            }
            else if(e.Key == System.Windows.Input.Key.Escape)
            {
                textRenameRevert(sender);
            }
        }

        private void textRename_LostFocus(object sender, RoutedEventArgs e)
        {
            textRenameApply(sender);
        }

        private void textRenameApply(object sender)
        {
            RenameTextBox textbox = (RenameTextBox)sender;
            string newName = textbox.Text;
            int index = textbox.ListIndex;
            CheckBox item = new CheckBox();
            item.Content = newName;
            item.IsChecked = textbox.CheckState;
            item.MouseDoubleClick += new MouseButtonEventHandler(checkboxRename_MouseDoubleClick);
            checkedlistboxBDTs.Items.RemoveAt(index);
            checkedlistboxBDTs.Items.Insert(index, item);
            renameInProgress = false;
        }

        private void textRenameRevert(object sender)
        {
            RenameTextBox textbox = (RenameTextBox)sender;
            int index = textbox.ListIndex;
            CheckBox item = new CheckBox();
            item.Content = textbox.OldText;
            item.IsChecked = textbox.CheckState;
            item.MouseDoubleClick += new MouseButtonEventHandler(checkboxRename_MouseDoubleClick);
            checkedlistboxBDTs.Items.RemoveAt(index);
            checkedlistboxBDTs.Items.Insert(index, item);
            renameInProgress = false;
        }
        // end rename listbox-item test

        private void comboCCLs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void comboACCs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void checkboxBCCs_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkboxBCCs_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkedlistboxBBIEs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void checkedlistboxBDTs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void checkedlistboxABIEs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void checkedlistboxASCCs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void textPrefix_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void textABIEName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void comboBDTLs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void comboBIELs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}