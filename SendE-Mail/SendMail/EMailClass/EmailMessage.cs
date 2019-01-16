using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EMailClass
{
    class EmailMessage
    {
        //Class that contains all the information for sending the email
        public EmailMessage()
        {
            ToAddresses = new EmailAddress();
            FromAddresses = new EmailAddress();
            CCAddresses = new List<EmailAddress>();
        }
        public EmailAddress ToAddresses { get; set; }
        public EmailAddress FromAddresses { get; set; }
        public List<EmailAddress> CCAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
