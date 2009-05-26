using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal class SetElementStereotypeQuickFix : QuickFix
    {
        private readonly string stereotype;

        public SetElementStereotypeQuickFix(string stereotype)
        {
            this.stereotype = stereotype;
        }

        public void Execute(Repository repository, object item)
        {
            var element = (Element) item;
            element.Stereotype = stereotype;
            element.Update();
            element.Refresh();
            repository.RefreshModelView(element.PackageID);
        }
    }
}