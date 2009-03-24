// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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
            CCDTLs = new Dictionary<string, CCDTL>();
        }

        public IDictionary<string, CCCL> CCLs { get; set; }
        public IDictionary<string, CBDTL> CBDTLs { get; set; }
        public IDictionary<string, CBIEL> CBIELs { get; set; }
        public IDictionary<string, CCDTL> CCDTLs { get; set; }
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

    public class CCDTL : BasicCacheItem
    {
        public CCDTL()
        {
            CDTs = new Dictionary<string, CCDT>();
        }

        public CCDTL(string initName, int initId, IDictionary<string, CCDT> initCDTDictionary)
            : base(initName, initId)
        {
            CDTs = initCDTDictionary;
        }

        public IDictionary<string, CCDT> CDTs { get; set; }
    }

    public class CCON : BasicCacheItem
    {
        public CCON(string initName, int initId, CheckState initState) : base(initName, initId)
        {
            State = initState;
        }

        public CheckState State { get; set; }
    }

    public class CSUP : BasicCacheItem
    {
        public CSUP(string initName, int initId, CheckState initState) : base(initName, initId)
        {
            State = initState;
        }

        public CheckState State { get; set; }
    }

    public class CCDT : BasicCacheItem
    {
        public CCDT(string initName, int initId) : base(initName, initId)
        {
            CON = null;
            SUPs = new Dictionary<string, CSUP>();
        }

        public CCDT(string initName, int initId, CCON initCON, IDictionary<string, CSUP> initSUPs) : base(initName, initId)
        {
            CON = initCON;
            SUPs = initSUPs;
        }

        public CCON CON { get; set; }
        public IDictionary<string, CSUP> SUPs { get; set; }
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
        public CABIE(string initName, int initId, int initBasedOn) : base(initName, initId)
        {
            BasedOn = initBasedOn;
        }

        public int BasedOn { get; set; }
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

    public class CASCC : BasicCacheItem
    { 
        public CASCC()
        {
            State = CheckState.Unchecked;
        }

        public CASCC(string initName, int initId, CheckState initState) : base(initName, initId)
        {
            State = initState;
        }

        public CheckState State { get; set;}
    }

    public class CACC : BasicCacheItem
    {
        public CACC()
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = new Dictionary<string, CBCC>();      
            ASCCs = new Dictionary<string, CASCC>();
        }

        public CACC(string initName, int initId) : base(initName, initId)
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = new Dictionary<string, CBCC>();
            ASCCs = new Dictionary<string, CASCC>();
        }

        public CACC(string initName, int initId, IDictionary<string, CBCC> initBCCs) : base(initName, initId)
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = initBCCs;
            ASCCs = new Dictionary<string, CASCC>();
        }

        public CACC(string initName, int initId, IDictionary<string, CBCC> initBCCs, IDictionary<string, CASCC> initASCCs) : base(initName, initId)
        {
            AllAttributesChecked = CheckState.Unchecked;
            BCCs = initBCCs;
            ASCCs = initASCCs;
        }


        public CheckState AllAttributesChecked { get; set; }

        public IDictionary<string, CBCC> BCCs { get; set; }
        public IDictionary<string, CASCC> ASCCs { get; set; }
    }

    public class CCCL : BasicCacheItem
    {
        public CCCL()
        {
            ACCs = new Dictionary<string, CACC>();
        }

        public CCCL(string initName, int initId) : base(initName, initId)
        {
            ACCs = new Dictionary<string, CACC>();
        }

        public CCCL(string initName, int initId, IDictionary<string, CACC> initACCs) : base(initName, initId)
        {
            ACCs = initACCs;
        }

        public IDictionary<string, CACC> ACCs { get; set; }
    }
}
