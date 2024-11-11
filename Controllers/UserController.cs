using Microsoft.AspNetCore.Mvc;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Repositories;
using SmartAgroAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartAgroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController()
        {
            _userRepository = new UserRepository();
        }



        // POST api/User/Login
        [HttpPost("Login")]
        public IActionResult Post([FromBody] UserAuthenticationDTO userCredentials)
        {
            var user = _userRepository.Login(userCredentials);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = JWTService.GenerateToken(user.Id, user.Email);

            return Ok(new TokenDTO() { Token = token });

        }

        // POST api/User
        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterDTO userData)
        {

            _userRepository.Register(userData);
            return Ok(userData);
        }


        // GET api/User
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAll());
        }


        // GET api/User/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);

        }

        // PATCH api/User/{id}
        [HttpPatch("{id}")]
        public IActionResult EditUserById(Guid id, [FromBody] UserDTO editedUser)
        {
            var user = _userRepository.GetById(id);

            if (!string.IsNullOrEmpty(editedUser.Email)) user!.Email = editedUser.Email;
            if (!string.IsNullOrEmpty(editedUser.Name)) user!.Nome = editedUser.Name;
            if (!string.IsNullOrEmpty(editedUser.Phone)) user!.Telefone = editedUser.Phone;
            if (editedUser.CodeExpiration != null) user!.ExpiracaoCodigo = editedUser.CodeExpiration;
            if (editedUser.RecoveryCode != null) user!.CodigoVerificacao = editedUser.RecoveryCode;

            _userRepository.Edit(user!);

            return NoContent();
        }

        //// POST api/User/send-recovery-code
        //[HttpPost("send-recoverycode")]
        //public IActionResult InitiatePasswordRecovery([FromBody] RecoverPasswordDTO model)
        //{
        //    var user = _userRepository.GetByEmail(model!.Email);
        //    if (user == null) return Ok("Code sent.");

        //    var authCode = Guid.NewGuid();
        //    var expiration = DateTime.Now.AddMinutes(15);

        //    user.CodigoVerificacao = authCode;
        //    user.ExpiracaoCodigo = expiration;

        //    _userRepository.Edit(user);





        //}




    }
}
