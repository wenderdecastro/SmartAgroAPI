using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class RecoverPasswordDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
