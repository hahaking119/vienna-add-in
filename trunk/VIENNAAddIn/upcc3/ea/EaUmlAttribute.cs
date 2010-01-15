using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlAttribute : IUmlAttribute
    {
        private readonly Attribute eaAttribute;
        private readonly Repository eaRepository;

        public EaUmlAttribute(Repository eaRepository, Attribute eaAttribute)
        {
            this.eaRepository = eaRepository;
            this.eaAttribute = eaAttribute;
        }

        #region IUmlAttribute Members

        public int Id
        {
            get { return eaAttribute.AttributeID; }
        }

        public string Name
        {
            get { return eaAttribute.Name; }
        }

        public string UpperBound
        {
            get { return eaAttribute.UpperBound; }
        }

        public string LowerBound
        {
            get { return eaAttribute.LowerBound; }
        }

        public IUmlClassifier Type
        {
            get { return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(eaAttribute.ClassifierID)); }
        }

        public IEnumerable<IUmlTaggedValue> GetTaggedValues()
        {
            foreach (AttributeTag eaAttributeTag in eaAttribute.TaggedValues)
            {
                yield return new EaAttributeTag(eaAttributeTag);
            }
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

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaAttributeTag = (AttributeTag) eaAttribute.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaAttributeTag.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaAttributeTag.Update();
        }

        #endregion

        public void Initialize(UmlAttributeSpec spec)
        {
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.ClassifierID = spec.Type.Id;
            eaAttribute.LowerBound = spec.LowerBound;
            eaAttribute.UpperBound = spec.UpperBound;
            eaAttribute.Update();
            foreach (var taggedValueSpec in spec.TaggedValues)
            {
                CreateTaggedValue(taggedValueSpec);
            }
        }

        public void Update(UmlAttributeSpec spec)
        {
            eaAttribute.Name = spec.Name;
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.ClassifierID = spec.Type.Id;
            eaAttribute.LowerBound = spec.LowerBound;
            eaAttribute.UpperBound = spec.UpperBound;
            eaAttribute.Update();
            foreach (var taggedValueSpec in spec.TaggedValues)
            {
                var taggedValue = GetTaggedValue(taggedValueSpec.Name);
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
    }
}