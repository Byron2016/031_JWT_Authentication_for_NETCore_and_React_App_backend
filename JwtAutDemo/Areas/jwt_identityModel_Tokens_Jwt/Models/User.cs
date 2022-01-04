using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JwtAutDemo.Areas.jwt_identityModel_Tokens_Jwt.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore] //54.00
        public string Password { get; set; }
    }
}
