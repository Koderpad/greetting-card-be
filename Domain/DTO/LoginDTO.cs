using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
