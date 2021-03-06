using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Dtos;
using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Data;
using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Helpers;
using Microsoft.AspNetCore.Http;

namespace JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _user;

        private readonly JwtService _jwtService;

        public AuthController(IUserRepository user, JwtService jwtService)
        {
            _user = user;
            _jwtService = jwtService;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            //http://localhost:8000/api/register
            ////Body raw json
            //{
            //    "name": "a",
            //    "email": "a.yahoo.com",
            //    "password": "a"
            //}
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
            //http://localhost:8000/api/login
            ////Body raw json
            //{
            //    "email": "a.yahoo.com",
            //    "password": "a"
            //}
            var user = _user.GetByEmail(dto.Email);
            if (user == null) return BadRequest(new { message = "Invalid credentials" });

            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            salt = "$2a$12$ncjskFMRG08WaoGrZkXhGe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = $"Invalid credentials en base {user.Password} enviado {hashedPassword}" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions 
            { 
                HttpOnly = true // front end cannot access it. the only propose is get it in the back end and backend can modify or access it. 
            });

            return Ok(new 
            { 
                message = "success"
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            //50.00
            //http://localhost:8000/api/user
            ////Body raw jso

            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                var userId = int.Parse(token.Issuer);

                var user = _user.GetById(userId);

                return Ok(user);
            } catch(Exception _)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //54.48
            //http://localhost:8000/api/logout
            ////Body raw jso

            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "success"
            });
        }
    }
}
