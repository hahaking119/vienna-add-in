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
    /// SUMMary description for VIENNAAddInAddInException.
	/// </summary>
	internal class AddInException : Exception 
	{	
        //private Logger logger = new Logger();

		internal AddInException() : base()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		internal AddInException(String message) : base(message) {
            //logger.Error(message);
		}
        internal AddInException(String message, Exception innerException)  : base(message, innerException)
        {
            //logger.Error(message);
		}
	}
}
