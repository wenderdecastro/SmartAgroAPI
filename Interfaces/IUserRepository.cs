using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Interfaces
{
    public interface IUserRepository
    {
        Usuario? Login(UserAuthenticationDTO user);
        void ChangePassword(Guid userId, string newPassword);
        Usuario? GetById(Guid id);
        Usuario? GetByEmail(string email);
        List<UserDTO> GetAll();
        void Register(UserRegisterDTO user);
        void Edit(Guid userId, UserEditDTO user);
        string? GenerateRecoveryCode(Guid userId);
        bool AuthenticateCode(Guid userId, string code);
    }
}
