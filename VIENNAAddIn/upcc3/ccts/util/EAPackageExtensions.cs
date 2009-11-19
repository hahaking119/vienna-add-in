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
using EA;
using CctsRepository;
using VIENNAAddInUtils;
using Stereotype=CctsRepository.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EAPackageExtensions
    {
        public static Path GetPath(this Package package, Repository repository)
        {
            int parentId = package.ParentID;
            if (parentId == 0)
            {
                return package.Name;
            }
            Package parent = repository.GetPackageByID(parentId);
            return parent.GetPath(repository)/package.Name;
        }

        public static Package AddPackage(this Package package, string name, Action<Package> initPackage)
        {
            var subPackage = (Package) package.Packages.AddNew(name, "Package");
            subPackage.Update();
            initPackage(subPackage);
            subPackage.Update();
            return subPackage;
        }

        public static Diagram AddDiagram(this Package package, string name, string type)
        {
            var diagram = (Diagram) package.Diagrams.AddNew(name, type);
            diagram.Update();
            return diagram;
        }

        public static Element AddElement(this Package package, string name, string type)
        {
            var element = (Element) package.Elements.AddNew(name, type);
            element.Update();
            return element;
        }

        public static Element AddClass(this Package package, string name)
        {
            return package.AddElement(name, "Class");
        }

        public static TaggedValue AddTaggedValue(this Package package, string name)
        {
            return package.Element.AddTaggedValue(name);
        }

        public static IEnumerable<string> GetTaggedValues(this Package package, TaggedValues key)
        {
            return package.Element.GetTaggedValues(key);
        }

        public static string GetTaggedValue(this Package package, TaggedValues key)
        {
            return package.Element.GetTaggedValue(key);
        }

        public static void SetTaggedValues(this Package package, TaggedValues key, IEnumerable<string> values)
        {
            package.Element.SetTaggedValues(key, values);
        }

        public static void SetTaggedValue(this Package package, TaggedValues key, string value)
        {
            package.Element.SetTaggedValue(key, value);
        }

        public static Package PackageByName(this Package package, string name)
        {
            foreach (Package child in package.Packages)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }

        public static Element ElementByName(this Package package, string name)
        {
            foreach (Element element in package.Elements)
            {
                if (element.Name == name)
                {
                    return element;
                }
            }
            return null;
        }

        private static TaggedValue GetTaggedValueByName(this Package package, string name)
        {
            foreach (TaggedValue tv in package.Element.TaggedValues)
            {
                if (tv.Name == name)
                {
                    return tv;
                }
            }
            return null;
        }

        public static bool HasTaggedValue(this Package package, string name)
        {
            return package.GetTaggedValueByName(name) != null;
        }

        /// <returns>True if the package has the given stereotype, false otherwise.</returns>
        public static bool IsA(this Package package, string stereotype)
        {
            return package != null && package.Element != null && package.Element.Stereotype == stereotype;
        }

        /// <returns>True if the attribute has the CCLibrary stereotype, false otherwise.</returns>
        public static bool IsCCLibrary(this Package package)
        {
            return package.IsA(Stereotype.CCLibrary);
        }

        /// <returns>True if the attribute has the CDTLibrary stereotype, false otherwise.</returns>
        public static bool IsCDTLibrary(this Package package)
        {
            return package.IsA(Stereotype.CDTLibrary);
        }

        /// <returns>True if the attribute has the BIELibrary stereotype, false otherwise.</returns>
        public static bool IsBIELibrary(this Package package)
        {
            return package.IsA(Stereotype.BIELibrary);
        }

        /// <returns>True if the attribute has the BDTLibrary stereotype, false otherwise.</returns>
        public static bool IsBDTLibrary(this Package package)
        {
            return package.IsA(Stereotype.BDTLibrary);
        }

        /// <returns>True if the attribute has the PRIMLibrary stereotype, false otherwise.</returns>
        public static bool IsPRIMLibrary(this Package package)
        {
            return package.IsA(Stereotype.PRIMLibrary);
        }

        /// <returns>True if the attribute has the ENUMLibrary stereotype, false otherwise.</returns>
        public static bool IsENUMLibrary(this Package package)
        {
            return package.IsA(Stereotype.ENUMLibrary);
        }

        /// <returns>True if the attribute has the DOCLibrary stereotype, false otherwise.</returns>
        public static bool IsDOCLibrary(this Package package)
        {
            return package.IsA(Stereotype.DOCLibrary);
        }

        /// <returns>True if the attribute has the bLibrary stereotype, false otherwise.</returns>
        public static bool IsBLibrary(this Package package)
        {
            return package.IsA(Stereotype.bLibrary);
        }

        /// <returns>True if the attribute has the bInformationV stereotype, false otherwise.</returns>
        public static bool IsBInformationV(this Package package)
        {
            return package.IsA(Stereotype.BInformationV);
        }
    }
}