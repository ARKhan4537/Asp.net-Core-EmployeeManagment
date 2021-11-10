using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class Employee
    {
        public int id { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        
        [Required]
        [MaxLength(50,ErrorMessage ="Name Cannot exceed 50 c")]
        
        public string Name { get; set;}
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid Email Format")]
        [Display(Name="Office Email")]
        public string Email { get; set;}
         
        [Required]
        public dept? Department{ get; set;}


        [Display(Name="Photo")]
        public string PhotoPath { get; set; }

    }
}
