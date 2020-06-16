using System;
using System.Security.Cryptography;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 Project: Case Study
 Purpose: Controller for the register api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppDbContext _db;
        public RegisterController(AppDbContext context)
        {
            _db = context;
        }
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public ActionResult<CustomerHelper> Index(CustomerHelper helper)
        {
            CustomerDAO dao = new CustomerDAO(_db);
            Customer already = dao.GetByEmail(helper.email);
            if (already == null)
            {
                HashSalt hashSalt = GenerateSaltedHash(64, helper.password); helper.password = "";
                // don’t need the string anymore                 
                Customer dbUser = new Customer();
                dbUser.Firstname = helper.firstname;
                dbUser.Lastname = helper.lastname;
                dbUser.Email = helper.email;
                dbUser.Hash = hashSalt.Hash;
                dbUser.Salt = hashSalt.Salt;
                dbUser = dao.Register(dbUser);
                if (dbUser.Id > 0)
                    helper.token = "customer registered";
                else
                    helper.token = "customer registration failed";
            }
            else
            {
                helper.token = "customer registration failed - email already in use";
            }
            return helper;
        }
        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.             
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key               
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt }; return hashSalt;
        }
        public class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }
    }
}