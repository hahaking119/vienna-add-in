using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace VIENNAAddInWpfUserControls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class ProgressBackgroundWorker : UserControl
    {
        private BackgroundWorker bw;

        public ProgressBackgroundWorker()
        {
            InitializeComponent();
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = false;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        public string Header {
            get
            {
                return (string)label.Content;
            }
            set
            {
                border.Width = 77 + label.ActualWidth;
                label.Content = value;
            }
        }

        public void RunWorkerAsync()
        {
            this.Visibility = Visibility.Visible;
            var sbdRotation = (Storyboard)FindResource("sbdRotation");
            sbdRotation.Begin(this);
            bw.RunWorkerAsync();
        }

        private static readonly RoutedEvent DoWorkEvent = EventManager.RegisterRoutedEvent("DoWork", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProgressBackgroundWorker));

        public event RoutedEventHandler DoWork
        {
            add { AddHandler(DoWorkEvent, value); }
            remove { RemoveHandler(DoWorkEvent, value); }
        }

        private static readonly RoutedEvent RunWorkerCompletedEvent = EventManager.RegisterRoutedEvent("RunWorkerCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSelector));

        public event RoutedEventHandler RunWorkerCompleted
        {
            add { AddHandler(RunWorkerCompletedEvent, value); }
            remove { RemoveHandler(RunWorkerCompletedEvent, value); }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DoWorkEvent));
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var sbdRotation = (Storyboard)FindResource("sbdRotation");
            sbdRotation.Stop();
            this.Visibility = Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs(RunWorkerCompletedEvent));
        }
    }
}