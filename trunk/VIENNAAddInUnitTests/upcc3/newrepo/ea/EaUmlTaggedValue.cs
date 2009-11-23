using System;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlTaggedValue : IUmlTaggedValue
    {
        private static readonly EaUmlTaggedValue EmptyTaggedValue = new EaUmlTaggedValue(null);

        private readonly TaggedValue eaTaggedValue;

        private EaUmlTaggedValue(TaggedValue eaTaggedValue)
        {
            this.eaTaggedValue = eaTaggedValue;
        }

        #region IUmlTaggedValue Members

        public string Value
        {
            get { return eaTaggedValue == null ? String.Empty : eaTaggedValue.Value; }
        }

        public string[] SplitValues
        {
            get
            {
                string value = Value;
                return String.IsNullOrEmpty(value) ? new string[0] : value.Split(MultiPartTaggedValue.ValueSeparator);
            }
        }

        #endregion

        public static EaUmlTaggedValue ForEaTaggedValue(TaggedValue eaTaggedValue)
        {
            return eaTaggedValue == null ? EmptyTaggedValue : new EaUmlTaggedValue(eaTaggedValue);
        }
    }
}