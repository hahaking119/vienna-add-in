using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlPackage : IUmlPackage
    {
        private readonly Package eaPackage;
        private readonly Repository eaRepository;

        public EaUmlPackage(Repository eaRepository, Package eaPackage)
        {
            this.eaRepository = eaRepository;
            this.eaPackage = eaPackage;
        }

        #region IUmlPackage Members

        public string Stereotype
        {
            get { return eaPackage.Element != null ? eaPackage.Element.Stereotype : string.Empty; }
        }

        public IUmlDataType CreateDataType(UmlDataTypeSpec spec)
        {
            throw new NotImplementedException();
        }

        public IUmlDataType GetDataTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public IUmlTaggedValue GetTaggedValue(TaggedValues name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name.ToString()));
        }

        public int Id
        {
            get { return eaPackage.PackageID; }
        }

        public string Name
        {
            get { return eaPackage.Name; }
        }

        public IEnumerable<IUmlClass> Classes
        {
            get
            {
                foreach (Element eaElement in eaPackage.Elements)
                {
                    yield return new EaUmlClass(eaRepository, eaElement);
                }
            }
        }

        public IEnumerable<IUmlDataType> DataTypes
        {
            get
            {
                foreach (Element eaElement in eaPackage.Elements)
                {
                    yield return new EaUmlDataType(eaRepository, eaElement);
                }
            }
        }

        public IUmlPackage Parent
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaPackage.ParentID)); }
        }

        #endregion

        private TaggedValue GetEATaggedValueByName(string name)
        {
            foreach (TaggedValue eaTaggedValue in eaPackage.Element.TaggedValues)
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