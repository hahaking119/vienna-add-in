using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal abstract class EaUmlClassifier : IUmlClassifier
    {
        protected readonly Element eaElement;
        protected readonly Repository eaRepository;

        public EaUmlClassifier(Repository eaRepository, Element eaElement)
        {
            this.eaRepository = eaRepository;
            this.eaElement = eaElement;
        }

        #region IUmlClassifier Members

        public int Id
        {
            get { return eaElement.ElementID; }
        }

        public string GUID
        {
            get { return eaElement.ElementGUID; }
        }

        public string Name
        {
            get { return eaElement.Name; }
        }

        public IUmlPackage Package
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaElement.PackageID)); }
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name));
        }

        #endregion

        private TaggedValue GetEATaggedValueByName(string name)
        {
            foreach (TaggedValue eaTaggedValue in eaElement.TaggedValues)
            {
                if (eaTaggedValue.Name.Equals(name))
                {
                    return eaTaggedValue;
                }
            }
            return null;
        }
    }
}