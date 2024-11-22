using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Services;
using SmartAgroAPI.Services.EmailService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartAgroAPI.Controllers
{

    /// <summary>
    /// Controller that manage the users in the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;

        private readonly EmailSendingService _emailService;

        /// <summary>
        /// Constructor that gets the repository and emailservice dependencies
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="emailSendingService"></param>
        /// <param name="mapper"></param>
        public UserController(IUserRepository userRepository, EmailSendingService emailSendingService, IMapper mapper)
        {
            _userRepository = userRepository;
            _emailService = emailSendingService;
            _mapper = mapper;
        }


        // POST api/User/Login
        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="userCredentials">The user's login credentials, including email and password.</param>
        /// <returns>
        /// A `200 OK` response with a JWT token if the login is successful; 
        /// `401 Unauthorized` if the credentials are invalid.
        /// </returns>
        /// <remarks>
        /// This endpoint authenticates a user by validating their email and password. 
        /// If the credentials match an existing user, a JSON Web Token (JWT) is generated and returned to the client.
        /// </remarks>
        /// <response code="200">Returns a JWT token upon successful authentication.</response>
        /// <response code="401">Unauthorized, invalid email or password.</response>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Post([FromBody] UserAuthenticationDTO userCredentials)
        {
            var user = _userRepository.Login(userCredentials);

            if (user == null)
                return Unauthorized();

            var token = JWTService.GenerateToken(user);

            return Ok(new TokenDTO() { user = new UserDTO(user), Token = token });

        }

        // POST api/User

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="userData">An object containing the user's registration details.</param>
        /// <returns>
        /// A `200 OK` response with the registered user details upon successful registration.
        /// </returns>
        /// <remarks>
        /// This endpoint creates a new user account using the provided data. All required fields should be supplied for the registration to succeed.
        /// </remarks>
        /// <response code="200">Returns the created user's information.</response>
        /// <response code="400">Bad request, invalid input or missing required fields.</response>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterDTO userData)
        {
            if (_userRepository.GetByEmail(userData.Email) != null) return BadRequest("An user with the provided email already exists.");
            _userRepository.Register(userData);
            return Ok(userData);
        }


        // GET api/User
        /// <summary>
        /// Returns all users without their passwords.
        /// </summary>
        /// <returns>
        /// A `200 Ok` and a list of users. 
        /// </returns>
        /// <remarks>
        /// This endpoint allows getting all users from the system without exposing their passwords.
        /// </remarks>
        /// <response code="200">Returns all users in the system.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepository.GetAll());
        }


        // GET api/User/{id}

        /// <summary>
        /// Returns an user searched by his Id.
        /// </summary>
        /// <param name="id">The unique identifier of the user to find.</param>
        /// <returns>
        /// A `200 Ok` if the user was found . 
        /// A `404 Not Found` response if the user with the specified ID does not exist.
        /// </returns>
        /// <remarks>
        /// This endpoint allows getting an user object by his unique identifier.
        /// </remarks>
        /// <response code="200">The user was found.</response>
        /// <response code="404">User not found with the specified ID.</response>
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);

        }

        // PATCH api/User/{id}

        /// <summary>
        /// Updates specified fields of an existing user by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="editedUser">An object containing the user details to update. Only non-null fields will be updated.</param>
        /// <returns>
        /// A `204 No Content` response if the update was successful. 
        /// A `404 Not Found` response if the user with the specified ID does not exist.
        /// </returns>
        /// <remarks>
        /// This endpoint allows partial updates to a user’s information. Fields that are not provided or are null in the request body will not be updated. 
        /// </remarks>
        /// <response code="204">The user was successfully updated with the provided details.</response>
        /// <response code="404">User not found with the specified ID.</response>
        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult EditUserById(Guid id, [FromBody] UserDTO editedUser)
        {
            var user = _userRepository.GetById(id);

            _mapper.Map(editedUser, user);

            _userRepository.Edit(user!);

            return NoContent();
        }

        // POST api/User/recover-password

        /// <summary>
        /// Initiates password recovery by generating a recovery code and sending it to the user email.
        /// </summary>
        /// <param name="model">User email</param>
        /// <returns>
        /// A message indicating that a recovery code has been sent if an account with the given email exists. 
        /// For security reasons, the response is the same whether or not the email exists in the system.
        /// </returns>
        /// /// <response code="200">Recovery code sent if an account with the given email exists.</response>
        [AllowAnonymous]
        [HttpPost("recover-password")]
        public async Task<IActionResult> InitiatePasswordRecovery([FromBody] RecoverPasswordDTO model)
        {
            var user = _userRepository.GetByEmail(model!.Email!);

            if (user == null)
                return Ok("Code sent if the account with the given email exists.");

            var recoveryCode = _userRepository.GenerateRecoveryCode(user.Id);

            await _emailService.SendRecoveryEmailAsync(user.Nome!, user.Email, recoveryCode!);

            return Ok("Code sent if the account with the given email exists.");

        }

        /// <summary>
        /// Verifies a temporary password recovery code.
        /// </summary>
        /// <param name="model">
        /// An object containing the user's email and temporary token.
        /// </param>
        /// <returns>
        /// A `200 OK` response with a success message if the token is valid; 
        /// `400 Bad Request` if the provided token is invalid.
        /// </returns>
        /// <remarks>
        /// This endpoint allows users to authenticate the password reset process
        /// If the token is correct, the user will have permission to reset his password
        /// </remarks>
        /// <response code="200">Token verified.</response>
        /// <response code="400">Bad request, invalid token or other input errors.</response>
        [AllowAnonymous]
        [HttpPost("verify-code")]
        public IActionResult VerifyPasswordRecoveryCode([FromBody] VerifyCodeDTO model)
        {
            var user = _userRepository.GetByEmail(model.Email);

            var authenticated = _userRepository.AuthenticateCode(user!.Id, model.Code!);

            if (!authenticated || user.ExpiracaoCodigo!.Value < DateTime.Now)
                return BadRequest("Invalid token.");

            return Ok("Valid token.");

        }

        /// <summary>
        /// Verifies a temporary password recovery code and resets the user's password if valid.
        /// </summary>
        /// <param name="model">
        /// An object containing the user's email, temporary token, and new password.
        /// </param>
        /// <returns>
        /// A `200 OK` response with a success message if the password is successfully reset; 
        /// `400 Bad Request` if the provided token is invalid.
        /// </returns>
        /// <remarks>
        /// This endpoint allows users to reset their password by providing a valid recovery token. 
        /// If the token is correct, the user's password will be updated with the new one provided.
        /// </remarks>
        /// <response code="200">Password was successfully reset.</response>
        /// <response code="400">Bad request, invalid token or other input errors.</response>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = _userRepository.GetByEmail(model.Email!);

            var authenticated = _userRepository.AuthenticateCode(user!.Id, model.TemporaryToken!);

            _userRepository.ChangePassword(user.Id, model.NewPassword!);
            if (authenticated) return Ok("User password sucessfully changed.");

            return BadRequest("Invalid token.");
        }
    }
}
