using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Helpers
{
    public class OrderDetailsHelper
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ProductId { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public float CostMSRP { get; set; }
        public int QTYSold { get; set; }
        public int QTYOrdered { get; set; }
        public int QTYBacked { get; set; }
        public int QTY { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax {get; set; }
        public decimal OrderTotal { get; set; }
        public string DateCreated { get; set; }
    }
}
