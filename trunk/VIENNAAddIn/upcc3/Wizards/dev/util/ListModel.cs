using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class ListModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> content;

        public ObservableCollection<string> Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                this.sendPropertyChanged("Content");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void sendPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
