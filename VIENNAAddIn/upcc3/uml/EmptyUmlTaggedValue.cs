using System;

namespace VIENNAAddIn.upcc3.uml
{
    internal class EmptyUmlTaggedValue : IUmlTaggedValue
    {
        #region IUmlTaggedValue Members

        public bool IsDefined
        {
            get { throw new NotImplementedException(); }
        }

        public string Value
        {
            get { return string.Empty; }
        }

        public string[] SplitValues
        {
            get { return new string[0]; }
        }

        #endregion
    }
}