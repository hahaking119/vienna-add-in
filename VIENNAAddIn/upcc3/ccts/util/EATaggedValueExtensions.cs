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
}