using SendMail.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EMailClass
{
    class Utilities
    {
        //loads all the values for configuring the connection from a configuration file into an object
        public EmailConfiguration Inject()
        {
            return new EmailConfiguration
            {
                SmtpPassword = new Configuration().GetField("SmtpPassword"),
                SmtpPort = int.Parse(new Configuration().GetField("SmtpPort")),
                SmtpServer= new Configuration().GetField("SmtpServer"),
                SmtpUsername= new Configuration().GetField("SmtpUsername")
            };
        }
    }
}
