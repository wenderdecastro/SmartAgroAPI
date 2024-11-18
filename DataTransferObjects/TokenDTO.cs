namespace SmartAgroAPI.DataTransferObjects
{
    public class TokenDTO
    {
        public required UserDTO user { get; set; }
        public required string Token { get; set; }
    }
}
