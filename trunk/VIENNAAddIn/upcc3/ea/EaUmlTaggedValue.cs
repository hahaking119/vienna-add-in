using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlTaggedValue : IUmlTaggedValue
    {
        private static readonly EaUmlTaggedValue UndefinedTaggedValue = new EaUmlTaggedValue(null);

        private EaUmlTaggedValue(string value)
        {
            IsDefined = value != null;
            Value = value ?? string.Empty;
        }

        #region IUmlTaggedValue Members

        public bool IsDefined { get; private set; }

        public string Value { get; private set; }

        public string[] SplitValues
        {
            get { return IsDefined ? Value.Split(MultiPartTaggedValue.ValueSeparator) : new string[0]; }
        }

        #endregion

        public static EaUmlTaggedValue ForEaTaggedValue(TaggedValue eaTaggedValue)
        {
            return eaTaggedValue == null ? UndefinedTaggedValue : new EaUmlTaggedValue(eaTaggedValue.Value);
        }

        public static EaUmlTaggedValue ForEaAttributeTag(AttributeTag eaAttributeTag)
        {
            return eaAttributeTag == null ? UndefinedTaggedValue : new EaUmlTaggedValue(eaAttributeTag.Value);
        }

        public static EaUmlTaggedValue ForEaConnectorTag(ConnectorTag eaConnectorTag)
        {
            return eaConnectorTag == null ? UndefinedTaggedValue : new EaUmlTaggedValue(eaConnectorTag.Value);
        }
    }
}