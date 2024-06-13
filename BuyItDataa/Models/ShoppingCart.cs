using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyItData.Models
{
    public class ShoppingCart
    {
        
        
        public int ShoppingCartID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]               
        public virtual Product Product { get; set; }

        [Range(1,1000)]
        public int Count { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        [ValidateNever]        
        public virtual User User { get; set; }
        [NotMapped]
        public double PriceReal { get; set; }
    }
}
