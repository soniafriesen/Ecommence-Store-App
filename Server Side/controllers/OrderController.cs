using System;
using System.Collections.Generic;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
 Project: Case Study
 Purpose: Controller for the order api
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here         
        {             
            _ctx = context;      
        }
        [HttpGet("{email}")]
        public ActionResult<List<Order>> List(string email)
        {
            List<Order> orders = new List<Order>();
            CustomerDAO cDao = new CustomerDAO(_ctx);
            Customer customerOwner = cDao.GetByEmail(email);
            OrderDAO tDao = new OrderDAO(_ctx);
            orders = tDao.GetAll(customerOwner.Id);
            return orders;
        }
        [HttpGet("{orderid}/{email}")]
        public ActionResult<List<OrderDetailsHelper>> GetTrayDetails(int orderid, string email)
        {
            OrderDAO dao = new OrderDAO(_ctx);
            return dao.GetOrderDetails(orderid, email);
        }
        [HttpPost]
        [Produces("application/json")]
        public ActionResult<string> Index(OrderHelper helper)
        {
            string retVal = ""; 
            try
            {
                CustomerDAO uDao = new CustomerDAO(_ctx);
                Customer OrderOwner = uDao.GetByEmail(helper.email);
                OrderDAO cDAO = new OrderDAO(_ctx);                
                int orderId = cDAO.AddOrder(OrderOwner.Id, helper.selections);
                
                foreach(OrderSelectionHelper order in helper.selections)
                {
                    if (orderId > 0 || order.item.QtyOnHand == 0 )
                    {
                             retVal = "Order " + orderId + " created! Goods Backordered";
                    }
                    else if(orderId > 0 || order.item.QtyOnHand > 0)
                    {
                        retVal = "Order " + orderId + " created!";
                    }               
                    else {
                        retVal = "Order not created";
                    }
                }
               
            }
            catch (Exception ex)
            { 
                retVal = "Order not created " + ex.Message;
            }
            return retVal;
        }
    }
}