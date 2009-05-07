using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal class SetStereotypeQuickFix : QuickFix
    {
        private readonly string stereotype;

        public SetStereotypeQuickFix(string stereotype)
        {
            this.stereotype = stereotype;
        }

        public void Execute(Repository repository, string guid, ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.otElement:
                    {
                        Element element = repository.GetElementByGuid(guid);
                        element.Stereotype = stereotype;
                        element.Update();
                        element.Refresh();
                        repository.RefreshModelView(element.PackageID);
                        break;
                    }
            }
        }
    }
}