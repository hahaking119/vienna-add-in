using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CctsRepository;
using VIENNAAddIn.menu;
using ComboBox=System.Windows.Controls.ComboBox;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for Transform.xaml
    /// </summary>
    public partial class Transformer : Window
    {
        private ICctsRepository cctsR;
        private Cache cache;
        private string outputDirectory = "";        

        public Transformer(ICctsRepository cctsRepository)
        {
            cctsR = cctsRepository;
            try
            {
                cache = new Cache();
             } catch(CacheException ce)
            {
                System.Windows.Forms.MessageBox.Show(ce.Message, "VIENNA Add-In Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //MirrorDocLibsToUI();
            //MirrorBieLibsToUI();
            InitializeComponent();
        }

        private void MirrorBieLibsToUI()
        {
            throw new NotImplementedException();
        }

        private void MirrorDocLibsToUI()
        {
            throw new NotImplementedException();
        }

        public static void ShowForm(AddInContext context)
        {
            new Transformer(context.CctsRepository).Show();
        }

        private static void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private static void SetSafeIndex(System.Windows.Controls.ListBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboboxSourceDocLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBoxTargetDocLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void targetXSD_FileNameChanged(object sender, RoutedEventArgs e)
        {

        }

        private void comboBoxSourceBIELibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBoxTargetBIELibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OutputDirectory_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {

        }

        private void buttonTransform_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}