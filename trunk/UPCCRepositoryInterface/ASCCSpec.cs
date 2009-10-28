using System;

namespace UPCCRepositoryInterface
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
            ResolveAssociatedACC = () => ascc.AssociatedElement;
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
        [TaggedValue]
        public string SequencingKey { get; set; }

        ///<summary>
        ///</summary>
        public string LowerBound { get; set; }

        ///<summary>
        ///</summary>
        public string UpperBound { get; set; }

        public IACC AssociatedACC
        {
            get
            {
                return ResolveAssociatedACC();
            }
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IACC> ResolveAssociatedACC { get; set; }
    }
}