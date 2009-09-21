/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;


namespace VIENNAAddIn.Exceptions {
    /// <sUMM2ary>
    /// SUMM2ary description for OCLConstraintNotApplicableException.
    /// </sUMM2ary>
    internal class OCLConstraintNotApplicableException : AddInException {
        internal OCLConstraintNotApplicableException()
            : base() {
            //
            // TODO: Add constructor logic here
            //
        }
        internal OCLConstraintNotApplicableException(String message)
            : base(message) {
        }
    }
}