﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JeffMVC.Models;
using JeffMVC.Filters;

namespace JeffMVC.Controllers
{
	public class HolidaysController : Controller
    {
        private readonly HolidayContext _context;
		public List<SortOption> sortOptions = new List<SortOption>();
		
		public HolidaysController(HolidayContext context)
		{
            _context = context;
			sortOptions.Add(new SortOption { Id = 1, optionName = "Year" });
			sortOptions.Add(new SortOption { Id = 2, optionName = "Score" });
		}

        // GET: Holidays
        public async Task<IActionResult> Index()
        {
            return View(await _context.Holidays.ToListAsync());
        }

        // GET: Holidays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // GET: Holidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Year,Score")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holiday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }

        // GET: Holidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Year,Score")] Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holiday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayExists(holiday.Id))
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
            return View(holiday);
        }

        // GET: Holidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // POST: Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidayExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }

		[Logging]
		public async Task<IActionResult> Search(int? minscore, int? selectedoption)
		{
			var hols = new List<Holiday>();

			ViewBag.LastSelectedOption = selectedoption ?? 1;
			ViewBag.Minscore = minscore;
			ViewBag.SortOptions = sortOptions;

			if (minscore == null || minscore == 0)
			{
				return View(hols);
			}

			if (selectedoption == (int?)SortOptions.Year)
			{
				hols = await _context.Holidays.Where(h => h.Score >= minscore).OrderByDescending(y => y.Year).ToListAsync();
			} else
			{
				hols = await _context.Holidays.Where(h => h.Score >= minscore).OrderByDescending(s => s.Score).ToListAsync();
			}
						
			ViewBag.Numrows = hols.Count;
			return View(hols);
		}

		
    }
}
