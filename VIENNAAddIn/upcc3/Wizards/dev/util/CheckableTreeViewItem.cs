// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.ObjectModel;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckableTreeViewItem
    {
        public bool Checked { get; set; }
        public bool IsExpanded { get; set; }
        public string Text { get; set; }
        public string visibility { get; set; }
        public ObservableCollection<CheckableTreeViewItem> Children { get; set; }

        public CheckableTreeViewItem(bool initChecked, string initText, ObservableCollection<CheckableTreeViewItem> children)
        {
            Checked = initChecked;
            Text = initText;
            Children = children;
            visibility = "Visible";
        }
        public CheckableTreeViewItem(string initText, ObservableCollection<CheckableTreeViewItem> children)
        {
            Text = initText;
            Children = children;
            visibility = "Collapsed";
        }
    }
}