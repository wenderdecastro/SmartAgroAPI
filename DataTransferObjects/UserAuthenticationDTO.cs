using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class UserAuthenticationDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
