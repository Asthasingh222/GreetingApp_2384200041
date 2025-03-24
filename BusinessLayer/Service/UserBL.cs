using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using System.Net;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;
        private readonly JwtServices _jwtServices;
        private readonly ILogger<UserBL> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserBL(IUserRL userRepository, ILogger<UserBL> logger, IConfiguration configuration,JwtServices jwtServices,IEmailService emailservice)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
            _jwtServices = jwtServices;
            _emailService = emailservice;
        }

        public string Register(UserDTO userDto)
        {
            _logger.LogInformation("Registration attempt for Username: {Username}", userDto.Username);

            if (_userRepository.IsUserExists(userDto.Username, userDto.Email))
            {
                _logger.LogWarning("Registration failed: Username {Username} or Email {Email} already exists.", userDto.Username, userDto.Email);
                return "Username or Email already exists.";
            }

            string salt = _userRepository.GenerateSalt();
            string hashedPassword = _userRepository.HashPassword(userDto.Password, salt);

            var userEntity = new UserEntity
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            _userRepository.Register(userEntity);
            _logger.LogInformation("User {Username} registered successfully.", userDto.Username);

            return "User registered successfully.";
        }

        public string Login(string username, string password)
        {
            _logger.LogInformation("BL: User '{Username}' attempting to log in", username);

            var user = _userRepository.Login(username, password);
            if (user == null)
            {
                _logger.LogWarning("BL: Login failed for user {Username}", username);
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ForgetPasswordBL(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgetPasswordDTO.Email))
                {
                    throw new ArgumentException();
                }
                var user = _userRepository.GetUserByEmail(forgetPasswordDTO.Email);
                if (user == null)
                {
                    return false;
                }
                string token = _jwtServices.GenerateToken(user);
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException();
                }

                return _emailService.SendEmail(forgetPasswordDTO.Email, "Reset Password", token);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public bool ResetPasswordBL(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                string email = _jwtServices.ValidateToken(resetPasswordDTO.Token);
                if (string.IsNullOrEmpty(email))
                {
                    return false;
                }

                var user = _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return false;
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);

                return _userRepository.UpdatePasswordRL(user);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
