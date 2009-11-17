using System.Windows;
using System.Windows.Controls;
using EA;
using UPCCRepositoryInterface;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// Interaction logic for ABIEEditor.xaml
    /// </summary>
    public partial class ABIEEditor : Window
    {
        private readonly CCCache cache;

        /// <summary>
        /// Constructor for ABIE creation wizard
        /// </summary>
        /// <param name="eaRepository">The current repository</param>
        public ABIEEditor(Repository eaRepository)
        {
            cache = CCCache.GetInstance(new CCRepository(eaRepository));

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

            InitializeComponent();
            WindowLoaded();
        }

        public static void ShowForm(AddInContext context)
        {
            new ABIEEditor(context.EARepository).Show();
        }

        private void WindowLoaded()
        {
            ComboBoxItem testitem = new ComboBoxItem();
            testitem.Content = "test";
            comboCCLs.Items.Add(testitem);
            foreach (CCLibrary ccl in cache.GetCCLibraries())
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = ccl.Name;
                comboCCLs.Items.Add(item);
            }
        }

        private void comboCCLs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBoxItem testitem = new ComboBoxItem();
            testitem.Content = "test2";
            comboACCs.Items.Add(testitem);
            foreach (ACC acc in cache.GetCCsFromCCLibrary(((ComboBoxItem)(comboCCLs.SelectedItem)).Name))
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = acc.Name;
                comboACCs.Items.Add(item);
            }
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