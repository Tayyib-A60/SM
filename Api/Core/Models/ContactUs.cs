using System.ComponentModel.DataAnnotations;

namespace SkineroMotors.Core.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Phone { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
    }
}