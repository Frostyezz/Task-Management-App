using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;
using BusinessLayer.Services;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public AuthController(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _authService = new AuthService(configuration);
        }

        [HttpPost("register")]
        public ObjectResult Register(UserDto userDto)
        {
            var isEmailAlreadyUsed = _userRepository.Find(u => u.Email == userDto.Email).FirstOrDefault();
            if (isEmailAlreadyUsed != null ) 
            {
                return BadRequest("This email adress is already used.");
            }

            _authService.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            _userRepository.Add(new User(userDto.Name, passwordHash, passwordSalt, userDto.Email));
            _userRepository.SaveChanges();

            var user = _userRepository.Find(u => u.Email == userDto.Email).First();
            Console.WriteLine(user.Id);
            return Ok(_authService.CreateToken(user.Id));

        }

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
