using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyItData.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        [Required]
        public int OrderHeaderID { get; set; }
        [ForeignKey(nameof(OrderHeaderID))]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey(nameof (ProductID))]
        [ValidateNever]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double PriceReal { get; set; }

    }
}
