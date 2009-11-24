using CctsRepository.@enum;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class CodelistEntry : ICodelistEntry
    {
        private readonly Attribute attribute;

        public CodelistEntry(Attribute attribute)
        {
            this.attribute = attribute;
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public string CodeName
        {
            get { return GetTaggedValue(TaggedValues.codeName); }
        }

        public string Status
        {
            get { return GetTaggedValue(TaggedValues.status); }
        }

        private string GetTaggedValue(TaggedValues key)
        {
            return attribute.GetTaggedValue(key) ?? string.Empty;
        }
    }
}