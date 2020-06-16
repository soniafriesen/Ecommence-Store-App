using Casestudy.DAL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 Project: Case Study
 Purpose: DAO for the customer
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DAO
{
    public class CustomerDAO
    {
        private AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public Customer Register(Customer customer) //creates a customer 
        {
            _db.Customers.Add(customer);
            _db.SaveChanges();
            return customer;
        }
        public Customer GetByEmail(string email) //gets a customer by email
        {
            Customer customer = _db.Customers.FirstOrDefault(u => u.Email == email);
            return customer;
        }
    }
}
