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
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class CCTSSpec
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
                    TaggedValues taggedValue = DetermineTaggedValue(type, property, attribute);
                    if (property.PropertyType == typeof (string))
                    {
                        yield return
                            new TaggedValueSpec(taggedValue, (string) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (IEnumerable<string>))
                    {
                        yield return
                            new TaggedValueSpec(taggedValue,
                                                (IEnumerable<string>) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (bool))
                    {
                        yield return
                            new TaggedValueSpec(taggedValue,
                                                (bool) property.GetValue(this, new object[] {}));
                    }
                }
            }
        }

        private TaggedValues DetermineTaggedValue(Type type, PropertyInfo property, TaggedValueAttribute attribute)
        {
            TaggedValues taggedValue = attribute.Key;
            if (taggedValue == TaggedValues.undefined)
            {
                taggedValue = GetTaggedValue(property.Name);
            }
            if (taggedValue == TaggedValues.undefined && property.Name.EndsWith("s"))
            {
                taggedValue = GetTaggedValue(property.Name.Substring(0, property.Name.Length - 1));
            }
            if (taggedValue == TaggedValues.undefined)
            {
                throw new Exception("cannot determine tagged value of CCTSSpec property " + property.DeclaringType.Name +
                                    "." + property.Name);
            }
            return taggedValue;
        }

        private TaggedValues GetTaggedValue(string propertyName)
        {
            TaggedValues taggedValue;
            try
            {
                taggedValue = (TaggedValues) Enum.Parse(typeof (TaggedValues), propertyName, true);
            }
            catch (ArgumentException)
            {
                taggedValue = TaggedValues.undefined;
            }
            return taggedValue;
        }

        public IEnumerable<ConnectorSpec> GetConnectors(CCRepository repository)
        {
            Type type = GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(typeof (DependencyAttribute), true);
                if (attributes.Length > 0)
                {
                    var attribute = (DependencyAttribute) attributes[0];
                    string stereotype = attribute.Stereotype ?? Stereotype.Normalize(property.Name);
                    if (typeof (ICCTSElement).IsAssignableFrom(property.PropertyType))
                    {
                        var value = (ICCTSElement) property.GetValue(this, new object[0]);
                        if (value != null)
                        {
                            yield return ConnectorSpec.CreateDependency(stereotype, value.Id, "1", "1");
                        }
                    }
                    else if (typeof (IEnumerable<ICCTSElement>).IsAssignableFrom(property.PropertyType))
                    {
                        var values = (IEnumerable<ICCTSElement>) property.GetValue(this, new object[] {});
                        if (values != null)
                        {
                            foreach (ICCTSElement value in values)
                            {
                                yield return ConnectorSpec.CreateDependency(stereotype, value.Id, "1", "1");
                            }
                        }
                    }
                }
            }
            foreach (ConnectorSpec connector in GetCustomConnectors(repository))
            {
                yield return connector;
            }
        }

        public virtual IEnumerable<ConnectorSpec> GetCustomConnectors(CCRepository repository)
        {
            yield break;
        }
    }

    public class TaggedValueAttribute : Attribute
    {
        public TaggedValueAttribute()
            : this(TaggedValues.undefined)
        {
        }

        public TaggedValueAttribute(TaggedValues key)
        {
            Key = key;
        }

        public TaggedValues Key { get; private set; }
    }

    public class DependencyAttribute : Attribute
    {
        public DependencyAttribute(string stereotype)
        {
            Stereotype = stereotype;
        }

        public DependencyAttribute() : this(null)
        {
        }

        public string Stereotype { get; private set; }
    }
}