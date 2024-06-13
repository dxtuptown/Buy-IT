using Microsoft.AspNetCore.Identity;
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
    public class User:IdentityUser
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        
        [ForeignKey(nameof(CompanyID))]
        [ValidateNever]
        public int? CompanyID { get; set; }
        public virtual Company? Company { get; set; }
    }
}
