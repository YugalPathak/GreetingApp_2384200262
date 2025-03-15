using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GreetingId { get; set; }
        [Required]
        public string message { get; set; }

        // Foreign Key to UserEntity
        public int id { get; set; }

        [ForeignKey("UserId")]
        public virtual HelloGreetingEntity User { get; set; }

    }
}