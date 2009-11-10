namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    internal class EmptyUmlTaggedValue : IUmlTaggedValue
    {
        #region IUmlTaggedValue Members

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