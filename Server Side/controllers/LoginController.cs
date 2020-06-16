using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Authorization;
/*
 Project: Case Study
 Purpose: Controller for the login api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        AppDbContext _db;
        IConfiguration configuration;
        public LoginController(AppDbContext context, IConfiguration config)
        {
            _db = context; this.configuration = config;
        }

        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public ActionResult<CustomerHelper> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer user = dao.GetByEmail(helper.email);
            if (VerifyPassword(helper.password, user.Hash, user.Salt))
            {
                helper.password = "";
                var appSettings = configuration.GetSection("AppSettings").GetValue<string>("Secret");
                // authentication successful so generate jwt token                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string returnToken = tokenHandler.WriteToken(token);
                helper.token = returnToken;
            }
            else { 
                helper.token = "customer email or password invalid - login failed";
            }
            return helper;
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
    }
}