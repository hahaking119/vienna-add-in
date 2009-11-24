using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            fileNameTextBox.SetBinding(TextBox.TextProperty, new Binding("FileName")
                                                             {
                                                                 Source = this,
                                                                 UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                                             });
        }

        public string Filter { get; set; }

        public string DefaultExt { get; set; }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FileSelector));

        public string FileName
        {
            get
            {
                return (string) GetValue(FileNameProperty);
            }
            set
            {
                SetValue(FileNameProperty, value);
            }
        }

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