using System.ComponentModel.DataAnnotations;

namespace E_Greetings.Models.Entities
{
    public class Users
    {
        [Key]
        public Guid UserId { get; set; }

        //[Required, MaxLength(80)]
        public string Name { get; set; }

        //[Required, MaxLength(120)]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required, MaxLength(200)]
        public string PasswordHash { get; set; }

        // "Admin" or "User"
        //[Required]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        //Navigation
        //public ICollection<SentCards> SentCards { get; set; }
        //public ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
