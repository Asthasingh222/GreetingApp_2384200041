using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.Extensions.Logging;
using System;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;
        private readonly ILogger<UserBL> _logger;

        public UserBL(IUserRL userRepository, ILogger<UserBL> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user by hashing the password and storing the details.
        /// </summary>
        public string Register(UserDTO userDto)
        {
            try
            {
                _logger.LogInformation("Registration attempt for Username: {Username}", userDto.Username);

                if (_userRepository.IsUserExists(userDto.Username, userDto.Email))
                {
                    _logger.LogWarning("Registration failed: Username {Username} or Email {Email} already exists.", userDto.Username, userDto.Email);
                    throw new Exception("Username or Email already exists.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for Username: {Username}", userDto.Username);
                throw;
            }
        }

        /// <summary>
        /// Logs in a user by verifying the credentials.
        /// </summary>
        public UserDTO? Login(string username, string password)
        {
            try
            {
                _logger.LogInformation("Login attempt for Username: {Username}", username);

                var user = _userRepository.Login(username, password);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: Invalid credentials for Username: {Username}", username);
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                _logger.LogInformation("Login successful for Username: {Username}", username);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login for Username: {Username}", username);
                throw;
            }
        }
    }
}
