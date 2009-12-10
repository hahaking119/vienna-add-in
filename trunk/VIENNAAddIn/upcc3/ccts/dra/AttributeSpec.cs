using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class AttributeSpec
    {
        public AttributeSpec(string stereotype, string name, string typeName, int classifierId, string lowerBound, string upperBound, IEnumerable<TaggedValueSpec> taggedValueSpecs, string defaultDictionaryEntryName)
        {
            Stereotype = stereotype;
            Name = name;
            TypeName = typeName;
            ClassifierId = classifierId;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            TaggedValueSpecs = taggedValueSpecs;
            DefaultDictionaryEntryName = defaultDictionaryEntryName;
        }

        public string Stereotype { get; private set; }

        public string Name { get; private set; }

        public string TypeName { get; private set; }

        public int ClassifierId { get; private set; }

        public string LowerBound { get; private set; }

        public string UpperBound { get; private set; }

        public IEnumerable<TaggedValueSpec> TaggedValueSpecs { get; private set; }

        public string DefaultDictionaryEntryName { get; private set; }

        public string DefaultValue { get; set; }
    }
}