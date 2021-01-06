using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private const string SECRET_KEY = "TQvgjeABMPOwCycOqah5EQu5yyVjpmVG";  // 32 chars
        public static readonly SymmetricSecurityKey SIGNING_KEY = new
                               SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        [HttpGet("{username}/{password}")]
        public ActionResult<string> GetToken(string username, string password)
        {
            if (password == "correct")
            {
                var token = new JwtSecurityToken
                (
                    claims: new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, "Admins"),
                        //new Claim(ClaimTypes.Role, "Sales")
                    },
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                    signingCredentials: new SigningCredentials(SIGNING_KEY,
                                                                SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
