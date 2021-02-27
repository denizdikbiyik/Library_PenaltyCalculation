using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_PenaltyCalculation.Models
{
    public class ReturnBook
    {
        public int ReturnBookId { get; set; }

        [Required(ErrorMessage = "You have to select checked out date.")]
        public DateTime CheckedOutDate { get; set; }

        [Required(ErrorMessage = "You have to select return date.")]
        public DateTime ReturnDate { get; set; }
        
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        [NotMapped]
        public virtual IEnumerable<SelectListItem> countries { get; set; }

        public int CalculatedBusinessDays { get; set; }
        public decimal CalculatedPenalty { get; set; }
    }
}
