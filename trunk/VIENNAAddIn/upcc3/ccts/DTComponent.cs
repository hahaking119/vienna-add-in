using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{

    /// <summary>
    /// A data type component (content component or supplementary component).
    /// </summary>
    public class DTComponent : IDTComponent
    {
        private IList<string> businessTerms;
        private IList<string> usageRules;

        public DTComponent(DTComponentType componentType, int id, string name, string type)
        {
            ComponentType = componentType;
            Id = id;
            Name = name;
            Type = type;
        }

        public DTComponentType ComponentType { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Definition { get; set; }
        public string DictionaryEntryName { get; set; }
        public string LanguageCode { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }

        /// <summary>
        /// TODO this should be a boolean
        /// </summary>
        public string ModificationAllowedIndicator { get; set; }

        public IList<string> BusinessTerms
        {
            get { return businessTerms; }
            set { businessTerms = new List<string>(value).AsReadOnly(); }
        }

        public IList<string> UsageRules
        {
            get { return usageRules; }
            set { usageRules = new List<string>(value).AsReadOnly(); }
        }

        public string UpperBound { get; set; }

        public string LowerBound { get; set; }

        public IDT DT
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
    }
}