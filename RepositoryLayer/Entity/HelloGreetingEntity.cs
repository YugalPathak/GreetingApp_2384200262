using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class HelloGreetingEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string message { get; set; }
    }
}
