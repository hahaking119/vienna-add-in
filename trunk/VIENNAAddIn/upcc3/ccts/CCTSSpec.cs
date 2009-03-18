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
using System.Reflection;

namespace VIENNAAddIn.upcc3.ccts
{
    public class CCTSSpec
    {
        public IEnumerable<TaggedValueSpec> GetTaggedValues()
        {
            Type type = GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(typeof (TaggedValueAttribute), true);
                if (attributes.Length > 0)
                {
                    var attribute = (TaggedValueAttribute) attributes[0];
                    if (property.PropertyType == typeof (string))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key, (string) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (IEnumerable<string>))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key,
                                                (IEnumerable<string>) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (bool))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key,
                                                (bool) property.GetValue(this, new object[] {}));
                    }
                }
            }
        }
    }
}