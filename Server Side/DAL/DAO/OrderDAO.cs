using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 Project: Case Study
 Purpose: DAO for the order
 Coder: Sonia Friesen 0813682
 Date: Due June 17th, 2020
 */
namespace Casestudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Order> GetAll(int id)
        {
            return _db.Orders.Where(order => order.UserId == id).ToList<Order>();
        }
        public List<OrderDetailsHelper> GetOrderDetails(int cid, string email)
        {
            Customer customer = _db.Customers.FirstOrDefault(user => user.Email == email);

            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();// LINQ way of doing INNER JOINS 
            var results = from o in _db.Orders
                          join oi in _db.OrderLineItems on o.Id equals oi.OrderId
                          join pi in _db.Products on oi.ProductId equals pi.Id
                          where (o.UserId == customer.Id && o.Id == cid)
                          select new OrderDetailsHelper //create a new helperdetails for the details of a previous order
                          {
                              OrderId = o.Id,
                              CustomerId = customer.Id,
                              ProductId = pi.Id,
                              Description = pi.Description,
                              ProductName = pi.ProductName,
                              CostMSRP = pi.MSRP,
                              QTYSold = oi.QtySold,
                              QTYOrdered = oi.QtyOrdered,
                              QTYBacked = oi.QtyBackOrdered,
                              Subtotal = o.OrderAmount,
                              QTY = oi.QtyOrdered,
                              Tax = o.OrderAmount *(decimal) 0.13,
                              OrderTotal = o.OrderAmount + (o.OrderAmount * (decimal)0.13),
                              DateCreated = o.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")                              
                          };
            allDetails = results.ToList<OrderDetailsHelper>();
            return allDetails;
        }
        public int AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderid = 1;
            using(_db)
            {
                // we need a transaction as multiple entities involved    
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0;                                           
                        // calculate the totals and then add the  order row to the table 
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += Convert.ToDecimal(selection.item.CostPrice * selection.Qty);
                        }
                        _db.Orders.Add(order);
                        _db.SaveChanges();
                        // then add each item to the orderlineitems table   
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            ProductItemDAO productDAO = new ProductItemDAO(_db);
                            Product product = productDAO.GetById(selection.item.Id);
                            OrderLineItem oItem = new OrderLineItem();
                            oItem.OrderId = order.Id; 
                            oItem.ProductId = selection.item.Id;                           
                            oItem.SellingPrice = Convert.ToDecimal(selection.item.MSRP);
                            oItem.QtyOrdered = selection.Qty;
                            oItem.QtySold = selection.Qty;
                            oItem.QtyBackOrdered = 0;
                            
                            if(selection.Qty < selection.item.QtyOnHand) //if we order less than what we have on hand
                            {
                                product.QtyOnHand -= selection.Qty;
                                oItem.QtySold = selection.Qty;
                                oItem.QtyOrdered = selection.Qty;
                                oItem.QtyBackOrdered = 0;
                            }
                            if(selection.Qty > product.QtyOnHand) //if we order more than we have on hand
                            {
                                oItem.SellingPrice = 0;
                                oItem.QtyOrdered = selection.Qty;                                                  
                                oItem.QtyBackOrdered = selection.Qty - product.QtyOnHand;
                                product.QtyOnBackOrder = selection.Qty - product.QtyOnHand;
                                product.QtyOnHand = 0;
                                oItem.QtySold = product.QtyOnHand;

                                //QtySold = QtyOnHand, QtyOrdered = Qty, QtyBackOrdered = Qty - QtyOnHand 
                            }
                            _db.Products.Update(product);
                            _db.OrderLineItems.Add(oItem);                            
                            _db.SaveChanges();                          
                        }
                        _trans.Commit();
                        orderid = order.Id;
                    }
                    catch (Exception ex) { 
                        Console.WriteLine(ex.Message);
                        _trans.Rollback(); 
                    }
                }
            }
            return orderid;
        }
    }
}
