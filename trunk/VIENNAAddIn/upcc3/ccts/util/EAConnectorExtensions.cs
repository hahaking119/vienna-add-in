using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;

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

        public static IEnumerable<string> GetTaggedValues(this Connector connector, TaggedValues key)
        {
            return MultiPartTaggedValue.Split(GetTaggedValue(connector, key.ToString()));
        }

        public static string GetTaggedValue(this Connector connector, string name)
        {
            var taggedValue = connector.GetTaggedValueByName(name);
            return taggedValue != null ? taggedValue.Value : null;
        }

        private static ConnectorTag GetTaggedValueByName(this Connector connector, string name)
        {
            foreach (ConnectorTag tv in connector.TaggedValues)
            {
                if (tv.Name == name)
                {
                    return tv;
                }
            }
            return null;
        }

        public static bool HasTaggedValue(this Connector connector, string name)
        {
            return connector.GetTaggedValueByName(name) != null;
        }

        public static void SetOrGenerateTaggedValue(this Connector connector, TaggedValueSpec taggedValueSpec, string defaultValue)
        {
            if (String.IsNullOrEmpty(taggedValueSpec.Value))
            {
                if (String.IsNullOrEmpty(connector.GetTaggedValue(taggedValueSpec.Key.ToString())))
                {
                    connector.AddTaggedValue(taggedValueSpec.Key.ToString()).WithValue(defaultValue);
                }
            }
            else
            {
                connector.AddTaggedValue(taggedValueSpec.Key.ToString()).WithValue(taggedValueSpec.Value);
            }
        }

        public static ConnectorEnd GetAssociatedEnd(this Connector connector, int associatingElementId)
        {
            return connector.ClientID == associatingElementId ? connector.SupplierEnd : connector.ClientEnd;
        }

        public static int GetAssociatedElementId(this Connector connector, int associatingElementId)
        {
            return connector.ClientID == associatingElementId ? connector.SupplierID : connector.ClientID;
        }

        public static ConnectorEnd GetAssociatingEnd(this Connector connector, int associatingElementId)
        {
            return connector.ClientID == associatingElementId ? connector.ClientEnd : connector.SupplierEnd;
        }

        /// <returns>True if the connector has the given stereotype, false otherwise.</returns>
        public static bool IsA(this Connector connector, string stereotype)
        {
            return connector != null && connector.Stereotype == stereotype;
        }

        /// <returns>True if the connector has the isEquivalentTo stereotype, false otherwise.</returns>
        public static bool IsIsEquivalentTo(this Connector con)
        {
            return con.IsA(Stereotype.isEquivalentTo);
        }

        /// <returns>True if the connector has the basedOn stereotype, false otherwise.</returns>
        public static bool IsBasedOn(this Connector con)
        {
            return con.IsA(Stereotype.basedOn);
        }

        /// <returns>True if the connector has the ASBIE stereotype, false otherwise.</returns>
        public static bool IsASBIE(this Connector con)
        {
            return con.Stereotype == Stereotype.ASBIE;
        }

        /// <returns>True if the connector has the ASMA stereotype, false otherwise.</returns>
        public static bool IsAsma(this Connector con)
        {
            return con.Stereotype == Stereotype.ASMA;
        }

        /// <returns>True if the connector has the ASCC stereotype, false otherwise.</returns>
        public static bool IsASCC(this Connector con)
        {
            return con.IsA(Stereotype.ASCC);
        }
    }
}