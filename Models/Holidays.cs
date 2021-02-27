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
    public class Holidays
    {
        public int Id { get; set; }
        public DateTime HolidayDate { get; set; }

        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        [NotMapped]
        public virtual IEnumerable<SelectListItem> countries { get; set; }
    }
}
