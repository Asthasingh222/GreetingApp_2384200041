using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;

namespace HelloGreetingApplication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserBL userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Method to Register User
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO user)
        {
            _logger.LogInformation("Register method called for Username: {Username}", user.Username);

            string result = _userService.Register(user);

            if (result == "User registered successfully.")
            {
                _logger.LogInformation("User {Username} registered successfully.", user.Username);
                return Ok(new { message = result });
            }

            _logger.LogWarning("User {Username} registration failed: {Result}", user.Username, result);
            throw new Exception("User registration failed. Username or Email already exists.");
        }

        /// <summary>
        /// Method to Login User
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO loginRequest)
        {
            _logger.LogInformation("Login attempt for Username: {Username}", loginRequest.Username);

            var user = _userService.Login(loginRequest.Username!, loginRequest.Password!);

            if (user == null)
            {
                _logger.LogWarning("Login failed for Username: {Username}. Invalid credentials.", loginRequest.Username);
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            _logger.LogInformation("Login successful for Username: {Username}", loginRequest.Username);
            return Ok(new { message = "Login successful", user });
        }
    }
}
