using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class BLibrary : BusinessLibrary, IBLibrary
    {
        #region IBLibrary Members

        public BLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        public IEnumerable<IBusinessLibrary> Children
        {
            get
            {
                foreach (IEAPackage subPackage in subPackages)
                {
                    if (subPackage is IBusinessLibrary)
                    {
                        yield return (IBusinessLibrary) subPackage;
                    }
                }
            }
        }

        public IEnumerable<IBusinessLibrary> AllChildren
        {
            get
            {
                foreach (IBusinessLibrary child in Children)
                {
                    yield return child;
                    if (child is IBLibrary)
                    {
                        foreach (IBusinessLibrary grandChild in ((IBLibrary) child).AllChildren)
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public IBusinessLibrary FindChildByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICDTLibrary CreateCDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICCLibrary CreateCCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBIELibrary CreateBIELibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IENUMLibrary CreateENUMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IDOCLibrary CreateDOCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}