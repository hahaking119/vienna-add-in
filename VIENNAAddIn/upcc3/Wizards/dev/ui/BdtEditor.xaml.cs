using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class BdtEditor
    {
        public BdtEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent(); 
        }

        public static void ShowForm(AddInContext context)
        {
            new BdtEditor(context.CctsRepository).Show();
        }

        private void buttonGenerateBDT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}