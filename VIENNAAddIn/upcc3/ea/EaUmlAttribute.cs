using EA;
using VIENNAAddIn.upcc3.uml;

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

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            foreach (AttributeTag eaAttributeTag in eaAttribute.TaggedValues)
            {
                if (eaAttributeTag.Name.Equals(name))
                {
                    return EaUmlTaggedValue.ForEaAttributeTag(eaAttributeTag);
                }
            }
            return null;
        }

        #endregion
    }
}