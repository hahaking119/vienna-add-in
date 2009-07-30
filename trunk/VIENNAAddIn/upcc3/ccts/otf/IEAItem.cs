using System;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IEAItem
    {
        string Name { get; }
        ItemId Id { get; }
    }

    public class ItemId : IEquatable<ItemId>
    {
        public static readonly ItemId Null = new ItemId(ItemType.Package, 0);

        public ItemId(ItemType type, int value)
        {
            Value = value;
            Type = type;
        }

        public static ItemId ForPackage(int packageId)
        {
            return packageId == 0 ? Null : new ItemId(ItemType.Package, packageId);
        }

        public static ItemId ForElement(int elementId)
        {
            return new ItemId(ItemType.Element, elementId);
        }

        public int Value { get; private set; }

        public ItemType Type { get; private set; }

        public bool Equals(ItemId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Value == Value && Equals(other.Type, Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (ItemId) && Equals((ItemId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value*397) ^ Type.GetHashCode();
            }
        }

        public override string ToString()
        {
            return "ItemId[Type=" + Type + ", Value=" + Value + "]";
        }

        public enum ItemType
        {
            Package,
            Element
        }

        public bool IsNull
        {
            get { return Null.Equals(this); }
        }
    }
}