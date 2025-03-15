using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Represents a user entity stored in the database.
    /// </summary>
    public class HelloGreetingEntity
    {
        [Key]
        public int id { get; set; }
        //public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string? Salt { get; set; }

        // Navigation Property (One-to-Many Relationship)
        public ICollection<GreetingEntity> Greetings { get; set; } = new List<GreetingEntity>();
        public string? Message { get; set; }
        public int Id { get; internal set; }
    }
}