// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EAPackageExtensions
    {
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
    }
}