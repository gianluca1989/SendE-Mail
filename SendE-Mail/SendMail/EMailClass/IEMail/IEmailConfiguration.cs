using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EMailClass.IEMail
{
    interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }
    }
}
