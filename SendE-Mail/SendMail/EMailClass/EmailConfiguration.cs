using SendMail.EMailClass.IEMail;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EMailClass
{
    //Class that contains all the information for connecting to the server
    class EmailConfiguration : IEmailConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
