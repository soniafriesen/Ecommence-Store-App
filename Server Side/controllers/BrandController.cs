using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
/*
 Project: Case Study
 Purpose: Controller for the brand api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public ActionResult<List<Brand>> Index() 
        {
            BrandDAO dao = new BrandDAO(_db);
            List<Brand> allBrands = dao.GetAll(); 
            return allBrands; 
        }
    }
}