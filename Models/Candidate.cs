using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JobWebsite.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Avatar { get; set; }
        [NotMapped]
        public IFormFile? AvatarImg { get; set; }
        public string? Address { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Description { get; set; }

        public virtual IdentityUser? User { get; set; }
    }
}
