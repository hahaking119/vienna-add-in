/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;

namespace VIENNAAddIn.Exceptions
{
    /// <summary>
    /// Summary description for UnexpectedElementException.
    /// </summary>
    internal class UnexpectedElementException : AddInException
    {
        internal UnexpectedElementException(String message)
            : base(message)
        {
        }
    }
}