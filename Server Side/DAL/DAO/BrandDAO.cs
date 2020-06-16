using Casestudy.DAL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 Project: Case Study
 Purpose: DAO for the brand
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DAO
{
    public class BrandDAO
    {
        private AppDbContext _db;
        public BrandDAO(AppDbContext ctx)
        { 
            _db = ctx; 
        }
        public List<Brand> GetAll() //returns a list of brands
        {
            return _db.Brands.ToList<Brand>();
        }
    }
}
