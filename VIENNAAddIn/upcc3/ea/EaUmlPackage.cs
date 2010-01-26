using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlPackage : IUmlPackage, IEquatable<EaUmlPackage>
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

        public IEnumerable<IUmlDataType> GetDataTypesByStereotype(string stereotype)
        {
            return GetClassifiersByStereotype<IUmlDataType>(stereotype);
        }

        public IUmlDataType CreateDataType(UmlDataTypeSpec spec)
        {
            return CreateEaUmlClassifier(spec);
        }

        public IUmlDataType UpdateDataType(IUmlDataType dataType, UmlDataTypeSpec spec)
        {
            UpdateEaUmlClassifier((EaUmlClassifier) dataType, spec);
            return dataType;
        }

        public void RemoveDataType(IUmlDataType dataType)
        {
            RemoveEaUmlClassifier((EaUmlClassifier) dataType);
        }

        public IEnumerable<IUmlEnumeration> GetEnumerationsByStereotype(string stereotype)
        {
            return GetClassifiersByStereotype<IUmlEnumeration>(stereotype);
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            try
            {
                Element eaPackageElement = eaPackage.Element;
                if (eaPackageElement == null)
                {
                    return new UndefinedTaggedValue(name);
                }
                var eaTaggedValue = eaPackageElement.TaggedValues.GetByName(name) as TaggedValue;
                return eaTaggedValue == null ? (IUmlTaggedValue) new UndefinedTaggedValue(name) : new EaTaggedValue(eaTaggedValue);
            }
            catch (Exception)
            {
                return new UndefinedTaggedValue(name);
            }
        }

        public IEnumerable<IUmlClass> GetClassesByStereotype(string stereotype)
        {
            return GetClassifiersByStereotype<IUmlClass>(stereotype);
        }

        public IUmlClass CreateClass(UmlClassSpec spec)
        {
            return CreateEaUmlClassifier(spec);
        }

        public IUmlClass UpdateClass(IUmlClass umlClass, UmlClassSpec spec)
        {
            UpdateEaUmlClassifier((EaUmlClassifier) umlClass, spec);
            return umlClass;
        }

        public void RemoveClass(IUmlClass umlClass)
        {
            RemoveEaUmlClassifier((EaUmlClassifier) umlClass);
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
            get { return GetClassifiers<IUmlClass>(); }
        }

        public IEnumerable<IUmlDataType> DataTypes
        {
            get { return GetClassifiers<IUmlDataType>(); }
        }

        public IEnumerable<IUmlEnumeration> Enumerations
        {
            get { return GetClassifiers<IUmlEnumeration>(); }
        }

        public IUmlEnumeration CreateEnumeration(UmlEnumerationSpec spec)
        {
            return CreateEaUmlClassifier(spec);
        }

        public IUmlEnumeration UpdateEnumeration(IUmlEnumeration umlEnumeration, UmlEnumerationSpec spec)
        {
            UpdateEaUmlClassifier((EaUmlClassifier) umlEnumeration, spec);
            return umlEnumeration;
        }

        public void RemoveEnumeration(IUmlEnumeration umlEnumeration)
        {
            RemoveEaUmlClassifier((EaUmlClassifier) umlEnumeration);
        }

        public IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype)
        {
            foreach (Package eaSubPackage in eaPackage.Packages)
            {
                Element eaPackageElement = eaSubPackage.Element;
                if (eaPackageElement != null)
                {
                    if (eaPackageElement.Stereotype == stereotype)
                    {
                        yield return new EaUmlPackage(eaRepository, eaSubPackage);
                    }
                }
            }
        }

        public IUmlPackage CreatePackage(UmlPackageSpec spec)
        {
            var eaSubPackage = (Package) eaPackage.Packages.AddNew(spec.Name, string.Empty);
            eaSubPackage.Update(); // must update before accessing Package.Element!
            eaSubPackage.ParentID = Id;
            var eaUmlPackage = new EaUmlPackage(eaRepository, eaSubPackage);
            eaUmlPackage.Initialize(spec);
            AddToPackageDiagram(eaSubPackage);
            return eaUmlPackage;
        }

        public IUmlPackage UpdatePackage(IUmlPackage umlPackage, UmlPackageSpec spec)
        {
            ((EaUmlPackage) umlPackage).Update(spec);
            return umlPackage;
        }

        public void RemovePackage(IUmlPackage umlPackage)
        {
            short i = 0;
            Collection eaSubPackages = eaPackage.Packages;
            foreach (Package eaSubPackage in eaSubPackages)
            {
                if (eaSubPackage.PackageID == umlPackage.Id)
                {
                    eaSubPackages.Delete(i);
                }
                i++;
            }
            eaSubPackages.Refresh();
        }

        public IUmlPackage Parent
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaPackage.ParentID)); }
        }

        #endregion

        private EaUmlClassifier CreateEaUmlClassifier(UmlClassifierSpec spec)
        {
            var eaElement = (Element) eaPackage.Elements.AddNew(spec.Name, "Class");
            eaElement.PackageID = Id;
            var eaUmlClassifier = new EaUmlClassifier(eaRepository, eaElement);
            eaUmlClassifier.Initialize(spec);
            AddToClassDiagram(eaElement);

            eaPackage.Update();
            eaPackage.Elements.Refresh();

            return eaUmlClassifier;
        }

        private void AddToClassDiagram(Element eaElement)
        {
            AddToDiagram(eaElement, "Class");
        }

        private void AddToPackageDiagram(Package eaSubPackage)
        {
            AddToDiagram(eaSubPackage.Element, "Package");
        }

        private void AddToDiagram(Element eaElement, string diagramType)
        {
            var diagram = (Diagram) eaPackage.Diagrams.GetByName(Name);
            if (diagram != null && diagram.Type == diagramType)
            {
                var newDiagramObject = (DiagramObject) diagram.DiagramObjects.AddNew(string.Empty, string.Empty);
                newDiagramObject.DiagramID = diagram.DiagramID;
                newDiagramObject.ElementID = eaElement.ElementID;
                newDiagramObject.Update();
            }
        }

        private static void UpdateEaUmlClassifier(EaUmlClassifier classifier, UmlClassifierSpec spec)
        {
            classifier.Update(spec);
        }

        private void RemoveEaUmlClassifier(EaUmlClassifier classifier)
        {
            short i = 0;
            Collection eaElements = eaPackage.Elements;
            foreach (Element eaElement in eaElements)
            {
                if (eaElement.ElementID == classifier.Id)
                {
                    eaElements.Delete(i);
                }
                i++;
            }
            eaElements.Refresh();
        }

        private IEnumerable<T> GetClassifiers<T>() where T : class
        {
            foreach (Element eaElement in eaPackage.Elements)
            {
                yield return new EaUmlClassifier(eaRepository, eaElement) as T;
            }
        }

        private IEnumerable<T> GetClassifiersByStereotype<T>(string stereotype) where T : class
        {
            foreach (Element eaElement in eaPackage.Elements)
            {
                if (eaElement.Stereotype == stereotype)
                {
                    yield return new EaUmlClassifier(eaRepository, eaElement) as T;
                }
            }
        }

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            Element eaPackageElement = eaPackage.Element;
            if (eaPackageElement != null)
            {
                var eaTaggedValue = (TaggedValue) eaPackageElement.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
                eaTaggedValue.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
                eaTaggedValue.Update();
            }
        }

        public void Initialize(UmlPackageSpec spec)
        {
            eaPackage.Element.Stereotype = spec.Stereotype;
            eaPackage.Update();

            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                CreateTaggedValue(taggedValueSpec);
            }

            var diagram = (Diagram) eaPackage.Diagrams.AddNew(spec.Name, spec.DiagramType.ToString());
            diagram.Update();
        }

        private void Update(UmlPackageSpec spec)
        {
            eaPackage.Element.Stereotype = spec.Stereotype;
            eaPackage.Update();

            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                IUmlTaggedValue taggedValue = GetTaggedValue(taggedValueSpec.Name);
                if (taggedValue.IsDefined)
                {
                    taggedValue.Update(taggedValueSpec);
                }
                else
                {
                    CreateTaggedValue(taggedValueSpec);
                }
            }
        }

        public bool Equals(EaUmlPackage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaPackage.PackageID, eaPackage.PackageID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlPackage)) return false;
            return Equals((EaUmlPackage) obj);
        }

        public override int GetHashCode()
        {
            return (eaPackage != null ? eaPackage.PackageID : 0);
        }

        public static bool operator ==(EaUmlPackage left, EaUmlPackage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlPackage left, EaUmlPackage right)
        {
            return !Equals(left, right);
        }
    }
}