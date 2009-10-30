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
        private List<CacheItemBIELibraries> bieLibraries;

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
            //cdtLibCache.RetrieveElements();
            // 1. frage: hab ich's?
            // 2. versuch laden
            // 3. falls nicht gefunden -> exception

            throw new NotImplementedException();
        }

        public List<BDTLibrary> GetBDTLibraries()
        {
            throw new NotImplementedException();
        }

        public List<BIELibrary> GetBIELibraries()
        {
            throw new NotImplementedException();
        }

        public List<CDT> GetCDTsFromCDTLibrary(string cdtLibraryName)
        {
            throw new NotImplementedException();
        }

        public CDT GetCDTFromCDTLibrary(string cdtLibraryName, string cdtName)
        {
            throw new NotImplementedException();
        }

        public List<ACC> GetCCsFromCCLibrary(string ccLibraryName)
        {
            throw new NotImplementedException();
        }

        public ACC GetCCFromCCLibrary(string ccLibraryName, string ccName)
        {
            throw new NotImplementedException();
        }

        public BIELibrary GetBDTLibraryByName(string bdtLibraryName)
        {
            throw new NotImplementedException();
        }

        public BIELibrary GetBIELibraryByName(string bieLibraryName)
        {
            throw new NotImplementedException();
        }

        public void PrepareForABIE(IABIE abie)
        {
            throw new NotImplementedException();
        }
    }
}