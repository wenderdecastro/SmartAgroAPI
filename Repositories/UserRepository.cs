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
            throw new NotImplementedException();
        }

        public void Edit(Guid userId, Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> GetAll()
        {
            throw new NotImplementedException();
        }

        public Usuario? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Usuario? Login(UserAuthenticationDTO user)
        {
            var hashedPassword = SHA256Encrypt.HashPassword(user.Password!);
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == user.Email && x.Senha == hashedPassword);

            return usuario;
        }

        public void Register(UserRegisterDTO user)
        {
            var newUser = new Usuario()
            {
                Id = Guid.NewGuid(),
                Nome = user.Name!,
                Email = user.Email!,
                Senha = SHA256Encrypt.HashPassword(user.Password!)
            };

            _context.Usuarios.Add(newUser);
            _context.SaveChanges();

        }
    }
}
