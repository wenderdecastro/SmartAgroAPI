using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Interfaces
{
    public interface IUserRepository
    {
        Usuario? Login(UserAuthenticationDTO user);
        bool AuthenticateCode(Guid userId, string code);
        Usuario? GetById(Guid id);
        List<Usuario> GetAll();
        void Register(UserRegisterDTO user);
        void Edit(Guid userId, Usuario user);


    }
}
