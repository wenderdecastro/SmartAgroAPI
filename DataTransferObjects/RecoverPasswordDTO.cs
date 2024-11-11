using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class RecoverPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
