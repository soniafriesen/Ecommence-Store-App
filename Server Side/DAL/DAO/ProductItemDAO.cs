using System.Collections.Generic;
using System.Linq;
using Casestudy.DAL.DomainClasses;
/*
 Project: Case Study
 Purpose: DAO for the productitem
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DAO
{
    public class ProductItemDAO
    {
        private AppDbContext _db;
        public ProductItemDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Product> GetAllByBrand(int id) //get the product from what brand is comes from
        {
            return _db.Products.Where(item => item.Brand.Id == id).ToList();
        }
        public Product GetById(string id) //get the product by id
        {
            return _db.Products.Single(product => product.Id == id);
        }
    }
}
