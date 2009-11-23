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
using System.Linq;
using CctsRepository;
using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class CCLibrary : ElementLibrary<IACC, ACC, ACCSpec>, ICCLibrary
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="package"></param>
        public CCLibrary(CCRepository repository, Package package)
            : base(repository, package)
        {
        }

        protected override ACC WrapEaElement(Element element)
        {
            return new ACC(repository, element);
        }
    }
}