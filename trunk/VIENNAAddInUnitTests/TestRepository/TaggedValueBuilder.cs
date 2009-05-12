// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.TestRepository
{
    public class TaggedValueBuilder
    {
        public TaggedValueBuilder(TaggedValues key, string value)
        {
            Name = key.ToString();
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}