using SmartAgroAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartAgroAPI.DataTransferObjects
{
    public class UserEditDTO
    {

        public string? Nome { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Telefone { get; set; }

        public UserEditDTO()
        {

        }

        public UserEditDTO(Usuario user)
        {
            this.Nome = user.Nome;
            this.Email = user.Email;
            this.Telefone = user.Telefone;
        }

    }
}
