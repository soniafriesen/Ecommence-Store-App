using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*
 Project: Case Study
 Purpose:Class for customer
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DomainClasses
{
    public class Customer
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
        [Required] public string Firstname { get; set; }
        [Required] public string Lastname { get; set; }
        [Required] public string Email { get; set; }
        // we don't store an actual password         
        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }
    }
}
