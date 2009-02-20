using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class DT : IDT
    {
        private IList<string> businessTerms;
        private readonly List<IDTComponent> supplementaryComponents = new List<IDTComponent>();
        private IList<string> usageRules;

        protected DT(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        string IDT.Name
        {
            get { return Name; }
//            set { Name = value; }
        }

        public IBusinessLibrary Library
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Name { get; private set; }
        public string Definition { get; set; }
        public string DictionaryEntryName { get; set; }
        public string LanguageCode { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }

        public string ContentType
        {
            get { return CON.Type; }
        }

        public IList<string> BusinessTerms
        {
            get { return businessTerms; }
            set { businessTerms = new List<string>(value).AsReadOnly(); }
        }

        IList<string> IDT.UsageRules
        {
            get { return UsageRules; }
//            set { UsageRules = value; }
        }

        public IList<string> UsageRules
        {
            get { return usageRules; }
            internal set { usageRules = new List<string>(value).AsReadOnly(); }
        }

        public IList<IDTComponent> SUPs
        {
            get { return supplementaryComponents.AsReadOnly(); }
        }

        public IDTComponent CON { get; set; }

        public void AddSupplementaryComponent(DTComponent component)
        {
            supplementaryComponents.Add(component);
        }

    }

}