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

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/User/Login
        [HttpPost("Login")]
        public IActionResult Post(UserAuthenticationDTO userCredentials)
        {
            var user = _userRepository.Login(userCredentials);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = JWTService.GenerateToken(user.Id, user.Email);

            return Ok(new TokenDTO() { Token = token });

        }


        // POST api/User/Register
        [HttpPost("Register")]
        public IActionResult Post(UserRegisterDTO userData)
        {
            _userRepository.Register(userData);
            return Ok();

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}
