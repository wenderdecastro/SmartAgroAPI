using SmartAgroAPI.Contexts;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Models;
using SmartAgroAPI.Utils;

namespace SmartAgroAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartAgroDbContext _context;

        public UserRepository()
        {
            _context = new SmartAgroDbContext();
        }

        public bool AuthenticateCode(Guid userId, string code)
        {
            var user = GetById(userId);
            if (user!.ExpiracaoCodigo!.Value < DateTime.Now)
            {
                return false;
            }

            return user.CodigoVerificacao == code;

        }

        public void ChangePassword(Guid userId, string newPassword)
        {
            var user = _context.Usuarios.Find(userId);
            user!.Senha = SHA256Encrypt.HashPassword(newPassword);
            _context.Update(user);
            _context.SaveChanges();
        }

        public void Edit(Usuario editedUser)
        {
            _context.Usuarios.Update(editedUser!);
            _context.SaveChanges();

        }

        public string? GenerateRecoveryCode(Usuario user)
        {
            var random = new Random();
            var recoveryCode = $"{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}{random.Next(0, 9)}";

            var expiration = DateTime.Now.AddMinutes(15);

            user.CodigoVerificacao = recoveryCode;
            user.ExpiracaoCodigo = expiration;

            Edit(user);

            return recoveryCode;
        }

        public List<UserDTO> GetAll()
        {
            return [.. _context.Usuarios.Select(x => new UserDTO(x))];
        }

        public Usuario? GetByEmail(string email)
        {
            var user = _context.Usuarios.FirstOrDefault(x => x.Email == email);
            if (user != null) user.Senha = string.Empty;

            return user;
        }

        public Usuario? GetById(Guid id)
        {
            var user = _context.Usuarios.Find(id);
            if (user != null) user.Senha = string.Empty;

            return user;
        }

        public Usuario? Login(UserAuthenticationDTO user)
        {
            var hashedPassword = SHA256Encrypt.HashPassword(user.Password!);
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == user.Email && x.Senha == hashedPassword);

            return usuario;
        }

        public void Register(UserRegisterDTO userData)
        {
            var newUser = new Usuario()
            {
                Id = Guid.NewGuid(),
                Nome = userData.Name!,
                Email = userData.Email!,
                Senha = SHA256Encrypt.HashPassword(userData.Password!),
                Telefone = userData.Phone!,
                IsAdmin = false

            };

            _context.Usuarios.Add(newUser);
            _context.SaveChanges();

        }
    }
}
