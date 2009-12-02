namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckableText
    {
        public bool Checked { get; set; }
        public string Text { get; set; }

        public CheckableText(bool isChecked, string text)
        {
            Checked = isChecked;
            Text = text;
        }
    }
}