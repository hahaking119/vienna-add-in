using System.Collections.Generic;
using System.Windows.Forms;


namespace VIENNAAddIn.upcc3.Wizards
{
    public class Cache
    {
        public Cache()
        {            
            CBDTLs = new Dictionary<string, CBDTL>();
            CBIELs = new Dictionary<string, CBIEL>();
            CCLs = new Dictionary<string, CCCL>();
        }

        public IDictionary<string, CCCL> CCLs { get; set; }
        public IDictionary<string, CBDTL> CBDTLs { get; set; }
        public IDictionary<string, CBIEL> CBIELs { get; set; }        
    }

    public class BasicCacheItem
    {
        public BasicCacheItem()
        {
            Name = "";
            Id = 0;
        }

        public BasicCacheItem(string initName, int initId)
        {
            Name = initName;
            Id = initId;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class CBDT : BasicCacheItem
    {
        public CBDT()
        {
            InLibrary = 0;
        }

        public CBDT(string initName, int initId, int initBasedOn, int initInLibrary) : base(initName, initId)
        {
            State = CheckState.Unchecked;
            InLibrary = initInLibrary;
            BasedOn = initBasedOn;
        }

        public CBDT(string initName, int initId, CheckState initState, int initBasedOn, int initInLibrary) : base(initName, initId)
        {
            State = initState;
            InLibrary = initInLibrary;
            BasedOn = initBasedOn;
        }

        public CheckState State { get; set; }
        public int BasedOn { get; set; }
        public int InLibrary { get; set; }
    }

    public class CBDTL : BasicCacheItem
    {
        public CBDTL()
        {
            BDTs = new Dictionary<string, CBDT>();
        }

        public CBDTL (string initName, int initId, IDictionary<string, CBDT> initBDTDictionary) : base (initName, initId)
        {
            BDTs = initBDTDictionary;
        }

        public IDictionary<string, CBDT> BDTs { get; set; }
    }

    public class CABIE : BasicCacheItem
    {
        public CABIE(string initName, int initId) : base(initName, initId) { }
    }

    public class CBIEL : BasicCacheItem
    {
        public CBIEL ()
        {
            ABIEs = new Dictionary<string, CABIE>();
        }

        public CBIEL (string initName, int initId, IDictionary<string, CABIE> initABIEs) : base (initName, initId)
        {
            ABIEs = initABIEs;
        }

        public IDictionary<string, CABIE> ABIEs { get; set; }
    }


    public class CBBIE : BasicCacheItem
    {
        public CBBIE()
        {
            State = CheckState.Unchecked;
            BDTs = new List<CBDT>();
        }

        public CBBIE(string initName, int initId, CheckState initState, IList<CBDT> initBDTs) : base(initName, initId)
        {
            State = initState;
            BDTs = initBDTs;
        }

        public CheckState State { get; set; }
        public IList<CBDT> BDTs { get; set; }
        
    }

    public class CBCC : BasicCacheItem
    {
        public CBCC()
        {
            Type = 0;
            State = CheckState.Unchecked;
            BBIEs = new Dictionary<string, CBBIE>();
        }

        public CBCC(string initName, int initId, int initType, CheckState initState, IDictionary<string, CBBIE> initBBIEs) : base(initName, initId)
        {
            Type = initType;
            State = initState;
            BBIEs = initBBIEs;
        }

        public int Type { get; set; } 
        public CheckState State { get; set; }
        public IDictionary<string, CBBIE> BBIEs { get; set; }
    }

    public class CACC : BasicCacheItem
    {
        public CACC()
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = new Dictionary<string, CBCC>();            
        }

        public CACC(string initName, int initId) : base(initName, initId)
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = new Dictionary<string, CBCC>();            
        }

        public CACC(string initName, int initId, IDictionary<string, CBCC> initBCCs) : base(initName, initId)
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = initBCCs;            
        }

        public CheckState AllAttributesChecked { get; set; }

        public IDictionary<string, CBCC> BCCs { get; set; }
    }

    public class CCCL : BasicCacheItem
    {
        public CCCL()
        {
            ACCs = new Dictionary<string, CACC>();
            //BDTs = new List<CBDT>();
        }

        public CCCL(string initName, int initId) : base(initName, initId)
        {
            ACCs = new Dictionary<string, CACC>();
            //BDTs = new List<CBDT>();
        }

        public CCCL(string initName, int initId, IDictionary<string, CACC> initACCs) : base(initName, initId)
        {
            ACCs = initACCs;
          //  BDTs = new List<CBDT>();
        }

        public IDictionary<string, CACC> ACCs { get; set; }
        //public IList<CBDT> BDTs { get; set; }
    }
}
