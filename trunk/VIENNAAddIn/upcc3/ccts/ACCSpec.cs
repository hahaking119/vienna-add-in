using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    ///<summary>
    ///</summary>
    public class ACCSpec : CCSpec
    {
        private List<ASCCSpec> asccs;
        private List<BCCSpec> bccs;

        ///<summary>
        ///</summary>
        ///<param name="acc"></param>
        public ACCSpec(IACC acc) : base(acc)
        {
            BCCs = acc.BCCs.Convert(bcc => new BCCSpec(bcc));
            ASCCs = acc.ASCCs.Convert(ascc => new ASCCSpec(ascc));
            IsEquivalentTo = acc.IsEquivalentTo;
        }

        public ACCSpec()
        {
            bccs = new List<BCCSpec>();
            asccs = new List<ASCCSpec>();
        }

        public IEnumerable<BCCSpec> BCCs
        {
            get { return bccs; }
            set { bccs = new List<BCCSpec>(value); }
        }

        public IEnumerable<ASCCSpec> ASCCs
        {
            get { return asccs; }
            set { asccs = new List<ASCCSpec>(value); }
        }

        ///<summary>
        ///</summary>
        [Dependency]
        public IACC IsEquivalentTo { get; set; }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        public void RemoveASCC(string name)
        {
            asccs.RemoveAll(ascc => ascc.Name == name);
        }

        public void RemoveBCC(string name)
        {
            bccs.RemoveAll(bcc => bcc.Name == name);
        }

        public override IEnumerable<ConnectorSpec> GetCustomConnectors()
        {
            if (ASCCs != null)
            {
                foreach (ASCCSpec ascc in ASCCs)
                {
                    yield return
                        ConnectorSpec.CreateAggregation(AggregationKind.Shared, Stereotype.ASCC, ascc.Name,
                                                        ascc.AssociatedACCId, ascc.LowerBound, ascc.UpperBound);
                }
            }
        }

        public override IEnumerable<AttributeSpec> GetAttributes()
        {
            if (BCCs != null)
            {
                foreach (BCCSpec bcc in BCCs)
                {
                    yield return new AttributeSpec(Stereotype.BCC, bcc.Name, bcc.Type.Name, bcc.Type.Id, bcc.LowerBound, bcc.UpperBound, bcc.GetTaggedValues());
                }
            }

        }
    }
}