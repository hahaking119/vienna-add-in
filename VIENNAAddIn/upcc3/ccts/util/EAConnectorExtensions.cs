using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    /// Extension methods for EA.Connector.
    ///</summary>
    public static class EAConnectorExtensions
    {
        ///<summary>
        /// Add a TaggedValue to the Connector.
        ///</summary>
        ///<param name="connector"></param>
        ///<param name="name"></param>
        ///<returns></returns>
        public static ConnectorTag AddTaggedValue(this Connector connector, string name)
        {
            var taggedValue = (ConnectorTag) connector.TaggedValues.AddNew(name, "");
            taggedValue.Value = "";
            taggedValue.Update();
            return taggedValue;
        }
    }
}