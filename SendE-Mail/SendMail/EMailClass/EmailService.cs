using log4net;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SendMail.EF;
using SendMail.EF.EFInterface;
using SendMail.EfStructures.Entities;
using SendMail.EMailClass.IEMail;
using SendMail.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SendMail.EMailClass
{
    class EmailService : IEmailService
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Send the mail
        public void Send(EmailMessage emailMessage, Employes emp)
        {
            IEmailConfiguration emailConfiguration;
            try
            {
                Utilities util = new Utilities();
                emailConfiguration = util.Inject();
            }
            catch (Exception e)
            {
                throw new Exception($"Error in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n Data needed to connect to the missing server in the configuration file.\n{e.Message}");
            }

            var message = new MimeMessage();
            var builder = new BodyBuilder();

            try
            {
                //Adds the cc from and to parameters needed to send the e-mail
                message.Cc.AddRange(emailMessage.CCAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                message.To.Add(new MailboxAddress(emailMessage.ToAddresses.Name, emailMessage.ToAddresses.Address));
                message.From.Add(new MailboxAddress(emailMessage.FromAddresses.Name, emailMessage.FromAddresses.Address));

                if (emailMessage.FromAddresses.Address == null || emailMessage.FromAddresses.Address == "")
                    throw new Exception("Address from which the missing e-mail is sent. Mail not sent.");
                if (emailMessage.ToAddresses.Address == null || emailMessage.ToAddresses.Address == "")
                    throw new Exception("Mailing address missing receiver. Mail not sent.");
                if (emailMessage.FromAddresses.Name == null || emailMessage.FromAddresses.Name == "" || emailMessage.ToAddresses.Name == null || emailMessage.ToAddresses.Name == "")
                    log.Warn($"Warning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.Name of the sender or missing recipient");

                //Upload subject of the mail
                message.Subject = emailMessage.Subject;

                //Upload the text of the email
                builder.TextBody = emailMessage.Content;

                //Takes the string with the path to the attachments and splits it into several strings, (one for each attachment)
                string Allegati = new Configuration().GetField("PathAllegati");
                var arr = Allegati.Split(';');
                if (arr == null)
                    log.Info($"Info in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.No allegate found");

                //Builds the paths of the attachments
                buildPath(arr, emp);

                //Add all attachments
                foreach (var a in arr)
                {
                    try
                    {
                        builder.Attachments.Add(a);
                    }
                    catch (Exception e)
                    {
                        log.Warn($"Warning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nFile not found\n{e.Message}");
                    }
                }

                //Adds attachments to the body of the email
                message.Body = builder.ToMessageBody();

                log.Info($"\nInfo in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nStart logging and sending");

            }
            catch(Exception e)
            {
                log.Error($"\nError in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\n {e.Message}");
            }



            //Create connection and send the mail
            using (var emailClient = new SmtpClient())
            {
           
                try
                {
                    emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, true);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);

                    emailClient.Send(message);
    
                }
                catch (Exception e)
                {
                    throw new Exception($"\nError in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nIncorrect connection data to the server. Don't send.\n{e.Message}");
                }
                finally
                {
                    emailClient.Disconnect(true);
                }

                if (emailMessage.FromAddresses.Address != null && emailMessage.FromAddresses.Address != "" && emailMessage.ToAddresses.Address != null && emailMessage.ToAddresses.Address != "")
                    log.Info($"E-Mail sent at {emailMessage.ToAddresses.Address} with {builder.Attachments.Count} attachments.");
            }
        }



        //Takes the path from the configuration file and modifies it to create the file name to be attached with regular expression
        public void buildPath(string[] str, Employes emp)
        {
            //The pattern of the regular expression
            string pattern = "({[a-zA-Z0-9_-]*})";
            //Get thew property name of the our class with thew reflection
            var myType = emp.GetType();
            string nameProperties = "";

            for (int i = 0; i < str.Length; i++)
            {
                //Creates a list of elements extracted thanks to the regular expression pattern
                var nomiTanbelle = Regex.Matches(str[i], pattern);
                //Replaces keys in the path with database items
                foreach (var a in nomiTanbelle)
                {
                    try
                    {
                        nameProperties = a.ToString();
                        //Get the value of our property with the reflection
                        string change = myType.GetProperty(nameProperties.Replace("{", "").Replace("}", "")).GetValue(emp, null).ToString();
                        str[i] = str[i].Replace(nameProperties, change);
                    }
                    catch(Exception e)
                    {
                        log.Warn($"Warning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.nPath wrong. File not attached.\n{e.Message}");
                        str[i] = str[i].Replace(nameProperties, "");
                    }

                }
            }
        }

        //upload data from databases and configuration files
        public void uploadData(EmailMessage em,ref List<Employes> list)
        {
            IEFEmployes emp = new EFEmployesRepository();
            List<EmailAddress> listCC = new List<EmailAddress>();

            //it takes all the necessary data from databases and configuration files
            try
            {
                list = emp.GetAll().ToList();

                if (list == null)
                {
                    log.Warn($"\nWarning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nNo data in the database");
                }

                //upload to an object the data needed to send the mail from configuration files and databases
                EmailAddress eaFrom = new EmailAddress();
                eaFrom.Name = new Configuration().GetField("DaNome");
                eaFrom.Address = new Configuration().GetField("DaIndirizzo");
                em.FromAddresses = eaFrom;

                em.Content = new Configuration().GetField("Body");
                em.Subject = new Configuration().GetField("Oggetto");

                string IndirizzoCC = new Configuration().GetField("CCIndirizzo");
                var arrInd = IndirizzoCC.Split(';');
                string NomeCC = new Configuration().GetField("CCNome");
                var arrName = NomeCC.Split(';');

                if (arrInd.Length != arrName.Length) log.Warn($"\nWarning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nError in the ccAddress and ccName configuration file. Some Cc may not be entered, or there may be missing data");
                if (arrInd != null)
                {
                   
                    for (int i = 0; i < arrInd.Length; i++)
                    {
                        if (arrInd[i] != "")
                        {
                            EmailAddress eaCC = new EmailAddress();
                            if (i >= arrName.Length) eaCC.Name = "";
                            else eaCC.Name = arrName[i];
                            eaCC.Address = arrInd[i];
                            listCC.Add(eaCC);
                        }
                        else log.Warn($"\nWarning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nCc address empty");
                    }

                    em.CCAddresses = listCC;
                   
                }
                else log.Warn($"\nWarning in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nCc address empty");
            }
            catch (Exception e)
            {
                log.Error($"\nError in Class: {MethodBase.GetCurrentMethod().ReflectedType.Name} function: {MethodBase.GetCurrentMethod().Name}.\nWrong or missing data, mail not send\n{e.Message}");
            }
        }
    }
}
