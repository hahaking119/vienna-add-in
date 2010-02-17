using System;
using EA;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlEnumerationLiteral : IUmlEnumerationLiteral, IEquatable<EaUmlEnumerationLiteral>
    {
        private readonly Attribute eaAttribute;

        public EaUmlEnumerationLiteral(Attribute eaAttribute)
        {
            this.eaAttribute = eaAttribute;
        }

        #region IUmlEnumerationLiteral Members

        public int Id
        {
            get { return eaAttribute.AttributeID; }
        }

        public string Name
        {
            get { return eaAttribute.Name; }
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            try
            {
                var eaAttributeTag = eaAttribute.TaggedValues.GetByName(name) as AttributeTag;
                return eaAttributeTag == null ? (IUmlTaggedValue) new UndefinedTaggedValue(name) : new EaAttributeTag(eaAttributeTag);
            }
            catch (Exception)
            {
                return new UndefinedTaggedValue(name);
            }
        }

        #endregion

        public void Initialize(UmlEnumerationLiteralSpec spec)
        {
            eaAttribute.Stereotype = spec.Stereotype;
            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                CreateTaggedValue(taggedValueSpec);
            }
        }

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaAttributeTag = (AttributeTag) eaAttribute.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaAttributeTag.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaAttributeTag.Update();
        }

        public void Update(UmlEnumerationLiteralSpec spec)
        {
            eaAttribute.Name = spec.Name;
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.Update();
            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                IUmlTaggedValue taggedValue = GetTaggedValue(taggedValueSpec.Name);
                if (taggedValue.IsDefined)
                {
                    taggedValue.Update(taggedValueSpec);
                }
                else
                {
                    CreateTaggedValue(taggedValueSpec);
                }
            }
        }

        public bool Equals(EaUmlEnumerationLiteral other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaAttribute.AttributeID, eaAttribute.AttributeID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlEnumerationLiteral)) return false;
            return Equals((EaUmlEnumerationLiteral) obj);
        }

        public override int GetHashCode()
        {
            return (eaAttribute != null ? eaAttribute.AttributeID : 0);
        }

        public static bool operator ==(EaUmlEnumerationLiteral left, EaUmlEnumerationLiteral right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlEnumerationLiteral left, EaUmlEnumerationLiteral right)
        {
            return !Equals(left, right);
        }
    }
}