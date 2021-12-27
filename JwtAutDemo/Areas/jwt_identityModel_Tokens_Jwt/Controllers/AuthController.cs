using JwtAutDemo.Areas.Dtos;
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
    }
}
