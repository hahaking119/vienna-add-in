// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;

namespace VIENNAAddIn.upcc3.ccts.util
{
    /// <summary>
    /// An exception class used for wrapping EA-API error messages.
    /// </summary>
    public class EAException : Exception
    {
        public EAException(string errorMessage) : base(errorMessage)
        {
        }
    }
}