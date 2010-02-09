using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SourceItem : IEquatable<SourceItem>
    {
        private readonly XsdObjectType xsdObjectType;
        private readonly List<SourceItem> children;
        private SourceItem parent;

        public SourceItem(string name, XmlSchemaType type, XsdObjectType xsdObjectType, string mappingTargetKey)
        {
            this.xsdObjectType = xsdObjectType;
            Name = name;
            XsdType = type;
            MappingTargetKey = mappingTargetKey;
            children = new List<SourceItem>();
        }

        public string Name { get; private set; }

        public List<SourceItem> Children
        {
            get { return new List<SourceItem>(children); }
        }

        public string XsdTypeName
        {
            get
            {
                if (XsdType == null)
                {
                    return "";
                }

                return XsdType.Name ?? XsdType.TypeCode.ToString();
            }
        }

        public XmlSchemaType XsdType { get; private set; }

        public string Path
        {
            get
            {
                if (parent != null)
                {
                    return parent.Path + "/" + Name;
                }
                return Name;
            }
        }

        public string MappingTargetKey { get; private set; }

        public bool IsMapped
        {
            get { return MappingTargetKey != null; }
        }

        #region IEquatable<SourceItem> Members

        public bool Equals(SourceItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        #endregion

        public void AddChild(SourceItem newChild)
        {
            foreach (SourceItem existingChild in children)
            {
                if ((newChild.xsdObjectType == existingChild.xsdObjectType) && (existingChild.Name == newChild.Name))
                {
                    existingChild.MergeWith(newChild);
                    return;
                }
            }
            
            children.Add(newChild);
            newChild.parent = this;
        }

        public void MergeWith(SourceItem otherItem)
        {
            if (otherItem.IsMapped)
            {
                if (IsMapped)
                {
                    if (otherItem.MappingTargetKey != MappingTargetKey)
                    {
                        throw new MappingError("Same Source Item mapped to multiple targets: " + Path + ", " +
                                               otherItem.Path);
                    }
                }
                else
                {
                    MappingTargetKey = otherItem.MappingTargetKey;
                }
            }
            MergeChildren(otherItem.Children);
        }

        public void MergeChildren(List<SourceItem> otherChildren)
        {
            foreach (SourceItem otherChild in otherChildren)
            {
                AddChild(otherChild);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SourceItem)) return false;
            return Equals((SourceItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SourceItem left, SourceItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SourceItem left, SourceItem right)
        {
            return !Equals(left, right);
        }

        public bool HasSimpleType()
        {
            return (XsdType is XmlSchemaSimpleType);
        }

        public bool HasComplexType()
        {
            return (XsdType is XmlSchemaComplexType);
        }

        public bool HasSimpleContent()
        {
            return (((XmlSchemaComplexType) XsdType).ContentModel is XmlSchemaSimpleContent);
        }
    }
}