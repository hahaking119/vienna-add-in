using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    class RenameTextBox : TextBox
    {
        public string OldText { get; set; }
        public int ListIndex { get; set; }
        public bool CheckState { get; set; }
    }
}
