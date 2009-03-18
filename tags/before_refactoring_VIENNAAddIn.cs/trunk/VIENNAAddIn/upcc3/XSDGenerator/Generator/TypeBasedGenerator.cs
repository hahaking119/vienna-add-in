// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    internal class TypeBasedGenerator : TypeBasedSelector<XmlSchema>
    {
        public TypeBasedGenerator()
        {
            Default(arg => null);
        }

        public void AddGenerator<T>(ILibraryGenerator<T> generator)
        {
            Case<T>(l => generator.GenerateXSD(l));
        }
    }
}