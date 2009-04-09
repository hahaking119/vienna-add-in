﻿// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards
{
    public class cItem
    {
        public cItem()
        {
            Name = "";
            Id = 0;
        }

        public cItem(string initName, int initId)
        {
            Name = initName;
            Id = initId;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class cCheckItem : cItem
    {
        public cCheckItem()
        {
            State = CheckState.Unchecked;
        }

        public cCheckItem(string initName, int initId, CheckState initState)
            : base(initName, initId)
        {
            State = initState;
        }

        public CheckState State { get; set; }
    }

    public class cABIE : cItem
    {
        public cABIE()
        {
        }

        public cABIE(string initName, int initId, int initBasedOn)
            : base(initName, initId)
        {
            BasedOn = initBasedOn;
        }

        public int BasedOn { get; set; }
    }

    public class cCON : cCheckItem
    {
        public cCON()
        {
        }

        public cCON(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
        }
    }

    public class cSUP : cCheckItem
    {
        public cSUP()
        {
        }

        public cSUP(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
        }
    }

    public class cCDT : cItem
    {
        public cCDT()
        {
            CON = new cCON();
            SUPs = new Dictionary<string, cSUP>();
        }

        public cCDT(string initName, int initId)
            : base(initName, initId)
        {
            CON = new cCON();
            SUPs = new Dictionary<string, cSUP>();
        }

        public cCDT(string initName, int initId, cCON initCON, IDictionary<string, cSUP> initSUPs)
            : base(initName, initId)
        {
            CON = initCON;
            SUPs = initSUPs;
        }

        public cCON CON { get; set; }
        public IDictionary<string, cSUP> SUPs { get; set; }

        public void LoadCONAndSUPs(CCRepository repository)
        {
            if ((CON.Name.Equals("") || SUPs.Count < 1))
            {
                int cdtId = Id;
                ICDT cdt = repository.GetCDT(cdtId);

                //CON = new cCON(cdt.CON.Name, cdt.CON.Id, CheckState.Checked);
                CON.Name = cdt.CON.Name;
                CON.Id = cdt.CON.Id;
                CON.State = CheckState.Checked;

                foreach (ISUP sup in cdt.SUPs)
                {
                    SUPs.Add(sup.Name, new cSUP(sup.Name, sup.Id, CheckState.Unchecked));
                }
            }
        }
    }

    public class cCDTLibrary : cItem
    {
        public cCDTLibrary()
        {
            CDTs = new Dictionary<string, cCDT>();
        }

        public cCDTLibrary(string initName, int initId, IDictionary<string, cCDT> initCDTs)
            : base(initName, initId)
        {
            CDTs = initCDTs;
        }

        public cCDTLibrary(string initName, int initId)
            : base(initName, initId)
        {
            CDTs = new Dictionary<string, cCDT>();
        }

        public IDictionary<string, cCDT> CDTs { get; set; }

        public void LoadCDTs(CCRepository repository)
        {
            if (CDTs.Count == 0)
            {
                ICDTLibrary cdtl = (ICDTLibrary)repository.GetLibrary(Id);

                foreach (ICDT cdt in cdtl.CDTs)
                {
                    if (CDTs.ContainsKey(cdt.Name))
                    {
                        CDTs.Clear();
                        throw new CacheException(CacheConstants.CDT_EXISTS);
                    }

                    CDTs.Add(cdt.Name, new cCDT(cdt.Name, cdt.Id));
                }

                if (CDTs.Count == 0)
                {
                    throw new CacheException(CacheConstants.NO_CDTs);
                }
            }
        }
    }

    public class cBBIE : cCheckItem
    {
        public cBBIE()
        {
            BDTs = new List<cBDT>();
        }

        public cBBIE(string initName, int initId, int initIdOfUnderlyingType, CheckState initState, IList<cBDT> initBDTs)
            : base(initName, initId, initState)
        {
            BDTs = initBDTs;
            IdOfUnderlyingType = initIdOfUnderlyingType;
        }

        public cBBIE(string initName, int initId, int initIdOfUnderlyingType, CheckState initState)
            : base(initName, initId, initState)
        {
            BDTs = new List<cBDT>();
            IdOfUnderlyingType = initIdOfUnderlyingType;
        }

        public IList<cBDT> BDTs { get; set; }
        public int IdOfUnderlyingType { get; set; }

        
        public void SearchAndAssignRelevantBDTs(int cdtid, IDictionary<string, cBDTLibrary> bdtls)
        {
            BDTs = GetRelevantBDTs(cdtid, bdtls);
        }

        public IList<cBDT> GetRelevantBDTs(int cdtid, IDictionary<string, cBDTLibrary> bdtls)
        {
            IList<cBDT> relevantBdts = new List<cBDT>();

            foreach (cBDTLibrary bdtl in bdtls.Values)
            {
                foreach (cBDT bdt in bdtl.BDTs.Values)
                {
                    if (bdt.BasedOn == cdtid)
                    {
                        relevantBdts.Add(new cBDT(bdt.Name, bdt.Id, bdt.BasedOn, CheckState.Unchecked));
                    }
                }
            }

            relevantBdts.Add(new cBDT("Create new BDT", -1, cdtid, CheckState.Unchecked));

            return relevantBdts;
        }
    }

    public class cBDT : cCheckItem
    {
        public cBDT()
        {
            State = CheckState.Unchecked;
        }

        public cBDT(string initName, int initId, int initBasedOn, CheckState initState)
            : base(initName, initId, initState)
        {
            BasedOn = initBasedOn;
        }

        public int BasedOn { get; set; }
    }

    public class cBCC : cCheckItem
    {
        public cBCC()
        {
            BBIEs = new Dictionary<string, cBBIE>();
        }

        public cBCC(string initName, int initId, int initType, CheckState initState)
            : base(initName, initId, initState)
        {
            BBIEs = new Dictionary<string, cBBIE>();
            Type = initType;
        }

        public cBCC(string initName, int initId, int initType, CheckState initState, IDictionary<string, cBBIE> initBBIEs)
            : base(initName, initId, initState)
        {
            BBIEs = initBBIEs;
            Type = initType;
        }

        public IDictionary<string, cBBIE> BBIEs { get; set; }

        public int Type { get; set; }
    }

    public class cASCC : cCheckItem
    {
        public cASCC()
        {
            ABIEs = new Dictionary<string, cABIE>();
        }

        public cASCC(string initName, int initId, CheckState initState, IDictionary<string, cABIE> initABIEs)
            : base(initName, initId, initState)
        {
            ABIEs = initABIEs;
        }

        public IDictionary<string, cABIE> ABIEs { get; set; }
    }

    public class cACC : cCheckItem
    {
        public cACC()
        {
            BCCs = new Dictionary<string, cBCC>();
            ASCCs = new Dictionary<string, cASCC>();
        }

        public cACC(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
            BCCs = new Dictionary<string, cBCC>();
            ASCCs = new Dictionary<string, cASCC>();            
        }


        public cACC(string initName, int initId, IDictionary<string, cBCC> initBCCs, IDictionary<string, cASCC> initASCCs, CheckState initState)
            : base(initName, initId, initState)
        {
            BCCs = initBCCs;
            ASCCs = initASCCs;
        }

        public IDictionary<string, cBCC> BCCs { get; set; }
        public IDictionary<string, cASCC> ASCCs { get; set; }

        public void LoadBCCsAndCreateDefaults(CCRepository repository, IDictionary<string, cBDTLibrary> bdtls)
        {
            //if (BCCs.Count == 0)
            if (!HasBCCs())
            {
                IACC acc = repository.GetACC(Id);

                foreach (IBCC bcc in acc.BCCs)
                {
                    if (BCCs.ContainsKey(bcc.Name))
                    {
                        BCCs.Clear();
                        throw new CacheException(CacheConstants.BCC_EXISTS);
                    }

                    BCCs.Add(bcc.Name, new cBCC(bcc.Name, bcc.Id, bcc.Type.Id, CheckState.Unchecked));

                    BCCs[bcc.Name].BBIEs.Add(bcc.Name, new cBBIE(bcc.Name, -1, bcc.Type.Id, CheckState.Unchecked));

                    BCCs[bcc.Name].BBIEs[bcc.Name].SearchAndAssignRelevantBDTs(bcc.Type.Id, bdtls);

                    //BCCs[bcc.Name].BBIEs[bcc.Name].BDTs = GetRelevantBDTs(bcc.Type.Id, bdtls);
                }                
            }
        }

        public void LoadASCCs(CCRepository repository, IDictionary<string, cBIELibrary> biels)
        {
            if (!HasASCCs())
            {
                IACC acc = repository.GetACC(Id);

                foreach (IASCC ascc in acc.ASCCs)
                {
                    IDictionary<string, cABIE> relevantABIEs = new Dictionary<string, cABIE>();

                    foreach (cBIELibrary biel in biels.Values)
                    {
                        foreach (cABIE abie in biel.ABIEs.Values)
                        {
                            if (abie.BasedOn == ascc.AssociatedElement.Id)
                            {
                                relevantABIEs.Add(abie.Name, new cABIE(abie.Name, abie.Id, abie.BasedOn));
                            }
                        }
                    }

                    if (relevantABIEs.Count > 0)
                    {
                        if (!(ascc.Name.Equals("")))
                        {                            
                            if (!ASCCs.ContainsKey(ascc.Name))
                            {
                                ASCCs.Add(ascc.Name, new cASCC(ascc.Name, ascc.Id, CheckState.Unchecked, relevantABIEs));
                            }
                            else
                            {
                                throw new CacheException(CacheConstants.ASCC_EXISTS);
                            }                                                    
                        }
                        else
                        {
                            MessageBox.Show(CacheConstants.ASCC_ERRONEOUS, "ABIE Wizard", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        public bool HasBCCs()
        {
            if (BCCs.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool HasASCCs()
        {
            if (ASCCs.Count > 0)
            {
                return true;
            }

            return false;
        }  
    }

    public class cCCLibrary : cItem
    {
        public cCCLibrary()
        {
            ACCs = new Dictionary<string, cACC>();
        }

        public cCCLibrary(string initName, int initId)
            : base(initName, initId)
        {
            ACCs = new Dictionary<string, cACC>();
        }


        public cCCLibrary(string initName, int initId, IDictionary<string, cACC> initACCs)
            : base(initName, initId)
        {
            ACCs = initACCs;
        }

        public IDictionary<string, cACC> ACCs { get; set; }

        public void LoadACCs(CCRepository repository)
        {
            if (ACCs.Count == 0)
            {
                ICCLibrary ccl = (ICCLibrary)repository.GetLibrary(Id);

                foreach (IACC acc in ccl.ACCs)
                {
                    if (ACCs.ContainsKey(acc.Name))
                    {
                        ACCs.Clear();
                        throw new CacheException(CacheConstants.ACC_EXISTS);
                    }

                    ACCs.Add(acc.Name, new cACC(acc.Name, acc.Id, CheckState.Unchecked));
                }
            }

            if (ACCs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_ACCs);
            }
        }
    }

    public class cBDTLibrary : cItem
    {
        public cBDTLibrary()
        {
            BDTs = new Dictionary<string, cBDT>();
        }

        public cBDTLibrary(string initName, int initId)
            : base(initName, initId)
        {
            BDTs = new Dictionary<string, cBDT>();
        }

        public cBDTLibrary(string initName, int initId, IDictionary<string, cBDT> initBDTs)
            : base(initName, initId)
        {
            BDTs = initBDTs;
        }

        public IDictionary<string, cBDT> BDTs { get; set; }
    }

    public class cBIELibrary : cItem
    {
        public cBIELibrary()
        {
            ABIEs = new Dictionary<string, cABIE>();
        }

        public cBIELibrary(string initName, int initId)
            : base(initName, initId)
        {
            ABIEs = new Dictionary<string, cABIE>();
        }

        public cBIELibrary(string initName, int initId, IDictionary<string, cABIE> initABIEs)
            : base(initName, initId)
        {
            ABIEs = initABIEs;
        }

        public IDictionary<string, cABIE> ABIEs { get; set; }
    }
  
    public class cDOC : cCheckItem
    {
        public cDOC(string initName, int initId, CheckState initState, string initTargetNS, string initTargetNSPrefix) : base(initName, initId, initState)
        {            
            //RootElements = new List<string>();
            TargetNamespace = initTargetNS;
            TargetNamespacePrefix = initTargetNSPrefix;
        }

        //public IList<string> RootElements { get; set; }
        public string TargetNamespace { get; set; }
        public string TargetNamespacePrefix { get; set; }
        public string OutputDirectory { get; set; }
    }

    public class cBIV : cItem
    {
        public cBIV(string initName, int initId) : base(initName, initId)
        {
            DOCs = new Dictionary<string, cDOC>();            
        }

        public string DocumentModel { get; set; }

        public IDictionary<string, cDOC> DOCs { get; set;}

        public void LoadDOCsInBIV(CCRepository repository)
        {
            // doc library containing all the different Documents
            IDOCLibrary docl = (IDOCLibrary)repository.GetLibrary(Id);

            // check if previously cached
            if (DOCs.Count == 0)
            {
                foreach (IABIE document in docl.RootElements)                
                {
                    cDOC newDOC = new cDOC(document.Name, document.Id, CheckState.Unchecked, docl.BaseURN, docl.NamespacePrefix);
                    DOCs.Add(document.Name, newDOC);
                }                
            }
        }
    }

    public class Cache
    {
        public IDictionary<string, cCCLibrary> CCLs { get; set; }
        public IDictionary<string, cCDTLibrary> CDTLs { get; set; }
        public IDictionary<string, cBDTLibrary> BDTLs { get; set; }
        public IDictionary<string, cBIELibrary> BIELs { get; set; }
        public IDictionary<string, cBIV> BIVs { get; set; }

        public Cache()
        {
            CCLs = new Dictionary<string, cCCLibrary>();
            CDTLs = new Dictionary<string, cCDTLibrary>();
            BDTLs = new Dictionary<string, cBDTLibrary>();
            BIELs = new Dictionary<string, cBIELibrary>();
            BIVs = new Dictionary<string, cBIV>();
        }

        public void EmptyCache()
        {
            CCLs.Clear();
            CDTLs.Clear();
            BDTLs.Clear();
            BIELs.Clear();
        }

        public bool PathIsValid(int typeOfPath, string[] path)
        {
            switch(typeOfPath)
            {
                case CacheConstants.PATH_BCCs:
                    {
                        if ((path.Length > 0) && !(CCLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CCLs[path[0]].ACCs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CCLs[path[0]].ACCs[path[1]].BCCs.ContainsKey(path[2])))
                        {
                            return false;
                        }

                        if ((path.Length > 3) && !(CCLs[path[0]].ACCs[path[1]].BCCs[path[2]].BBIEs.ContainsKey(path[3])))
                        {
                            return false;
                        }

                        if (path.Length > 4)
                        {
                            int countMatchingBDTs = 0;

                            foreach (cBDT bdt in CCLs[path[0]].ACCs[path[1]].BCCs[path[2]].BBIEs[path[3]].BDTs)
                            {
                                if (bdt.Name.Equals(path[4]))
                                {
                                    countMatchingBDTs++;
                                }
                            }

                            if (countMatchingBDTs < 1)
                            {
                                return false;
                            }
                        }                        
                    }
                    break;

                case CacheConstants.PATH_ASCCs:
                    {
                        if ((path.Length > 0) && !(CCLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CCLs[path[0]].ACCs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CCLs[path[0]].ACCs[path[1]].ASCCs.ContainsKey(path[2])))
                        {
                            return false;
                        }
                        
                    }
                    break;

                case CacheConstants.PATH_BDTLs:
                    {
                        if ((path.Length > 0) && !(BDTLs.ContainsKey(path[0])))
                        {
                            return false;
                        }
                    }
                    break;

                case CacheConstants.PATH_BIELs:
                    {
                        if ((path.Length > 0) && !(BIELs.ContainsKey(path[0])))
                        {
                            return false;
                        }                        
                    }
                    break;

                case CacheConstants.PATH_CDTs:
                    {
                        if ((path.Length > 0) && !(CDTLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CDTLs[path[0]].CDTs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CDTLs[path[0]].CDTs[path[1]].SUPs.ContainsKey(path[2])))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public void LoadCDTLs(CCRepository repository)
        {
            foreach (ICDTLibrary cdtl in repository.Libraries<CDTLibrary>())
            {                
                if (CDTLs.ContainsKey(cdtl.Name))
                {
                    CDTLs.Clear();
                    throw new CacheException(CacheConstants.CDTL_EXISTS);
                }
                CDTLs.Add(cdtl.Name, new cCDTLibrary(cdtl.Name, cdtl.Id));
            }

            if (CDTLs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_CDTLs);
            }
        }

        public void LoadCCLs(CCRepository repository)
        {
            foreach (ICCLibrary ccl in repository.Libraries<CCLibrary>())
            {
                if (CCLs.ContainsKey(ccl.Name))
                {
                    CCLs.Clear();
                    throw new CacheException(CacheConstants.CCL_EXISTS);
                }

                CCLs.Add(ccl.Name, new cCCLibrary(ccl.Name, ccl.Id));
            }

            if (CCLs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_CCLs);
            }
        }

        public void LoadBIELs(CCRepository repository)
        {
            foreach (IBIELibrary biel in repository.Libraries<IBIELibrary>())
            {
                if (BIELs.ContainsKey(biel.Name))
                {
                    BIELs.Clear();
                    throw new CacheException(CacheConstants.BIEL_EXISTS);
                }

                BIELs.Add(biel.Name, new cBIELibrary(biel.Name, biel.Id));

                foreach (IABIE abie in biel.BIEs)
                {
                    if (BIELs[biel.Name].ABIEs.ContainsKey(abie.Name))
                    {
                        BIELs[biel.Name].ABIEs.Clear();
                        throw new CacheException(CacheConstants.ABIE_EXISTS);
                    }

                    BIELs[biel.Name].ABIEs.Add(abie.Name, new cABIE(abie.Name, abie.Id, abie.BasedOn.Id));
                }
            }

            if (BIELs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_BIELs);
            }
        }

        public void LoadBDTLs(CCRepository repository)
        {
            foreach (IBDTLibrary bdtl in repository.Libraries<IBDTLibrary>())
            {
                if (BDTLs.ContainsKey(bdtl.Name))
                {
                    BDTLs.Clear();
                    throw new CacheException(CacheConstants.BDTL_EXISTS);
                }

                BDTLs.Add(bdtl.Name, new cBDTLibrary(bdtl.Name, bdtl.Id));

                foreach (IBDT bdt in bdtl.BDTs)
                {
                    if (BDTLs[bdtl.Name].BDTs.ContainsKey(bdt.Name))
                    {
                        BDTLs[bdtl.Name].BDTs.Clear();
                        throw new CacheException(CacheConstants.BDT_EXISTS);
                    }

                    BDTLs[bdtl.Name].BDTs.Add(bdt.Name, new cBDT(bdt.Name, bdt.Id, bdt.BasedOn.CDT.Id, CheckState.Unchecked));
                }
            }

            if (BDTLs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_BDTLs);
            }
        }        

        public void LoadBIVs(CCRepository repository)
        {
            foreach (IDOCLibrary docl in repository.Libraries<IDOCLibrary>())
            {
                if (BIVs.ContainsKey(docl.Name))
                {
                    BIVs.Clear();
                    throw new CacheException(CacheConstants.BIV_EXISTS);
                }

                BIVs.Add(docl.Name, new cBIV(docl.Name, docl.Id));
            }

            if (BIVs.Count == 0)
            {
                throw new CacheException(CacheConstants.NO_BIVs);
            }
        }
    }

    public class CacheConstants
    {
        public const string CCL_EXISTS = "The wizard encountered two CC libraries having identical names. Please verify your model!";
        public const string CDTL_EXISTS = "The wizard encountered two CDT libraries having identical names. Please verify your model!";
        public const string BIEL_EXISTS = "The wizard encountered two BIE libraries having identical names. Please verify your model!";
        public const string ABIE_EXISTS = "The wizard encountered two ABIEs within one BIE library having identical names. Please verify your model!";
        public const string BDTL_EXISTS = "The wizard encountered two BDT libraries having identical names. Please verify your model!";
        public const string BDT_EXISTS = "The wizard encountered two BDTs within one BDT library having identical names. Please verify your model!";
        public const string BIV_EXISTS = "The wizard encountered two BIVs having identical names. Please verify your model!";
        public const string NO_CCLs = "The repository did not contain any CC libraries. Please make sure at least one CC library is present before proceeding with the wizard.";
        public const string NO_CDTLs = "The repository did not contain any CDT libraries. Please make sure at least one CDT library is present before proceeding with the wizard.";
        public const string NO_BIELs = "The repository did not contain any BIE libraries. Please make sure at least one BIE library is present before proceeding with the wizard.";
        public const string NO_BDTLs = "The repository did not contain any BDT libraries. Please make sure at least one BDT library is present before proceeding with the wizard.";
        public const string ACC_EXISTS = "The wizard encountered two ACCs having identical names. Please verify your model!";
        public const string NO_ACCs = "The CC library did not contain any ACCs. Please make sure at least one ACC is present in the library before proceeding with the wizard.";
        public const string BCC_EXISTS = "The wizard encountered two BCCs within one ACC having identical names. Please verify your model!";
        public const string CDT_EXISTS = "The wizard encountered two CDTs within one CDT library having identical names. Please verify your model!";
        public const string NO_CDTs = "The CC library did not contain any CDTs. Please make sure at least one ACC is present in the library before proceeding with the wizard.";
        public const string ASCC_EXISTS = "The wizard encountered two ASCCs having identical target role names. Please verify your model!";
        public const string ASCC_ERRONEOUS = "The wizard encountered an association whose target role name is not set properly. Please verify your model!";
        public const string NO_BIVs = "The repository did not contain any BIVs. Please make sure at least one BIV is present before proceeding with the wizard.";

        public const int PATH_BCCs = 1;
        public const int PATH_ASCCs = 2;
        public const int PATH_BDTLs = 3;
        public const int PATH_BIELs = 4;
        public const int PATH_CDTs = 5;
    }

    public class CacheException : Exception
    {
        public CacheException(string message)
            : base(message)
        {
        }
    }
}