using SendMail.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EF.EFInterface
{
    public interface IEFEmployes
    {
        IEnumerable<Employes> GetAll();
    }
}
