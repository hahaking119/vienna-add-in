using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class ENUMLibrary : ElementLibrary<IENUM, ENUMSpec>, IENUMLibrary
    {
        public ENUMLibrary(int id, string name, int parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }
    }
}