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

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CCCache
    {
        //cdtLibCache.RetrieveElements();
        // 1. frage: hab ich's?
        // 2. versuch laden
        // 3. falls nicht gefunden -> exception


        private CCCache()
        {
            
        }

        public static CCCache GetInstance(CCRepository ccRepository)
        {
            // Singleton Implementierung

            throw new NotImplementedException();
        }

        public ICDTLibrary CDTLibraryByName(string libraryName)
        {
            throw new NotImplementedException();
        }

        public CDTLibraryCache CDTLibraryCacheByName(string libraryName)
        {
            throw new NotImplementedException();
        }
    }

    public class CDTLibraryCache
    {
        public ICDT ElementByName(string cdtName)
        {
            throw new NotImplementedException();
        }

        public List<ICDT> AllElements()
        {
            throw new NotImplementedException();
        }
    }
}