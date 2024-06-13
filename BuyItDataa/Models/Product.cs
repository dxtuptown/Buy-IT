using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyItData.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(1,100000)]
        public double ListPrice {  get; set; }
        [Required]
        [Range(1, 100000)]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 100000)]
        public double Price100 { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public int CategoryID { get; set; }
        public virtual Category? Category { get; set; }
    }
}
