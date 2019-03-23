using System.ComponentModel.DataAnnotations;
using Api.Core.Models;

namespace SkineroMotors.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
    }
}