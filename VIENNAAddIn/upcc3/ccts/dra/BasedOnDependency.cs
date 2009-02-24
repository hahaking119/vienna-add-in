using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BasedOnDependency: IBasedOnDependency
    {
        private readonly CCRepository repository;
        private readonly Connector con;

        public BasedOnDependency(CCRepository repository, Connector con)
        {
            this.repository = repository;
            this.con = con;
        }

        public string Pattern
        {
            get { return con.GetTaggedValue(TaggedValues.Pattern); }
        }

        public string FractionDigits
        {
            get { return con.GetTaggedValue(TaggedValues.FractionDigits); }
        }

        public string Length
        {
            get { return con.GetTaggedValue(TaggedValues.Length); }
        }

        public string MaxExclusive
        {
            get { return con.GetTaggedValue(TaggedValues.MaxExclusive); }
        }

        public string MaxInclusive
        {
            get { return con.GetTaggedValue(TaggedValues.MaxInclusive); }
        }

        public string MaxLength
        {
            get { return con.GetTaggedValue(TaggedValues.MaxLength); }
        }

        public string MinExclusive
        {
            get { return con.GetTaggedValue(TaggedValues.MinExclusive); }
        }

        public string MinInclusive
        {
            get { return con.GetTaggedValue(TaggedValues.MinInclusive); }
        }

        public string MinLength
        {
            get { return con.GetTaggedValue(TaggedValues.MinLength); }
        }

        public string TotalDigits
        {
            get { return con.GetTaggedValue(TaggedValues.TotalDigits); }
        }

        public string WhiteSpace
        {
            get { return con.GetTaggedValue(TaggedValues.WhiteSpace); }
        }

        public string ApplyTo
        {
            get { return con.GetTaggedValue(TaggedValues.ApplyTo); }
        }
    }
}