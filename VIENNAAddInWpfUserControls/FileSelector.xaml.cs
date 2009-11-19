using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace VIENNAAddInWpfUserControls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class FileSelector : UserControl
    {
        public FileSelector()
        {
            InitializeComponent();
        }

        public string Filter { get; set; }

        public string DefaultExt { get; set; }

        private void openFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                      {
                          DefaultExt = DefaultExt,
                          Filter = Filter,
                      };
            if (dlg.ShowDialog() == true)
            {
                fileNameTextBox.Text = dlg.FileName;
            }
        }

        public new double Width
        {
            get
            {
                return dockPanel.Width;
            }
            set
            {
                dockPanel.Width = value;
            }
        }

        public new double Height
        {
            get
            {
                return dockPanel.Height;
            }
            set
            {
                dockPanel.Height = value;
            }
        }
    }
}