/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;

namespace VIENNAAddIn.Exceptions { 
	/// <sUMM2ary>
	/// Thrown if an error occurs when accessing configuration values of the 
	/// AddIn in the registry
	/// </sUMM2ary>
	internal class RegistryAccessException : AddInException {
		internal RegistryAccessException() : base() {
			//
			// TODO: Add constructor logic here
			//
		}
		internal RegistryAccessException(String message) : base(message) {
		}
		internal RegistryAccessException(String message,Exception innerException) : base(message,innerException) {
		}
	}
}
