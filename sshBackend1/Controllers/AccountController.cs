//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using sshBackend1.Helpers;
//using System;

//namespace sshBackend1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly JwtTokenHelper _jwtTokenHelper;

//        public AccountController(JwtTokenHelper jwtTokenHelper)
//        {
//            _jwtTokenHelper = jwtTokenHelper;
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginRequest request)
//        {
//            // For simplicity, hardcoding user validation, replace with your user service
//            if (request.Username == "admin" && request.Password == "password")
//            {
//                var token = _jwtTokenHelper.GenerateToken(request.Username, "Admin");
//                return Ok(new { Token = token });
//            }

//            return Unauthorized("Invalid credentials");
//        }

//        [HttpPost("register")]
//        public IActionResult Register([FromBody] RegisterRequest request)
//        {
//            // Register logic (e.g., create user in database) goes here
//            return Ok("User registered successfully");
//        }
//    }

//    public class LoginRequest
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }

//    public class RegisterRequest
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }
//}
