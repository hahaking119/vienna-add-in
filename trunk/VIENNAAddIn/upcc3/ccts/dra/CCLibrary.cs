using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CCLibrary : BusinessLibrary, ICCLibrary
    {
        public CCLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType) : base(repository, package, libraryType)
        {
        }

        public IEnumerable<IACC> ACCs
        {
            get
            {
                foreach (Element element in package.Elements)
                {
                    yield return new ACC(repository, element);
                }
            }
        }
    }
}