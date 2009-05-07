using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    ///<summary>
    ///</summary>
    public class ASCCSpec : CCSpec
    {
        ///<summary>
        ///</summary>
        ///<param name="ascc"></param>
        public ASCCSpec(IASCC ascc) : base(ascc)
        {
            SequencingKey = ascc.SequencingKey;
            AssociatedACCId = ascc.AssociatedElement.Id;
            LowerBound = ascc.LowerBound;
            UpperBound = ascc.UpperBound;
        }

        ///<summary>
        ///</summary>
        public ASCCSpec()
        {
        }

        ///<summary>
        ///</summary>
        [TaggedValue(TaggedValues.SequencingKey)]
        public string SequencingKey { get; set; }

        ///<summary>
        ///</summary>
        public int AssociatedACCId { get; set; }

        ///<summary>
        ///</summary>
        public string LowerBound { get; set; }

        ///<summary>
        ///</summary>
        public string UpperBound { get; set; }
    }
}