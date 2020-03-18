using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Web.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public byte[] Password { get; set; }
    }
}
