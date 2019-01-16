using log4net;
using log4net.Config;
using SendMail.EF;
using SendMail.EF.EFInterface;
using SendMail.EfStructures.Entities;
using SendMail.EMailClass;
using SendMail.EMailClass.IEMail;
using SendMail.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SendMail
{
    class Program
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //creation of the log4net file
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            IEmailService es = new EmailService();
            EmailMessage em = new EmailMessage();
            List<Employes> list = new List<Employes>();

            try
            {
                es.uploadData(em,ref list);

                //upload the email address data and send the mails
                foreach (var l in list)
                {
                    EmailAddress eaTo = new EmailAddress();

                    eaTo.Address = l.EMail;
                    eaTo.Name = l.Nome;
                    em.ToAddresses = eaTo;

                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nStart send mail");
                    es.Send(em, l);
                }
            }
            catch(Exception e)
            {
                log.Error($"Error: {e.Message}\n.Mail not send.");
            }
        }
    }
}
