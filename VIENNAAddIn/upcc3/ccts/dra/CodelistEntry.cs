using System;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class CodelistEntry : ICodelistEntry
    {
        private readonly Attribute attribute;

        public CodelistEntry(Attribute attribute)
        {
            this.attribute = attribute;
        }

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public IEnum Enum
        {
            get { throw new NotImplementedException(); }
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