// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    public class CCCache
    {
        private static CCCache ClassInstance;

        private CCRepository ccRepository;
        private List<CacheItemCDTLibrary> cdtLibraries;
        private List<CacheItemCCLibrary> ccLibraries;
        private List<CacheItemBDTLibrary> bdtLibraries;
        private List<CacheItemBIELibrary> bieLibraries;

        private CCCache(CCRepository repository)
        {
            cdtLibraries = null;
            ccLibraries = null;
            bdtLibraries = null;
            bieLibraries = null;
            ccRepository = repository;
        }

        public static CCCache GetInstance(CCRepository ccRepository)
        {
            if (ClassInstance == null)
            {
                ClassInstance = new CCCache(ccRepository);
            }

            return ClassInstance;
        }

        public List<CDTLibrary> GetCDTLibraries()
        {
            if (cdtLibraries == null)
            {
                cdtLibraries = new List<CacheItemCDTLibrary>();

                foreach (CDTLibrary cdtLibrary in ccRepository.Libraries<CDTLibrary>())
                {
                    cdtLibraries.Add(new CacheItemCDTLibrary(cdtLibrary));
                }                
            }

            return cdtLibraries.ConvertAll(new Converter<CacheItemCDTLibrary, CDTLibrary>(CacheItemCDTLibraryToCDTLibrary));
        }

        private CDTLibrary CacheItemCDTLibraryToCDTLibrary(CacheItemCDTLibrary cacheItemCDTLibrary)
        {           
            return cacheItemCDTLibrary.CDTLibrary;
        }

        public List<CCLibrary> GetCCLibraries()
        {
            if (ccLibraries == null)
            {
                ccLibraries = new List<CacheItemCCLibrary>();
                foreach (CCLibrary ccLibrary in ccRepository.Libraries<CCLibrary>())
                {
                    ccLibraries.Add(new CacheItemCCLibrary(ccLibrary));
                }
            }
            return ccLibraries.ConvertAll(new Converter<CacheItemCCLibrary, CCLibrary>(CacheItemCCLibraryToCCLibrary));
        }

        //cdtLibCache.RetrieveElements();
        // 1. frage: hab ich's?
        // 2. versuch laden
        // 3. falls nicht gefunden -> exception

        private CCLibrary CacheItemCCLibraryToCCLibrary(CacheItemCCLibrary cacheItemCCLibrary)
        {
            return cacheItemCCLibrary.CCLibrary;
        }

        public List<BDTLibrary> GetBDTLibraries()
        {
            if(bdtLibraries == null)
            {
                bdtLibraries = new List<CacheItemBDTLibrary>();
                foreach (BDTLibrary bdtLibrary in ccRepository.Libraries<BDTLibrary>())
                {
                    bdtLibraries.Add(new CacheItemBDTLibrary(bdtLibrary));
                }
            }
            return bdtLibraries.ConvertAll(new Converter<CacheItemBDTLibrary, BDTLibrary>(CacheItemBDTLibraryToBDTLibrary));
        }

        private BDTLibrary CacheItemBDTLibraryToBDTLibrary(CacheItemBDTLibrary cacheItemBDTLibrary)
        {
            return cacheItemBDTLibrary.BDTLibrary;
        }

        public List<BIELibrary> GetBIELibraries()
        {
            if(bieLibraries == null)
            {
                bieLibraries = new List<CacheItemBIELibrary>();
                foreach (BIELibrary bieLibrary in ccRepository.Libraries<BIELibrary>())
                {
                    bieLibraries.Add(new CacheItemBIELibrary(bieLibrary));
                }
            }
            return bieLibraries.ConvertAll(new Converter<CacheItemBIELibrary, BIELibrary>(CacheItemBIELibraryToBIELibrary));
        }

        private BIELibrary CacheItemBIELibraryToBIELibrary(CacheItemBIELibrary cacheItemBIELibrary)
        {
            return cacheItemBIELibrary.bieLibrary;
        }

        public List<CDT> GetCDTsFromCDTLibrary(string cdtLibraryName)
        {
            if(cdtLibraries == null)
            {
                GetCDTLibraries();
            }
            var tmpCDTs = new List<CDT>();
            foreach (CacheItemCDTLibrary cdtLibrary in cdtLibraries)
            {
                if(cdtLibrary.CDTLibrary.Name.Equals(cdtLibraryName))
                {
                    if (cdtLibrary.CDTsInLibrary == null)
                    {
                        foreach (CDT element in ccRepository.LibraryByName<ICDTLibrary>(cdtLibraryName).Elements)
                        {
                            tmpCDTs.Add(element);
                        }
                    }
                    else
                    {
                        return cdtLibrary.CDTsInLibrary;
                    }
                }
            }
            if(tmpCDTs.Count.Equals(0))
            {
                throw new Exception("No corresponding CDT Library found with name '"+cdtLibraryName+"'.");
            }
            
            return tmpCDTs;
        }

        public CDT GetCDTFromCDTLibrary(string cdtLibraryName, string cdtName)
        {
            var tmpCDTs = GetCDTsFromCDTLibrary(cdtLibraryName);
            foreach (CDT cdt in tmpCDTs)
            {
                if(cdt.Name.Equals(cdtName))
                {
                    return cdt;
                }
            }
            throw new Exception("No corresponding CDT with Name '"+cdtName+"' found in CDT Library '"+cdtLibraryName+"'.");
        }

        public List<ACC> GetCCsFromCCLibrary(string ccLibraryName)
        {
            if(ccLibraries == null)
            {
                GetCCLibraries();
            }
            var tmpCCs = new List<ACC>();
            foreach (CacheItemCCLibrary ccLibrary in ccLibraries)
            {
                if (ccLibrary.CCLibrary.Name.Equals(ccLibraryName))
                {
                    if (ccLibrary.CCsInLibrary == null)
                    {
                        foreach (ACC acc in ccRepository.LibraryByName<ICCLibrary>(ccLibraryName).Elements)
                        {
                            tmpCCs.Add(acc);
                        }
                    }
                    else
                    {
                        return ccLibrary.CCsInLibrary;
                    }
                }
            }
            if (tmpCCs.Count.Equals(0))
            {
                throw new Exception("No corresponding CC Library found with name '" + ccLibraryName + "'.");
            }

            return tmpCCs;
        }

        public ACC GetCCFromCCLibrary(string ccLibraryName, string ccName)
        {
            var tmpCCs = GetCCsFromCCLibrary(ccLibraryName);
            foreach (ACC acc in tmpCCs)
            {
                if (acc.Name.Equals(ccName))
                {
                    return acc;
                }
            }
            throw new Exception("No corresponding CDT with Name '" + ccName + "' found in CDT Library '" + ccLibraryName + "'.");
        }

        public BDTLibrary GetBDTLibraryByName(string bdtLibraryName)
        {
            if(bdtLibraries == null)
            {
                GetBDTLibraries();
            }
            foreach (CacheItemBDTLibrary bdtLibrary in bdtLibraries)
            {
                 if(bdtLibrary.BDTLibrary.Name.Equals(bdtLibraryName))
                 {
                     return bdtLibrary.BDTLibrary;
                 }   
            }
            throw new Exception("No corresponding BDT Library found with name '" + bdtLibraryName + "'.");
        }

        public BIELibrary GetBIELibraryByName(string bieLibraryName)
        {
            if (bieLibraries == null)
            {
                GetBIELibraries();
            }
            foreach (CacheItemBIELibrary bieLibrary in bieLibraries)
            {
                if (bieLibrary.bieLibrary.Name.Equals(bieLibraryName))
                {
                    return bieLibrary.bieLibrary;
                }
            }
            throw new Exception("No corresponding BDT Library found with name '" + bieLibraryName + "'.");
        }

        public void PrepareForABIE(IABIE abie)
        {
            var BIELibrary = abie.Library;
            IACC basedONACC = abie.BasedOn;
            var CClibrary = basedONACC.Library;
            foreach (IBBIE bbie in abie.BBIEs)
            {
                
            }
            throw new NotImplementedException();
        }
    }
}