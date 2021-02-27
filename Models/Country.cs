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
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string CountryMoneyType { get; set; }

        public int CountryWeekendNum1 { get; set; }

        public int CountryWeekendNum2 { get; set; }
        public virtual IList<ReturnBook> ReturnBook { get; set; }

        public virtual IList<Holidays> Holidays { get; set; }

    }
}
