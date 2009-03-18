// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Xml.Schema;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDGenerator.Generator
{
    public class BDTLibraryGenerator : AbstractLibraryGenerator<IBDTLibrary>
    {
        public BDTLibraryGenerator(GenerationContext context)
            : base(context)
        {
        }

        public override XmlSchema GenerateXSD(IBDTLibrary library)
        {
            var schema = new XmlSchema();
            return schema;
        }
    }
}