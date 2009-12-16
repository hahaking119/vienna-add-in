using System;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlTaggedValue : IUmlTaggedValue
    {
        private static readonly EaUmlTaggedValue UndefinedTaggedValue = new EaUmlTaggedValue(null);

        private readonly TaggedValue eaTaggedValue;

        private EaUmlTaggedValue(TaggedValue eaTaggedValue)
        {
            this.eaTaggedValue = eaTaggedValue;
        }

        #region IUmlTaggedValue Members

        public bool IsDefined
        {
            get { return eaTaggedValue != null; }
        }

        public string Value
        {
            get { return IsDefined ? eaTaggedValue.Value : String.Empty; }
        }

        public string[] SplitValues
        {
            get { return IsDefined ? Value.Split(MultiPartTaggedValue.ValueSeparator) : new string[0]; }
        }

        #endregion

        public static EaUmlTaggedValue ForEaTaggedValue(TaggedValue eaTaggedValue)
        {
            return eaTaggedValue == null ? UndefinedTaggedValue : new EaUmlTaggedValue(eaTaggedValue);
        }
    }
}