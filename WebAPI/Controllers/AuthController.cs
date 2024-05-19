using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Dto;
using BusinessLayer.Services;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        /// <param name="userRepository">Repository for user data.</param>
        /// <param name="configuration">Configuration object for accessing app settings.</param>
        public AuthController(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _authService = new AuthService(configuration);
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="userDto">DTO containing user registration data.</param>
        /// <returns>A JWT token.</returns>
        [HttpPost("register")]
        public ObjectResult Register(UserDto userDto)
        {
            var isEmailAlreadyUsed = _userRepository.Find(u => u.Email == userDto.Email).FirstOrDefault();
            if (isEmailAlreadyUsed != null)
            {
                return BadRequest("This email address is already used.");
            }

            _authService.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            _userRepository.Add(new User(userDto.Name, passwordHash, passwordSalt, userDto.Email));
            _userRepository.SaveChanges();

            var user = _userRepository.Find(u => u.Email == userDto.Email).First();
            Console.WriteLine(user.Id);
            return Ok(_authService.CreateToken(user.Id));
        }

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="userDto">DTO containing user login data.</param>
        /// <returns>A JWT token.</returns>
        [HttpPost("login")]
        public ObjectResult Login(UserDto userDto)
        {
            var user = _userRepository.Find(u => u.Email == userDto.Email).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Wrong email or password.");
            }
            if (!_authService.VerifyPasswordHash(userDto.Password, user.Password, user.PasswordSalt))
            {
                return BadRequest("Wrong email or password.");
            }
            string token = _authService.CreateToken(user.Id);
            return Ok(token);
        }
    }
}
