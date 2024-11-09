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


        
    }
}
