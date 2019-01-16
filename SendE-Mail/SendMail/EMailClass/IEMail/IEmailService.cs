using SendMail.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EMailClass.IEMail
{
    interface IEmailService
    {
        void Send(EmailMessage emailMessage, Employes emp);
        void buildPath(string[] arr, Employes emp);
        void uploadData(EmailMessage em,ref List<Employes> list);
    }
}
