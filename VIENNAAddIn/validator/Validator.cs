/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System.Collections.Generic;
using VIENNAAddIn.validator.umm;

namespace VIENNAAddIn.validator
{
    internal class Validator : AbstractValidator
    {
        internal override void validate(IValidationContext context, string scope)
        {
            new bCollModelValidator().validate(context, scope);
            //TODO - Implement CCTS validation logic here
        }
    }
}