using System.Collections.Generic;
using VIENNAAddInUtils;

namespace UPCCRepositoryInterface
{
    ///<summary>
    ///</summary>
    public class ACCSpec : CCSpec
    {
        private readonly List<ASCCSpec> asccs;
        private readonly List<BCCSpec> bccs;
        private readonly List<string> duplicateBCCNames = new List<string>();
        private readonly List<string> duplicateASCCNames = new List<string>();

        ///<summary>
        ///</summary>
        ///<param name="acc"></param>
        public ACCSpec(IACC acc) : base(acc)
        {
            bccs = new List<BCCSpec>(acc.BCCs.Convert(bcc => new BCCSpec(bcc)));
            asccs = new List<ASCCSpec>(acc.ASCCs.Convert(ascc => new ASCCSpec(ascc)));
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
        }

        public IEnumerable<ASCCSpec> ASCCs
        {
            get { return asccs; }
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
            duplicateASCCNames.Remove(name);
            asccs.RemoveAll(ascc => ascc.Name == name);
        }

        public void RemoveBCC(string name)
        {
            duplicateBCCNames.Remove(name);
            bccs.RemoveAll(bcc => bcc.Name == name);
        }

        public override IEnumerable<ConnectorSpec> GetCustomConnectors(ICCRepository repository)
        {
            if (ASCCs != null)
            {
                foreach (ASCCSpec ascc in ASCCs)
                {
                    var associatedACC = ascc.AssociatedACC;
                    if (associatedACC == null)
                    {
                        // TODO throw meaningful exception instead
                        continue;
                    }
                    var name = ascc.Name;
                    if (duplicateASCCNames.Contains(name))
                    {
                        name = name + associatedACC.Name;
                    }
                    yield return
                        ConnectorSpec.CreateAggregation(EAAggregationKind.Shared, Stereotype.ASCC, name,
                                                        ascc.AssociatedACC.Id, ascc.LowerBound, ascc.UpperBound, ascc.GetTaggedValues());
                }
            }
        }

        public override IEnumerable<AttributeSpec> GetAttributes()
        {
            if (BCCs != null)
            {
                foreach (BCCSpec bcc in BCCs)
                {
                    var name = bcc.Name;
                    if (duplicateBCCNames.Contains(name))
                    {
                        name = name + bcc.Type.Name;
                    }
                    yield return new AttributeSpec(Stereotype.BCC, name, bcc.Type.Name, bcc.Type.Id, bcc.LowerBound, bcc.UpperBound, bcc.GetTaggedValues());
                }
            }
        }

        public void AddBCC(BCCSpec bcc)
        {
            CheckDuplicateBCCNames(bcc);
            bccs.Add(bcc);
        }

        private void CheckDuplicateBCCNames(BCCSpec newBCC)
        {
            foreach (BCCSpec bcc in bccs)
            {
                if (newBCC.Name == bcc.Name)
                {
                    duplicateBCCNames.Add(bcc.Name);
                }
            }
        }

        public void AddASCC(ASCCSpec ascc)
        {
            CheckDuplicateASCCNames(ascc);
            asccs.Add(ascc);
        }

        private void CheckDuplicateASCCNames(ASCCSpec newASCC)
        {
            foreach (ASCCSpec ascc in asccs)
            {
                if (newASCC.Name == ascc.Name)
                {
                    duplicateASCCNames.Add(ascc.Name);
                }
            }
        }
    }
}