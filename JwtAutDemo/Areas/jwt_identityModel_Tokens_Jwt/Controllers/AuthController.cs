using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Dtos;
using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Data;
using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _user;

        public AuthController(IUserRepository user)
        {
            _user = user;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password) 
            };
            
            return Created("success", _user.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _user.GetByEmail(dto.Email);
            if (user == null) return BadRequest(new { message = "Invalid credentials" });

            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            salt = "$2a$12$ncjskFMRG08WaoGrZkXhGe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = $"Invalid credentials en base {user.Password} enviado {hashedPassword}" });
            }

            return Ok(user);
        }
    }
}
