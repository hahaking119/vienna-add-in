using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckedListModel : INotifyPropertyChanged
    {
        private ObservableCollection<CheckedListItem> content;

        public ObservableCollection<CheckedListItem> Content
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

    public class CheckedListItem
    {
        public string Name { get; set; }
        public bool Check { get; set; }

        public CheckedListItem()
        {
            Name = "";
            Check = false;
        }

        public CheckedListItem(string _name, bool _check)
        {
            Name = _name;
            Check = _check;
        }
    }
}
