using System.Windows;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev
{
    /// <summary>
    /// Interaction logic for ImporterWizard.xaml
    /// </summary>
    public partial class ABIEModeler : Window
    {
        private readonly CCRepository ccRepository;

        public ABIEModeler(Repository eaRepository)
        {
            ccRepository = new CCRepository(eaRepository);

            InitializeComponent();
        }
    }
}