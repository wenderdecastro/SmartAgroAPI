using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class ResetPasswordDTO
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? TemporaryToken { get; set; }

        [Required]
        public string? NewPassowrd { get; set; }

    }
}
