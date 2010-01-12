using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
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

        public IUmlDataType UpdateDataType(IUmlDataType dataType, UmlDataTypeSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveDataType(IUmlDataType dataType)
        {
            throw new NotImplementedException();
        }

        public IUmlDataType GetDataTypeById(int id)
        {
            throw new NotImplementedException();
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            return EaUmlTaggedValue.ForEaTaggedValue(GetEATaggedValueByName(name));
        }

        public IUmlClass CreateClass(UmlClassSpec spec)
        {
            throw new NotImplementedException();
        }

        public IUmlClass UpdateClass(IUmlClass umlClass, UmlClassSpec umlClassSpec)
        {
            throw new NotImplementedException();
        }

        public void RemoveClass(IUmlClass umlClass)
        {
            throw new NotImplementedException();
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
                    yield return new EaUmlClassifier(eaRepository, eaElement);
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

        public IEnumerable<IUmlEnumeration> Enumerations
        {
            get
            {
                foreach (Element eaElement in eaPackage.Elements)
                {
                    yield return new EaUmlEnumeration(eaRepository, eaElement);
                }
            }
        }

        public IUmlEnumeration CreateEnumeration(UmlEnumerationSpec umlEnumerationSpec)
        {
            throw new NotImplementedException();
        }

        public IUmlEnumeration UpdateEnumeration(IUmlEnumeration umlEnumeration, UmlEnumerationSpec umlEnumerationSpec)
        {
            throw new NotImplementedException();
        }

        public void RemoveEnumeration(IUmlEnumeration umlEnumeration)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype)
        {
            throw new NotImplementedException();
        }

        public IUmlPackage CreatePackage(UmlPackageSpec umlPackageSpec)
        {
            throw new NotImplementedException();
        }

        public IUmlPackage UpdatePackage(IUmlPackage umlPackage, UmlPackageSpec umlPackageSpec)
        {
            throw new NotImplementedException();
        }

        public void RemovePackage(IUmlPackage umlPackage)
        {
            throw new NotImplementedException();
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