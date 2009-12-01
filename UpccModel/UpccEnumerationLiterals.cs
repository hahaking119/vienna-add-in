using System.Collections.Generic;
using System.Reflection;

namespace UpccModel
{
    public class UpccEnumerationLiterals
    {
        public readonly MetaEnumerationLiteral CodelistEntry;

        public UpccEnumerationLiterals(UpccTaggedValues taggedValues, UpccEnumerations enumerations)
        {
            CodelistEntry = new MetaEnumerationLiteral
                            {
                                Stereotype = "CodelistEntry",
                                Name = "CodelistEntries",
                                Cardinality = Cardinality.Many,
                                ContainingEnumerationType = enumerations.Enum,
                                TaggedValues = new[]
                                               {
                                                   taggedValues.CodeName,
                                                   taggedValues.Status,
                                               }
                            };
        }

        public IEnumerable<MetaEnumerationLiteral> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaEnumerationLiteral) field.GetValue(this);
                }
            }
        }
    }
}