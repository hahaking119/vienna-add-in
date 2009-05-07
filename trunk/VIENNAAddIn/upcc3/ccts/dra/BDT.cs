// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class BDT : AbstractDT<BDTSpec>, IBDT
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="element"></param>
        public BDT(CCRepository repository, Element element) : base(repository, element, "BDT")
        {
        }

        #region IBDT Members

        ///<summary>
        ///</summary>
        public IBasedOnDependency BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsBasedOn);
                return connector != null ? new BasedOnDependency(repository, connector) : null;
            }
        }

        #endregion

        protected override void AddConnectors(BDTSpec spec)
        {
            if (spec.BasedOn != null)
            {
                element.AddDependency(Stereotype.BasedOn, spec.BasedOn.Id, "1", "1");
            }
        }
    }
}