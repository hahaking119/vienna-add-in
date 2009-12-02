using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    public partial class AbieEditor
    {
        public TemporaryAbieModel Model { get; set; }        

        public AbieEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent(); 

            Model = new TemporaryAbieModel(cctsRepository);           

            DataContext = this;            
        }

        public static void ShowForm(AddInContext context)
        {
            new AbieEditor(context.CctsRepository).Show();
        }

        private void textPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textPrefix.Text;
        }

        private void textABIEName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textABIEName.Text;
        }

        private void comboCCLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboCCLs.SelectedItem.ToString());
        }

        private void comboACCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateAcc(comboACCs.SelectedItem.ToString());
        }

        private void comboBDTLs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboBDTLs.SelectedItem.ToString());
        }

        private void comboBIELs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboBIELs.SelectedItem.ToString());
        }

        private void textboxKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void SelectBCCCheckboxItemOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void checkedlistboxBCCs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void buttonAddBBIE_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hallo");            
            Model.AbieName = "changed through button";
            Model.AbiePrefix = "well hello there";
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

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}