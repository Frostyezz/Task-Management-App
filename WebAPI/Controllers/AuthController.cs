using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ObjectResult Register(UserDto userDto)
        {
            var isEmailAlreadyUsed = _userRepository.Find(u => u.Email == userDto.Email).FirstOrDefault();
            if (isEmailAlreadyUsed != null ) 
            {
                return BadRequest("This email adress is already used.");
            }

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            _userRepository.Add(new User(userDto.Name, passwordHash, passwordSalt, userDto.Email));
            _userRepository.SaveChanges();

            var user = _userRepository.Find(u => u.Email == userDto.Email).First();

            return Ok(CreateToken(user.Id));
        }

        [HttpPost("login")]
        public ObjectResult Login(UserDto userDto)
        {
            var user = _userRepository.Find(u => u.Email == userDto.Email).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Wrong email or password.");
            }
            if (!VerifyPasswordHash(userDto.Password, user.Password, user.PasswordSalt))
            {
                return BadRequest("Wrong email or password.");
            }
            string token = CreateToken(user.Id);
            return Ok(token);
        }

        private string CreateToken(Guid UserId)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, UserId.ToString())
            ];
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(String password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
    }
