namespace CctsRepository
{
    public class BCCSpec : CCSpec
    {
        public BCCSpec(IBCC bcc) : base(bcc)
        {
            SequencingKey = bcc.SequencingKey;
            UpperBound = bcc.UpperBound;
            LowerBound = bcc.LowerBound;
            Type = bcc.Type;
        }

        public BCCSpec()
        {
        }

        [TaggedValue]
        public string SequencingKey { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public ICDT Type { get; set; }
    }
}