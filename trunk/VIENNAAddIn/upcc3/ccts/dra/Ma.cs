using System;
using System.Collections.Generic;
using CctsRepository.DocLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class Ma : IMa
    {
        private readonly Element element;

        public Ma(CCRepository repository, Element element)
        {
            this.element = element;
        }

        #region IMa Members

        public string Name
        {
            get { return element.Name; }
        }

        public IDocLibrary DocLibrary
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAsma> Asmas { get; set; }

        public IAsma CreateAsma(AsmaSpec specification)
        {
            throw new NotImplementedException();
        }

        public IAsma UpdateAsma(IAsma asma, AsmaSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveAsma(IAsma asma)
        {
            throw new NotImplementedException();
        }

        public int Id
        {
            get { return element.ElementID; }
        }

        #endregion

        public void Update(MaSpec spec)
        {
            element.Name = spec.Name;

            for (var i = (short)(element.Connectors.Count - 1); i >= 0; i--)
            {
                if (DeleteConnectorOnUpdate((Connector)element.Connectors.GetAt(i)))
                {
                    element.Connectors.Delete(i);
                }
            }
            element.Connectors.Refresh();
            foreach (ConnectorSpec connector in GetConnectorSpecs(spec))
            {
                element.AddConnector(connector);
            }
            element.Connectors.Refresh();

            element.Update();
            element.Refresh();
        }

        private bool DeleteConnectorOnUpdate(Connector connector)
        {
            return IsAsma(connector);
        }

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(MaSpec spec)
        {
            IEnumerable<AsmaSpec> asmaSpecs = spec.Asmas;
            if (asmaSpecs != null)
            {
                foreach (AsmaSpec asmaSpec in asmaSpecs)
                {
                    yield return ConnectorSpec.CreateAggregation(EaAggregationKind.Shared, Stereotype.ASMA, asmaSpec.Name, asmaSpec.AssociatedBieAggregator.Id, asmaSpec.LowerBound, asmaSpec.UpperBound, new List<TaggedValueSpec>(), null);
                }
            }
        }

        private bool IsAsma(Connector connector)
        {
            if (connector.IsAsma())
            {
                if (connector.ClientID == element.ElementID)
                {
                    return connector.ClientEnd.Aggregation != (int)EaAggregationKind.None;
                }
                if (connector.SupplierID == element.ElementID)
                {
                    return connector.SupplierEnd.Aggregation != (int)EaAggregationKind.None;
                }
            }
            return false;
        }
    }
}