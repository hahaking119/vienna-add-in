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

namespace VIENNAAddIn.validator
{
    class ValidationMessage
    {

        public enum errorLevelTypes { INFO, WARN, ERROR };
        static int messageIDcounter;

        
        /// <summary>
        /// Create a new Validationmessage and generate an automatic messageID
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="messageDetail_"></param>
        /// <param name="affectedView_"></param>
        /// <param name="affectedElementID_"></param>
        public ValidationMessage(String message_, String messageDetail_, String affectedView_, errorLevelTypes errorLevel_, int affectedPackageID_)
        {

            this.message = message_;
            this.messageDetail = messageDetail_;
            this.affectedView = affectedView_;
            this.errorLevel = errorLevel_;
            this.affectedPackageID = affectedPackageID_;
            this.messageID = ++messageIDcounter;
                      
        }


        int messageID;

        public int MessageID
        {
            get { return messageID; }
            set { messageID = value; }
        }


        errorLevelTypes errorLevel;

        public errorLevelTypes ErrorLevel
        {
            get { return errorLevel; }
            set { errorLevel = value; }
        }

        
        
        String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
        String messageDetail;

        public String MessageDetail
        {
            get { return messageDetail; }
            set { messageDetail = value; }
        }

        String affectedView;

        public String AffectedView
        {
            get { return affectedView; }
            set { affectedView = value; }
        }
        int affectedPackageID;

        public int AffectedPackageID
        {
            get { return affectedPackageID; }
            set { affectedPackageID = value; }
        }
        

        

    }
}
