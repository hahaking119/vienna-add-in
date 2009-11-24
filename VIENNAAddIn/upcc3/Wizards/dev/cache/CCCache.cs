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
using System.Diagnostics;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    public class CCCache
    {
        private static CCCache ClassInstance;
        private readonly CCRepository ccRepository;
        private List<CacheItemBDTLibrary> bdtLibraries;
        private List<CacheItemBIELibrary> bieLibraries;
        private List<CacheItemCCLibrary> ccLibraries;
        private List<CacheItemCDTLibrary> cdtLibraries;

        /// <summary>
        /// Saves Representations of EA Elements in lists and implements "Lazy Loading" from repository
        /// </summary>
        /// <param name="repository">The Repository where Elements are stored in.</param>
        private CCCache(CCRepository repository)
        {
            cdtLibraries = null;
            ccLibraries = null;
            bdtLibraries = null;
            bieLibraries = null;
            ccRepository = repository;
        }

        /// <summary>
        /// Create a new Instance of CCCache automatically
        /// </summary>
        /// <param name="ccRepository">The repository where Elements are stored in.</param>
        /// <returns>An instance of CCCache.</returns>
        public static CCCache GetInstance(CCRepository ccRepository)
        {
            if (ClassInstance == null)
            {
                ClassInstance = new CCCache(ccRepository);
            }
            return ClassInstance;
        }

        /// <summary>
        /// Tries to load CC Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CC Libraries in the repository.</returns>
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

        /// <summary>
        /// Tries to load CDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CDT Libraries in the repository.</returns>
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
            return
                cdtLibraries.ConvertAll(new Converter<CacheItemCDTLibrary, CDTLibrary>(CacheItemCDTLibraryToCDTLibrary));
        }

        //cdtLibCache.RetrieveElements();
        // 1. frage: hab ich's?
        // 2. versuch laden
        // 3. falls nicht gefunden -> exception

        /// <summary>
        /// Tries to load BDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BDT Libraries in the repository.</returns>
        public List<BDTLibrary> GetBDTLibraries()
        {
            if (bdtLibraries == null)
            {
                bdtLibraries = new List<CacheItemBDTLibrary>();
                foreach (BDTLibrary bdtLibrary in ccRepository.Libraries<BDTLibrary>())
                {
                    bdtLibraries.Add(new CacheItemBDTLibrary(bdtLibrary));
                }
            }
            return
                bdtLibraries.ConvertAll(new Converter<CacheItemBDTLibrary, BDTLibrary>(CacheItemBDTLibraryToBDTLibrary));
        }

        /// <summary>
        /// Tries to load BIE Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BIE Libraries in the repository.</returns>
        public List<BIELibrary> GetBIELibraries()
        {
            if (bieLibraries == null)
            {
                Debug.WriteLine("BIE Cache Load triggered.");
                bieLibraries = new List<CacheItemBIELibrary>();
                foreach (BIELibrary bieLibrary in ccRepository.Libraries<BIELibrary>())
                {
                    bieLibraries.Add(new CacheItemBIELibrary(bieLibrary));
                }
            }
            return
                bieLibraries.ConvertAll(new Converter<CacheItemBIELibrary, BIELibrary>(CacheItemBIELibraryToBIELibrary));
        }

        /// <summary>
        /// Tries to get CDTs from Library <paramref name="cdtLibraryName"/>. If there are no CDT Libraries in Cache, repository Loading is triggered.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library CDTs should be retrieved from.</param>
        /// <returns>A list of CDTs.</returns>
        public List<CDT> GetCDTsFromCDTLibrary(string cdtLibraryName)
        {
            var tmpCDTs = new List<CDT>();
            if (cdtLibraries == null)
            {
                GetCDTLibraries();
            }
// ReSharper disable PossibleNullReferenceException
            foreach (CacheItemCDTLibrary cdtLibrary in cdtLibraries)
// ReSharper restore PossibleNullReferenceException
            {
                if (cdtLibrary.CDTLibrary.Name.Equals(cdtLibraryName))
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
            if (tmpCDTs.Count.Equals(0))
            {
                throw new Exception("No corresponding CDT Library found with name '" + cdtLibraryName + "'.");
            }

            return tmpCDTs;
        }

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="cdtName"/> from CDT Library with Name <paramref name="cdtLibraryName"/>.
        /// This method uses GetCDTsFromCDTLibrary <seealso cref="GetCDTsFromCDTLibrary"/> for retrieving available CDTs.
        /// If CDT or CDT Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library where the CDT should be retrieved from.</param>
        /// <param name="cdtName">The name of the CDT which should be retrieved.</param>
        /// <returns>The corresponding UPCC3 CDT Element.</returns>
        public CDT GetCDTFromCDTLibrary(string cdtLibraryName, string cdtName)
        {
            List<CDT> tmpCDTs = GetCDTsFromCDTLibrary(cdtLibraryName);
            foreach (CDT cdt in tmpCDTs)
            {
                if (cdt.Name.Equals(cdtName))
                {
                    return cdt;
                }
            }
            throw new Exception("No corresponding CDT with Name '" + cdtName + "' found in CDT Library '" +
                                cdtLibraryName + "'.");
        }

        /// <summary>
        /// Tries to get ACCs from Library <paramref name="ccLibraryName"/>. If there are no CC Libraries in Cache, repository loading is triggered.
        /// </summary>
        /// <param name="ccLibraryName">The name of the CC Library ACCs should be retrieved from.</param>
        /// <returns>A list of ACCs.</returns>
        public List<ACC> GetCCsFromCCLibrary(string ccLibraryName)
        {
            if (ccLibraries == null)
            {
                GetCCLibraries();
            }
            var tmpCCs = new List<ACC>();
// ReSharper disable PossibleNullReferenceException
            foreach (CacheItemCCLibrary ccLibrary in ccLibraries)
// ReSharper restore PossibleNullReferenceException
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

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="ccName"/> from CC Library with name <paramref name="ccLibraryName"/>.
        /// This method uses GetCCsFromCCLibrary <seealso cref="GetCCsFromCCLibrary"/> for retrieving available ACCs.
        /// If ACC or CC Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="ccLibraryName"></param>
        /// <param name="ccName"></param>
        /// <returns></returns>
        public ACC GetCCFromCCLibrary(string ccLibraryName, string ccName)
        {
            List<ACC> tmpCCs = GetCCsFromCCLibrary(ccLibraryName);
            foreach (ACC acc in tmpCCs)
            {
                if (acc.Name.Equals(ccName))
                {
                    return acc;
                }
            }
            throw new Exception("No corresponding CDT with Name '" + ccName + "' found in CDT Library '" + ccLibraryName +
                                "'.");
        }

        /// <summary>
        /// Try to retrieve a specific BDT Library by Name. If not found an exception will be thrown.
        /// </summary>
        /// <param name="bdtLibraryName">The Name of the BDT Library to retrieve.</param>
        /// <returns>A UPPC3 BDT Library Element.</returns>
        public BDTLibrary GetBDTLibraryByName(string bdtLibraryName)
        {
            if (bdtLibraries == null)
            {
                GetBDTLibraries();
            }
// ReSharper disable PossibleNullReferenceException
            foreach (CacheItemBDTLibrary bdtLibrary in bdtLibraries)
// ReSharper restore PossibleNullReferenceException
            {
                if (bdtLibrary.BDTLibrary.Name.Equals(bdtLibraryName))
                {
                    return bdtLibrary.BDTLibrary;
                }
            }
            throw new Exception("No corresponding BDT Library found with name '" + bdtLibraryName + "'.");
        }

        /// <summary>
        /// Try to retrieve a specific BIE Library by Name. If not found an exception will be thrown.
        /// </summary>
        /// <param name="bieLibraryName">The Name of the BIE Library to retrieve.</param>
        /// <returns>A UPCC3 BIE Library Element.</returns>
        public BIELibrary GetBIELibraryByName(string bieLibraryName)
        {
            if (bieLibraries == null)
            {
                GetBIELibraries();
            }
// ReSharper disable PossibleNullReferenceException
            foreach (CacheItemBIELibrary bieLibrary in bieLibraries)
// ReSharper restore PossibleNullReferenceException
            {
                if (bieLibrary.bieLibrary.Name.Equals(bieLibraryName))
                {
                    return bieLibrary.bieLibrary;
                }
            }
            throw new Exception("No corresponding BDT Library found with name '" + bieLibraryName + "'.");
        }

        /// <summary>
        /// Prepares the context for the given ABIE. Loads needed CC, BIE, BDT and CDT Libraries.
        /// </summary>
        /// <param name="abie">The ABIE for which the context should be prepared.</param>
        public void PrepareForABIE(IABIE abie)
        {
            bieLibraries = new List<CacheItemBIELibrary>();
            ccLibraries = new List<CacheItemCCLibrary>();
            bdtLibraries = new List<CacheItemBDTLibrary>();
            cdtLibraries = new List<CacheItemCDTLibrary>();
            bieLibraries.Add(new CacheItemBIELibrary((BIELibrary) abie.Library));
            ccLibraries.Add(new CacheItemCCLibrary((CCLibrary) abie.BasedOn.Library));
            CacheItemCDTLibrary cdtLibrary = null;
            CacheItemBDTLibrary bdtLibrary = null;

            //because a ABIE can have multiple BBIEs with maybe different underlying BDT and CDT Libraries we look at each BBIE and check if the containing
            //Library is already in the cache. If not we add it now.
            foreach (IBBIE bbie in abie.BBIEs)
            {
                if (bdtLibrary == null || bdtLibrary.BDTLibrary.Name != bbie.Type.Name)
                {
                    bdtLibrary = new CacheItemBDTLibrary((BDTLibrary) bbie.Type.Library);
                    bdtLibraries.Add(bdtLibrary);
                }
                if (cdtLibrary == null || cdtLibrary.CDTLibrary.Name != bbie.Type.BasedOn.Library.Name)
                {
                    cdtLibrary = new CacheItemCDTLibrary((CDTLibrary) bbie.Type.BasedOn.Library);
                    bdtLibraries.Add(bdtLibrary);
                }
            }
            cdtLibraries.Add(cdtLibrary);
        }

        #region Converters used for ConvertAll

        /// <summary>
        /// Static function to convert a cacheItemCDTLibrary to a CDTLibrary.
        /// </summary>
        /// <param name="cacheItemCDTLibrary">A Cache representation of a CDT Library</param>
        /// <returns>A UPCC3 CDT Library Element.</returns>
        private static CDTLibrary CacheItemCDTLibraryToCDTLibrary(CacheItemCDTLibrary cacheItemCDTLibrary)
        {
            return cacheItemCDTLibrary.CDTLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemCCLibrary to a CCLibrary.
        /// </summary>
        /// <param name="cacheItemCCLibrary">A Cache representation of a CC Library</param>
        /// <returns>A UPCC3 CC Library Element.</returns>
        private static CCLibrary CacheItemCCLibraryToCCLibrary(CacheItemCCLibrary cacheItemCCLibrary)
        {
            return cacheItemCCLibrary.CCLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBDTLibrary to a BDTLibrary.
        /// </summary>
        /// <param name="cacheItemBDTLibrary">A Cache representation of a BDT Library</param>
        /// <returns>A UPCC3 BDT Library Element.</returns>
        private static BDTLibrary CacheItemBDTLibraryToBDTLibrary(CacheItemBDTLibrary cacheItemBDTLibrary)
        {
            return cacheItemBDTLibrary.BDTLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBIELibrary to a BIELibrary.
        /// </summary>
        /// <param name="cacheItemBIELibrary">A Cache representation of a BIE Library</param>
        /// <returns>A UPCC3 BIE Library Element.</returns>
        private static BIELibrary CacheItemBIELibraryToBIELibrary(CacheItemBIELibrary cacheItemBIELibrary)
        {
            return cacheItemBIELibrary.bieLibrary;
        }

        #endregion
    }
}