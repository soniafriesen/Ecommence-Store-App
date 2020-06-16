using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
/*
 Project: Case Study
 Purpose: Controller for the branch api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */

namespace Casestudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        AppDbContext _db; public BranchController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet("{lat}/{lng}")] public ActionResult<List<Branch>> Index(float lat, float lng)
        {
            BranchDAO dao = new BranchDAO(_db); 
            return dao.GetThreeClosetStores(lat, lng);
        }
    }
}