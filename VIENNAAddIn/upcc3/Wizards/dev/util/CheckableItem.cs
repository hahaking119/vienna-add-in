namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckableItem
    {
        public bool Checked { get; set; }
        public string Text { get; set; }

        public CheckableItem(bool isChecked, string text)
        {
            Checked = isChecked;
            Text = text;
        }
    }
}