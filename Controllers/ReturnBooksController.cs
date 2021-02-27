using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library_PenaltyCalculation.Data;
using Library_PenaltyCalculation.Models;
using System.Data;

namespace Library_PenaltyCalculation.Controllers
{
    public class ReturnBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReturnBooksController(ApplicationDbContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        // GET: ReturnBooks/Penalty
        [HttpGet]
        public IActionResult Penalty()
        {

            List<Country> countrylist = new List<Country>();
            countrylist = (from Country in _context.Country select Country).ToList();
            ViewData["CountryId"] = GetCountries(countrylist);
            return View();
        }
        
        [AllowAnonymous]
        // POST: ReturnBooks/Penalty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Penalty([Bind("ReturnBookId,CheckedOutDate,ReturnDate,CountryId")] ReturnBook returnBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(returnBook);
                await _context.SaveChangesAsync();

                if (returnBook.CheckedOutDate > returnBook.ReturnDate)
                {
                    //throw new ArgumentException("Checkout date cannot be before return date.");
                    returnBook.CalculatedBusinessDays = 0;
                    returnBook.CalculatedPenalty = 0;
                }

                else
                {
                    var innerJoin = from r in _context.ReturnBook
                                    join c in _context.Country on r.CountryId equals c.CountryId
                                    select new
                                    {
                                        weekend1Select = c.CountryWeekendNum1,
                                        weekend2Select = c.CountryWeekendNum2,
                                        moneyTypeSelect = c.CountryMoneyType,
                                        countryIdSelect = c.CountryId
                                    };

                    int weekend1 = 0;
                    int weekend2 = 0;

                    foreach (var data in innerJoin)
                    {
                        if(data.countryIdSelect == returnBook.CountryId)
                        {
                            weekend1 = data.weekend1Select;
                            weekend2 = data.weekend2Select;
                        }                      
                    }

                    TimeSpan span = returnBook.ReturnDate - returnBook.CheckedOutDate;

                    int daysBetween = span.Days + 1;
                    int newDaysBetween = daysBetween;

                    if (daysBetween > 10)
                    {
                        newDaysBetween = daysBetween - 10;
                    }

                    int businessDays = newDaysBetween;

                    int fullWeekCount = businessDays / 7;
                    // find out if there are weekends during the time exceeding the full weeks
                    if (businessDays > fullWeekCount * 7)
                    {
                        // below is to find out if there is a 1-day or 2-days weekend in the time interval remaining after subtracting the complete weeks
                        int firstDayOfWeek = (int)returnBook.CheckedOutDate.DayOfWeek;
                        if (daysBetween > 10)
                        {
                            firstDayOfWeek = (int)returnBook.CheckedOutDate.AddDays(10).DayOfWeek;
                        }
                        int lastDayOfWeek = (int)returnBook.ReturnDate.DayOfWeek;
                        if (lastDayOfWeek < firstDayOfWeek)
                            lastDayOfWeek += 7;
                        if (firstDayOfWeek <= weekend1)
                        {
                            if (lastDayOfWeek >= weekend2)// Both weekend1 and weekend2 are in the remaining time interval
                                businessDays -= 2;
                            else if (lastDayOfWeek >= weekend1)// Only weekend1 is in the remaining time interval
                                businessDays -= 1;
                        }
                        else if (firstDayOfWeek <= weekend2 && lastDayOfWeek >= weekend2)// Only weekend2 is in the remaining time interval
                            businessDays -= 1;
                    }

                    // subtract the weekends during the full weeks in the interval
                    businessDays -= fullWeekCount + fullWeekCount;

                    // subtract the holidays
                    DateTime startDate = returnBook.CheckedOutDate;
                    DateTime endDate = returnBook.ReturnDate;                   
                    if (daysBetween > 10)
                    {
                        startDate = returnBook.CheckedOutDate.AddDays(10);

                        DateTime currentDate = startDate;

                        var holiMatch = (_context.Holidays.Where(x => x.CountryId == returnBook.CountryId)).ToList();
                        foreach (var holi in holiMatch)
                        {
                            while (currentDate <= endDate)
                            {
                                if (currentDate.Day == holi.HolidayDate.Day && currentDate.Month == holi.HolidayDate.Month && currentDate.Year == holi.HolidayDate.Year && (int)currentDate.DayOfWeek != weekend1 && (int)currentDate.DayOfWeek != weekend2)
                                {
                                    businessDays -= 1;
                                }
                                currentDate = currentDate.AddDays(1);
                            }
                            currentDate = startDate;
                        }

                    }

                    if (daysBetween > 10)
                    {
                        returnBook.CalculatedBusinessDays = businessDays;
                        returnBook.CalculatedPenalty = businessDays * 5;
                    }
                    else
                    {
                        returnBook.CalculatedBusinessDays = 0;
                        returnBook.CalculatedPenalty = 0;
                    }
                }
                _context.ReturnBook.Update(returnBook);
                await _context.SaveChangesAsync();
            }
            List<Country> countrylist = new List<Country>();
            countrylist = (from Country in _context.Country select Country).ToList();
            ViewData["CountryId"] = GetCountries(countrylist);

            return View(returnBook);
        }

        private IEnumerable<SelectListItem> GetCountries(IEnumerable<Country> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.CountryId.ToString(),
                    Text = element.CountryName
                });
            }
            return selectList;
        }



        [Authorize]
        // GET: ReturnBooks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReturnBook.Include(r => r.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        // GET: ReturnBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBook
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.ReturnBookId == id);
            if (returnBook == null)
            {
                return NotFound();
            }

            return View(returnBook);
        }

        [Authorize]
        // GET: ReturnBooks/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId");
            return View();
        }

        [Authorize]
        // POST: ReturnBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReturnBookId,CheckedOutDate,ReturnDate,CountryId")] ReturnBook returnBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(returnBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", returnBook.CountryId);
            return View(returnBook);
        }

        [Authorize]
        // GET: ReturnBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBook.FindAsync(id);
            if (returnBook == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", returnBook.CountryId);
            return View(returnBook);
        }

        [Authorize]
        // POST: ReturnBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReturnBookId,CheckedOutDate,ReturnDate,CountryId")] ReturnBook returnBook)
        {
            if (id != returnBook.ReturnBookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(returnBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnBookExists(returnBook.ReturnBookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", returnBook.CountryId);
            return View(returnBook);
        }

        [Authorize]
        // GET: ReturnBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returnBook = await _context.ReturnBook
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.ReturnBookId == id);
            if (returnBook == null)
            {
                return NotFound();
            }

            return View(returnBook);
        }

        [Authorize]
        // POST: ReturnBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var returnBook = await _context.ReturnBook.FindAsync(id);
            _context.ReturnBook.Remove(returnBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnBookExists(int id)
        {
            return _context.ReturnBook.Any(e => e.ReturnBookId == id);
        }
    }
}
