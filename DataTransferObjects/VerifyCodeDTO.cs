namespace SmartAgroAPI.DataTransferObjects
{
    public class VerifyCodeDTO
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
