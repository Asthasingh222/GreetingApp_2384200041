﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

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
        /// Method to Login User and return JWT Token
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO loginRequest)
        {
            _logger.LogInformation("Login attempt for Username: {Username}", loginRequest.Username);

            string token = _userService.Login(loginRequest.Username!, loginRequest.Password!);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Login failed for Username: {Username}. Invalid credentials.", loginRequest.Username);
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            _logger.LogInformation("Login successful for Username: {Username}", loginRequest.Username);
            return Ok(new { message = "Login successful", token });
        }

        /// <summary>
        /// Protected method which require JWT authentication
        /// </summary>
        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok(new { message = "You have accessed a protected resource!" });
        }


        /// <summary>
        /// Reset Password - Updates password using JWT token.
        /// </summary>
        [HttpPost("forget-password")]
        public IActionResult ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {
                var result = _userService.ForgetPasswordBL(forgetPasswordDTO);
                if (!result)
                {
                    return BadRequest("Email not found!");
                }

                return Ok("Reset password email sent successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var result = _userService.ResetPasswordBL(resetPasswordDTO);
                if (!result)
                {
                    return BadRequest("Invalid or expired token.");
                }
                return Ok("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
