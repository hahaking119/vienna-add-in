/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VIENNAAddIn.validator.umm;
using System.ComponentModel;
using VIENNAAddIn.common.logging;



namespace VIENNAAddIn.validator
{
    class Validator : AbstractValidator
    {


        public Validator(EA.Repository repository, String scope)
        {
            this.repository = repository;
            this.scope = scope;
        }



        internal override List<ValidationMessage> validate()
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            //Which validator should be invoked?         

            if (true)
            {
                bCollModelValidator bcmV = new bCollModelValidator(repository, scope);
                try
                {
                    messages.AddRange(bcmV.validate());
                } 
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, LogType.ERROR); 
                    ExceptionEventArgs args = new ExceptionEventArgs(ex);
                    this.OnChange(args);
                }
            }
            //TODO - Implement CCTS validation logic here
            else
            {


            }


            return messages;
        }

        protected override void OnChange(ExceptionEventArgs args)
        {
            base.OnChange(args);
        }
    }
}

