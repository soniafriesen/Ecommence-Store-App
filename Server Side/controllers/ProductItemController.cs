using System.Collections.Generic;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 Project: Case Study
 Purpose: Controller for the productItem api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        AppDbContext _db;
        public ProductItemController(AppDbContext context)
        {
            _db = context;
        }

        [Route("{brandid}")]
        public ActionResult<List<Product>> Index(int brandid)
        {
            ProductItemDAO dao = new ProductItemDAO(_db);
            List<Product> itemsForBrand = dao.GetAllByBrand(brandid);
            foreach(Product product in itemsForBrand)
            {
                product.BrandId = brandid;
            }
            return itemsForBrand;
        }

    }
}