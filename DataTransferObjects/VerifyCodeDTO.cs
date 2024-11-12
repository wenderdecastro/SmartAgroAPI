using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class VerifyCodeDTO
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
