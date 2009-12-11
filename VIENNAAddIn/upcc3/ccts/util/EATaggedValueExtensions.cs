using System;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    ///</summary>
    public static class EATaggedValueExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="taggedValue"></param>
        ///<param name="doSomething"></param>
        ///<returns></returns>
        public static TaggedValue With(this TaggedValue taggedValue, params Action<TaggedValue>[] doSomething)
        {
            foreach (var doNextThingWith in doSomething)
            {
                doNextThingWith(taggedValue);
            }
            taggedValue.Update();
            return taggedValue;
        }

        public static TaggedValue WithValue(this TaggedValue taggedValue, string value)
        {
            return taggedValue.With(SetTaggedValue.Value(value));
        }
    }

    public static class SetTaggedValue
    {
        public static Action<TaggedValue> Value(string value)
        {
            return taggedValue => taggedValue.Value = value;
        }
    }

    ///<summary>
    ///</summary>
    public static class EAAttributeTagExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="taggedValue"></param>
        ///<param name="doSomething"></param>
        ///<returns></returns>
        public static AttributeTag With(this AttributeTag taggedValue, params Action<AttributeTag>[] doSomething)
        {
            foreach (var doNextThingWith in doSomething)
            {
                doNextThingWith(taggedValue);
            }
            taggedValue.Update();
            return taggedValue;
        }

        public static AttributeTag WithValue(this AttributeTag taggedValue, string value)
        {
            return taggedValue.With(SetAttributeTag.Value(value));
        }
    }

    ///<summary>
    ///</summary>
    public static class EAConnectorTagExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="taggedValue"></param>
        ///<param name="doSomething"></param>
        ///<returns></returns>
        public static ConnectorTag With(this ConnectorTag taggedValue, params Action<ConnectorTag>[] doSomething)
        {
            foreach (var doNextThingWith in doSomething)
            {
                doNextThingWith(taggedValue);
            }
            taggedValue.Update();
            return taggedValue;
        }

        public static ConnectorTag WithValue(this ConnectorTag taggedValue, string value)
        {
            return taggedValue.With(SetConnectorTag.Value(value));
        }
    }

    public static class SetAttributeTag
    {
        public static Action<AttributeTag> Value(string value)
        {
            return taggedValue => taggedValue.Value = value;
        }
    }

    public static class SetConnectorTag
    {
        public static Action<ConnectorTag> Value(string value)
        {
            return taggedValue => taggedValue.Value = value;
        }
    }
}