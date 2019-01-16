using Microsoft.EntityFrameworkCore;
using SendMail.EF.EFInterface;
using SendMail.EfStructures.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMail.EF
{
    public class EFEmployesRepository : IEFEmployes
    {
        private AdventureWorksContext context = new AdventureWorksContext();

        //Returns everything found in the table used on the database
        public IEnumerable<Employes> GetAll()
        {   
            return context.Employes;
        }
    }
}
