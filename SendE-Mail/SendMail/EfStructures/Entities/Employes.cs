using SendMail.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendMail.EfStructures.Entities
{
    //Class that contains all the information of the employee
    public partial class Employes
    {
        [Key]
        public int Matricola { get; set; }
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
        [Required]
        [StringLength(50)]
        public string Cognome { get; set; }
        [Required]
        [Column("E-Mail")]
        public string EMail { get; set; }
    }
}
