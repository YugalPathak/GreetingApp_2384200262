using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Entity
{
    /// <summary>
    /// Represents a user entity stored in the database.
    /// </summary>
    public class HelloGreetingEntity
    {
        public int id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string? message { get; set; }  // Nullable so it's optional
    }
}