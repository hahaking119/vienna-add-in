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
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    public class CcCache
    {
        private static CcCache ClassInstance;
        private readonly ICctsRepository mCctsRepository;
        private List<CacheItemBdtLibrary> mBdtLibraries;
        private List<CacheItemBieLibrary> mBieLibraries;
        private List<CacheItemCcLibrary> mCcLibraries;
        private List<CacheItemCdtLibrary> mCdtLibraries;

        /// <summary>
        /// Saves Representations of EA Elements in lists and implements "Lazy Loading" from repository
        /// </summary>
        /// <param name="cctsRepository">The Repository where Elements are stored in.</param>
        private CcCache(ICctsRepository cctsRepository)
        {
            mCdtLibraries = null;
            mCcLibraries = null;
            mBdtLibraries = null;
            mBieLibraries = null;
            mCctsRepository = cctsRepository;
        }

        /// <summary>
        /// Create a new Instance of CcCache automatically
        /// </summary>
        /// <param name="cctsRepository">The repository where Elements are stored in.</param>
        /// <returns>An instance of CcCache.</returns>
        public static CcCache GetInstance(ICctsRepository cctsRepository)
        {
            if (ClassInstance == null || ClassInstance.mCctsRepository != cctsRepository)
            {
                ClassInstance = new CcCache(cctsRepository);
            }
            return ClassInstance;
        }

        public static CcCache GetInstance()
        {
            if (ClassInstance == null)
            {
                throw new Exception("Can't return instantiate Cache since Ccts Repository is not net. The method GetInstance(ICctsRepository cctsRepository) needs to be called at least one. ");
            }

            return ClassInstance;
        }

        /// <summary>
        /// Tries to load CC Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CC Libraries in the repository.</returns>
        public List<ICcLibrary> GetCCLibraries()
        {
            LoadCcLibraries();
            return mCcLibraries.ConvertAll(new Converter<CacheItemCcLibrary, ICcLibrary>(CacheItemCCLibraryToCCLibrary));
        }

        private void LoadCcLibraries()
        {
            if (mCcLibraries == null)
            {
                mCcLibraries = new List<CacheItemCcLibrary>();
                foreach (ICcLibrary ccLibrary in mCctsRepository.GetCcLibraries())
                {
                    mCcLibraries.Add(new CacheItemCcLibrary(ccLibrary));
                }
            }
        }

        /// <summary>
        /// Tries to load CDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CDT Libraries in the repository.</returns>
        public List<ICdtLibrary> GetCDTLibraries()
        {
            LoadCdtLibraries();
            return
                mCdtLibraries.ConvertAll(new Converter<CacheItemCdtLibrary, ICdtLibrary>(CacheItemCDTLibraryToCDTLibrary));
        }

        private void LoadCdtLibraries()
        {
            if (mCdtLibraries == null)
            {
                mCdtLibraries = new List<CacheItemCdtLibrary>();

                foreach (ICdtLibrary cdtLibrary in mCctsRepository.GetCdtLibraries())
                {
                    mCdtLibraries.Add(new CacheItemCdtLibrary(cdtLibrary));
                }
            }
        }

        //cdtLibCache.RetrieveElements();
        // 1. frage: hab ich's?
        // 2. versuch laden
        // 3. falls nicht gefunden -> exception

        /// <summary>
        /// Tries to load BDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BDT Libraries in the repository.</returns>
        public List<IBdtLibrary> GetBDTLibraries()
        {
            LoadBdtLibraries();
            return
                mBdtLibraries.ConvertAll(new Converter<CacheItemBdtLibrary, IBdtLibrary>(CacheItemBDTLibraryToBDTLibrary));
        }

        private void LoadBdtLibraries()
        {
            if (mBdtLibraries == null)
            {
                mBdtLibraries = new List<CacheItemBdtLibrary>();
                foreach (IBdtLibrary bdtLibrary in mCctsRepository.GetBdtLibraries())
                {
                    mBdtLibraries.Add(new CacheItemBdtLibrary(bdtLibrary));
                }
            }
        }

        /// <summary>
        /// Tries to load BIE Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BIE Libraries in the repository.</returns>
        public List<IBieLibrary> GetBIELibraries()
        {
            LoadBieLibraries();
            return
                mBieLibraries.ConvertAll(new Converter<CacheItemBieLibrary, IBieLibrary>(CacheItemBIELibraryToBIELibrary));
        }

        private void LoadBieLibraries()
        {
            if (mBieLibraries == null)
            {
                mBieLibraries = new List<CacheItemBieLibrary>();
                foreach (IBieLibrary bieLibrary in mCctsRepository.GetBieLibraries())
                {
                    mBieLibraries.Add(new CacheItemBieLibrary(bieLibrary));
                }
            }
        }

        /// <summary>
        /// Tries to get CDTs from Library <paramref name="cdtLibraryName"/>. If there are no CDT Libraries in Cache, repository Loading is triggered.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library CDTs should be retrieved from.</param>
        /// <returns>A list of CDTs.</returns>
        public List<ICdt> GetCDTsFromCDTLibrary(string cdtLibraryName)
        {
            LoadCdtLibraries();
            foreach (CacheItemCdtLibrary cdtLibrary in mCdtLibraries)
            {
                if (cdtLibrary.CDTLibrary.Name.Equals(cdtLibraryName))
                {
                    if (cdtLibrary.CDTsInLibrary == null)
                    {
                        cdtLibrary.CDTsInLibrary = new List<ICdt>(cdtLibrary.CDTLibrary.Cdts);
                    }
                    return cdtLibrary.CDTsInLibrary;
                }
            }
            throw new Exception("No corresponding CDT Library found with name '" + cdtLibraryName + "'.");
        }

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="cdtName"/> from CDT Library with Name <paramref name="cdtLibraryName"/>.
        /// This method uses GetCDTsFromCDTLibrary <seealso cref="GetCDTsFromCDTLibrary"/> for retrieving available CDTs.
        /// If CDT or CDT Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library where the CDT should be retrieved from.</param>
        /// <param name="cdtName">The name of the CDT which should be retrieved.</param>
        /// <returns>The corresponding UPCC3 CDT Element.</returns>
        public ICdt GetCDTFromCDTLibrary(string cdtLibraryName, string cdtName)
        {
            List<ICdt> tmpCDTs = GetCDTsFromCDTLibrary(cdtLibraryName);
            foreach (ICdt cdt in tmpCDTs)
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
        public List<IAcc> GetCCsFromCCLibrary(string ccLibraryName)
        {
            LoadCcLibraries();
            foreach (CacheItemCcLibrary ccLibrary in mCcLibraries)
            {
                if (ccLibrary.CCLibrary.Name.Equals(ccLibraryName))
                {
                    if (ccLibrary.CCsInLibrary == null)
                    {
                        ccLibrary.CCsInLibrary = new List<IAcc>(ccLibrary.CCLibrary.Accs);
                    }
                    return ccLibrary.CCsInLibrary;
                }
            }
            throw new Exception("No corresponding CC Library found with name '" + ccLibraryName + "'.");
        }

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="ccName"/> from CC Library with name <paramref name="ccLibraryName"/>.
        /// This method uses GetCCsFromCCLibrary <seealso cref="GetCCsFromCCLibrary"/> for retrieving available ACCs.
        /// If ACC or CC Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="ccLibraryName"></param>
        /// <param name="ccName"></param>
        /// <returns></returns>
        public IAcc GetCCFromCCLibrary(string ccLibraryName, string ccName)
        {
            List<IAcc> tmpCCs = GetCCsFromCCLibrary(ccLibraryName);
            foreach (IAcc acc in tmpCCs)
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
        public IBdtLibrary GetBDTLibraryByName(string bdtLibraryName)
        {
            LoadBdtLibraries();
            foreach (CacheItemBdtLibrary bdtLibrary in mBdtLibraries)
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
        public IBieLibrary GetBIELibraryByName(string bieLibraryName)
        {
            LoadBieLibraries();
            foreach (CacheItemBieLibrary bieLibrary in mBieLibraries)
            {
                if (bieLibrary.bieLibrary.Name.Equals(bieLibraryName))
                {
                    return bieLibrary.bieLibrary;
                }
            }
            throw new Exception("No corresponding BDT Library found with name '" + bieLibraryName + "'.");
        }

        #region Converters used for ConvertAll

        /// <summary>
        /// Static function to convert a cacheItemCDTLibrary to a CDTLibrary.
        /// </summary>
        /// <param name="cacheItemCDTLibrary">A Cache representation of a CDT Library</param>
        /// <returns>A UPCC3 CDT Library Element.</returns>
        private static ICdtLibrary CacheItemCDTLibraryToCDTLibrary(CacheItemCdtLibrary cacheItemCDTLibrary)
        {
            return cacheItemCDTLibrary.CDTLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemCCLibrary to a CCLibrary.
        /// </summary>
        /// <param name="cacheItemCCLibrary">A Cache representation of a CC Library</param>
        /// <returns>A UPCC3 CC Library Element.</returns>
        private static ICcLibrary CacheItemCCLibraryToCCLibrary(CacheItemCcLibrary cacheItemCCLibrary)
        {
            return cacheItemCCLibrary.CCLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBDTLibrary to a BDTLibrary.
        /// </summary>
        /// <param name="cacheItemBDTLibrary">A Cache representation of a BDT Library</param>
        /// <returns>A UPCC3 BDT Library Element.</returns>
        private static IBdtLibrary CacheItemBDTLibraryToBDTLibrary(CacheItemBdtLibrary cacheItemBDTLibrary)
        {
            return cacheItemBDTLibrary.BDTLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBIELibrary to a BIELibrary.
        /// </summary>
        /// <param name="cacheItemBIELibrary">A Cache representation of a BIE Library</param>
        /// <returns>A UPCC3 BIE Library Element.</returns>
        private static IBieLibrary CacheItemBIELibraryToBIELibrary(CacheItemBieLibrary cacheItemBIELibrary)
        {
            return cacheItemBIELibrary.bieLibrary;
        }

        #endregion
    }
}